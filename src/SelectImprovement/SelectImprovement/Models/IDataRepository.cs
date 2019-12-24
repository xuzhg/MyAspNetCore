using System.Collections.Generic;

namespace SelectImprovement.Models
{
    public interface IDataRepository
    {
        IEnumerable<Customer> GetCustomers();
    }
}
