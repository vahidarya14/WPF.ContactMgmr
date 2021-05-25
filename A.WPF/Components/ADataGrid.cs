using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace A.WPF.Components
{
    public class ADataGrid : DataGrid
    {

        public ADataGrid()
        {
            AutoGenerateColumns = false;
            IsTextSearchEnabled = true;
            CanUserAddRows = false;
            CanUserResizeRows = false;
            Focusable = false;
            CanUserDeleteRows = false;
            AreRowDetailsFrozen = true;
            IsReadOnly = true;
            HorizontalGridLinesBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0xEE, 0xEE, 0xEE));
            VerticalGridLinesBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0xCB, 0xCB, 0xCB));
            AlternatingRowBackground = new SolidColorBrush(Color.FromArgb(255, 244, 244, 244));
            AlternationCount = 1;
            MinColumnWidth = 55;
            RowHeight = 30;
            BorderThickness = new Thickness(0);
            HeadersVisibility = DataGridHeadersVisibility.All;
            Background = new SolidColorBrush(Colors.Transparent);


            LoadingRow += XDataGrid_LoadingRow;
            AddingNewItem += XDataGrid_AddingNewItem;
        }

        void XDataGrid_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            ScrollIntoView(Items[Items.Count - 1]);
        }

        void XDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString(CultureInfo.InvariantCulture);
        }
    }
}
