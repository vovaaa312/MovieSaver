using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSaver.Model
{
    [Serializable]

    public class Genre
    {
        public string Name { get; set; }

        public Genre(string name)
        {
            Name = name;
        }

        public override bool Equals(object? obj)
        {
            return obj is Genre genre &&
                   Name == genre.Name;
        }

    }
}
