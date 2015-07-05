using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Microsoft.Win32;

using Duality.Editor;
using Duality.Serialization;

namespace DualityTiled.Editor
{
	/// <summary>
	/// Defines a Duality editor plugin.
	/// </summary>
    public class DualityTiled : EditorPlugin
	{

		public override string Id
		{
			get { return "DualityTiled"; }
		}

        protected override void LoadPlugin()
        {
            base.LoadPlugin();
        }
	}
}
