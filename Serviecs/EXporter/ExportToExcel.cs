using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Windows.Forms;
using Spire.Xls;

namespace Serviecs.EXporter
{
    public class ExportToExcel
    {
        //public void Export(List<Settled> data)
        //{
        //    var sv = new SaveFileDialog();
        //    if (sv.ShowDialog() != DialogResult.OK) return;

        //    var manager = new Workbook();
        //    var sheet = manager.CreateEmptySheet("myname");


        //    var currentRowIndex = 2;
        //    const int currentColIndex = 1;


        //    var no = 1;
        //    foreach (Settled item in data)
        //    {

        //        sheet[currentRowIndex, currentColIndex].Value = (no++).ToString(CultureInfo.InvariantCulture);
        //        sheet[currentRowIndex, currentColIndex + 1].Value = item.RefNo;
        //        sheet[currentRowIndex, currentColIndex + 2].Value = item.CertificateNo;
        //        sheet[currentRowIndex, currentColIndex + 3].Value = item.Fee.ToString(CultureInfo.InvariantCulture).MonyShow();
        //        sheet[currentRowIndex, currentColIndex + 4].Value = item.FeeUnit.ToString();
        //        sheet[currentRowIndex, currentColIndex + 5].Value = item.DeliveryStatus.ToString();
        //        sheet[currentRowIndex, currentColIndex + 6].Value = item.Description;

        //        //sheet[currentRowIndex, currentColIndex, currentRowIndex, currentColIndex + 16].Borders.LineStyle = LineStyleType.Thin;
        //        //sheet[currentRowIndex, currentColIndex, currentRowIndex, currentColIndex + 16].Borders[
        //        //    BordersLineType.DiagonalDown].LineStyle = LineStyleType.None;
        //        //sheet[currentRowIndex, currentColIndex, currentRowIndex, currentColIndex + 16].Borders[
        //        //    BordersLineType.DiagonalUp].LineStyle = LineStyleType.None;

        //        currentRowIndex++;

        //    }
        //    for (int i = 1; i <= 3; i++)
        //        try { manager.Worksheets["Sheet" + i].Remove(); }
        //        catch { }


        //    manager.SaveToFile(sv.FileName + ".xls", ExcelVersion.Version97to2003);
        //    //Process.Start(sv.FileName + ".xls");

        //}

        public void Export(List<string> data)
        {
            var sv = new SaveFileDialog();
            if (sv.ShowDialog() != DialogResult.OK) return;

            var manager = new Workbook();
            var sheet = manager.CreateEmptySheet("myname");

            const int startingRowIndex = 1;
            const int startingColIndex = 1;

            var currentRowIndex = startingRowIndex;
            int currentColIndex = startingColIndex;


            var no = 1;
            sheet[currentRowIndex, currentColIndex].Value = "#";
            foreach (var item in data)
            {
                if (item == StaticPublic.PrintSeprator)
                {
                    currentColIndex = startingColIndex;
                    currentRowIndex++;
                    sheet[currentRowIndex, currentColIndex].Value = (no++).ToString(CultureInfo.InvariantCulture);
                    continue;
                }
                currentColIndex++;
                sheet[currentRowIndex, currentColIndex].Value = item;


            }

            MakeHeader(sheet, startingRowIndex, currentColIndex, currentRowIndex);

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

        protected static void MakeHeader(Worksheet sheet, int startingRowIndex, int currentColIndex, int currentRowIndex)
        {
            sheet[startingRowIndex, currentColIndex, startingRowIndex, currentColIndex + 12].Style.Color = Color.Yellow;
            sheet[currentRowIndex, currentColIndex, currentRowIndex, currentColIndex + 12].Borders.LineStyle = LineStyleType.Thin;
            sheet[currentRowIndex, currentColIndex, currentRowIndex, currentColIndex + 12].Borders[BordersLineType.DiagonalDown].LineStyle = LineStyleType.None;
            sheet[currentRowIndex, currentColIndex, currentRowIndex, currentColIndex + 12].Borders[BordersLineType.DiagonalUp].LineStyle = LineStyleType.None;
        }

        private List<string> _data;
        public void Print(List<string> data)
        {
            _data = data;
            var dlgPrint = new PrintDialog();
            var docPrint = new PrintDocument();
            docPrint.PrintPage += DocPrintPrintPage;
            docPrint.DefaultPageSettings = new PageSettings { Margins = new Margins(50, 50, 50, 50) };
            dlgPrint.Document = docPrint;
            if (dlgPrint.ShowDialog() == DialogResult.OK)
                docPrint.Print();
        }

        private int _lastIndex;
        private int _pageIndex;
        private const int RowPerPage = 40;
        private void DocPrintPrintPage(object sender, PrintPageEventArgs e)
        {
            _pageIndex++;

            var t = _data.IndexOf(StaticPublic.PrintSeprator) + 1;
            var sss = _data.Count / t;
            var pageAmount = (sss / RowPerPage);
            e.HasMorePages = (_pageIndex <= pageAmount);
            const int top = 150;
            const int leftPadding = 50;
            const int normalWidth = 780;


            var contextFont = new Font("Times New Roman", 10, FontStyle.Regular);
            var titletFont = new Font("Times New Roman", 10, FontStyle.Bold);

            string strDisplay = "Archive List Record";
            var fntString = new Font("Times New Roman", 28, FontStyle.Bold);
            e.Graphics.DrawString(strDisplay, fntString, Brushes.Black, 280, top - 100);

            strDisplay = "Loan Evaluation";
            fntString = new Font("Times New Roman", 18, FontStyle.Bold);
            e.Graphics.DrawString(strDisplay, fntString, Brushes.Black, 320, top - 50);

            e.Graphics.DrawLine(new Pen(Color.Black, 2), leftPadding, top, normalWidth, top);
            e.Graphics.DrawLine(new Pen(Color.Black, 1), leftPadding, top + 3, normalWidth, top + 3);

            var left = leftPadding;
            var lefts = new List<int>();
            for (var i = 0; i < _data.Count; i++)
            {
                if (i > 0 && _data[i - 1].ToLower() == "client") left += 150;
                if (_data[i] == StaticPublic.PrintSeprator) break;
                e.Graphics.DrawString(_data[i], titletFont, Brushes.Black, left, top + 5);
                lefts.Add(left);
                left += 80;

            }

            e.Graphics.DrawLine(new Pen(Color.Black, 1), leftPadding, top + 37, normalWidth, top + 37);
            e.Graphics.DrawLine(new Pen(Color.Black, 2), leftPadding, top + 40, normalWidth, top + 40);

            var y = top + 45;
            var x = 0;
            bool notyet = (_pageIndex > 1);
            int rowcout = 0;
            for (; _lastIndex < _data.Count; _lastIndex++)
            {
                if (rowcout > RowPerPage) break;

                if (_data[_lastIndex] == StaticPublic.PrintSeprator)
                {
                    if (notyet)
                    {
                        y += 17;
                        //e.Graphics.DrawLine(new Pen(Color.Gray, 1), leftPadding, y, normalWidth, y);
                        y += 5;
                        x = 0;
                    }
                    notyet = true;
                    if (rowcout % 2 == 1)
                        e.Graphics.DrawRectangle(new Pen(Color.LightGray, 13), leftPadding, y + 2, normalWidth, 13);
                    else
                        e.Graphics.DrawRectangle(new Pen(Color.YellowGreen, 13), leftPadding, y + 2, normalWidth, 13);

                    rowcout++;

                    continue;
                }
                if (!notyet) continue;

                e.Graphics.DrawString(_data[_lastIndex], contextFont, Brushes.Black, lefts[x], y/*,new StringFormat(StringFormatFlags.DirectionRightToLeft)*/);
                x++;


            }

            for (int i = 1; i < lefts.Count; i++)
                e.Graphics.DrawLine(new Pen(Color.FromArgb(205, 205, 205), 1), lefts[i] - 2, 191, lefts[i] - 2, 1070);


        }

     

    }
}
