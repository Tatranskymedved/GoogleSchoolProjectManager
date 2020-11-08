using Google.Apis.Sheets.v4.Data;
using GoogleSchoolProjectManager.Lib.GoogleAPI.Sheets.POCOs;
using System.Collections.Generic;

namespace GoogleSchoolProjectManager.Lib.GoogleAPI.Sheets.Requests
{
    public enum GUpdateBordersType
    {
        InnerRows = 0,
        InnerColumns = 1,
        Outer = 2
    }

    public enum GBorderLineType
    {
        Dotted = 0,
        Dashed = 1,
        SolidThick = 2,
        SolidMedium = 3,
        Solid = 4,
        None = 5,
        Double = 6
    }

    public static class GUpdateBordersFactory
    {
        public static Request GenerateRequest(GUpdateBordersType type, GBorderLineType lineType, GRange range)
        {
            UpdateBordersRequest request;
            switch (type)
            {
                case GUpdateBordersType.InnerRows:
                    request = GetInnerRows(range, lineType);
                    break;
                case GUpdateBordersType.InnerColumns:
                    request = GetInnerCols(range, lineType);
                    break;
                case GUpdateBordersType.Outer:
                default:
                    request = GetOuterBorders(range, lineType);
                    break;
            }

            return new Request()
            {
                UpdateBorders = request
            };
        }

        public static UpdateBordersRequest GetOuterBorders(GRange range, GBorderLineType lineType)
        {
            return new UpdateBordersRequest()
            {
                Bottom = BorderDictionary[lineType],
                Left = BorderDictionary[lineType],
                Right = BorderDictionary[lineType],
                Top = BorderDictionary[lineType],
                Range = range.GetGridRange()
            };
        }

        public static UpdateBordersRequest GetInnerRows(GRange range, GBorderLineType lineType)
        {
            return new UpdateBordersRequest()
            {
                InnerHorizontal = BorderDictionary[lineType],
                Range = range.GetGridRange()
            };
        }

        public static UpdateBordersRequest GetInnerCols(GRange range, GBorderLineType lineType)
        {
            return new UpdateBordersRequest()
            {
                InnerHorizontal = BorderDictionary[lineType],
                Range = range.GetGridRange()
            };
        }

        //Border styles:
        //
        //STYLE_UNSPECIFIED     The style is not specified. Do not use this.
        //DOTTED                The border is dotted.
        //DASHED                The border is dashed.
        //SOLID                 The border is a thin solid line.
        //SOLID_MEDIUM          The border is a medium solid line.
        //SOLID_THICK           The border is a thick solid line.
        //NONE                  No border. Used only when updating a border in order to erase it.
        //DOUBLE                The border is two solid lines.
        private static Dictionary<GBorderLineType, Border> BorderDictionary = new Dictionary<GBorderLineType, Border>
        {
            { GBorderLineType.Dotted,      new Border() { Style = "DOTTED" } },
            { GBorderLineType.Dashed,      new Border() { Style = "DASHED" } },
            { GBorderLineType.SolidThick,  new Border() { Style = "SOLID_THICK" } },
            { GBorderLineType.SolidMedium, new Border() { Style = "SOLID_MEDIUM" } },
            { GBorderLineType.Solid,       new Border() { Style = "SOLID" } },
            { GBorderLineType.None,        new Border() { Style = "NONE" } },
            { GBorderLineType.Double,      new Border() { Style = "DOUBLE" } },
        };
    }
}
