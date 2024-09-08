using System.Linq;
using System.Web.Mvc;
using CombinationBooks.Data;
using CombinationBooks.Models;

namespace CombinationBooks.Controllers
{
    public class AuthorDetailsController : Controller
    {
        public ActionResult Index(int authId)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var authorDetails = session.Query<AuthorDetails>().FirstOrDefault(ad => ad.Author.Id == authId);

                if (authorDetails.City == null)
                {
                    return RedirectToAction("Create", new { authId = authId });
                }

                return View(authorDetails);
            }
        }

        public ActionResult Create(int authId)
        {
            var authorDetails = new AuthorDetails { Author = new Author { Id = authId } };
            return View(authorDetails);
        }

        [HttpPost]
        public ActionResult Create(AuthorDetails authorDetails)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var author = session.Get<Author>(authorDetails.Author.Id);

                if (author == null)
                {
                    return HttpNotFound();
                }

                authorDetails.Author = author;

                using (var transaction = session.BeginTransaction())
                {
                    session.Update(authorDetails);
                    transaction.Commit();
                }

                return RedirectToAction("Index", new { authId = author.Id });
            }
        }

        public ActionResult Edit(int id)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var authorDetails = session.Get<AuthorDetails>(id);

                if (authorDetails == null)
                {
                    return HttpNotFound();
                }

                return View(authorDetails);
            }
        }

        // POST: Edit existing author details
        [HttpPost]
        public ActionResult Edit(AuthorDetails authorDetails)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var existingDetails = session.Get<AuthorDetails>(authorDetails.Id);

                if (existingDetails == null)
                {
                    return HttpNotFound();
                }

                existingDetails.Street = authorDetails.Street;
                existingDetails.City = authorDetails.City;
                existingDetails.State = authorDetails.State;
                existingDetails.Country = authorDetails.Country;

                using (var transaction = session.BeginTransaction())
                {
                    session.Update(existingDetails);
                    transaction.Commit();
                }

                return RedirectToAction("Index", new { authId = existingDetails.Author.Id });
            }
        }

        // GET: Delete confirmation
        public ActionResult Delete(int id)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var authorDetails = session.Get<AuthorDetails>(id);

                if (authorDetails == null)
                {
                    return HttpNotFound();
                }

                return View(authorDetails);
            }
        }

        // POST: Confirm delete
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var authorDetails = session.Get<AuthorDetails>(id);

                if (authorDetails != null)
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        session.Delete(authorDetails);
                        transaction.Commit();
                    }

                    return RedirectToAction("Index", new { authId = authorDetails.Author.Id });
                }

                return HttpNotFound();
            }
        }
    }
}