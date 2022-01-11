namespace NewQueryOptionIn8.Models
{
    public interface IProductSaleRepository
    {
        IEnumerable<Product> Products { get; }

        IEnumerable<Sale> Sales { get; }
    }
}
