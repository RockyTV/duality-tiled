using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

using Duality;
using Duality.Editor;

using DualityTiled.Extensions;

namespace DualityTiled.Core
{
    public class TmxLayer
    {
        public string Name;
        public float Opacity;
        public bool Visible;
        public TmxData Data;
        public Dictionary<string, string> Properties = new Dictionary<string, string>();

        public static TmxLayer FromXmlElement(XElement xmlElem)
        {
            TmxLayer tmxLayer = new TmxLayer();
            TmxData tmxData = new TmxData();
            tmxData.Tiles = new List<TmxTile>();

            tmxLayer.Name = xmlElem.GetAttributeValue("name");
            
            foreach (XElement childElem in xmlElem.Descendants())
            {
                if (childElem.Name == "data")
                {
                    TmxLayerEncoding encoding;
                    TmxBase64Compression compression = TmxBase64Compression.None;

                    if (Enum.TryParse(childElem.GetAttributeValue("encoding").CapitalizeFirstLetter(), out encoding))
                        tmxData.Encoding = encoding;
                    if (childElem.Attribute("compression") != null)
                        if (Enum.TryParse(childElem.GetAttributeValue("compression").CapitalizeFirstLetter(), out compression))
                            tmxData.Compression = compression;

                    if (tmxData.Encoding == TmxLayerEncoding.Xml)
                    {
                        foreach (XElement tileNode in childElem.Descendants())
                        {
                            TmxTile tmxTile = new TmxTile();

                            int outInt;

                            if (int.TryParse(tileNode.GetAttributeValue("gid"), out outInt))
                                tmxTile.GlobalID = outInt;

                            tmxData.Tiles.Add(tmxTile);
                        }
                    }
                }
            }

            tmxLayer.Data = tmxData;
            return tmxLayer;
        }
    }

    public struct TmxData
    {
        [EditorHintFlags(MemberFlags.ReadOnly)]
        public TmxLayerEncoding Encoding;
        [EditorHintFlags(MemberFlags.ReadOnly)]
        public TmxBase64Compression Compression;
        public List<TmxTile> Tiles;
    }
}
