namespace OmitNullPropertySample.Models
{
    public class Student
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public Color? FavoriteColor { get; set; }

        public Address HomeLocation { get; set; }
    }
}
