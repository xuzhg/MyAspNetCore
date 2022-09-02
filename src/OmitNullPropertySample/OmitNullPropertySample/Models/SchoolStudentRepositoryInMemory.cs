namespace OmitNullPropertySample.Models
{
    public class SchoolStudentRepositoryInMemory : ISchoolStudentRepository
    {
        private static IList<School> _schools;
        private static IList<Student> _students;

        static SchoolStudentRepositoryInMemory()
        {
            _students = new List<Student>
            {
                new Student
                {
                    ID = 1, Name = "John", Age = 19, FavoriteColor = Color.Red,
                    HomeLocation = new Address { City = "RedStone", Street = "Red Rd", ZipCode = 98123 }
                },
                new Student
                {
                    ID = 2, Name = "Peter", Age = 9, FavoriteColor = Color.Green,
                    HomeLocation = new Address { City = "GreenStone", Street = "Green Rd", ZipCode = 12345 }
                },
                new Student
                {
                    ID = 3, Name = "Keti", Age = 45, FavoriteColor = null,
                    HomeLocation = new Address { City = "BlueStone", Street = "Blue Rd", ZipCode = 99111 }
                },
                new Student
                {
                    ID = 4, Name = "Ben", Age = 72, FavoriteColor = Color.Yellow,
                    HomeLocation = new Address { City = "YellowStone", Street = "Yellow Rd", ZipCode = 10982 }
                },
                new Student
                {
                    ID = 5, Name = "Alice", Age = 29, FavoriteColor = Color.Black,
                    HomeLocation = new Address { City = "BlackStone", Street = "Black Rd", ZipCode = 78012 }
                },
            };

            _schools = new List<School>
            {
                new School
                {
                    ID = 1,
                    Name = "Moon Middle School",
                    Emails = new List<string> { "efg@efg.com" },
                    Addresses = new List<Address>
                    {
                        new Address
                        {
                            City = "Moon City",
                            Street = "145TH AVE"
                        },
                        new Address
                        {
                            City = "Sun City",
                            Street = "24TH ST"
                        }
                    },
                    HeadQuarter = null,
                    Students = _students.Where(s => s.ID == 1 || s.ID == 3 || s.ID == 5).ToList()
                },
                new School
                {
                    ID = 2,
                    Name = "Jupiter Middle School",
                    Emails = null,
                    Addresses = null,
                    HeadQuarter = new Address { City = "Jupiter City", Street = "1110 AVE" },
                    Students = _students.Where(s => s.ID == 2 || s.ID == 4).ToList()
                },
                new School
                {
                    ID = 3,
                    Name = "Mars High School",
                    Emails = new List<string> { "abc@abc.com" },
                    Addresses = null,
                    HeadQuarter = new Address { City = "Mars City", Street = "Space Rd" },
                    Students = null
                }
            };
        }

        public IList<School> Schools => _schools;

        public IList<Student> Students => _students;
    }
}
