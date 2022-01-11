using Microsoft.OData.Edm;

namespace NewQueryOptionIn8.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Category Category { get; set; }

        public Color Color { get; set; }

        public Address Location { get; set; }

        public double Price { get; set; }

        public int Qty { get; set; }

        public double TaxRate { get; set; }

        public IList<Sale> Sales { get; set; }
    }

    public class Sale
    {
        public int Id { get; set; }

        public Date SaleDate { get; set; }

        public int ProductId { get; set; }

        public int Amount { get; set; }
    }

    public class Address
    {
        public string Street { get; set; }

        public string City { get; set; }

        public int ZipCode { get; set; }
    }

    public enum Category
    {
        Food,

        Office,

        Music
    }

    public enum Color
    {
        Red,

        Yellow,

        Blue,

        Brown,

        White
    }
}
