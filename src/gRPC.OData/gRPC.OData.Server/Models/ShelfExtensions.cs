namespace Bookstores
{
    public sealed partial class Shelf
    {
        public IList<Book> Books { get; set; } = new List<Book>();
    }
}