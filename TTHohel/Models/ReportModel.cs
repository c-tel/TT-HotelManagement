using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Office.Interop.Word;
using TTHohel.Services;
using TTHotel.Contracts.Bookings;

namespace TTHohel.Models
{
    public class ReportModel
    {
        private static string[] Headers = new[] { "Номер кімнати", "Сума", "Тип оплати", "Час оплати" }; 

        internal List<ReportItem> GetReportItems(DateTime selectedDate)
        {
            return HotelApiClient.GetInstance().GetReport(selectedDate);
        }

        public void CreateDocument(IEnumerable<ReportItem> report, DateTime asOfDate)
        {
            Application winword = new Application();
            winword.ShowAnimation = false;
            winword.Visible = false;

            object missing = System.Reflection.Missing.Value;

            Document document = winword.Documents.Add(ref missing, ref missing, ref missing, ref missing);

            foreach (Section section in document.Sections)
            {
                Range headerRange = section.Headers[WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                headerRange.Fields.Add(headerRange, WdFieldType.wdFieldPage);
                headerRange.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                headerRange.Font.ColorIndex = WdColorIndex.wdBlue;
                headerRange.Font.Size = 25;
                headerRange.Text = $"Звіт за {asOfDate.ToString("dd-MM-yyyy")}";
            }

            Paragraph para1 = document.Content.Paragraphs.Add(ref missing);
            para1.Range.InsertParagraphAfter();

            Table firstTable = document.Tables.Add(para1.Range, report.Count(), 4, ref missing, ref missing);

            firstTable.Borders.Enable = 1;
            foreach (Row row in firstTable.Rows)
            {
                foreach (Cell cell in row.Cells)
                {
                    //Header row
                    if (cell.RowIndex == 1)
                    {
                        cell.Range.Text = "Column " + cell.ColumnIndex.ToString();
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
                        cell.Range.Text = (cell.RowIndex - 2 + cell.ColumnIndex).ToString();
                    }
                }
            }

            //Save the document
            document.Activate();
            document.Save();
            document.Close(ref missing, ref missing, ref missing);
            winword.Quit(ref missing, ref missing, ref missing);
        }
    }
}
