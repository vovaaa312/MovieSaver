using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSaver.Model
{
    [Serializable]

    public class Movie : MediaItem
    {
        public TimeSpan MovieLength { get; set; }
        public Movie() { }

        public Movie(int id, string name, string description, List<Genre> genres, List<Author> authors, WatchStatus status, TimeSpan movieLength) : base(id, name, description, genres, authors, status)
        {
            MovieLength = movieLength;
        }

        //public override TimeSpan Duration => MovieLength;

        public override string DurationToString { get => MovieLength.ToString(); }
    }
}
