using MovieSaver.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Medias.MediaIO
{
    public class Serializer
    {
        public static void SaveToFile(List<MediaItem> Movies, string filePath)
        {
            if (Movies.Count == 0 && Movies is null) throw new NullReferenceException("Movies list is empty.");

            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                formatter.Serialize(stream, Movies);
            }
        }
    }
}
