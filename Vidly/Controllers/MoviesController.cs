using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using System.Data.Entity;
using Vidly.ViewModels;
using System.Data.Entity.Validation;

namespace Vidly.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MoviesController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        // GET: Movies
        public ViewResult Index()
        {
            var movies = _context.Movies.Include(m => m.Genre).ToList();
            return View(movies);
        }

        public ActionResult Edit(int id)
        {
            var movie = _context.Movies.SingleOrDefault(m => m.Id == id);
            if (movie == null)
                return HttpNotFound();

            var genres = _context.Genres.ToList();
            var movieForm = new MovieFormViewModel()
            {
                Genres = genres,
                Movie = movie
            };

            return View("MovieForm", movieForm);
        }

        public ActionResult New()
        {
            var genres = _context.Genres.ToList();
            var movieFormModel = new MovieFormViewModel()
            {
                Genres = genres
            };
            return View("MovieForm", movieFormModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(MovieFormViewModel movieForm)
        {
            if (!ModelState.IsValid)
            {
                var genres = _context.Genres.ToList();
                var movieFormModel = new MovieFormViewModel()
                {
                    Genres = genres,
                    Movie = movieForm.Movie
                };

                return View("MovieForm", movieFormModel);
            }

            if (movieForm.Movie.Id == 0)
            {
                movieForm.Movie.DateAdded = DateTime.Now;
                _context.Movies.Add(movieForm.Movie);
            }       
            else
            {
                var movie = _context.Movies.SingleOrDefault(m => m.Id == movieForm.Movie.Id);

                movie.Name = movieForm.Movie.Name;
                movie.ReleaseDate = movieForm.Movie.ReleaseDate;
                movie.GenreId = movieForm.Movie.GenreId;
                movie.NumberInStock = movieForm.Movie.NumberInStock;
            }

            _context.SaveChanges();
            

            return RedirectToAction("Index", "Movies");
        }
    }
}