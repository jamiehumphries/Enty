namespace Enty.Tests.TestHelpers.Models
{
    public class Dog
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? OwnerId { get; set; }
        public virtual Person Owner { get; set; }
    }
}
