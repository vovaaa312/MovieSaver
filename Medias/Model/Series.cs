﻿using Medias.Model;
using Medias.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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



        public Series(int id, string name, string description, List<Genre> genres,
            List<Author> authors, WatchStatus status, List<Season> seasons) : base(id, name, description, genres, authors, status)
        {
            Seasons = seasons;
        }

      
    }
}
