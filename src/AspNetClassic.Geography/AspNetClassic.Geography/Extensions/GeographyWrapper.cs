using Microsoft.Spatial;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;

namespace AspNetClassic.Geography.Extensions
{
    public class GeographyWrapper
    {
        public static implicit operator GeographyPoint(GeographyWrapper wrapper)
        {
            return GeographConvert.ConvertPointTo(wrapper._dbGeo);
        }

        public static implicit operator GeographyWrapper(GeographyPoint pt)
        {
            return new GeographyWrapper(GeographConvert.ConvertFrom(pt));
        }

        public static implicit operator GeographyLineString(GeographyWrapper wrapper)
        {
            return GeographConvert.ConvertLineStringTo(wrapper._dbGeo);
        }

        public static implicit operator GeographyWrapper(GeographyLineString lineString)
        {
            return new GeographyWrapper(GeographConvert.ConvertFrom(lineString));
        }

        public static implicit operator DbGeography(GeographyWrapper wrapper)
        {
            return wrapper._dbGeo;
        }

        public static implicit operator GeographyWrapper(DbGeography dbGeo)
        {
            return new GeographyWrapper(dbGeo);
        }

        protected GeographyWrapper(DbGeography dbGeo)
        {
            _dbGeo = dbGeo;
        }

        private readonly DbGeography _dbGeo;
    }
}