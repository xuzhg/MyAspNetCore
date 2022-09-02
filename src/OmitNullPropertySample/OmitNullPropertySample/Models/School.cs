namespace OmitNullPropertySample.Models
{
    public class School
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public IList<string> Emails { get; set; }

        public Address HeadQuarter { get; set; }

        public IList<Address> Addresses { get; set; }

        public IList<Student> Students { get; set; }
    }
}
