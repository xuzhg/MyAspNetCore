using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.OData.NetTopologySuit.Models
{
    public class Customer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Point Location { get; set; }
    }

    public static class EdmModelBuilder
    {
        public static IEdmModel GetEdmModel()
        {
            var builder = new ODataModelBuilder();

            builder.EntitySet<Customer>("Customers");
            var customer = builder.EntityType<Customer>();
            customer.Property(c => c.Id);
            customer.Property(c => c.Name);
            customer.ComplexProperty(c => c.Location);
            customer.HasKey(c => c.Id);

            var point = builder.ComplexType<Point>();
            point.Property(p => p.X);
            point.Property(p => p.Y);
            point.Property(p => p.Z);
            point.Property(p => p.M);
            point.ComplexProperty(p => p.CoordinateSequence);
            point.Property(p => p.NumPoints);

            var coordinateSequence = builder.ComplexType<CoordinateSequence>();
            coordinateSequence.Property(c => c.Dimension);
            coordinateSequence.Property(c => c.Measures);
            coordinateSequence.Property(c => c.Spatial);
          //  coordinateSequence.Property(c => c.Ordinates);
            coordinateSequence.Property(c => c.HasZ);
            coordinateSequence.Property(c => c.HasM);
            coordinateSequence.Property(c => c.ZOrdinateIndex);
            coordinateSequence.Property(c => c.MOrdinateIndex);
            coordinateSequence.Property(c => c.Count);

            return builder.GetEdmModel();
        }
    }
}
