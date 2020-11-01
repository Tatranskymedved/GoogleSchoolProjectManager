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
        public static Request GenerateRepeatCellRequest(GRange range, GCell cell)
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

        public static Request GenerateRepeatCellRequest(GCoordinate start, GRowList rows)
        {
            return new Request()
            {
                UpdateCells = new UpdateCellsRequest()
                {
                    Rows = rows.GetRowDataList(),
                    Fields = rows.GetFields().GetFieldsString(),
                    Start = start.GetGridCoordinate(),
                }
            };
        }
    }

    public class GRowList : List<GRow>
    {
        public IList<RowData> GetRowDataList() => this.Select(a => a.GetRowData()).ToList();

        public GFields GetFields() => this.Aggregate(new GFields(), (result, item) =>
        {
            result.UnionWith(item.GetFields());
            return result;
        });
    }

    public class GRow : List<GCell>
    {
        public RowData GetRowData()
        {
            return new RowData()
            {
                Values = this.Select(a => a.GetCellData()).ToList()
            };
        }

        public GFields GetFields() => this.Aggregate(new GFields(), (result, item) =>
            {
                result.UnionWith(item.Fields);
                return result;
            });
    }

    public class GFields : HashSet<string>
    {
        public string GetFieldsString()
        {
            return string.Join(",", this);
        }

        public override string ToString()
        {
            return GetFieldsString();
        }
    }

    public class GCell
    {
        public GFields Fields { get; private set; } = new GFields();
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
            return Fields.GetFieldsString();
        }

        public GCell AddFormat(GCellFormat format, object value)
        {
            if (mCellFormat == null) mCellFormat = new CellFormat();

            switch (format)
            {
                case GCellFormat.WrapStrategy:
                    mCellFormat.WrapStrategy = value.ToString();
                    Fields.Add("userEnteredFormat.wrapStrategy");
                    break;
                case GCellFormat.TextRotationVertical:
                    if (mCellFormat.TextRotation == null) mCellFormat.TextRotation = new TextRotation();

                    mCellFormat.TextRotation.Vertical = bool.Parse(value.ToString());
                    Fields.Add("userEnteredFormat.textRotation.vertical");
                    break;
                case GCellFormat.TextRotationAngle:
                    if (mCellFormat.TextRotation == null) mCellFormat.TextRotation = new TextRotation();

                    mCellFormat.TextRotation.Angle = int.Parse(value.ToString());
                    Fields.Add("userEnteredFormat.textRotation.angle");
                    break;
                case GCellFormat.VerticalAlignment:
                    mCellFormat.VerticalAlignment = value.ToString();
                    Fields.Add("userEnteredFormat.verticalAlignment");
                    break;
                case GCellFormat.HorizontalAlignment:
                    mCellFormat.HorizontalAlignment = value.ToString();
                    Fields.Add("userEnteredFormat.horizontalAlignment");
                    break;
                case GCellFormat.TextFormatBold:
                    if (mCellFormat.TextFormat == null) mCellFormat.TextFormat = new TextFormat();

                    mCellFormat.TextFormat.Bold = bool.Parse(value.ToString());
                    Fields.Add("userEnteredFormat.textFormat.bold");
                    break;
                case GCellFormat.TextFormatFontSize:
                    if (mCellFormat.TextFormat == null) mCellFormat.TextFormat = new TextFormat();

                    mCellFormat.TextFormat.FontSize = int.Parse(value.ToString());
                    Fields.Add("userEnteredFormat.textFormat.fontSize");
                    break;
                case GCellFormat.BackgroundColorRed:
                    if (mCellFormat.BackgroundColor == null) mCellFormat.BackgroundColor = new Color();

                    mCellFormat.BackgroundColor.Red = float.Parse(value.ToString());
                    Fields.Add("userEnteredFormat.backgroundColor.red");
                    break;
                case GCellFormat.BackgroundColorGreen:
                    if (mCellFormat.BackgroundColor == null) mCellFormat.BackgroundColor = new Color();

                    mCellFormat.BackgroundColor.Green = float.Parse(value.ToString());
                    Fields.Add("userEnteredFormat.backgroundColor.green");
                    break;
                case GCellFormat.BackgroundColorBlue:
                    if (mCellFormat.BackgroundColor == null) mCellFormat.BackgroundColor = new Color();

                    mCellFormat.BackgroundColor.Blue = float.Parse(value.ToString());
                    Fields.Add("userEnteredFormat.backgroundColor.blue");
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
