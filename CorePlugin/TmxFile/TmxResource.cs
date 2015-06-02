using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Collections.Generic;

using Duality;
using Duality.Editor;
using Duality.Drawing;
using Duality.Resources;
using Duality.Properties;

using Tilety.Core.Extensions;

namespace Tilety.Core
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
        public readonly float Version = 1.0f;
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

                        TmxMapOrientation orientation;
                        TmxTileRenderOrder renderOrder;

                        if (Enum.TryParse(mapNode.GetAttributeValue("orientation").ToUpperInvariant(), out orientation))
                            this.Orientation = orientation;
                        if (Enum.TryParse(mapNode.GetAttributeValue("renderorder").ToUpperInvariant().Replace("-", "_"), out renderOrder))
                            this.RenderOrder = renderOrder;

                        // Substring it so we don't include the hashtag.
                        string bgColorHex = mapNode.GetAttributeValue("backgroundcolor").Substring(1) + "FF";

                        // Hacky way to parse HEX colors
                        int outHex;

                        if (int.TryParse(bgColorHex, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out outHex))
                            this.BackgroundColor = new ColorRgba(outHex);


                        // Parsing properties
                        foreach (XElement descendantNode in mapNode.Descendants())
                        {
                            // Map properties
                            if (descendantNode.Name == "properties" && descendantNode.Parent == mapNode)
                            {
                                foreach (XElement propertyElem in descendantNode.Descendants())
                                {
                                    this.Properties.Add(propertyElem.GetAttributeValue("name"), propertyElem.GetAttributeValue("value"));
                                }
                            }

                            // Tileset
                            if (descendantNode.Name == "tileset" && descendantNode.Parent == mapNode)
                            {
                                TmxTileset tmxTileset = new TmxTileset();
                                tmxTileset.Name = descendantNode.GetAttributeValue("name");

                                string imgFileRelativePath = descendantNode.GetElementValue("image");
                                string sourceFilePath = System.IO.Path.GetDirectoryName(srcFile);

                                Log.Editor.Write(System.IO.Path.GetFullPath(System.IO.Path.Combine(sourceFilePath, imgFileRelativePath)));

                                int outInt;

                                if (int.TryParse(descendantNode.GetAttributeValue("firstgid"), out outInt))
                                    tmxTileset.FirstGlobalID = outInt;
                                if (int.TryParse(descendantNode.GetAttributeValue("tilewidth"), out outInt))
                                    tmxTileset.TileWidth = outInt;
                                if (int.TryParse(descendantNode.GetAttributeValue("tileheight"), out outInt))
                                    tmxTileset.TileHeight = outInt;

                                foreach (XElement childElement in descendantNode.Descendants())
                                {
                                    if (childElement.Name == "terraintypes")
                                    {
                                        foreach (XElement terrainElement in childElement.Descendants())
                                        {
                                            TmxTerrain tmxTerrain = new TmxTerrain();
                                            tmxTerrain.Name = terrainElement.GetAttributeValue("name");

                                            int tryParseInt;
                                            if (int.TryParse(terrainElement.GetAttributeValue("tile"), out tryParseInt))
                                                tmxTerrain.TileID = tryParseInt;

                                            tmxTileset.TerrainTypes.Add(tmxTerrain);
                                        }
                                    }

                                    if (childElement.Name == "tile")
                                    {
                                        TmxTilesetTile tmxTilesetTile = new TmxTilesetTile();

                                        int tryParseInt;
                                        if (int.TryParse(childElement.GetAttributeValue("id"), out tryParseInt))
                                            tmxTilesetTile.ID = tryParseInt;

                                        tmxTileset.Tiles.Add(tmxTilesetTile);
                                    }
                                }

                                this.Tilesets.Add(tmxTileset);
                            }

                            // Layer
                            if (descendantNode.Name == "layer" && descendantNode.Parent == mapNode)
                            {
                                TmxLayer tmxLayer = new TmxLayer();
                                tmxLayer.Name = descendantNode.GetAttributeValue("name");
                                XElement dataElement = descendantNode.Descendants("data").FirstOrDefault();

                                TmxLayerEncoding encoding;
                                TmxBase64Compression compression = TmxBase64Compression.None;

                                if (Enum.TryParse(dataElement.GetAttributeValue("encoding").CapitalizeFirstLetter(), out encoding))
                                {
                                    tmxLayer.Data.Encoding = encoding;
                                    this.LayerFormat = encoding;
                                }

                                if (dataElement.Attribute("compression") != null)
                                {
                                    if (Enum.TryParse(dataElement.GetAttributeValue("compression").CapitalizeFirstLetter(), out compression))
                                        tmxLayer.Data.Compression = compression;
                                }
                                tmxLayer.Data.Compression = compression;

                                this.Layers.Add(tmxLayer);
                            }
                        }
                    }
                }
            }
        }
    }
}
