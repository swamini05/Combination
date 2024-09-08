namespace CombinationBooks.Models
{
    public class AuthorDetails
    {
        public virtual int Id { get; set; }
        public virtual string Street { get; set; }
        public virtual string City { get; set; }
        public virtual string State { get; set; }
        public virtual string Country { get; set; }
        public virtual Author Author { get; set; }
    }
}