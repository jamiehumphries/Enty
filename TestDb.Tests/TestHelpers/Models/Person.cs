namespace EntityTestDb.Tests.Test.Models
{
    using System.Collections.Generic;

    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Dog> Dogs { get; set; }
    }
}
