using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSaver.Model
{
    [Serializable]

    public class Author
    {
        public string Name { get; set; }

        public Author(string name)
        {
            Name = name;
        }

        public override bool Equals(object? obj)
        {
            return obj is Author author &&
                   Name == author.Name;
        }
    }
}
