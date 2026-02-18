// See https://aka.ms/new-console-template for more information

using System.Xml;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Csdl;
using Microsoft.OData.Edm.Validation;
using Microsoft.OData.ModelBuilder;

var builder = new ODataConventionModelBuilder();
var customer = builder.EntityType<Customer>();
customer.Property(c => c.FullName).HasComputed().IsComputed(true);
customer.Property(c => c.Salary).HasImmutable().IsImmutable(true);


var model = builder.GetEdmModel();

// Write model to CSDL (XML) file
using (var writer = XmlWriter.Create("model.csdl", new XmlWriterSettings { Indent = true }))
{
    IEnumerable<EdmError> errors;
    if (!CsdlWriter.TryWriteCsdl(model, writer, CsdlTarget.OData, out errors))
    {
        Console.WriteLine("Failed to write CSDL:");
        foreach (var error in errors)
        {
            Console.WriteLine(error.ErrorMessage);
        }
    }
}

Console.WriteLine(File.ReadAllText("model.csdl"));

Console.WriteLine("Hello, World!");


public class Customer
{
    public int Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Salary { get; set; }

    // Computed property
    public string FullName
    {
        get { return $"{FirstName} {LastName}"; }
    }

}