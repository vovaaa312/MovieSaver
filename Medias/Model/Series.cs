using Medias.Model;
using Medias.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MovieSaver.Model
{
    [Serializable]

    public class Series : MediaItem
    {
        public int SeriesCount { get; set; }
        public TimeSpan AverageSeriesLength { get; set; }

        public List<Season> Seasons{ get; set; }

        public override string DurationToString => $"Seasons: {Seasons.Count}";

        public Series()
        {
        }



        public Series(int id, string name, int rating, string description, List<Genre> genres,
            List<Author> authors, List<Actor> actors, WatchStatus status, List<Season> seasons) : base(id, name, rating, description, genres, authors, actors, status)
        {
            Seasons = seasons;
        }

        public override string ToString()
        {
            StringBuilder sb = new();

            sb.Append("Series =[");
            sb.Append(base.ToString()).Append(", ").Append(DurationToString).Append("]");
            return base.ToString();
        }
    }
}
