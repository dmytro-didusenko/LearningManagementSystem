using System.Drawing;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace LearningManagementSystem.Core.Helpers
{
    public static class ExcelStyleHelper
    {
        public static void AddStyles(ExcelRange cells, List<SuccessReportStyles> styles)
        {
            foreach (var style in styles)
            {
                if (style is SuccessReportStyles.HeaderStyling)
                {
                    cells.Style.Font.Size = 15;
                    cells.Style.Font.Bold = true;
                    cells.Style.Font.Color.SetColor(Color.White);
                    cells.AutoFitColumns();
                    cells.Style.Fill.SetBackground(Color.DodgerBlue);
                }

                if (style is SuccessReportStyles.SubjectStyling)
                {
                    cells.Style.Font.Bold = true;
                    cells.Style.Font.Size = 13;
                    cells.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    cells.Style.Fill.SetBackground(Color.MediumSeaGreen);
                    cells.AutoFitColumns();
                    cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                }

                if (style is SuccessReportStyles.TopicStyling)
                {
                    cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                    cells.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    cells.Style.Fill.SetBackground(Color.PaleGreen);
                    cells.Style.Font.Italic = true;
                    cells.AutoFitColumns();
                }

                if (style is SuccessReportStyles.CenteringAndBorderThinStyling)
                {
                    cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                    cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    cells.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                }
            }
        }
    }

    public enum SuccessReportStyles
    {
        HeaderStyling,
        SubjectStyling,
        TopicStyling,
        CenteringAndBorderThinStyling,
    }
}
