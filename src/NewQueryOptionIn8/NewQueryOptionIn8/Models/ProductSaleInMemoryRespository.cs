using Microsoft.OData.Edm;

namespace NewQueryOptionIn8.Models
{
    public class ProductSaleInMemoryRespository : IProductSaleRepository
    {
        static IList<Product> _products;
        static IList<Sale> _sales;

        static ProductSaleInMemoryRespository()
        {
            List<Address> addresses = new List<Address>()
            {
                new Address { City = "Redd", Street = "152TH AVE", ZipCode = 98052 },
                new Address { City = "Issah", Street = "32TH AVE", ZipCode = 13029 },
                new Address { City = "Sammsh", Street = "228TH AVE", ZipCode = 58031 },
            };

            _products = new List<Product>()
            {
                new Product
                {
                    Id = 1, Name = "Bread", Color = Color.White, Price = 6.5, Qty = 10, Category = Category.Food, TaxRate = 0.10,
                    Location = addresses[2],
                    Sales = new List<Sale>
                    {
                        new Sale { SaleDate = new Date(2021, 11, 12), ProductId = 1, Amount = 2 },
                        new Sale { SaleDate = new Date(2021, 11, 13), ProductId = 1, Amount = 3 },
                        new Sale { SaleDate = new Date(2021, 12, 22), ProductId = 1, Amount = 1 },
                    }
                },

                new Product
                {
                    Id = 2, Name = "Pencil", Color = Color.Blue, Price = 3.99, Qty = 15, Category = Category.Office, TaxRate = 0.14,
                    Location = addresses[0],
                    Sales = new List<Sale>
                    {
                        new Sale { SaleDate = new Date(2011, 1, 2), ProductId = 2, Amount = 4 },
                        new Sale { SaleDate = new Date(2017, 3, 2), ProductId = 2, Amount = 6 },
                    }
                },
                new Product
                {
                    Id = 3, Name = "Noodle", Color = Color.Brown, Price = 2.99, Qty = 50, Category = Category.Food, TaxRate = 0.10,
                    Location = addresses[1],
                    Sales = new List<Sale>
                    {
                        new Sale { SaleDate = new Date(1999, 8, 12), ProductId = 3, Amount = 11 },
                        new Sale { SaleDate = new Date(2018, 3, 19), ProductId = 3, Amount = 8 },
                    }
                },
                new Product
                {
                    Id = 4, Name = "Flute", Color = Color.Yellow, Price = 12.0, Qty = 75, Category = Category.Music, TaxRate = 0.14 ,
                    Location = addresses[0],
                    Sales = new List<Sale>
                    {
                        new Sale { SaleDate = new Date(2022, 12, 9), ProductId = 4, Amount = 15 },
                        new Sale { SaleDate = new Date(2022, 5, 29), ProductId = 4, Amount = 18 },
                    }
                },
                new Product
                {
                    Id = 5, Name = "Paper", Color = Color.White, Price = 6.99, Qty = 14 , Category = Category.Office, TaxRate = 0.24,
                    Location = addresses[2],
                    Sales = new List<Sale>
                    {
                        new Sale { SaleDate = new Date(2013, 1, 9), ProductId = 5, Amount = 2 },
                        new Sale { SaleDate = new Date(2022, 1, 1), ProductId = 5, Amount = 3 },
                    }
                },
                new Product
                {
                    Id = 6, Name = "Violin", Color = Color.Blue, Price = 125.00, Qty = 15, Category = Category.Music, TaxRate = 0.44,
                    Location = addresses[1],
                     Sales = new List<Sale>
                    {
                        new Sale { SaleDate = new Date(2012, 6, 9), ProductId = 6, Amount = 5 },
                        new Sale { SaleDate = new Date(1999, 6, 23), ProductId = 6, Amount = 3 },
                    }
                },
            };

            _sales = new List<Sale>();
            int index = 1;
            foreach (var product in _products)
            {
                foreach (var sale in product.Sales)
                {
                    sale.Id = index++;
                    _sales.Add(sale);
                }
            }

        }
        public IEnumerable<Product> Products => _products;

        public IEnumerable<Sale> Sales => _sales;
    }
}
