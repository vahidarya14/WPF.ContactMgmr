using System.Collections.Generic;
using System.Linq;
using System.Windows;
using A.Extenssions;
using A.WPF;
using ContactMgr.viewModel;
using DAL.DataModel;
using Serviecs;
using Field = ContactMgr.Components.Field;

namespace ContactMgr.view
{
  

    public partial class ContactVw : Window
    {
        private readonly WindowMode _windowMode;
        private readonly Contact _contact;
        private readonly ContactVm _vm;
        private MainWindow _contactwindow;

        public ContactVw(ContactVm vm, MainWindow contactwindow)
        {
            InitializeComponent();
            foreach (var s in new[] { "Company/Organization", "Mr", "Mrs" }) GenderCombo.Items.Add(s);

            NameTextBox.Focus();

            _windowMode = WindowMode.New;

            _vm = vm;
            _contactwindow = contactwindow;
            DataContext = _vm;



        }

        public ContactVw(Contact contact, ContactVm vm, WindowMode windowMode, MainWindow contactwindow)
        {
            InitializeComponent();
            foreach (var s in new[] { "Company/Organization", "Mr", "Mrs" }) GenderCombo.Items.Add(s);


            _windowMode = windowMode;
            _contact = contact;
            _vm = vm;
            _contactwindow = contactwindow;
            DataContext = _vm;

            NameTextBox.Text = _contact.Name;
            TitleTextBox.Text = _contact.Title;
            MobileTextBox.Text = _contact.Mobile;
            WebSiteTextBox.Text = _contact.WebSite;
            MailTextBox.Text = _contact.EMail;
            CompanyTextBox.Text = _contact.Company;
            AdressTextBox.Text = _contact.Address;
            GroupNameTextBox.Text = _contact.GroupName;
            FaxTextBox.Text = _contact.Fax;
            TellTextBox.Text = _contact.Tel;
            DomesticTextBox.Text = _contact.Extenssion;
            GenderCombo.SelectedIndex = (int)_contact.Gender;
            RemarkTextBox.Text = _contact.Remark;


            foreach (var f in _contact.ExtraFields)
            {
                 var ss =  MakeField();
              //G1.Children.OfType<Field>().Last();
                ss.Title = f.Key;
                ss.Value = f.Value;
                ss.HiddenValue = f.Id.ToString();
            }

            if (windowMode == WindowMode.Edit)
                AddEditButton.Content = "Save";
            else
            {
                _vm.CopyiedContact = null;
            }
        }

        private void AddButtonOnClick(object sender, RoutedEventArgs e)
        {
            switch (_windowMode)
            {
                case WindowMode.New:
                    var c = new Contact
                    {
                        Id = _vm.Contacts.Count > 0 ? _vm.Contacts.Max(a => a.Id) + 1 : 1,
                        Name = NameTextBox.Text,
                        Title = TitleTextBox.Text,
                        Mobile = MobileTextBox.Text,
                        Company = CompanyTextBox.Text,
                        Address = AdressTextBox.Text,
                        WebSite = WebSiteTextBox.Text,
                        EMail = MailTextBox.Text,
                        GroupName = GroupNameTextBox.Text,
                        Fax = FaxTextBox.Text,
                        Tel = TellTextBox.Text,
                        Extenssion = DomesticTextBox.Text,
                        Gender = (Gender)GenderCombo.SelectedIndex,
                        Remark = RemarkTextBox.Text
                    };
                    c.ExtraFields.AddRange(G1.Children.OfType<Field>().Where(a => a.Title != "" && a.Value != "").Select(a => new ExtraField { Key = a.Title, Value = a.Value }).ToList());


                    _vm.Add(c);



                    break;

                case WindowMode.Edit:

                    _contact.Name = NameTextBox.Text;
                    _contact.Title = TitleTextBox.Text;
                    _contact.Mobile = MobileTextBox.Text;
                    _contact.Company = CompanyTextBox.Text;
                    _contact.Address = AdressTextBox.Text;
                    _contact.WebSite = WebSiteTextBox.Text;
                    _contact.EMail = MailTextBox.Text;
                    _contact.GroupName = GroupNameTextBox.Text;
                    _contact.Fax = FaxTextBox.Text;
                    _contact.Tel = TellTextBox.Text;
                    _contact.Extenssion = DomesticTextBox.Text;
                    _contact.ExtraFields = G1.Children.OfType<Field>().Where(a => a.Title != "" && a.Value != "").Select(a => new ExtraField {Id=long.Parse(a.HiddenValue), Key = a.Title, Value = a.Value }).ToList();
                    _contact.Gender = (Gender)GenderCombo.SelectedIndex;
                    _contact.Remark = RemarkTextBox.Text;

                    _vm.Edit(_contact);
                    break;
            }

            _contactwindow.OnlineSearchHappening(_contactwindow.LastPredcate);

            ////_contactwindow.ComponyListBox.GetBindingExpression(ListBox.ItemsSourceProperty).UpdateTarget();
            // var parent = (Panel)Parent;
            // parent.Children.Remove(this);
            Close();
        }


        private int _lasttop = 0;
        private void ButtonClick1(object sender, RoutedEventArgs e)
        {
            var newFiled = MakeField();
            // new ChangeTitle(newFiled.lbl){Top = newFiled.Margin.Top+Margin.Top,Left = newFiled.Margin.Left}.ShowDialog();

        }

        private Field MakeField()
        {
            Height += 35;
            G1. Height += 35;

            var newFiled = new Field
            {
                Margin = new Thickness(0, _lasttop, 0, 0),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                titleTxt = { Visibility = _windowMode == WindowMode.New ? Visibility.Visible : Visibility.Hidden },
                
            };
            newFiled.titleTxt.Text = newFiled.lbl.Text;
            newFiled.titleTxt.SelectAll();
            G1.Children.Add(newFiled);
            _lasttop += 35;

            return newFiled;
        }

        private void UserControlLoaded1(object sender, RoutedEventArgs e)
        {
            EventToCommandBind.Bind(CancelButton, "Click", _vm.CancelButtonClick, new List<object> { this }, true);
            NameTextBox.Focus();

        }

    }
}
