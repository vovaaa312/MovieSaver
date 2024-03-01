using Medias.Model;
using MovieSaver.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medias.Controller
{
    public class SeasonsController
    {
        public List<Season> Seasons { get; private set; }

        private int maxId = 0;

        public SeasonsController()
        {
            Seasons = new List<Season>();
        }

        public SeasonsController(List<Season> seasons)
        {
            if (seasons.Count != 0)
            {
                Seasons = seasons;
                if (Seasons.Any()) maxId = Seasons.Max(season => season.Id);
                else maxId = 0;
            }
            else Seasons = new();
        }

        public bool IsEmpty() { return Seasons.Count == 0; }

        public int Count () { return Seasons.Count; }

        public void AddSeason(Season season)
        {
            if (season != null)
            {
                season.Id = ++maxId;
                Seasons.Add(season);
            }
        }
        public void DeleteSeason(Season season)
        {
            if (!(season is null)) Seasons.Remove(season);
            else throw new NullReferenceException($"Season '{season.Name}' is not exists");
        }

        public void Clear() { Seasons.Clear(); maxId = 0; }
    }
}
