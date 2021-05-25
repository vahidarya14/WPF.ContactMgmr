using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using A.WPF;
using DAL.Access;
using DAL.DataModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Serviecs;
using StaticPublic = Serviecs.StaticPublic;

namespace ContactMgr.viewModel
{
    public class ContactVm : Npc
    {
        private ObservableCollection<Contact> _filterdContacts;
        private ObservableCollection<Contact> _contacts;
        ObservableCollection<Contact> _filterdGroups;
        readonly ContactRepository _access;
        private Contact _copyiedContact;
        private ICommand _deleteClick;


        public ICommand CancelButtonClick { get; set; }
        public ICommand AddButtonClick { get; set; }
        public ICommand DeleteClick { get => _deleteClick; set { _deleteClick = value;OnPropertyChanged(); } }
        public ObservableCollection<Contact> Contacts { get => _contacts; set { _contacts = value; OnPropertyChanged(); } }
        public ObservableCollection<Contact> FilterdContacts { get => _filterdContacts; set { _filterdContacts = value; OnPropertyChanged(); } }
        public Contact CopyiedContact { get => _copyiedContact; set { _copyiedContact = value; OnPropertyChanged(); } }
        public ObservableCollection<Contact> FilterdGroups { get => _filterdGroups; set { _filterdGroups = value; OnPropertyChanged(); } }

        


        public ContactVm()
        {
            _contacts = new ObservableCollection<Contact>();
            _filterdContacts = new ObservableCollection<Contact>();
            _access = new ContactRepository();

            CancelButtonClick=new Cmd(CancelButtonOnClick);
            AddButtonClick=new Cmd(AddButtonOnClick);
            DeleteClick=new Cmd(Delete);
        }

        private void CancelButtonOnClick(object sender)
        {
            var cs = (List<object>)sender;
            var usc = (Window) cs[0];

            usc.Close();
        }

        private void AddButtonOnClick(object sender)
        {
            var paramss = (List<object>) sender;
            var windowMode = (WindowMode)paramss[0];
            var contact = (Contact)paramss[1];
            var usc = (UserControl)paramss[2];

            switch (windowMode)
            {
                case WindowMode.New:

                    Add(contact);

                    break;

                case WindowMode.Edit:


                    Edit(contact);

                    break;
            }


            ((Panel)(usc).Parent).Children.Remove(usc);
        }

        public static string ReadJson()
        {
            var serializer = new JsonSerializer();
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;
            if (!File.Exists(StaticPublic.JsonPath)) return "";
            using (var sw = new StreamReader(StaticPublic.JsonPath))
            {
                var xs = sw.ReadToEnd();
                return xs;
            }
            
        }

        public List<Contact> GetAll(Func<Contact, bool> predict = null)
        {
          
            var list = _access.SelectContacts();

            return predict == null ? list : list.Where(predict).ToList();
        }

        public int Add(Contact c)
        {
            var ss = _access.Add(c);

            Contacts.Add(c);
            
            return ss;
        }

        public int Delete(Contact c)
        {

            var dres = MessageBox.Show("Are you sure?", "", MessageBoxButton.YesNo);
            if (dres != MessageBoxResult.Yes) return 0;


            var ss = _access.Delete( c.Id);

            Contacts.Remove(c);
            FilterdContacts.Remove(c);

            return ss;
        }

        public void Delete(object c)
        {
            Delete((Contact) c);
        }

        public int Edit(Contact c)
        {
  
            _access.CommandText("UPDATE Contact SET " +
                    "[CName]='" + c.Name + "',[Title]='" + c.Title + "',[Mobile]='" + c.Mobile + "',[Company]='" + c.Company + "',[WebSite]='" + c.WebSite + "',[EMail]='" + c.EMail +
                    "',[Address]='" + c.Address + "',[GroupName]='" + c.GroupName + "',[Fax]='" + c.Fax + "',[Extenssion]='" + c.Extenssion + "',[Tel]='" + c.Tel + "',[Gender]='" + (int)c.Gender +
                    "',[Remark]='"+c.Remark+
                    "' WHERE [Id]="+c.Id);

            var ss = _access.CommandText("DELETE FROM ContactExtraFiled WHERE [Contactid]=" + c.Id);
            foreach (var extraField in c.ExtraFields)
            {
                ss = _access.CommandText("INSERT INTO ContactExtraFiled ([Key],[Value],[Contactid]) VALUES('" + extraField.Key + "','" + extraField.Value + "','" + c.Id + "')");
            }

            FilterdContacts = Contacts;

            return ss;
        }

      
    }


}
