namespace ODataMinimalApi.Models;

public class Customer
{
    public int Id { get; set; }

    public string Name { get; set; }

    public Address Location { get; set; }

    public Info Info { get; set; }

    public List<Order> Orders { get; set; }
}
