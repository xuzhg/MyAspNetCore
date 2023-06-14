//-----------------------------------------------------------------------------
// <copyright file="DataSource.cs">
//      Copyright (C) saxu@microsoft.com
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.AspNetCore.OData.Formatter.Value;
using System.Text.Json;

namespace UntypedApp.Models;

public class DataSource
{
    private static IList<Person> _persons = new List<Person>
    {
        new Person
        {
            Id = 1,
            Name = "Kai",
            Gender = Gender.Female,
            Data = 42,
            Infos = new object[] { 1, true, 3.1415f, new byte[] { 4, 5 } },
      //      HomeAddress = new Address { City = "Mars", Street = "Mars St"}
        },

        new Person
        {
            Id = 2,
            Name = "Ezra",
            Gender = Gender.Male,
            Data = UserType.Admin.ToString(), // why call ToString(), see https://github.com/OData/AspNetCoreOData/issues/931
            Infos = new object[] { UserType.Admin, 2, Gender.Male },
       //     HomeAddress = new Address { City = "Jupiter", Street = "Jupiter St"},
            DynamicContainer = new Dictionary<string, object>
            {
                { "D_Data", UserType.Admin.ToString() } // why call ToString(), see https://github.com/OData/AspNetCoreOData/issues/931
            }
        },

        new Person
        {
            Id = 3,
            Name = "Chang",
            Gender = Gender.Male,
            Data = new Course { Title = "Maths", Credits = new List<int> { 77, 97 }},
            Infos = new List<object>
            {
                new Address { City = "Issaquay", Street = "Main ST" }
            },
       //     HomeAddress = new Address { City = "Venus", Street = "Venus 158TH AVE"}
        },

        new Person
        {
            Id = 4,
            Name = "Robert",
            Gender = Gender.Female,
            Data = new Address { City = "Redmond", Street = "152TH AVE" },
            Infos = new List<object>
            {
                new Course { Title = "Geometry", Credits = new List<int> { 95, 96 }}
            },
       //     HomeAddress = new Address { City = "Mercury", Street = "Water RD"}
        },

        new Person
        {
            Id = 5,
            Name = "Xu",
            Gender = Gender.Male,
            Data = new Dictionary<string, object>
            {
                { "Age", 8 },
                { "Email", "xu@abc.com" }
            },
            Infos = new List<object>
            {
                null,
                new Dictionary<string, object>
                {
                    { "Title", "Software engineer" }
                },
                42
            },
        //    HomeAddress = new Address { City = "Uranus", Street = "!@#$ RD"}
        },

        new Person
        {
            Id = 6,
            Name = "Wu",
            Gender = Gender.Female,
            Data = new EdmUntypedObject
            {
                { "NickName", "YX" },
                { "FavoriteColor", Color.Blue }
            },
            Infos = new EdmUntypedCollection
            {
                new List<object>
                {
                    new Dictionary<string, object>
                    {
                        { "AnotherNickName", "Rain" }
                    },
                    42
                },
            },
        //    HomeAddress = new Address { City = "Saturn", Street = "===>"}
        },

        new Person
        {
            Id = 7,
            Name = "Wen",
            Gender = Gender.Male,
            Data = new object[]
            {
                null,
                42,
                new Dictionary<string, object> {{ "City", "Shanghai"} },
                new Course { Title = "Science", Credits = new List<int> { 78, 88 }}
            },
            Infos = new EdmUntypedCollection
            {
                new List<object>
                {
                    new EdmUntypedCollection
                    {
                        null
                    }
                },
            },
           // HomeAddress = new Address { City = "Neptune", Street = "Hello Neptune RD"}
        },

        new Person
        {
            Id = 8,
            Name = "JSON",
            Gender = Gender.Male,
            Data = JsonDocument.Parse(@"{""name"": ""Joe"",""age"": 22,""canDrive"": true}"),
            Infos = new List<object>(),
         //   HomeAddress = new Address { City = "Neptune", Street = "Hello Neptune RD"}
        },
    };

    public static IEnumerable<Person> GetPeople() => _persons;

    public static Person AddPerson(Person person)
    {
        int maxId = _persons.Max(p => p.Id);
        ++maxId;
        person.Id = maxId;
        _persons.Add(person);
        return person;
    }
}
