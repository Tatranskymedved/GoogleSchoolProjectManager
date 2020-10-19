using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSchoolProjectManager.Lib.GoogleAPI.Sheets
{
    public enum GUpdateBordersType
    {
        InnerRows = 0,
        InnerColumns = 1,
        Outer = 2
    }

    public static class GUpdateBorders
    {
        public static Request GenerateRequest(GUpdateBordersType type, GRange range)
        {
            UpdateBordersRequest request;
            switch (type)
            {
                case GUpdateBordersType.InnerRows:
                    request = GetInnerRows(range);
                    break;
                case GUpdateBordersType.InnerColumns:
                    request = GetInnerCols(range);
                    break;
                case GUpdateBordersType.Outer:
                default:
                    request = GetOuterBorders(range);
                    break;
            }

            return new Request()
            {
                UpdateBorders = request
            };
        }

        public static UpdateBordersRequest GetOuterBorders(GRange range)
        {
            return new UpdateBordersRequest()
            {
                Bottom = Border_SolidMedium,
                Left = Border_SolidMedium,
                Right = Border_SolidMedium,
                Top = Border_SolidMedium,
                Range = range.GetGridRange()
            };
        }

        public static UpdateBordersRequest GetInnerRows(GRange range)
        {
            return new UpdateBordersRequest()
            {
                InnerHorizontal = Border_Dashed,
                Range = range.GetGridRange()
            };
        }

        public static UpdateBordersRequest GetInnerCols(GRange range)
        {
            return new UpdateBordersRequest()
            {
                InnerHorizontal = Border_Dashed,
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
        public static Border Border_Dotted => new Border() { Style = "DOTTED" };
        public static Border Border_Dashed => new Border() { Style = "DASHED" };
        public static Border Border_SolidThick => new Border() { Style = "SOLID_THICK" };
        public static Border Border_SolidMedium => new Border() { Style = "SOLID_MEDIUM" };
        public static Border Border_Solid => new Border() { Style = "SOLID" };

    }
}
