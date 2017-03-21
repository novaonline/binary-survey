using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;

namespace BinarySurveyPOC.Helper
{
    public class Geo
    {
        public static DbGeography getCircle(double lat, double lng, double km)
        {
            string textPoint = String.Format("POINT ({0} {1})", lng, lat);
            DbGeography point = DbGeography.PointFromText(textPoint, DbGeography.DefaultCoordinateSystemId); //4326 = [WGS84]
            DbGeography targetCircle = point.Buffer(km);
            return targetCircle;
        }

        public static DbGeography getCircle(Models.Coordinates coords, double km)
        {
            string textPoint = String.Format("POINT ({0} {1})", coords.Lng, coords.Lat);
            DbGeography point = DbGeography.PointFromText(textPoint, DbGeography.DefaultCoordinateSystemId); //4326 = [WGS84]
            DbGeography targetCircle = point.Buffer(km);
            return targetCircle;
        }
    }
}