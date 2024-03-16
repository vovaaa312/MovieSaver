using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medias.Model
{
    [Serializable]

    public class Actor
    {
        public string Name { get; set; }

        public string CharacterName{ get; set; }

        public Actor(string name, string characterName)
        {
            Name = name;
            CharacterName = characterName;
        }

        public Actor()
        {
        }
    }
}
