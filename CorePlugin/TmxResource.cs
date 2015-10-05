using System;
using System.IO;
using System.Xml.Linq;
using System.Collections.Generic;

using DualityTiled.Core.Properties;
using DualityTiled.Extensions;

using Duality;
using Duality.Editor;
using Duality.Drawing;

namespace DualityTiled.Core
{
	/// <summary>
	/// Defines a Tiled map file (.tmx) resource to be used with Duality.
	/// </summary>
	[EditorHintImage(TiledResNames.ImageMap)]
	[EditorHintCategory("Tiled")]
	public class TmxMap : Resource
	{
		
	}
}
