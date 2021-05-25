using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ContactMgr.Components
{


    public partial class Field 
    {
        public string Title { get { return lbl.Text; } set { lbl.Text = value; titleTxt.Text = value; } }
        public string Value { get { return txt.Text; } set { txt.Text = value; } }

        public Field()
        {
            InitializeComponent();
        }

        private void MenuItemClick1(object sender, RoutedEventArgs e)
        {
            titleTxt.Text = lbl.Text;
            titleTxt.SelectAll();
            titleTxt.Visibility = Visibility.Visible;
        }


        private void ChangeTitle(object sender, TextChangedEventArgs e)
        {
            lbl.Text = titleTxt.Text;
        }

        private void TitleTxtKeyDown1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) titleTxt.Visibility = Visibility.Hidden;
        }

        private void UserControlLoaded1(object sender, RoutedEventArgs e)
        {
            if (titleTxt.Visibility == Visibility.Visible) titleTxt.Focus(); else txt.Focus();
        }

        private void TxtOnMouseDown(object sender, MouseButtonEventArgs e)
        {
            titleTxt.Visibility = Visibility.Hidden;
        }

    }
}
