using System;
using System.Xml;
using System.Collections.Generic;

namespace ProgettoFinale
{
    class TmxNodeParser
    {
        public static TileSet ParseTileset(XmlNode nodeMap)
        {
            XmlNode tilesetNode = nodeMap.SelectSingleNode("tileset");
            int tileW = int.Parse(tilesetNode.Attributes.GetNamedItem("tilewidth").InnerText);
            int tileH = int.Parse(tilesetNode.Attributes.GetNamedItem("tileheight").InnerText);
            XmlNode tsImgNode = tilesetNode.SelectSingleNode("image");
            string tsImgPath = "Assets/" + tsImgNode.Attributes.GetNamedItem("source").InnerText;
            int tsImgWidth = int.Parse(tsImgNode.Attributes.GetNamedItem("width").InnerText);
            int tsImgHeigh = int.Parse(tsImgNode.Attributes.GetNamedItem("height").InnerText);

            TileSet result = TileSetFactory.Create(tileW, tileH, tsImgPath, tsImgWidth, tsImgHeigh);

            XmlNodeList tileNodes = tilesetNode.SelectNodes("tile");

            Dictionary<int, TileProperties> propsPerTile = ParseTilesetProperties(tileNodes);
            foreach(int tileId in propsPerTile.Keys)
            {
                TileType t = result.At(tileId);
                t.Props = propsPerTile[tileId];
            }
            return result;
        }

        private static Dictionary<int, TileProperties> ParseTilesetProperties(XmlNodeList tileNodes)
        {
            Dictionary<int, TileProperties> result = new Dictionary<int, TileProperties>();
            foreach(XmlNode tileNode in tileNodes)
            {
                int id = int.Parse(tileNode.Attributes.GetNamedItem("id").InnerText);
                XmlNodeList propNodes = tileNode.SelectNodes("properties/property");
                result[id] = ParseProperties(propNodes);
            }
           
            return result;
        }

        private static TileProperties ParseProperties(XmlNodeList propNodes)
        {
            TileProperties result = new TileProperties();
            foreach (XmlNode propNode in propNodes)
            {
                string name = propNode.Attributes.GetNamedItem("name").InnerText;
                string value = propNode.Attributes.GetNamedItem("value").InnerText;
                XmlNode typeNode = propNode.Attributes.GetNamedItem("type");
                string type = "string";
                if (typeNode != null)
                {
                    type = typeNode.InnerText;
                }


                if (type.Equals("bool"))
                {
                    result.SetBool(name, bool.Parse(value));
                } else if (type.Equals("string"))
                {
                    result.SetString(name, value);
                }
            }
            return result;
        }

        public static List<Layer> ParseLayers(XmlNode nodeMap, TileSet ts)
        {
            List<Layer> result = new List<Layer>();

            XmlNodeList layerNodes = nodeMap.SelectNodes("layer");

            foreach(XmlNode each in layerNodes)
            {
                result.Add(ParseLayer(each, ts));
            }
            return result;
        }

        public static Layer ParseLayer(XmlNode layerNode, TileSet ts)
        {
            string name = layerNode.Attributes.GetNamedItem("name").InnerText;
            int layerCols = int.Parse(layerNode.Attributes.GetNamedItem("width").InnerText);
            int layerRows = int.Parse(layerNode.Attributes.GetNamedItem("height").InnerText);
            XmlNode dataNode = layerNode.SelectSingleNode("data");
            string csvString = dataNode.InnerText;
            csvString = csvString.Replace("\r\n", "").Replace("\n", "");
            TileGrid tg = TileGridFactory.Create(layerRows, layerCols, csvString, ts);

            Layer result = new Layer(name, tg);

            XmlNodeList propNodes = layerNode.SelectNodes("properties/property");
            result.Props = ParseProperties(propNodes);
            return result;
        }
    }
}