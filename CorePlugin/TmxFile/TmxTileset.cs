using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality.Resources;
using Duality;

namespace Tilety.Core
{
    [Serializable]
    public class TmxTileset
    {
        public int FirstGlobalID = 1;
        public string Source = string.Empty;
        public string Name = string.Empty;
        public int TileWidth = 32;
        public int TileHeight = 32;
        public int Spacing = 0;
        public int Margin = 0;
        public TmxTileOffset TileOffset = TmxTileOffset.Zero;
        public Dictionary<string, string> Properties = new Dictionary<string, string>();
        public Pixmap Image;
        public List<TmxTerrain> TerrainTypes = new List<TmxTerrain>();
        public List<TmxTilesetTile> Tiles = new List<TmxTilesetTile>();
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
