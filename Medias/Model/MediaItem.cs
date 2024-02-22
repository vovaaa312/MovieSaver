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

        private List<Genre> _genres;
        public List<Genre> Genres
        {
            get => _genres;
            set
            {
                _genres = value;
                UpdateGenresString();
            }
        }

        private List<Author> _authors;
        public List<Author> Authors
        {
            get => _authors;
            set
            {
                _authors = value;
                UpdateAuthorsString();
            }
        }
        public WatchStatus Status { get; set; }

        public abstract TimeSpan Duration { get; }

        public string GenresString { get; private set; }
        public string AuthorsString { get; private set; }

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

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append($"Id: {Id}, Name: {Name}, Description: {Description}");

            if (Genres != null && Genres.Any())
            {
                builder.Append(", Genres: ");
                builder.Append(string.Join(", ", Genres.Select(genre => genre.Name)));
            }

            if (Authors != null && Authors.Any())
            {
                builder.Append(", Authors: ");
                builder.Append(string.Join(", ", Authors.Select(author => author.Name)));
            }

            builder.Append($", Status: {Status}");
            builder.Append($", Duration: {Duration.TotalMinutes}");
            return builder.ToString();
        }

        public override bool Equals(object? obj)
        {
            return 
                   obj is MediaItem item &&
                   Id == item.Id &&
                   Name == item.Name &&
                   Description == item.Description &&
                   EqualityComparer<List<Genre>>.Default.Equals(Genres, item.Genres) &&
                   EqualityComparer<List<Author>>.Default.Equals(Authors, item.Authors) &&
                   Status == item.Status &&
                   Duration.Equals(item.Duration);
        }



        private void UpdateGenresString()
        {
            GenresString = string.Join(", ", Genres.Select(genre => genre.Name));
        }

        private void UpdateAuthorsString()
        {
            AuthorsString = string.Join(", ", Authors.Select(author => author.Name));
        }
    }
}
