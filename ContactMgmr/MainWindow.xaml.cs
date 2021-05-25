using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ContactMgr.viewModel;
using DAL.DataModel;
using Serviecs;
using Serviecs.EXporter;
using StaticPublic = Serviecs.StaticPublic;

namespace ContactMgr
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ContactVm _vm;
        public Func<Contact, bool> LastPredcate { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            _vm = new ContactVm();
            DataContext = _vm;


            _vm.Contacts = new ObservableCollection<Contact>(_vm.GetAll());
            _vm.FilterdContacts = _vm.Contacts;
            _vm.FilterdGroups = _vm.Contacts;

        }




        view.ContactVw ShoeNewSettle(WindowMode windowMode, Contact c)
        {
            var ss = new view.ContactVw(c, _vm, windowMode, this) { Margin = new Thickness(10, 71, 0, 0) };
            ss.SetValue(Grid.ColumnSpanProperty, 3);
            return ss;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var ss = new view.ContactVw(_vm, this) { Margin = new Thickness(10, 71, 0, 0) };
            ss.SetValue(Grid.ColumnSpanProperty, 3);

            ss.Show();
        }

        private void TextBoxTextChanged1(object sender, TextChangedEventArgs e)
        {
            OnlineSearchHappening();
        }
        public void OnlineSearchHappening(Func<Contact, bool> predcate = null, ObservableCollection<Contact> contacts = null)
        {
            var txt = SearchTextBox.Text;
            if (predcate == null)
                predcate = a => a.Name.Contains(txt) || a.Title.Contains(txt) || a.Company.Contains(txt) || a.Mobile.Contains(txt);

            if (contacts == null)
                _vm.FilterdContacts = new ObservableCollection<Contact>(_vm.Contacts.AsQueryable().Where(predcate).OrderBy(a => a.Name).ToList());
            else
                _vm.FilterdContacts = new ObservableCollection<Contact>(contacts.AsQueryable().Where(predcate).OrderBy(a => a.Name).ToList());


            LastPredcate = predcate;
            //gd.Columns[gd.Columns.Count - 1].Visibility = Visibility.Hidden;
        }


        private void ContaListBoxMouseUp(object sender, MouseButtonEventArgs e)
        {
            //var btn = sender as Button;
            //var st = ((Panel)((btn.Content) as Panel).Children[0]);
            //var chk = st.Children.OfType<CheckBox>().First();
            //chk.IsChecked = !chk.IsChecked;
        }
        private void DoubleClickEditClick(object sender, MouseButtonEventArgs e)
        {
            var btn = sender as Button;
            var st = ((Panel)((btn.Content) as Panel).Children[0]);
            var chk = st.Children.OfType<TextBlock>().First();
            var id = long.Parse(chk.Text);
            var c = _vm.FilterdContacts.First(a => a.Id == id);
            ShoeNewSettle(WindowMode.Edit, c).Show();
        }



        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            var dres = MessageBox.Show("Are you sure?", "", MessageBoxButton.YesNo);
            if (dres != MessageBoxResult.Yes) return;

            var st = ((Panel)((Panel)((Control)sender).Parent).Parent);
            var chk = (st.Children.OfType<Panel>().First()).Children.OfType<TextBlock>().First();
            var id = long.Parse(chk.Text);
            var c = _vm.Contacts.First(a => a.Id == id);


            _vm.Delete(c);
            ComponyListBox.GetBindingExpression(ListBox.ItemsSourceProperty).UpdateTarget();

        }

        private void EditClick(object sender, RoutedEventArgs e)
        {
            var st = ((Panel)((Panel)((Control)sender).Parent).Parent);
            var chk = (st.Children.OfType<Panel>().First()).Children.OfType<TextBlock>().First();
            var id = long.Parse(chk.Text);
            var c = _vm.FilterdContacts.First(a => a.Id == id);
            //ManiGrid.Children.Add(ShoeNewSettle(WindowMode.Edit, c));
            ShoeNewSettle(WindowMode.Edit, c).Show();


        }


        private void CopyClick(object sender, RoutedEventArgs e)
        {
            var st = ((Panel)((Panel)((Control)sender).Parent).Parent);
            var chk = (st.Children.OfType<Panel>().First()).Children.OfType<TextBlock>().First();
            var id = long.Parse(chk.Text);
            _vm.CopyiedContact = _vm.FilterdContacts.First(a => a.Id == id);


            var ss = new view.ContactVw(_vm.CopyiedContact, _vm, WindowMode.New, this) { Margin = new Thickness(10, 71, 0, 0) };
            ss.SetValue(Grid.ColumnSpanProperty, 3);
            //            ManiGrid.Children.Add(ss);
            ss.Show();

        }


        private TextBlock lastone;
        private void ListBoxItemMouseDoubleClick1(object sender, RoutedEventArgs routedEventArgs)
        {
            //if (lastone != null)
            //{
            //    ((Button)lastone.Parent).Background = Brushes.Transparent;
            //    ((Button)lastone.Parent).Foreground = Brushes.Black;
            //    ((Button)lastone.Parent).FontWeight = FontWeights.Normal;

            //}
            //lastone = ((TextBlock)((Button)sender).Content);
            //((Button)lastone.Parent).Background = new SolidColorBrush(Color.FromArgb(255, 117, 160, 196));
            //((Button)lastone.Parent).Foreground = Brushes.Black;
            //((Button)lastone.Parent).FontWeight = FontWeights.Bold;

            var txt = (string)sender;// ((TextBlock)((Button)sender).Content).Text;
            if (txt == null) txt = StaticPublic.All;
            if (txt == StaticPublic.All)
                if (GroupNameTextBox.SelectedItem is null || GroupNameTextBox.SelectedItem.ToString() == StaticPublic.All)
                    OnlineSearchHappening(a => true);
                else
                    OnlineSearchHappening(a => a.GroupName == GroupNameTextBox.SelectedItem.ToString());
            else

                OnlineSearchHappening(a => a.Company.Trim() == txt.Trim());

        }


        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            GroupNameTextBox.SelectedIndex = 0;

        }

        private void GroupNameTextBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (GroupNameTextBox.SelectedItem.ToString() == StaticPublic.All)
                _vm.FilterdGroups = _vm.Contacts;
            else
                _vm.FilterdGroups = new ObservableCollection<Contact>(_vm.Contacts.Where(a => a.GroupName == GroupNameTextBox.SelectedItem.ToString()));

            ListBoxItemMouseDoubleClick1(StaticPublic.All, e);
        }

        private bool _filter = false;
        private void FilterOrNotClick(object sender, RoutedEventArgs e)
        {
            _filter = !_filter;

            OnlineSearchHappening(null, _filter ? _vm.FilterdContacts : null);
        }

        private void ExportClick(object sender, RoutedEventArgs e)
        {
            new  ContactExportToExcel().Export(_vm.FilterdContacts.ToList());
        }

        private void ComponyListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItemMouseDoubleClick1(((ListBox)sender).SelectedItem, e);
        }


        private Panel _selectedItem;
        private CheckBox _ch;
        private void GridGotFocus1(object sender, RoutedEventArgs e)
        {
            if (_selectedItem != null) _selectedItem.Background = new SolidColorBrush(Colors.White);

            var g = (Panel)sender;
            g.Background = new SolidColorBrush(Color.FromRgb(213, 218, 232));
            _selectedItem = g;


            var st = ((Panel)g.Children[0]);
            var chk = st.Children.OfType<CheckBox>().First();

            if (_ch != null && !Equals(_ch, chk)) _ch.IsChecked = false;


            //برای اینکه اگر چند بار پشت سر هم یک ایتم رو روش کلیک کرد 
            //هم اون ایتم مدام باز و بسته بشه و بشه جزئیات رو دید
            if (Equals(_ch, chk))
            {
                if ((bool)!chk.IsChecked)
                {
                    chk.IsChecked = true;
                    _selectedItem.Background = new SolidColorBrush(Color.FromRgb(213, 218, 232));
                    return;
                }
                chk.IsChecked = false;
                _selectedItem.Background = new SolidColorBrush(Colors.White);
                return;
            }

            _ch = chk;

            chk.IsChecked = !chk.IsChecked;
        }


    }
}
