using MovieSaver.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Medias.MediaIO
{
    public class Deserializer
    {
        public static List<MediaItem> ReadFile(string filePath)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            List<MediaItem> mediaItemsFromFile;

            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            {
                mediaItemsFromFile = (List<MediaItem>)formatter.Deserialize(stream);
            }
            return mediaItemsFromFile;
        }
    }
}
