using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AXRESTTestConsole
{
    public class ExtendedDataTable : DataTable
    {
        public ExtendedDataTable()
            : base()
        {
        }

        public ExtendedDataTable(string tableName)
            : base(tableName)
        {
        }

        public ExtendedDataTable(string tableName, string tableNamespace)
            : base(tableName, tableNamespace)
        {
        }

        // Return the RowType as ExtendedDataRow instead of DataRow
        protected override Type GetRowType()
        {
            return typeof(ExtendedDataRow);
        }

        // Use the RowBuilder to return an ExtendedDataRow instead of DataRow
        protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
        {
            return new ExtendedDataRow(builder);
        }
    }

    public class ExtendedDataRow : DataRow
    {
        public ExtendedDataRow()
            : base(null)
        {
        }

        public ExtendedDataRow(DataRowBuilder rb)
            : base(rb)
        {
        }

        // The tag object attached to the ExtendedDataRow
        public object Tag { get; set; }
    }
}
