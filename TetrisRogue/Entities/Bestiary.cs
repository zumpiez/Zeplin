using System;
using System.Collections.Generic;
using System.Linq;
//using System.Xml.Linq;
using System.Xml.Serialization;
using System.Text;
using System.Xml;

namespace TetrisRogue.Entities
{
    [XmlRoot("monsters")]
    public class Bestiary
    {
        public static Bestiary Create(string filename)
        {
            XmlRootAttribute root = new XmlRootAttribute("monsters");
            XmlSerializer serializer = new XmlSerializer(typeof(Bestiary), root);
            
            //XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            //ns.Add("", "");
            Bestiary b = (Bestiary)serializer.Deserialize(XmlReader.Create(filename));
            return b;
        }


        [XmlElement("monster")]
        public List<Entity> Monsters = new List<Entity>();
    }
}
