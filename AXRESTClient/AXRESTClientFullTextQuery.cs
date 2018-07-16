using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XtenderSolutions.AXRESTDataModel;

namespace XtenderSolutions.AXRESTClient
{
    public class AXRESTClientFullTextQuery
    {
        private AXFulltextQuery fulltextQuery;

        public AXRESTClientFullTextQuery(AXFulltextQuery fulltextQuery)
        {
            this.fulltextQuery = fulltextQuery;
        }

        public string FTQueryExpression
        {
            get
            {
                if (this.fulltextQuery != null)
                    return this.fulltextQuery.FTQueryExpression.ToString();
                else
                    throw new NullReferenceException("The AX full text query is not initialized");
            }
        }

        public string FTQueryOperator
        {
            get
            {
                if (this.fulltextQuery != null)
                    return this.fulltextQuery.FTQueryOperator.ToString();
                else
                    throw new NullReferenceException("The AX full text query is not initialized");
            }
        }

        public string FTQueryValue
        {
            get
            {
                if (this.fulltextQuery != null)
                    return this.fulltextQuery.FTQueryValue;
                else
                    throw new NullReferenceException("The AX full text query is not initialized");
            }
        }

        public Dictionary<string, bool> FTQueryOptions
        {
            get
            {
                if (this.fulltextQuery != null)
                    return this.fulltextQuery.FTQueryOptions;
                else
                    throw new NullReferenceException("The AX full text query is not initialized");
            }
        }
    }
}
