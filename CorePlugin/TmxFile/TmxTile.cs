using System;
using System.Collections.Generic;

namespace DualityTiled.Core
{
    public class TmxTilesetTile
    {
        public int ID;
        public TmxTerrain Terrain;
        public float Probability;
        public Dictionary<string, string> Properties = new Dictionary<string, string>();
    }

    public struct TmxTile
    {
        public int GlobalID;
    }
}
