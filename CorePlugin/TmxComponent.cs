using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Editor;
using Duality.Properties;

namespace Tilety.Core
{
    [Serializable]
    [RequiredComponent(typeof(TmxMap))]
    [EditorHintCategory(typeof(CoreRes), "Tiled")]
    public class TmxComponent : Component
    {

    }
}
