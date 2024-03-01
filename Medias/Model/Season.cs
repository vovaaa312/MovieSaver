using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medias.Model
{
    [Serializable]

    public class Season
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public int EpisodesCount { get; set; }

        //

        public Season() { }

        public Season(int id, string name, string? description, int episodesCount)
        {
            Id = id;
            Name = name;
            Description = description;
            EpisodesCount = episodesCount;
        }

        public Season(int id, string name, int episodesCount)
        {
            Id = id;
            Name = name;
            Description = "";
            EpisodesCount = episodesCount;
        }
       
        public override string ToString()
        {
            return $"Season {Id}: {Name} - Episodes: {EpisodesCount}";
        }
    }
}
