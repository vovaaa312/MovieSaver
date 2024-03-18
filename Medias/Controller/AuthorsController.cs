using Medias.Exceptions;
using Medias.Model;
using MovieSaver.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Medias.Controller
{
    public class AuthorsController
    {
        public List<Author> Authors { get; private set; }

        public AuthorsController(List<Author> authors)
        {
            Authors = new List<Author>();
            if (authors is not null && authors.Any()) Authors.AddRange(authors);
        }

        public AuthorsController()
        {
            Authors = new List<Author>();
        }

        public void Add(Author obj)
        {
            if (obj is null) throw new ArgumentNullException("Cannot add a NULL author.");
            if (Contains(obj)) throw new DublicateObjectException($"{obj} already in the list.");
            Authors.Add((Author)obj);
        }
        public void Remove(Author obj)
        {
            if (obj is null) throw new ArgumentNullException("Cannot delete a NULL author.");
            if (!Contains(obj)) throw new Exception($"{obj} does not exist in the list.");

            Authors.Remove((Author)obj);
        }

        public bool IsEmpty()
        {
            return Authors.Count == 0;
        }

        public void Clear()
        {
            Authors.Clear();
        }
        public int Count() { return Authors.Count; }

        public bool Contains(Author obj)
        {
            return Authors.Contains(obj);
        }
    }
}
