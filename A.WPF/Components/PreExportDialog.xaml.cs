using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using A.Extenssions;
using Serviecs;
using Serviecs.EXporter;
using StaticPublic = Serviecs.StaticPublic;


namespace A.WPF.Components
{

    public partial class PreExportDialog
    {
        
        private readonly DataGrid _dg;
        public PreExportDialog(DataGrid dg)
        {
            InitializeComponent();

            _dg = dg;

             var cols = _dg.Columns.Select(a => a.Header).ToList();
            for(var i=0;i< cols.Count;i++ )
            {
                var s = ((DataGridTextColumn)_dg.Columns[i]).Binding;
                var dd = ((Binding) s).Path.Path;

                pnl.Children.Add(new CheckBox { Content = cols[i], IsChecked = true, Name = dd, Margin = new Thickness(10, 10, 0, 0) });
            }

            var btn = new Button {Content = "Ok",Margin = new Thickness(0,20,0,0),Width = 150};
            btn.Click += (sender, e) =>
                             {
                                 
                                 var data = new List<string>();
                                 var chks = pnl.Children.OfType<CheckBox>().Where(a => a.IsChecked == true && a.Name != "").Select(a => a.Name).ToList();
                                 var titles = pnl.Children.OfType<CheckBox>().Where(a => a.IsChecked == true && a.Name != "").Select(a => a.Content.ToString()).ToList();

                                 pnl.Children.Clear();
                                 pnl.Children.Add(new TextBlock {Text = "Please Wait ..."});

                                 data.AddRange(titles);
                                 data.Add(StaticPublic.PrintSeprator);
                                 var ss = dg.Items;
                                 foreach (var s in ss)
                                 {
                                     foreach (var chk in chks)
                                     {
                                         var v = s.GetType().GetProperty(chk).GetValue(s);
                                         data.Add(v!=null?chk=="Fee"?v.ToString().MonyShow():v.ToString():"");
                                     }
                                     data.Add(StaticPublic.PrintSeprator);
                                 }

                                 new ExportToExcel().Export(data);
                               Close();
                             };
            pnl.Children.Add(btn);


            var btn2 = new Button { Content = "Print", Margin = new Thickness(0, 20, 0, 0), Width = 150 };
            btn2.Click += (sender, e) =>
            {

                var data = new List<string>();
                var chks = pnl.Children.OfType<CheckBox>().Where(a => a.IsChecked == true && a.Name != "").Select(a => a.Name).ToList();
                var titles = pnl.Children.OfType<CheckBox>().Where(a => a.IsChecked == true && a.Name != "").Select(a => a.Content.ToString().Replace(" ","\n\r")).ToList();

                pnl.Children.Clear();
                pnl.Children.Add(new TextBlock { Text = "Please Wait ..." });

                data.AddRange(titles);
                data.Add(StaticPublic.PrintSeprator);
                var ss = dg.Items;
                foreach (var s in ss)
                {
                    foreach (var chk in chks)
                    {
                        var v = s.GetType().GetProperty(chk).GetValue(s);
                        data.Add(v != null ? chk == "Fee" ? v.ToString().MonyShow() : v.ToString() : "");
                    }
                    data.Add(StaticPublic.PrintSeprator);
                }

                new ExportToExcel().Print(data);
                Close();
            };
            pnl.Children.Add(btn2);
        }



        public static string GetName(DependencyObject obj)
        {
            return (string)obj.GetValue(NameProperty);
        }

        public PreExportDialog(List<string> cols)
        {
            InitializeComponent();

            foreach (var col in cols)
            {
                pnl.Children.Add(new CheckBox { Content = col, IsChecked = true });
            }
        }

        private void WindowLoaded1(object sender, RoutedEventArgs e)
        {
           
        }
    }
}
