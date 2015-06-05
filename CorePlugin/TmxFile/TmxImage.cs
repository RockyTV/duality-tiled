using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tilety.Core.TmxFile
{
    [Serializable]
    public struct TmxImage
    {
        public string Source;
        public int Width;
        public int Height;
    }
}
