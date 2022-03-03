using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Spatial;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Spatial;
using AspNetClassic.Geography.Extensions;

namespace AspNetClassic.Geography.Models
{
    public class Customer
    {
        private GeographyWrapper _ptWrapper;
        private GeographyWrapper _lineWrapper;

        public int Id { get; set; }

        public string Name { get; set; }

        public DbGeography Location
        {
            get { return _ptWrapper; }
            set { _ptWrapper = value; }
        }

        [NotMapped]
        public GeographyPoint EdmLocation
        {
            get { return _ptWrapper; }
            set { _ptWrapper = value; }
        }

        public DbGeography Line
        {
            get { return _lineWrapper; }
            set { _lineWrapper = value; }
        }

        [NotMapped]
        public GeographyLineString EdmLine
        {
            get { return _lineWrapper; }
            set { _lineWrapper = value; }
        }
    }
}