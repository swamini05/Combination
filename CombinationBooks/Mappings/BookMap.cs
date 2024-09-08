using CombinationBooks.Models;
using FluentNHibernate.Mapping;

namespace CombinationBooks.Mappings
{
    public class BookMap : ClassMap<Book>
    {
        public BookMap()
        {
            Table("Books");
            Id(b => b.Id).GeneratedBy.Identity();
            Map(b => b.Name);
            Map(b => b.Genre);
            Map(b => b.Description);
            References(o => o.Author).Column("AuthId").Cascade.None().Nullable();
        }
    }
}