using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medias.Model;
using Medias.Model.Enum;

namespace MovieSaver.Model
{
    [Serializable]

    public class Movie : MediaItem
    {
        public TimeSpan MovieLength { get; set; }
        public Movie() { }

        public Movie(int id, string name,int rating, string description, List<Genre> genres, List<Author> authors, List<Actor> actors, WatchStatus status, TimeSpan movieLength)
            : base(id, name, rating,description, genres, authors, actors, status)
        {
            MovieLength = movieLength;
        }

        //public override TimeSpan Duration => MovieLength;

        public override string DurationToString { get => MovieLength.ToString(); }

        public override string ToString()
        {
            StringBuilder sb = new();

            sb.Append("Movie =[");
            sb.Append(base.ToString()).Append(", ").Append(DurationToString).Append("]");
            return base.ToString();
        }
    }
}
