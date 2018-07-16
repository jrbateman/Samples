using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XtenderSolutions.AXRESTClient
{
    public class AXRESTClientDocODMA
    {
        private AXRESTDataModel.AXDocODMA odma;

        public AXRESTClientDocODMA(AXRESTDataModel.AXDocODMA odma)
        {
            this.odma = odma;
        }

        public string Name
        {
            get
            {
                if (this.odma != null)
                    return this.odma.Name;
                else
                    throw new NullReferenceException("The AXDoc ODMA properties are not initialized");
            }
        }

        public string Author
        {
            get
            {
                if (this.odma != null)
                    return this.odma.Author;
                else
                    throw new NullReferenceException("The AXDoc ODMA properties are not initialized");
            }
        }

        public string Subject
        {
            get
            {
                if (this.odma != null)
                    return this.odma.Subject;
                else
                    throw new NullReferenceException("The AXDoc ODMA properties are not initialized");
            }
        }

        public string Comment
        {
            get
            {
                if (this.odma != null)
                    return this.odma.Comment;
                else
                    throw new NullReferenceException("The AXDoc ODMA properties are not initialized");
            }
        }

        public DateTime? Created
        {
            get
            {
                if (this.odma != null)
                    return this.odma.Created;
                else
                    throw new NullReferenceException("The AXDoc ODMA properties are not initialized");
            }
        }

        public string Creator
        {
            get
            {
                if (this.odma != null)
                    return this.odma.Creator;
                else
                    throw new NullReferenceException("The AXDoc ODMA properties are not initialized");
            }
        }

        public DateTime? Modified
        {
            get
            {
                if (this.odma != null)
                    return this.odma.Modified;
                else
                    throw new NullReferenceException("The AXDoc ODMA properties are not initialized");
            }
        }

        public string Modifier
        {
            get
            {
                if (this.odma != null)
                    return this.odma.Modifier;
                else
                    throw new NullReferenceException("The AXDoc ODMA properties are not initialized");
            }
        }

        public string[] Keywords
        {
            get
            {
                if (this.odma != null)
                    return this.odma.Keywords;
                else
                    throw new NullReferenceException("The AXDoc ODMA properties are not initialized");
            }
        }
    }
}
