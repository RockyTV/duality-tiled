using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Drawing;

using DualityTiled.Core;
using DualityTiled.Core.Properties;

using Duality;
using Duality.Editor;
using Duality.Editor.Properties;
using Duality.Properties;
using System.Diagnostics;

namespace DualityTiled.Editor
{
    public class TmxEditorAction : EditorSingleAction<TmxMap>
    {
        public override string Description
        {
            get { return TiledRes.ActionDesc_OpenTmxMapExternal; }
        }

        public override void Perform(TmxMap obj)
        {
            if (obj != null)
                FileImportProvider.OpenSourceFile(obj, TmxFileImporter.SourceFileExtPrimary, obj.SaveMapData);
        }

        public override bool MatchesContext(string context)
        {
            return context == DualityEditorApp.ActionContextOpenRes;
        }
    }
}
