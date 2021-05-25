using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;
using A.Extenssions;
using DAL.DataModel;
using Spire.Xls;

namespace Serviecs.EXporter
{
   public class ContactExportToExcel : ExportToExcel
    {
        public void Export(List<Contact> data)
        {
            var sv = new SaveFileDialog();
            if (sv.ShowDialog() != DialogResult.OK) return;

            var manager = new Workbook();
            var sheet = manager.CreateEmptySheet("Contacts");


            var currentRowIndex = 1;
            const int currentColIndex = 1;

            sheet[currentRowIndex, currentColIndex].Value = "#";
            sheet[currentRowIndex, currentColIndex + 1].Value = "Name & Family";
            sheet[currentRowIndex, currentColIndex + 2].Value = "Title";
            sheet[currentRowIndex, currentColIndex + 3].Value = "Mobile";
            sheet[currentRowIndex, currentColIndex + 4].Value = "Company";
            sheet[currentRowIndex, currentColIndex + 5].Value = "WebSite";
            sheet[currentRowIndex, currentColIndex + 6].Value = "EMail";
            sheet[currentRowIndex, currentColIndex + 7].Value = "Address";
            sheet[currentRowIndex, currentColIndex + 8].Value = "GroupName";
            sheet[currentRowIndex, currentColIndex + 9].Value = "Fax";
            sheet[currentRowIndex, currentColIndex + 10].Value = "Extenssion";
            sheet[currentRowIndex, currentColIndex + 11].Value = "Tel";
            sheet[currentRowIndex, currentColIndex + 10].Value = "Gender";
            sheet[currentRowIndex, currentColIndex + 11].Value = "Extra Fields";
            MakeHeader(sheet, currentRowIndex, currentColIndex, currentRowIndex);

            currentRowIndex++;
            var no = 1;
            foreach (Contact item in data)
            {

                sheet[currentRowIndex, currentColIndex].Value = (no++).ToString(CultureInfo.InvariantCulture);
                sheet[currentRowIndex, currentColIndex + 1].Value = item.Name;
                sheet[currentRowIndex, currentColIndex + 2].Value = item.Title;
                sheet[currentRowIndex, currentColIndex + 3].Value = item.Mobile;
                sheet[currentRowIndex, currentColIndex + 4].Value = item.Company;
                sheet[currentRowIndex, currentColIndex + 5].Value = item.WebSite;
                sheet[currentRowIndex, currentColIndex + 6].Value = item.EMail;
                sheet[currentRowIndex, currentColIndex + 7].Value = item.Address;
                sheet[currentRowIndex, currentColIndex + 8].Value = item.GroupName;
                sheet[currentRowIndex, currentColIndex + 9].Value = item.Fax;
                sheet[currentRowIndex, currentColIndex + 10].Value = item.Extenssion;
                sheet[currentRowIndex, currentColIndex + 11].Value = item.Tel;
                sheet[currentRowIndex, currentColIndex + 10].Value = item.Gender.ToFarsi();

                var ss = "";
                foreach (var extraField in item.ExtraFields)
                {
                    ss += extraField.Key + ":" + extraField.Value + "\r\n";
                }
                sheet[currentRowIndex, currentColIndex + 11].Value = ss;

                currentRowIndex++;

            }
            for (int i = 1; i <= 3; i++)
                try { manager.Worksheets["Sheet" + i].Remove(); }
                catch { }


            manager.SaveToFile(sv.FileName + ".xls", ExcelVersion.Version97to2003);
            try { Process.Start(sv.FileName + ".xls"); }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}