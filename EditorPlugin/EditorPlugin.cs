using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality.Editor;

namespace DualityTiled.Editor
{
	/// <summary>
	/// Defines a Duality editor plugin.
	/// </summary>
    public class Main : EditorPlugin
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
