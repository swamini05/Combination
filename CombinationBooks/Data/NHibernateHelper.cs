using CombinationBooks.Mappings;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace CombinationBooks.Data
{
    public class NHibernateHelper
    {
        private static ISessionFactory _sessionFactory = null;
        public static ISession CreateSession()
        {
            if (_sessionFactory == null)
            {
                _sessionFactory = Fluently.Configure()
                    .Database(MsSqlConfiguration.MsSql2012.ConnectionString("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BooksDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;"))
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<AuthorMap>())
                    .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(true, true))
                    .BuildSessionFactory();
            }
            return _sessionFactory.OpenSession();
        }
    }
}