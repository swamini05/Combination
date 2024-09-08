using CombinationBooks.Models;
using FluentNHibernate.Mapping;

namespace CombinationBooks.Mappings
{
    public class AuthorDetailsMap : ClassMap<AuthorDetails>
    {
        public AuthorDetailsMap()
        {
            Table("AuthorDetails");
            Id(d => d.Id).GeneratedBy.Identity();
            Map(d => d.Street);
            Map(d => d.City);
            Map(d => d.State);
            Map(d => d.Country);
            References(a => a.Author).Column("Author_id").Unique().Cascade.None();
        }
    }
}