﻿using System;
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
using Serviecs.DAL.Access;
using StaticPublic = Serviecs.StaticPublic;

namespace ContactMgr.viewModel
{
    public class ContactVm : Npc
    {
        private ObservableCollection<Contact> _filterdContacts;
        private ObservableCollection<Contact> _contacts;
        ObservableCollection<Contact> _filterdGroups;
        readonly DbAppContext _access;
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
            _access = new DbAppContext();

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


            ((Panel)usc.Parent).Children.Remove(usc);
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
          
            var list = _access.Contacts.Select();

            return predict == null ? list : list.Where(predict).ToList();
        }

        public int Add(Contact c)
        {
            var ss = _access.Contacts.Add(c);

            Contacts.Add(c);
            
            return ss;
        }

        public int Delete(Contact c)
        {

            var dres = MessageBox.Show("Are you sure?", "", MessageBoxButton.YesNo);
            if (dres != MessageBoxResult.Yes) return 0;


            var ss = _access.Contacts.Delete( c.Id);

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
  
           var ss= _access.Contacts.Update(c);

            FilterdContacts = Contacts;

            return ss;
        }

      
    }


}
