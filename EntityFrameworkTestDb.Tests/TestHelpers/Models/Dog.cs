namespace EntityFrameworkTestDb.Tests.TestHelpers.Models
{
    public class Dog
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? OwnerId { get; set; }
        public Person Owner { get; set; }
    }
}
