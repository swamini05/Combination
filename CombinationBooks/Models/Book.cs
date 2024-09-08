namespace CombinationBooks.Models
{
    public class Book
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Genre { get; set; }
        public virtual string Description { get; set; }
        public virtual Author Author { get; set; }
    }
}