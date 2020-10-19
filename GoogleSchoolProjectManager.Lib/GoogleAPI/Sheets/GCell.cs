using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSchoolProjectManager.Lib.GoogleAPI.Sheets
{
    public enum GCellFormat
    {
        WrapStrategy,
        TextRotationVertical,
        TextRotationAngle,
        VerticalAlignment,
        HorizontalAlignment,
        TextFormatBold,
        TextFormatFontSize,
        BackgroundColorRed,
        BackgroundColorGreen,
        BackgroundColorBlue,
    }
    public enum GCellValue
    {

    }

    public static class GCellRequestFactory
    {
        public static Request GenerateRepeactCellRequest(GRange range, GCell cell)
        {
            return new Request()
            {
                RepeatCell = new RepeatCellRequest()
                {
                    Cell = cell.GetCellData(),
                    Fields = cell.GetFields(),
                    Range = range.GetGridRange()
                }
            };
        }

        //public static Request GenerateUpdateCellRequest(GRange range, List<GCell> cell)
        //{
        //    return new Request()
        //    {
        //        UpdateCells = new UpdateCellsRequest()
        //        {
        //            Rows = cell.GetCellData(),
        //            Fields = cell.GetFields(),
        //            Range = range.GetGridRange()
        //        }
        //    };
        //}
    }

    public class GCell
    {
        private List<string> mFields = new List<string>();
        private CellFormat mCellFormat;
        private ExtendedValue mCellData;

        public CellData GetCellData()
        {
            var result = new CellData() { };
            if (this.mCellFormat != null) result.UserEnteredFormat = mCellFormat;
            if (this.mCellData != null) result.UserEnteredValue = mCellData;

            return result;
        }

        public string GetFields()
        {
            return string.Join(",", mFields);
        }

        public GCell AddFormat(GCellFormat format, object value)
        {
            if (mCellFormat == null) mCellFormat = new CellFormat();

            switch (format)
            {
                case GCellFormat.WrapStrategy:
                    mCellFormat.WrapStrategy = value.ToString();
                    mFields.Add("userEnteredFormat.wrapStrategy");
                    break;
                case GCellFormat.TextRotationVertical:
                    if (mCellFormat.TextRotation == null) mCellFormat.TextRotation = new TextRotation();

                    mCellFormat.TextRotation.Vertical = bool.Parse(value.ToString());
                    mFields.Add("userEnteredFormat.textRotation.vertical");
                    break;
                case GCellFormat.TextRotationAngle:
                    if (mCellFormat.TextRotation == null) mCellFormat.TextRotation = new TextRotation();

                    mCellFormat.TextRotation.Angle = int.Parse(value.ToString());
                    mFields.Add("userEnteredFormat.textRotation.angle");
                    break;
                case GCellFormat.VerticalAlignment:
                    mCellFormat.VerticalAlignment = value.ToString();
                    mFields.Add("userEnteredFormat.verticalAlignment");
                    break;
                case GCellFormat.HorizontalAlignment:
                    mCellFormat.HorizontalAlignment = value.ToString();
                    mFields.Add("userEnteredFormat.horizontalAlignment");
                    break;
                case GCellFormat.TextFormatBold:
                    if (mCellFormat.TextFormat == null) mCellFormat.TextFormat = new TextFormat();

                    mCellFormat.TextFormat.Bold = bool.Parse(value.ToString());
                    mFields.Add("userEnteredFormat.textFormat.bold");
                    break;
                case GCellFormat.TextFormatFontSize:
                    if (mCellFormat.TextFormat == null) mCellFormat.TextFormat = new TextFormat();

                    mCellFormat.TextFormat.FontSize = int.Parse(value.ToString());
                    mFields.Add("userEnteredFormat.textFormat.fontSize");
                    break;
                case GCellFormat.BackgroundColorRed:
                    if (mCellFormat.BackgroundColor == null) mCellFormat.BackgroundColor = new Color();

                    mCellFormat.BackgroundColor.Red = float.Parse(value.ToString());
                    mFields.Add("userEnteredFormat.backgroundColor.red");
                    break;
                case GCellFormat.BackgroundColorGreen:
                    if (mCellFormat.BackgroundColor == null) mCellFormat.BackgroundColor = new Color();

                    mCellFormat.BackgroundColor.Green = float.Parse(value.ToString());
                    mFields.Add("userEnteredFormat.backgroundColor.green");
                    break;
                case GCellFormat.BackgroundColorBlue:
                    if (mCellFormat.BackgroundColor == null) mCellFormat.BackgroundColor = new Color();

                    mCellFormat.BackgroundColor.Blue = float.Parse(value.ToString());
                    mFields.Add("userEnteredFormat.backgroundColor.blue");
                    break;
                default:
                    break;
            }

            return this;
        }

        public GCell AddValue()
        {
            if (mCellData == null) mCellData = new ExtendedValue();

            return this;
        }
    }
}
