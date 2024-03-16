using Medias.Model;
using Medias.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSaver.Model
{
    [Serializable]
    public abstract class MediaItem 
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int Rating { get; set; }
        public string Description { get; set; }

        //добавить абстрактный список сезонов. у movie = null

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

        private List<Actor> _actors;

        public List<Actor> Actors {
            get=> _actors;
            set {
                _actors = value;
                UpdateActorsString();
            }
        }

  
        public WatchStatus Status { get; set; }

        //public abstract TimeSpan Duration { get; }

        public abstract string DurationToString { get; }

        public string GenresString { get; private set; }
        public string AuthorsString { get; private set; }

        public string ActorsString { get; private set; }


        public MediaItem()
        {
        }

        public MediaItem(int id, string name, int rating, string description, List<Genre> genres, List<Author> authors, List<Actor> actors, WatchStatus status)
        {
            Id = id;
            Name = name;
            Description = description;
            Genres = genres;
            Authors = authors;
            Actors = actors;
            Status = status;
            Rating = rating;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append($"Id='{Id}', Name='{Name}', Description='{Description}'");

            if (Genres is not null && Genres.Any())
            {
                builder.Append(", Genres=");
                builder.Append("{").Append(string.Join(", ", Genres.Select(genre => genre.Name))).Append("}");
            }

            if (Authors is not null && Authors.Any())
            {
                builder.Append(", Authors=");
                builder.Append("{").Append(string.Join(", ", Authors.Select(author => author.Name))).Append("}");
            }
            if (Actors is not null && Actors.Any()) 
            {
                builder.Append(", Actors=");
                builder.Append("{").Append(string.Join(", ", Actors.Select(author => author.Name))).Append("}");

            }
            builder.Append($", Status='{Status}'");
            
            return builder.ToString();
        }




        
        //public override bool Equals(object? obj)
        //{
        //    return 
        //           obj is MediaItem item &&
        //           Id == item.Id &&
        //           Name == item.Name &&
        //           Description == item.Description &&
        //           EqualityComparer<List<Genre>>.Default.Equals(Genres, item.Genres) &&
        //           EqualityComparer<List<Author>>.Default.Equals(Authors, item.Authors) &&
        //           Status == item.Status &&
        //           Duration.Equals(item.Duration);
        //}

        public override bool Equals(object? obj)
        {
            return
                   obj is MediaItem item &&
                   Id == item.Id &&
                   Name == item.Name &&
                   Description == item.Description &&
                    AreListsEqual(Genres, item.Genres)&&
                    AreListsEqual(Authors, item.Authors) &&

                   Status == item.Status;
        }

        private static bool AreListsEqual<T>(List<T> list1, List<T> list2)
        {
            // Checking list lengths
            if (list1.Count != list2.Count)
            {
                return false;
            }

            // Compare each element
            for (int i = 0; i < list1.Count; i++)
            {
                if (!list1[i].Equals(list2[i]))
                {
                    return false;
                }
            }

            // If all elements match
            return true;
        }



        private void UpdateGenresString()
        {
            GenresString = string.Join(", ", Genres.Select(genre => genre.Name));
        }

        private void UpdateAuthorsString()
        {
            AuthorsString = string.Join(", ", Authors.Select(author => author.Name));
        }

        private void UpdateActorsString()
        {
            ActorsString = string.Join(", ", Actors.Select(actor => actor.Name));
        }

    }
}
