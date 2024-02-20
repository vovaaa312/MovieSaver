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
        //public ObjectLinkedList Movies { get; set; }

        //public MovieController()
        //{
        //    Movies = new ObjectLinkedList();
        //}
        //public MovieController(ObjectLinkedList movies)
        //{
        //    Movies = movies;
        //}


        public List<MediaItem> Movies { get; set; }

        private int maxId = 0;


        public MovieController(List<MediaItem> movies)
        {
            Movies = movies;
        }
        public MovieController()
        {
            Movies = new List<MediaItem>();
        }

        public void AddMovie(MediaItem movie)
        {
            if (movie != null)
            {
                movie.Id = ++maxId; 
                Movies.Add(movie);
            }
            else throw new ArgumentNullException("Cannot add a NULL movie");
            
        }

        public void RemoveMovie(MediaItem movie)
        {
            if (!(movie is null))Movies.Remove(movie);                            
            else throw new NullReferenceException($"Movie '{movie.Name}' is not exists");
        }

        public void EditMovie(MediaItem movie)
        {

        }

    }
}
