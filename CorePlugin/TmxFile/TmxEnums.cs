using System;

namespace Tilety.Core
{
    public enum TmxMapOrientation : int
    {
        /// <summary>
        /// Orthogonal
        /// </summary>
        ORTHOGONAL,
        /// <summary>
        /// Isometric
        /// </summary>
        ISOMETRIC,
        /// <summary>
        /// Staggered
        /// </summary>
        STAGGERED
    }

    public enum TmxTileRenderOrder : int
    {
        RIGHT_DOWN,
        RIGHT_UP,
        LEFT_DOWN,
        LEFT_UP
    }

    public enum TmxLayerEncoding : int
    {
        Xml,
        Base64,
        Csv
    }

    public enum TmxBase64Compression : int
    {
        None,
        Gzip,
        Zlib
    }
}
