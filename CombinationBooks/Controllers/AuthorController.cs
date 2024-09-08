using System.Linq;
using System.Web.Mvc;
using CombinationBooks.Data;
using CombinationBooks.Models;
using NHibernate.Linq;

namespace CombinationBooks.Controllers
{
    public class AuthorController : Controller
    {
        // GET: Author
        public ActionResult Index()
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var authors = session.Query<Author>().Fetch(a => a.AuthorDetails).FetchMany(a => a.Books).ToList();
                return View(authors);
            }
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Author author)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    author.AuthorDetails.Author = author;
                    session.Save(author);
                    transaction.Commit();
                    return RedirectToAction("Index");

                }
            }
        }
        public ActionResult Edit(int id)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var author = session.Query<Author>().Fetch(a => a.AuthorDetails).FirstOrDefault(a => a.Id == id);
                if (author == null)
                {
                    return HttpNotFound();
                }
                TempData["AuthorId"] = author.Id;
                return View(author);
            }
        }

        [HttpPost]
        public ActionResult Edit(Author author)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var authorId = (int)TempData.Peek("AuthorId");

                var existingAuthor = session.Get<Author>(authorId);

                if (existingAuthor != null)
                {
                    existingAuthor.Name = author.Name;
                    existingAuthor.Email = author.Email;
                    existingAuthor.Age = author.Age;

                    if (author.AuthorDetails != null)
                    {
                        if (existingAuthor.AuthorDetails == null)
                        {
                            existingAuthor.AuthorDetails = new AuthorDetails();
                        }

                        existingAuthor.AuthorDetails.Street = author.AuthorDetails.Street;
                        existingAuthor.AuthorDetails.City = author.AuthorDetails.City;
                        existingAuthor.AuthorDetails.State = author.AuthorDetails.State;
                        existingAuthor.AuthorDetails.Country = author.AuthorDetails.Country;
                    }

                    using (var transaction = session.BeginTransaction())
                    {
                        session.Update(existingAuthor);
                        transaction.Commit();
                    }

                    TempData.Remove("AuthorId");
                }

                return RedirectToAction("Index");
            }
        }
        public ActionResult Delete(int id)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var author = session.Get<Author>(id);
                if (author == null)
                {
                    return HttpNotFound();
                }
                return View(author);
            }
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var author = session.Get<Author>(id);
                if (author != null)
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        session.Delete(author);
                        transaction.Commit();
                        return RedirectToAction("Index");

                    }
                }
                return RedirectToAction("Index");
            }
        }

    }
}
