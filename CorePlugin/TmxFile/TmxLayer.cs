using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tilety.Core
{
    [Serializable]
    public class TmxLayer
    {
        public string Name;
        public float Opacity;
        public bool Visible;
        public TmxData Data;
        public Dictionary<string, string> Properties = new Dictionary<string, string>();
    }

    [Serializable]
    public struct TmxData
    {
        public TmxLayerEncoding Encoding;
        public TmxBase64Compression Compression;
        public List<TmxTile> Tiles;
    }
}
