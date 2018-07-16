using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using XtenderSolutions.AXRESTDataModel;

namespace AXRESTTestConsole.UserControls
{
    /// <summary>
    /// Interaction logic for FullTextOptions.xaml
    /// </summary>
    public partial class FullTextOptions : UserControl
    {
        public FullTextOptions()
        {
            InitializeComponent();
        }

        public FullTextSearchOptions FTOptions
        {
            get
            {
                if (this.cbFTOptions.IsChecked.HasValue && this.cbFTOptions.IsChecked.Value)
                {
                    FullTextSearchOptions options = new FullTextSearchOptions();
                    options.Value = this.tbValue.Text;
                    options.Thesaurus = this.cbThesaurus.IsChecked.HasValue && this.cbThesaurus.IsChecked.Value;

                    if (this.rbOperationAnd.IsChecked.HasValue && this.rbOperationAnd.IsChecked.Value)
                    {
                        options.QueryOperator = AXFulltextQueryOperator.And;
                    }
                    else if (this.rbOperationOr.IsChecked.HasValue && this.rbOperationOr.IsChecked.Value)
                    {
                        options.QueryOperator = AXFulltextQueryOperator.Or;
                    }


                    if (this.rbSearchTypeAll.IsChecked.HasValue && this.rbSearchTypeAll.IsChecked.Value)
                    {
                        options.SearchType = AXFulltextQueryExpression.And;
                    }
                    else if (this.rbSearchTypeAny.IsChecked.HasValue && this.rbSearchTypeAny.IsChecked.Value)
                    {
                        options.SearchType = AXFulltextQueryExpression.OR;
                    }
                    else if (this.rbSearchTypeExact.IsChecked.HasValue && this.rbSearchTypeExact.IsChecked.Value)
                    {
                        options.SearchType = AXFulltextQueryExpression.Exact;
                    }
                    else if (this.rbSearchTypeExpression.IsChecked.HasValue && this.rbSearchTypeExpression.IsChecked.Value)
                    {
                        options.SearchType = AXFulltextQueryExpression.Expression;
                    }

                    return options;
                }
                else
                {
                    return null;
                }
        } }
    }
}
