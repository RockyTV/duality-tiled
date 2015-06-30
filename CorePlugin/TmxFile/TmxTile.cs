using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DualityTiled.Core
{
    [Serializable]
    public class TmxTilesetTile
    {
        public int ID;
        public TmxTerrain Terrain;
        public float Probability;
        public Dictionary<string, string> Properties = new Dictionary<string, string>();
    }

    [Serializable]
    public struct TmxTile
    {
        public int GlobalID;
    }
}
