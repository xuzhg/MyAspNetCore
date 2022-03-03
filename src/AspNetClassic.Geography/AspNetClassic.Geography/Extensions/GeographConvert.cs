using Microsoft.Spatial;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace AspNetClassic.Geography.Extensions
{
    public class GeographConvert
    {
        public static DbGeography ConvertFrom(Microsoft.Spatial.Geography geo)
        {
            string geographyEwkt = geo.ToString();
            int semicolon = geographyEwkt.IndexOf(';');

            string geographyWkt = geographyEwkt.Substring(semicolon + 1);
            return DbGeography.FromText(geographyWkt, int.Parse(geo.CoordinateSystem.Id));
        }

        public static GeographyPoint ConvertPointTo(DbGeography dbGeo)
        {
            Debug.Assert(dbGeo.SpatialTypeName == "Point");
            double lat = dbGeo.Latitude ?? 0;
            double lon = dbGeo.Longitude ?? 0;
            double? alt = dbGeo.Elevation;
            double? m = dbGeo.Measure;
            return GeographyPoint.Create(lat, lon, alt, m);
        }

        public static GeographyLineString ConvertLineStringTo(DbGeography dbGeo)
        {
            Debug.Assert(dbGeo.SpatialTypeName == "LineString");
            SpatialBuilder builder = SpatialBuilder.Create();
            var pipeLine = builder.GeographyPipeline;
            pipeLine.SetCoordinateSystem(CoordinateSystem.DefaultGeography);
            pipeLine.BeginGeography(SpatialType.LineString);

            int numPionts = dbGeo.PointCount ?? 0;
            for (int n = 0; n < numPionts; n++)
            {
                DbGeography pointN = dbGeo.PointAt(n + 1);
                double lat = pointN.Latitude ?? 0;
                double lon = pointN.Longitude ?? 0;
                double? alt = pointN.Elevation;
                double? m = pointN.Measure;
                GeographyPosition position = new GeographyPosition(lat, lon, alt, m);
                if (n == 0)
                {
                    pipeLine.BeginFigure(position);
                }
                else
                {
                    pipeLine.LineTo(position);
                }
            }
            pipeLine.EndFigure();
            pipeLine.EndGeography();
            return (GeographyLineString)(builder.ConstructedGeography);
        }
    }
}