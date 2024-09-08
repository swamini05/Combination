using System.Linq;
using System.Web.Mvc;
using CombinationBooks.Data;
using CombinationBooks.Models;

namespace CombinationBooks.Controllers
{
    public class BookController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult BookDetails(int authId)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var books = session.Query<Book>().Where(o => o.Author.Id == authId).ToList();

                TempData["AuthorId"] = authId;
                return View(books);
            }
        }
        public ActionResult Create()
        {
            if (TempData["AuthorId"] != null)
            {
                int authId = (int)TempData["AuthorId"];
                var book = new Book { Author = new Author { Id = authId } };
                return View(book);
            }
            return RedirectToAction("Index", "Author");
        }
        [HttpPost]
        public ActionResult Create(Book book)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var author = session.Get<Author>(book.Author.Id);

                if (author != null)
                {
                    book.Author = author;

                    using (var transaction = session.BeginTransaction())
                    {
                        session.Save(book);
                        transaction.Commit();
                        return RedirectToAction("BookDetails", new { authId = author.Id });

                    }
                }
                return View(book);

            }

        }
        public ActionResult Edit(int id)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var book = session.Get<Book>(id);

                if (book == null)
                {
                    return HttpNotFound();
                }
                return View(book);
            }
        }
        [HttpPost]
        public ActionResult Edit(Book book)
        {

            using (var session = NHibernateHelper.CreateSession())
            {
                var existingBook = session.Get<Book>(book.Id);

                if (existingBook == null)
                {
                    return HttpNotFound();
                }

                var author = session.Get<Author>(existingBook.Author.Id);

                if (author == null)
                {
                    return HttpNotFound();
                }

                existingBook.Description = book.Description;
                existingBook.Name = book.Name;
                existingBook.Genre = book.Genre;
                existingBook.Author = author;

                using (var transaction = session.BeginTransaction())
                {
                    session.Update(existingBook);
                    transaction.Commit();
                    return RedirectToAction("BookDetails", new { authId = existingBook.Author.Id });
                }
            }
        }

        public ActionResult Delete(int id)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var book = session.Get<Book>(id);
                if (book == null)
                {
                    return HttpNotFound();
                }
                return View(book);
            }
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult Deleteconfirmed(int id)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var book = session.Get<Book>(id);
                if (book != null)
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        session.Delete(book);
                        transaction.Commit();
                        return RedirectToAction("BookDetails", new { authId = book.Author.Id });
                    }
                }
                return HttpNotFound();
            }
        }
    }
}