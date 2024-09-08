using CombinationBooks.Models;
using FluentNHibernate.Mapping;

namespace CombinationBooks.Mappings
{
    public class AuthorMap : ClassMap<Author>
    {
        public AuthorMap()
        {
            Table("Authors");
            Id(a => a.Id).GeneratedBy.Identity();
            Map(a => a.Name);
            Map(a => a.Email);
            Map(a => a.Age);
            HasOne(a => a.AuthorDetails).Cascade.All().PropertyRef(a => a.Author);
            HasMany(e => e.Books).Inverse().Cascade.All();
        }
    }
}