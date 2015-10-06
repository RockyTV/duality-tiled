using System;
using System.IO;
using System.Xml.Linq;
using System.Collections.Generic;

using DualityTiled.Core.Properties;
using DualityTiled.Extensions;

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
	[EditorHintImage(TiledResNames.ImageMap)]
	public class TmxMap : Resource
	{
		[EditorHintFlags(MemberFlags.ReadOnly)]
		public readonly float Version = 1.0f;

		public TmxMapOrientation Orientation = TmxMapOrientation.ISOMETRIC;
		public int Width = 100;
		public int Height = 100;
		public int TileWidth = 32;
		public int TileHeight = 32;
		public ColorRgba BackgroundColor = ColorRgba.Grey;
		public TmxTileRenderOrder RenderOrder = TmxTileRenderOrder.RIGHT_DOWN;
		public TmxLayerEncoding LayerFormat = TmxLayerEncoding.Xml;
		public int NextObjectID = 1;
		public Dictionary<string, string> Properties = new Dictionary<string, string>();
		public List<TmxTileset> Tilesets = new List<TmxTileset>();
		public List<TmxLayer> Layers = new List<TmxLayer>();

		public void LoadMapData(string srcFile = null)
		{

		}

		private void SaveMapData(string srcFile)
		{

		}

		protected override void OnDisposing(bool manually)
		{
			base.OnDisposing(manually);
			this.Properties.Clear();
			this.Tilesets.Clear();
			this.Layers.Clear();
		}
	}
}
