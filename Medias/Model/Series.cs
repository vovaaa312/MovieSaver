using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSaver.Model
{
    public class Series : MediaItem
    {
        public int SeriesCount { get; set; }
        public TimeSpan AverageSeriesLength { get; set; }

        public Series()
        {
        }

        public Series(int id, string name, string description, List<Genre> genres,
            List<Author> authors, WatchStatus status, int seriesCount, TimeSpan averageSeriesLength) : base(id, name, description, genres, authors, status)
        {
            SeriesCount = seriesCount;
            AverageSeriesLength = averageSeriesLength;
        }

        public override TimeSpan Duration => TimeSpan.FromMinutes(AverageSeriesLength.TotalMinutes * SeriesCount);

    }
}
