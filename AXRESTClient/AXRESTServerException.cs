using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using XtenderSolutions.AXRESTDataModel;

namespace XtenderSolutions.AXRESTClient
{
    public class RestException : Exception
    {
        public string RequestUrl { get; private set; }

        public HttpStatusCode? ResponseStatus { get; private set; }

        public string ResponseContent { get; private set; }

        public RestException(string requestUrl, HttpStatusCode responseStatus, string responseContent)
            : base(responseStatus.ToString())
        {
            RequestUrl = requestUrl;
            ResponseStatus = responseStatus;
            ResponseContent = responseContent;
        }

        public RestException(string requestUrl, Exception innerException)
            : base(innerException.Message, innerException)
        {
            RequestUrl = requestUrl;
            ResponseStatus = null;
            ResponseContent = null;
        }
    }

    public static class RestExceptionExtention
    {
        public static string GetErrorMessage(this RestException exception)
        {
            string message;

            if (TryParseResponseError(exception, out message))
                return message;

            if (TryParseHttpError(exception, out message))
                return message;

            return string.Format("{0} Detail: {1}",
                "Internal server error. A generic error has occurred on the server.",
                exception.Message);
        }

        private static bool TryParseResponseError(RestException exception, out string message)
        {
            message = string.Empty;

            if (!exception.ResponseStatus.HasValue)
                return false;

            switch (exception.ResponseStatus)
            {
                case HttpStatusCode.Unauthorized:
                    message = "Invalid user name or password.";
                    break;
                case HttpStatusCode.Forbidden:
                    message = "The server rejected the request.";
                    break;
                case HttpStatusCode.NotFound:
                    message = "The requested resource does not exist on the server.";
                    break;
                case HttpStatusCode.InternalServerError:
                default:
                    message = string.Format("{0} Detail: {1}",
                        "Internal server error. A generic error has occurred on the server.",
                        exception.Message);
                    break;
            }

            return true;
        }

        private static bool TryParseHttpError(RestException exception, out string message)
        {
            message = string.Empty;

            if (exception.InnerException == null)
                return false;

            if (!(exception.InnerException is HttpRequestException) &&
                !(exception.InnerException is UriFormatException))
                return false;

            if (exception.InnerException is HttpRequestException)
            {
                var requestException = exception.InnerException as HttpRequestException;
                if (requestException.InnerException is WebException)
                {
                    var webException = requestException.InnerException as WebException;
                    switch (webException.Status)
                    {
                        case WebExceptionStatus.NameResolutionFailure:
                            message = "The requested resource does not exist on the server.";
                            break;
                        case WebExceptionStatus.TrustFailure:
                            message = "Unable to establish a trusted SSL connection.";
                            break;
                        case WebExceptionStatus.Timeout:
                            message = "Request timeout.";
                            break;
                        default:
                            message = "Network error.";
                            break;
                    }
                }
                else
                {
                    message = "Network error.";
                }
            }
            else if (exception.InnerException is UriFormatException)
            {
                message = "Invalid URI: The format of the REST Server Address could not be determined.";
            }

            return true;
        }

        public static string GetResponseError(this RestException exception)
        {
            // try parse as AXRest error
            try
            {
                Error error = AXRESTDataModelConvert.DeserializeObject<Error>(exception.ResponseContent, AXRESTMediaTypes.JSON);
                return error.Message;
            }
            catch { }

            // check if a raw string error (not HTML page return by server)
            if (!exception.ResponseContent.Contains("<"))
                return exception.ResponseContent;

            return string.Empty;
        }
    }
}
