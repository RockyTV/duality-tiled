using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

using System.Xml.Serialization;

using DualityTiled.Extensions;

using Duality;
using Duality.Editor;
using Duality.Drawing;
using Duality.Resources;
using Duality.Properties;

namespace DualityTiled.Core
{
    /// <summary>
    /// Defines a Tiled map file (.tmx) resource to be used with Duality.
    /// </summary>
    [Serializable]
    [ExplicitResourceReference()]
    [EditorHintCategory(typeof(CoreRes), "Tiled")]
    public class TmxMap : Resource
    {
        // This field never changes in the xml files.
        /// <summary>
        /// The TMX format version, generally 1.0.
        /// </summary>
        [EditorHintFlags(MemberFlags.ReadOnly)]
        public readonly float Version = 1f;
        /// <summary>
        /// Map orientation. 
        /// Tiled supports "orthogonal", "isometric" and "staggered" (since 0.9) at the moment.
        /// </summary>
        public TmxMapOrientation Orientation = TmxMapOrientation.ISOMETRIC;
        /// <summary>
        /// The map width in tiles.
        /// </summary>
        public int Width = 100;
        /// <summary>
        /// The map height in tiles.
        /// </summary>
        public int Height = 100;
        /// <summary>
        /// The width of a tile.
        /// </summary>
        public int TileWidth = 32;
        /// <summary>
        /// The height of a tile.
        /// </summary>
        public int TileHeight = 32;
        /// <summary>
        /// The background color of the map.
        /// </summary>
        public ColorRgba BackgroundColor = ColorRgba.Grey;
        /// <summary>
        /// The order in which tiles on tile layers are rendered. 
        /// Valid values are right-down (the default), right-up, left-down and left-up.
        /// </summary>
        public TmxTileRenderOrder RenderOrder = TmxTileRenderOrder.RIGHT_DOWN;
        /// <summary>
        /// The encoding used to encode the tile layer data.
        /// </summary>
        public TmxLayerEncoding LayerFormat = TmxLayerEncoding.Xml;
        public int NextObjectID = 1;
        /// <summary>
        /// Wraps any number of custom properties. 
        /// Can be used as a child of the map, tile (when part of a tileset), layer, objectgroup and object elements.
        /// </summary>
        public Dictionary<string, string> Properties = new Dictionary<string,string>();
        public List<TmxTileset> Tilesets = new List<TmxTileset>();
        public List<TmxLayer> Layers = new List<TmxLayer>();

        /// <summary>
        /// Load map data into a map resource file.
        /// </summary>
        public void LoadMapData(string srcFile)
        {
            Log.Editor.Write("Importing file " + srcFile);
            Log.Editor.Write("Source: {0}", System.IO.Path.GetDirectoryName(srcFile));

            using (FileStream fs = File.Open(srcFile, FileMode.Open, FileAccess.ReadWrite))
            {
                StreamReader sr = new StreamReader(fs);

                XDocument xmlDoc = XDocument.Load(fs);
                foreach (XElement mapNode in xmlDoc.Descendants())
                {
                    if (mapNode.Name == "map")
                    {
                        int tryParse;

                        if (int.TryParse(mapNode.GetAttributeValue("width"), out tryParse))
                            this.Width = tryParse;
                        if (int.TryParse(mapNode.GetAttributeValue("height"), out tryParse))
                            this.Height = tryParse;
                        if (int.TryParse(mapNode.GetAttributeValue("tilewidth"), out tryParse))
                            this.TileWidth = tryParse;
                        if (int.TryParse(mapNode.GetAttributeValue("tileheight"), out tryParse))
                            this.TileHeight = tryParse;
                        if (int.TryParse(mapNode.GetAttributeValue("nextobjectid"), out tryParse))
                            this.NextObjectID = tryParse;

                        TmxMapOrientation orientation;
                        TmxTileRenderOrder renderOrder;

                        if (Enum.TryParse(mapNode.GetAttributeValue("orientation").ToUpperInvariant(), out orientation))
                            this.Orientation = orientation;
                        if (Enum.TryParse(mapNode.GetAttributeValue("renderorder").ToUpperInvariant().Replace("-", "_"), out renderOrder))
                            this.RenderOrder = renderOrder;

                        if (mapNode.GetAttributeValue("backgroundcolor") != null)
                        {
                            // Substring it so we don't include the hashtag.
                            string bgColorHex = mapNode.GetAttributeValue("backgroundcolor").Substring(1) + "FF";

                            // Hacky way to parse HEX colors
                            int outHex;

                            if (int.TryParse(bgColorHex, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out outHex))
                                this.BackgroundColor = new ColorRgba(outHex);
                        }

                        TmxTileset tmxTileset = null;
                        TmxLayer tmxLayer = null;

                        OpenFileDialog ofd = null;

                        // Parsing child elements
                        foreach (XElement descendantNode in mapNode.Descendants())
                        {
                            // Tileset
                            if (descendantNode.Name == "tileset" && descendantNode.Parent == mapNode)
                            {
                                // Check if the tileset is defined in an external file.
                                // If it is, we parse the file.
                                if (descendantNode.GetAttributeValue("source") != null)
                                {
                                    ofd = new OpenFileDialog();
                                    ofd.CheckFileExists = true;
                                    ofd.CheckPathExists = true;
                                    ofd.Filter = "Tile Set XML files (*.tsx) | *.tsx";
                                    ofd.Title = "Import tileset TSX file";
                                    ofd.Multiselect = false;
                                    ofd.InitialDirectory = descendantNode.GetAttributeValue("source");
                                    ofd.FileName = descendantNode.GetAttributeValue("source");
                                    DialogResult dialogResult = ofd.ShowDialog();

                                    if (dialogResult == DialogResult.OK)
                                    {
                                        string ofdFile = ofd.FileName;
                                        using (FileStream tsxStream = File.Open(ofdFile, FileMode.Open, FileAccess.ReadWrite))
                                        {
                                            XDocument tsxDoc = XDocument.Load(tsxStream);
                                            tmxTileset = TmxTileset.FromXml(tsxDoc);
                                            tmxTileset.Source = ofdFile;
                                        }
                                    }
                                }
                                else
                                {
                                    tmxTileset = TmxTileset.FromXml(xmlDoc);
                                    tmxTileset.Source = srcFile;
                                }
                                tmxTileset.Image.AnimCols = tmxTileset.Image.Width / this.TileWidth;
                                tmxTileset.Image.AnimRows = tmxTileset.Image.Height / this.TileHeight;
                                this.Tilesets.Add(tmxTileset);
                            }

                            // Layer
                            if (descendantNode.Name == "layer" && descendantNode.Parent == mapNode)
                            {
                                tmxLayer = TmxLayer.FromXmlElement(descendantNode);
                                this.Layers.Add(tmxLayer);
                            }

                            // Properties
                            if (descendantNode.Name == "properties")
                            {
                                switch (descendantNode.Parent.Name.ToString())
                                {
                                    case "map":
                                        foreach (XElement propertyElem in descendantNode.Descendants())
                                        {
                                            this.Properties.Add(propertyElem.GetAttributeValue("name"), propertyElem.GetAttributeValue("value"));
                                        }
                                        break;
                                    case "layer":
                                        if (tmxLayer != null)
                                        {
                                            foreach (XElement propertyElem in descendantNode.Descendants())
                                            {
                                                tmxLayer.Properties.Add(propertyElem.GetAttributeValue("name"), propertyElem.GetAttributeValue("value"));
                                            }
                                        }
                                        break;
                                    /*case "objectgroup": // not yet implemented
                                        break;
                                    case "object":
                                        break;
                                    case "imagelayer":
                                        break;*/
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Save map data into a Tiled Map File (.tmx)
        /// </summary>
        /// <param name="destFile">The path of the file you want to save the map data to.</param>
        public void SaveMapData(string destFile)
        {
            Log.Editor.Write("Saving map data to '{0}'", destFile);

            using (FileStream fs = File.Create(destFile))
            {
                StreamWriter sw = new StreamWriter(fs);

                XDocument xmlDoc = new XDocument();

                XElement mapElement = new XElement("map");
                mapElement.SetAttributeValue("version", this.Version);
                mapElement.SetAttributeValue("orientation", this.Orientation.ToString().ToLowerInvariant());
                mapElement.SetAttributeValue("renderorder", this.RenderOrder.ToString().ToLowerInvariant().Replace("_", "-")); // Tiled uses hyphens instead of underlines
                mapElement.SetAttributeValue("width", this.Width);
                mapElement.SetAttributeValue("height", this.Height);
                mapElement.SetAttributeValue("tilewidth", this.TileWidth);
                mapElement.SetAttributeValue("tileheight", this.TileHeight);
                mapElement.SetAttributeValue("backgroundcolor", this.BackgroundColor.ToTiledHexString());
                mapElement.SetAttributeValue("nextobjectid", this.NextObjectID);
                xmlDoc.Add(mapElement);

                XElement propertiesElement = new XElement("properties");
                foreach (KeyValuePair<string, string> property in this.Properties)
                {
                    XElement propertyElement = new XElement("property");
                    propertyElement.SetAttributeValue("name", property.Key);
                    propertyElement.SetAttributeValue("value", property.Value);

                    propertiesElement.Add(propertyElement);
                }
                mapElement.Add(propertiesElement);

                foreach (TmxTileset tmxTileset in this.Tilesets)
                {
                    XElement tilesetElement = new XElement("tileset");
                    tilesetElement.SetAttributeValue("firstgid", tmxTileset.FirstGlobalID);
                    if (System.IO.Path.GetExtension(tmxTileset.Source) == ".tsx")
                        tilesetElement.SetAttributeValue("source", tmxTileset.Source.Replace(@"\", "/"));
                    else
                    {
                        tilesetElement.SetAttributeValue("name", tmxTileset.Name);
                        tilesetElement.SetAttributeValue("tilewidth", this.TileWidth);
                        tilesetElement.SetAttributeValue("tileheight", this.TileHeight);
                        XElement imgElement = new XElement("image");
                        imgElement.SetAttributeValue("source", tmxTileset.Image.SourcePath.Replace(@"\", "/"));
                        imgElement.SetAttributeValue("width", tmxTileset.Image.Width);
                        imgElement.SetAttributeValue("height", tmxTileset.Image.Height);

                        propertiesElement = new XElement("properties");
                        foreach (KeyValuePair<string, string> property in tmxTileset.Properties)
                        {
                            XElement propertyElement = new XElement("property");
                            propertyElement.SetAttributeValue("name", property.Key);
                            propertyElement.SetAttributeValue("value", property.Value);

                            propertiesElement.Add(propertyElement);
                        }

                        tilesetElement.Add(imgElement);
                        tilesetElement.Add(propertiesElement);
                    }

                    mapElement.Add(tilesetElement);
                }

                xmlDoc.Save(sw.BaseStream);
            }
        }
    }
}
