using System;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using System.Xml;
using System.Collections.Generic;
using DualityTiled.Extensions;
using DualityTiled.Properties;

using Duality;
using Duality.IO;
using Duality.Editor;
using Duality.Drawing;

namespace DualityTiled.Core
{
	/// <summary>
	/// Defines a Tiled map file (.tmx) resource to be used with Duality.
	/// </summary>
	[ExplicitResourceReference()]
	[EditorHintCategory("Tiled")]
	[EditorHintImage(TiledResNames.IconResourceMap)]
	public class TmxMap : Resource
	{
		[EditorHintFlags(MemberFlags.ReadOnly)]
		public readonly float Version = 1.0f;

		private TmxMapOrientation orientation = TmxMapOrientation.ISOMETRIC;
		public TmxMapOrientation Orientation
		{
			get { return orientation; }
			set { orientation = value; }
		}

		private int mapWidth = 100;
		public int MapWidth
		{
			get { return mapWidth; }
			set { mapWidth = value; }
		}

		private int mapHeight = 100;
		public int MapHeight
		{
			get { return mapHeight; }
			set { mapHeight = value; }
		}

		private int tileWidth = 32;
		public int TileWidth
		{
			get { return tileWidth; }
			set { tileWidth = value; }
		}

		private int tileHeight = 32;
		public int TileHeight
		{
			get { return tileHeight; }
			set { tileHeight = value; }
		}

		private ColorRgba backgroundColor = ColorRgba.Grey;
		public ColorRgba BackgroundColor
		{
			get { return backgroundColor; }
			set { backgroundColor = value; }
		}
		private TmxTileRenderOrder renderOrder = TmxTileRenderOrder.RIGHT_DOWN;
		public TmxTileRenderOrder RenderOrder
		{
			get { return renderOrder; }
			set { renderOrder = value; }
		}

		private TmxLayerEncoding layerFormat = TmxLayerEncoding.Xml;
		public TmxLayerEncoding LayerFormat
		{
			get { return layerFormat; }
			set { layerFormat = value; }
		}

		private int nextObjectID = 1;
		public int NextObjectID
		{
			get { return nextObjectID; }
			set { nextObjectID = value; }
		}
		private Dictionary<string, string> properties = new Dictionary<string, string>();
		public Dictionary<string, string> Properties
		{
			get { return properties; }
			set { properties = value; }
		}

		private List<ContentRef<TmxTileset>> tilesets = new List<ContentRef<TmxTileset>>();
		public List<ContentRef<TmxTileset>> Tilesets
		{
			get { return tilesets; }
			set { tilesets = value; }
		}

		private List<TmxLayer> layers = new List<TmxLayer>();
		public List<TmxLayer> Layers
		{
			get { return layers; }
			set { layers = value; }
		}

		private byte[] rawData = new byte[0];
		[EditorHintFlags(MemberFlags.ReadOnly)]
		public byte[] RawData
		{
			get { return rawData; }
			set { rawData = value; }
		}

		public void LoadMapData()
		{
			if (rawData != null)
			{
				
				using (MemoryStream ms = new MemoryStream(rawData))
				{
					XDocument xmlDoc = XDocument.Load(ms);
					foreach (XElement descendantNode in xmlDoc.Descendants())
					{
						if (descendantNode.Name == "map")
						{
							int tryParse;

							if (int.TryParse(descendantNode.GetAttributeValue("width"), out tryParse))
								this.mapWidth = tryParse;
							if (int.TryParse(descendantNode.GetAttributeValue("height"), out tryParse))
								this.mapHeight = tryParse;
							if (int.TryParse(descendantNode.GetAttributeValue("tilewidth"), out tryParse))
								this.tileWidth = tryParse;
							if (int.TryParse(descendantNode.GetAttributeValue("tileheight"), out tryParse))
								this.tileHeight = tryParse;

						}
					}
				}
			}
		}

		public void SaveMapData(string srcFile)
		{

		}

		protected override void OnDisposing(bool manually)
		{
			base.OnDisposing(manually);
			this.properties.Clear();
			this.tilesets.Clear();
			this.layers.Clear();
		}
	}
}
