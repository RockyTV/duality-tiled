using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Microsoft.Win32;

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
            PluginGlobals.TiledLocation = this.GetTiledLocationFromRegistry();

        }

        private string GetTiledLocationFromRegistry()
        {
            // Since Duality does not support other OSes, we will only retrieve Tiled's location if the current OS is Windows.
            if (Environment.OSVersion.Platform != (PlatformID.Unix | PlatformID.MacOSX | PlatformID.Xbox))
            {
                bool x64 = Environment.Is64BitOperatingSystem;
                string installPath = "";

                if (x64)
                    installPath = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Tiled", "DisplayIcon", "");
                else
                    installPath = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Tiled", "DisplayIcon", "");

                installPath = Path.Combine(installPath.Substring(0, installPath.IndexOf(Path.GetFileName(installPath))), "tiled.exe");
                return installPath;
            }
            return null;
        }
	}
}
