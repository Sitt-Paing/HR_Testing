using System.Reflection;
using OfficeOpenXml;

//install EPPlus package using dotnet cli and declare it in appsettings.json
namespace Hr_Testing.Service
{
    public class ExportService
    {
        private const string DefaultFontFamily = "Pyidaungsu";
        public Stream? ExportToExcelStreamSpecificColumns<T>(
        List<T>? list,
        KeyValuePair<string, string>[] columns,
        string sheetName)
        {

            using ExcelPackage package = new();
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(sheetName);
            worksheet.Cells.Style.Font.Name = DefaultFontFamily;

            if (list == null || list.Count == 0)
            {
                return default;
            }

            AddHeadersAndDataToWorksheetSpecificColumn(list, worksheet, columns, true);

            worksheet.Cells.Style.WrapText = false;
            worksheet.Cells.AutoFitColumns();

            MemoryStream stream = new();
            package.SaveAs(stream);
            stream.Position = 0;
            return stream;
        }

        private static void AddHeadersAndDataToWorksheet<T>(
        List<T> list,
        ExcelWorksheet worksheet,
        bool formatCells = false)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();

            for (int i = 0; i < properties.Length; i++)
            {
                var cell = worksheet.Cells[1, i + 1];
                cell.Value = properties[i].Name;
                cell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#c5dcfb"));
                cell.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
            }

            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < properties.Length; j++)
                {
                    object? value = properties[j].GetValue(list[i]);
                    if (value == null) continue;

                    if (formatCells)
                    {
                        SetCellFormat(worksheet.Cells[i + 2, j + 1], value);
                    }

                    worksheet.Cells[i + 2, j + 1].Value = value;
                }
            }
        }

        private static void AddHeadersAndDataToWorksheetSpecificColumn<T>(
            List<T> list,
            ExcelWorksheet worksheet,
            KeyValuePair<string, string>[] columns,
            bool formatCells = false)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            string[] columnNames = columns.Select(x => x.Key).ToArray();
            if (columnNames.Length > 0)
            {
                properties = typeof(T).GetProperties()
                    .Where(p => columnNames.Contains(p.Name))
                    .ToArray();
            }

            for (int i = 0; i < properties.Length; i++)
            {
                var cell = worksheet.Cells[1, i + 1];
                cell.Value = columns.FirstOrDefault(x => x.Key == properties[i].Name).Value;
                cell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#c5dcfb"));
                cell.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
            }

            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < properties.Length; j++)
                {
                    object? value = properties[j].GetValue(list[i]);
                    if (value == null) continue;

                    if (formatCells)
                    {
                        SetCellFormat(worksheet.Cells[i + 2, j + 1], value);
                    }

                    worksheet.Cells[i + 2, j + 1].Value = value;
                }
            }
        }

        private static void SetCellFormat(ExcelRange cell, object value)
        {
            if (value is DateTime)
            {
                cell.Style.Numberformat.Format = "dd-mmm-yyyy h:mm AM/PM";
            }
            else if (value is DateOnly)
            {
                cell.Style.Numberformat.Format = "dd-mmm-yyyy";
            }
            else if (value is TimeOnly)
            {
                cell.Style.Numberformat.Format = "h:mm AM/PM";
            }
            else if (value is int || value is double || value is long)
            {
                cell.Style.Numberformat.Format = "#,##0";
            }
        }
    }
}
