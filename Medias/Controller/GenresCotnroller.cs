using Medias.Exceptions;
using Medias.Model;
using MovieSaver.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medias.Controller
{
    public class GenresCotnroller
    {
        public List<Genre> Genres { get; private set; }

        public GenresCotnroller(List<Genre> genres)
        {
            if (genres is null || genres.Count == 0) Genres = new List<Genre>();

            else Genres = genres;
        }

        public GenresCotnroller()
        {
            Genres = new List<Genre>();
        }


        public void Add(Genre obj)
        {
            if (obj is null) throw new ArgumentNullException("Cannot add a NULL genre.");
            if (Contains(obj)) throw new DublicateObjectException($"{obj} already in the list.");

            Genres.Add(obj);

        }

        public void Remove(Genre obj)
        {
            if (obj is null) throw new ArgumentNullException("Cannot delete a NULL genre.");
            if (!Contains(obj)) throw new Exception($"{obj} does not exist in the list.");

            Genres.Remove(obj);
        }

        public void Clear()
        {
            Genres.Clear();
        }

        public bool IsEmpty()
        {
            return Genres.Count == 0;
        }

        public bool Contains(Genre obj)
        {

            return Genres.Contains(obj);
        }


        public int Count() { return Genres.Count; }





    }

}
