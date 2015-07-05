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
        private static DualityTiled singleton = new DualityTiled();
        public static DualityTiled fetch
        {
            get { return singleton; }
        }

        private string userDataPath = Path.Combine(Environment.CurrentDirectory, "DualityTiled.dat");

        private PluginUserData userData = null;
        public PluginUserData UserData
        {
            get { return this.userData; }
            set { this.userData = value ?? new PluginUserData(); }
        }

		public override string Id
		{
			get { return "DualityTiled"; }
		}

        protected override void LoadPlugin()
        {
            base.LoadPlugin();

            this.LoadUserData();
        }

        protected override void SaveUserData(System.Xml.Linq.XElement node)
        {
            base.SaveUserData(node);
            this.SaveUserData();
        }

        private void SaveUserData()
        {
            Formatter.WriteObject(this.userData, this.userDataPath, FormattingMethod.Xml);
        }

        private void LoadUserData()
        {
            this.userData = Formatter.TryReadObject<PluginUserData>(this.userDataPath) ?? new PluginUserData();
        }
	}
}
