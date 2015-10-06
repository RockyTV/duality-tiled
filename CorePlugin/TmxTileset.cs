using System;
using System.Collections.Generic;

using Duality;
using Duality.Editor;
using Duality.Resources;

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

	public class TmxTileset : Resource
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
		public string TilesetName = string.Empty;
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

		public void LoadTilesetData(string srcFile = null)
		{

		}

		public void SaveTilesetData(string srcFile = null)
		{

		}

		protected override void OnDisposing(bool manually)
		{
			base.OnDisposing(manually);
			this.TerrainTypes.Clear();
			this.Tiles.Clear();
		}
	}
}
