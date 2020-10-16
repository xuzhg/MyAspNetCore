using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Csdl;
using Microsoft.OData.Edm.Validation;
using Microsoft.OData.ModelBuilder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace ModelBuilderTest
{
    class Program
    {
        static void Main(string[] args)
        {
            ODataConventionModelBuilder modelBuilder = new ODataConventionModelBuilder();
            modelBuilder.EntitySet<Customer>("Customers");
            modelBuilder.EntitySet<Order>("Orders");
            IEdmModel model = modelBuilder.GetEdmModel();
            string csdl = GetCsdl(model);
            Console.WriteLine(csdl);
        }

        private static string GetCsdl(IEdmModel model)
        {
            using (StringWriter sw = new StringWriter())
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = System.Text.Encoding.UTF8;
                settings.Indent = true;

                using (XmlWriter xw = XmlWriter.Create(sw, settings))
                {
                    IEnumerable<EdmError> errors;
                    CsdlWriter.TryWriteCsdl(model, xw, CsdlTarget.OData, out errors);
                    xw.Flush();
                }

                string actual = sw.ToString();
                return actual;
            }
        }
    }

    public class Customer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Address Location { get; set; }

        public Order Order { get; set; }
    }

    public class Order
    {
        public int Id { get; set; }

        public string Title { get; set; }
    }

    public class Address
    {
        public string Street { get; set; }

        public string City { get; set; }
    }
}
