using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;

namespace GoogleSchoolProjectManager.Lib.GoogleAPI.Sheets.POCOs
{
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

        public GCell AddValue(object value)
        {
            if (mCellData == null) mCellData = new ExtendedValue();

            var type = value?.GetType();
            Action<object, ExtendedValue, GFields> act;
            if (type != null && TypeGExtendedValueDict.TryGetValue(type, out act))
            {
                act.Invoke(value, mCellData, Fields);
            }
            else
            {
                mCellData.StringValue = "";
                Fields.Add("userEnteredValue.stringValue");
            }

            return this;
        }

        private static Dictionary<Type, Action<object, ExtendedValue, GFields>> TypeGExtendedValueDict = new Dictionary<Type, Action<object, ExtendedValue, GFields>>
        {
            { typeof(string), (val,result, fields)   => { result.StringValue = val.ToString(); fields.Add("userEnteredValue.stringValue"); } },

            { typeof(int), (val,result, fields)      => { result.NumberValue = Convert.ToDouble(val); fields.Add("userEnteredValue.numberValue"); } },
            { typeof(float), (val,result, fields)    => { result.NumberValue = Convert.ToDouble(val); fields.Add("userEnteredValue.numberValue"); } },
            { typeof(double), (val,result, fields)   => { result.NumberValue = Convert.ToDouble(val); fields.Add("userEnteredValue.numberValue"); } },
            { typeof(byte), (val,result, fields)     => { result.NumberValue = Convert.ToDouble(val); fields.Add("userEnteredValue.numberValue"); } },
            { typeof(DateTime), (val,result, fields) => { result.NumberValue = Convert.ToDouble(val); fields.Add("userEnteredValue.numberValue"); } },

            { typeof(bool), (val,result, fields)     => { result.BoolValue = Convert.ToBoolean(val); fields.Add("userEnteredValue.boolValue"); } },
        };
    }
}
