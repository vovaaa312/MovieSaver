using Medias.Model;
using MovieSaver.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Medias.Controller
{
    public class ActorsController
    {
        public List<Actor> Actors { get; set; }

        //private int maxId = 0;

        public ActorsController()
        {
            Actors = new List<Actor>();
        }
        public ActorsController(List<Actor> actors)
        {
            Actors = new List<Actor>();

            if (actors is not null && actors.Any()) Actors.AddRange(actors);

        }

        public bool IsEmpty() { return Actors.Count == 0; }

        public void Add(Actor obj)
        {
            if (obj is null) throw new ArgumentNullException("Cannot add a NULL genre.");
            if (obj is not Actor) throw new InvalidCastException($"{obj.GetType} is not 'Genre' type.");

            Actors.Add((Actor)obj);

        }

        public void Delete(Actor actor)
        {
            if (!(actor is null)) Actors.Remove(actor);
            else throw new NullReferenceException($"Actor '{actor.Name}' is not exists");

        }

    }
}
