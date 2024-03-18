using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using MovieSaver.Model;

namespace MovieSaver.Controller
{
    public class MovieController
    {


        public List<MediaItem> Movies { get; set; }

        private int maxId = 0;


        public MovieController(List<MediaItem> movies)
        {

            //if (movies is null || movies.Count == 0)
            //{
            //    Movies = new List<MediaItem>();

            //}
            //else 
            //{
            //    Movies = movies;
            //    maxId = Movies.Max(movie => movie.Id);
            //}

            Movies = new List<MediaItem>();
            if (movies is not null && movies.Any()) 
            {
                Movies.AddRange(movies);
                maxId = Movies.Max(movie => movie.Id);
            }

        }
        public MovieController()
        {
            Movies = new List<MediaItem>();

        }

        public bool IsEmpty() { return Movies.Count == 0; }

        public void ClearAll()
        {
            maxId = 0;
            Movies = new List<MediaItem>();
        }

        public void AddMovie(MediaItem movie)
        {
            if (movie != null)
            {
                movie.Id = ++maxId;
                Movies.Add(movie);
            }
            else throw new ArgumentNullException("Cannot add a NULL movie.");

        }

        public void DeleteMovie(MediaItem movie)
        {
            if (!(movie is null)) Movies.Remove(movie);
            else throw new NullReferenceException($"Movie '{movie.Name}' is not exists.");
        }

        public void EditMovie(MediaItem newMovie)
        {
            if (newMovie == null)
            {
                throw new ArgumentNullException(nameof(newMovie), "Cannot edit a NULL movie.");
            }

            int index = Movies.FindIndex(m => m.Id == newMovie.Id);

            if (index == -1)
            {
                throw new NullReferenceException($"Movie with ID '{newMovie.Id}' does not exist.");
            }

            Movies[index] = newMovie;
        }


    }
}
