using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.Office.Interop.Word;
using TTHohel.Manager;
using TTHohel.Services;
using TTHotel.Contracts.Bookings;
using TTHotel.Contracts.Payments;

namespace TTHohel.Models
{
    public class ReportModel
    {
        private static string[] Headers = new[] { "Номер кімнати", "Сума", "Тип оплати", "Час оплати" }; 

        internal List<ReportItem> GetReportItems(DateTime selectedDate)
        {
            return HotelApiClient.GetInstance().GetReport(selectedDate);
        }

        public void CreateDocument(IEnumerable<ReportItemModel> report, DateTime asOfDate)
        {
            try
            {
               System.Threading.Tasks.Task.Run( () => CreateDocumentInternal(report, asOfDate));
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void CreateDocumentInternal(IEnumerable<ReportItemModel> report, DateTime asOfDate)
        {
            var reportItemsList = report.ToList();
            var winword = new Microsoft.Office.Interop.Word.Application();
            winword.ShowAnimation = false;

            object missing = System.Reflection.Missing.Value;
            winword.Visible = false;

            var document = winword.Documents.Add(ref missing, ref missing, ref missing, ref missing);

            foreach (Section section in document.Sections)
            {
                Range headerRange = section.Headers[WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                headerRange.Fields.Add(headerRange, WdFieldType.wdFieldPage);
                headerRange.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                headerRange.Font.ColorIndex = WdColorIndex.wdBlue;
                headerRange.Font.Size = 25;
                headerRange.Text = $"Звіт за {asOfDate.ToString("dd-MM-yyyy")}";
            }

            var para1 = document.Content.Paragraphs.Add(ref missing);
            para1.Range.InsertParagraphAfter();
            para1.Range.Text = "!!!!!!!!!!!!!!!!";

            Table firstTable = document.Tables.Add(para1.Range, reportItemsList.Count+1, 4, ref missing, ref missing);

            firstTable.Borders.Enable = 1;
            foreach (Row row in firstTable.Rows)
            {
                foreach (Cell cell in row.Cells)
                {
                    //Header row
                    if (cell.RowIndex == 1)
                    {
                        cell.Range.Text = Headers[cell.ColumnIndex - 1];
                        cell.Range.Font.Bold = 1;
                        //other format properties goes here
                        cell.Range.Font.Name = "verdana";
                        cell.Range.Font.Size = 10;
                        //cell.Range.Font.ColorIndex = WdColorIndex.wdGray25;                            
                        cell.Shading.BackgroundPatternColor = WdColor.wdColorGray25;
                        //Center alignment for the Header cells
                        cell.VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                        cell.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;

                    }
                    //Data row
                    else
                    {
                        var currItem = reportItemsList[cell.RowIndex - 2];
                        string text = null;
                        switch(cell.ColumnIndex)
                        {
                            case 1:
                                text = currItem.RoomNum.ToString();
                                break;
                            case 2:
                                text = currItem.Amount.ToString();
                                break;
                            case 3:
                                text = currItem.PaymentType;
                                break;
                            case 4:
                                text = currItem.PaymentTime;
                                break;
                        }
                        cell.Range.Text = text;
                    }
                }
            }
            var summaryCash = firstTable.Rows.Add();
            summaryCash.Cells.Merge();
            summaryCash.Range.Text = $"Всього готівкою - {reportItemsList.Where(i => i.PaymentType == "Готівкою").Sum(i => i.Amount)} грн.";
            var summaryCard = firstTable.Rows.Add();
            summaryCard.Cells.Merge();
            summaryCard.Range.Text = $"Всього карткою - {reportItemsList.Where(i => i.PaymentType == "Карткою").Sum(i => i.Amount)} грн";
            var summary = firstTable.Rows.Add();
            summary.Cells.Merge();
            summary.Range.Text = $"Всього - {reportItemsList.Sum(i => i.Amount)} грн";
            summaryCash.Cells[0].Alignment = WdRowAlignment.wdAlignRowRight;
            summary.Alignment = WdRowAlignment.wdAlignRowRight;
            summaryCard.Alignment = WdRowAlignment.wdAlignRowRight;


            //Save the document
            document.Activate();
            try
            {
                winword.Visible = true;
                document.Close(ref missing, ref missing, ref missing);
            }
            catch (Exception)
            {

            }
            winword.Quit(ref missing, ref missing, ref missing);
        }

        public void Back()
        {
            NavigationManager.Instance.Navigate(ModesEnum.Main);
        }
    }
}
