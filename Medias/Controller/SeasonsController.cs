using Medias.Exceptions;
using Medias.Model;
using MovieSaver.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Medias.Controller
{
    public class SeasonsController
    {
        public List<Season> Seasons { get; private set; }

        public SeasonsController()
        {
            Seasons = new List<Season>();
        }

        public SeasonsController(List<Season> seasons)
        {
            if (seasons.Any()) Seasons = seasons;

            else Seasons = new();
        }

        public bool IsEmpty() { return Seasons.Count == 0; }

        public int Count() { return Seasons.Count; }


        public void Clear() { Seasons.Clear();}

        public void Add(Season obj)
        {
            if (obj is null) throw new ArgumentNullException("Cannot add a NULL season.");
            if (Contains(obj)) throw new DublicateObjectException($"{obj} already in the list.");

            Seasons.Add(obj);

        }

        public void Remove(Season obj)
        {
            if (obj is null) throw new ArgumentNullException("Cannot delete a NULL season.");
            if (!Contains(obj)) throw new Exception($"{obj} does not exist in the list.");

            Seasons.Remove(obj);
        }

        public void Edit(Season obj)
        {
            if (obj is null) throw new ArgumentNullException(nameof(obj), "Cannot edit a NULL object.");
            if (!Seasons.Any(g => g == obj)) throw new Exception($"{obj} does not exist in the list.");

            int index = Seasons.FindIndex(g => g.Name == obj.Name);
            if (index == -1) throw new NullReferenceException($"Genre with name '{obj.Name}' does not exist.");

            Seasons[index] = obj;
        }

        public bool Contains(Season obj)
        {

            return Seasons.Contains(obj);
        }


    }
}
