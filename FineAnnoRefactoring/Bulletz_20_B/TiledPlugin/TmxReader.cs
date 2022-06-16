using System.Xml;
using System;
using System.Collections.Generic;

namespace ProgettoFinale
{
    class TmxReader
    {
        public TileSet TileSet { get; }
        public List<Layer> Layers { get; set; }

        public TmxReader(string filePath)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(filePath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            XmlNode nodeMap = doc.SelectSingleNode("map");

            TileSet = TmxNodeParser.ParseTileset(nodeMap);
            Layers = TmxNodeParser.ParseLayers(nodeMap, TileSet);
        }

       
    }
}