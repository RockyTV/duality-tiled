using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Duality.Resources;
using Duality;
using Duality.Editor;
using System.IO;


namespace Tilety.Core
{
    [Serializable]
    [EditorHintFlags(MemberFlags.AffectsOthers)]
    public class TmxTileset
    {
        /// <summary>
        /// The first global tile ID of this tileset (this global ID maps to the first tile in this tileset).
        /// </summary>
        public int FirstGlobalID = 1;
        /// <summary>
        /// If this tileset is stored in an external TSX (Tile Set XML) file, this attribute refers to that file.
        /// </summary>
        [EditorHintFlags(MemberFlags.ReadOnly)]
        public string Source = string.Empty;
        /// <summary>
        /// The name of this tileset.
        /// </summary>
        public string Name = string.Empty;
        /// <summary>
        /// The (maximum) width of the tiles in this tileset.
        /// </summary>
        public int TileWidth = 32;
        /// <summary>
        /// The (maximum) height of the tiles in this tileset.
        /// </summary>
        public int TileHeight = 32;
        /// <summary>
        /// The spacing in pixels between the tiles in this tileset (applies to the tileset image).
        /// </summary>
        public int Spacing = 0;
        /// <summary>
        /// The margin around the tiles in this tileset (applies to the tileset image).
        /// </summary>
        public int Margin = 0;
        /// <summary>
        /// Used to specify an offset in pixels, to be applied when drawing a tile from the related tileset. When not present, no offset is applied.
        /// </summary>
        public TmxTileOffset TileOffset = TmxTileOffset.Zero;
        /// <summary>
        /// Wraps any number of custom properties. 
        /// Can be used as a child of the map, tile (when part of a tileset), layer, objectgroup and object elements.
        /// </summary>
        public Dictionary<string, string> Properties = new Dictionary<string, string>();
        /// <summary>
        /// Each tileset has a single image associated with it, which is cut into smaller tiles based on the attributes defined on the tileset element.
        /// </summary>
        public Pixmap Image;
        /// <summary>
        /// Defines an array of terrain types.
        /// </summary>
        public List<TmxTerrain> TerrainTypes = new List<TmxTerrain>();
        public List<TmxTilesetTile> Tiles = new List<TmxTilesetTile>();

        public static TmxTileset FromXml(XDocument xmlDoc)
        {
            TmxTileset tmxTileset = new TmxTileset();
            foreach (XElement mainNode in xmlDoc.Descendants())
            {
                // We need to parse the tileset node only
                if (mainNode.Name == "tileset")
                {
                    int parseInt;
                    OpenFileDialog ofd;

                    tmxTileset.Name = mainNode.GetAttributeValue("name");

                    if (int.TryParse(mainNode.GetAttributeValue("firstgid"), out parseInt))
                        tmxTileset.FirstGlobalID = parseInt;
                    if (int.TryParse(mainNode.GetAttributeValue("tilewidth"), out parseInt))
                        tmxTileset.TileWidth = parseInt;
                    if (int.TryParse(mainNode.GetAttributeValue("tileheight"), out parseInt))
                        tmxTileset.TileHeight = parseInt;

                    // Since Duality does not let me get the original path for the imported file
                    // we create a dialog for the user let us know where the tileset images are located at.
                    XElement imageElement = mainNode.Element("image");
                    if (imageElement != null)
                    {
                        ofd = new OpenFileDialog();
                        ofd.CheckFileExists = true;
                        ofd.CheckPathExists = true;
                        ofd.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.gif, *.png, *.bmp) | *.jpg; *.jpeg; *.jpe; *.gif; *.png; *.bmp";
                        ofd.Title = "Import tileset image";
                        ofd.Multiselect = false;
                        ofd.InitialDirectory = imageElement.GetAttributeValue("source");
                        ofd.FileName = imageElement.GetAttributeValue("source");
                        DialogResult dialogResult = ofd.ShowDialog();

                        if (dialogResult == DialogResult.OK)
                        {
                            string ofdFile = ofd.FileName;
                            FileInfo imageFileInfo = new FileInfo(ofdFile);
                            tmxTileset.Image = new Pixmap(imageFileInfo.FullName);
                        }
                        else
                        {
                            tmxTileset.Image = null;
                        }
                    }

                    // Parse tileset child elements
                    foreach (XElement childElement in mainNode.Descendants())
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

                        if (childElement.Name == "properties")
                        {
                            foreach (XElement propertyElem in childElement.Descendants())
                            {
                                tmxTileset.Properties.Add(propertyElem.GetAttributeValue("name"), propertyElem.GetAttributeValue("value"));
                            }
                        }
                    }
                }
            }
            return tmxTileset;
        }
    }

    [Serializable]
    public struct TmxTileOffset
    {
        public int X;
        public int Y;

        public TmxTileOffset(int X)
        {
            this.X = X;
            this.Y = X;
        }

        public TmxTileOffset(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public static readonly TmxTileOffset Zero = new TmxTileOffset(0);
    }
}
