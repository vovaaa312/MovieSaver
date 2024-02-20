using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSaver.Model
{
    public abstract class MediaItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Genre> Genres { get; set; }

        public List<Author> Authors;

        public WatchStatus Status { get; set; }

        public abstract TimeSpan Duration { get; }

        public string GenresString => string.Join(", ", Genres.Select(genre => genre.Name));
        public string AuthorsString => string.Join(", ", Authors.Select(author => author.Name));


        public MediaItem()
        {
        }

        public MediaItem(int id, string name, string description, List<Genre> genres, List<Author> authors, WatchStatus status)
        {
            Id = id;
            Name = name;
            Description = description;
            Genres = genres;
            Authors = authors;
            Status = status;
        }
    }
}
