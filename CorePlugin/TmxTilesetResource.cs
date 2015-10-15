using System;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using System.IO;

using Duality;
using Duality.IO;
using Duality.Editor;
using Duality.Resources;

using DualityTiled.Properties;

namespace DualityTiled.Core
{
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

	[EditorHintCategory("Tiled")]
	[EditorHintImage(TiledResNames.IconResourceTileset)]
	public class TmxTileset : Resource
	{
		private int firstGlobalId = 1;
		/// <summary>
		/// The first global tile ID of this tileset (this global ID maps to the first tile in this tileset).
		/// </summary>
		public int FirstGlobalID
		{
			get { return firstGlobalId; }
			set { firstGlobalId = value; }
		}

		private string source = string.Empty;
		/// <summary>
		/// If this tileset is stored in an external TSX (Tile Set XML) file, this attribute refers to that file.
		/// </summary>
		[EditorHintFlags(MemberFlags.ReadOnly)]
		public string Source
		{
			get { return source; }
			set { source = value; }
		}

		private string tilesetName = string.Empty;
		/// <summary>
		/// The name of this tileset.
		/// </summary>
		public string TilesetName
		{
			get { return tilesetName; }
			set { tilesetName = value; }
		}

		private int tileWidth = 32;
		/// <summary>
		/// The (maximum) width of the tiles in this tileset.
		/// </summary>
		public int TileWidth
		{
			get { return tileWidth; }
			set { tileWidth = value; }
		}

		private int tileHeight = 32;
		/// <summary>
		/// The (maximum) height of the tiles in this tileset.
		/// </summary>
		public int TileHeight
		{
			get { return tileHeight; }
			set { tileHeight = value; }
		}

		private int spacing = 0;
		/// <summary>
		/// The spacing in pixels between the tiles in this tileset (applies to the tileset image).
		/// </summary>
		public int Spacing
		{
			get { return spacing; }
			set { spacing = value; }
		}

		private int margin = 0;
		/// <summary>
		/// The margin around the tiles in this tileset (applies to the tileset image).
		/// </summary>
		public int Margin
		{
			get { return margin; }
			set { margin = value; }
		}

		private TmxTileOffset tileOffset = TmxTileOffset.Zero;
		/// <summary>
		/// Used to specify an offset in pixels, to be applied when drawing a tile from the related tileset. When not present, no offset is applied.
		/// </summary>
		public TmxTileOffset TileOffset
		{
			get { return tileOffset; }
			set { tileOffset = value; }
		}

		private Dictionary<string, string> properties = new Dictionary<string, string>();
		/// <summary>
		/// Wraps any number of custom properties. 
		/// Can be used as a child of the map, tile (when part of a tileset), layer, objectgroup and object elements.
		/// </summary>
		public Dictionary<string, string> Properties
		{
			get { return properties; }
			set { properties = value; }
		}

		private ContentRef<Pixmap> image;
		/// <summary>
		/// Each tileset has a single image associated with it, which is cut into smaller tiles based on the attributes defined on the tileset element.
		/// </summary>
		public Pixmap Image
		{
			get { return image.Res; }
			set { image.Res = value; }
		}

		private List<TmxTerrain> terrainTypes = new List<TmxTerrain>();
		/// <summary>
		/// Defines an array of terrain types.
		/// </summary>
		public List<TmxTerrain> TerrainTypes
		{
			get { return terrainTypes; }
			set { terrainTypes = value; }
		}

		private List<TmxTilesetTile> tiles = new List<TmxTilesetTile>();
		public List<TmxTilesetTile> Tiles
		{
			get { return tiles; }
			set { tiles = value; }
		}

		private byte[] rawData = new byte[0];
		[EditorHintFlags(MemberFlags.ReadOnly)]
		public byte[] RawData
		{
			get { return rawData; }
			set { rawData = value; }
		}

		public void LoadTilesetData()
		{
			if (rawData != null)
			{
				using (MemoryStream ms = new MemoryStream(rawData))
				{
					XDocument xmlDoc = XDocument.Load(ms);
					foreach (XElement childElement in xmlDoc.Descendants())
					{
						if (childElement.Name == "tileset")
						{
							tilesetName = childElement.GetAttributeValue("name");

							int parseInt;
							if (int.TryParse(childElement.GetAttributeValue("firstgid"), out parseInt))
								firstGlobalId = parseInt;
							if (int.TryParse(childElement.GetAttributeValue("tilewidth"), out parseInt))
								tileWidth = parseInt;
							if (int.TryParse(childElement.GetAttributeValue("tileheight"), out parseInt))
								tileHeight = parseInt;
						}
					}
				}
			}
		}

		public void SaveTilesetData(string srcFile = null)
		{

		}

		protected override void OnDisposing(bool manually)
		{
			base.OnDisposing(manually);
			this.terrainTypes.Clear();
			this.tiles.Clear();
		}
	}
}
