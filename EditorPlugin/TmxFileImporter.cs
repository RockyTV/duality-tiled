using System.IO;

using Duality;
using Duality.Editor;
using Duality.Resources;

using DualityTiled.Core;

namespace DualityTiled.Editor
{
    public class TmxFileImporter : IFileImporter
    {
        public static readonly string SourceFileExtPrimary = ".tmx";

        public bool CanImportFile(string srcFile)
        {
            string extension = Path.GetExtension(srcFile).ToLower();
            return extension == SourceFileExtPrimary;
        }

        public bool CanReImportFile(ContentRef<Resource> r, string srcFile)
        {
            return r.Is<TmxMap>();
        }

        public string[] GetOutputFiles(string srcFile, string targetName, string targetDir)
        {
            string targetResPath = PathHelper.GetFreePath(Path.Combine(targetDir, targetName), Resource.GetFileExtByType(typeof(TmxMap)));
            return new string[] { targetResPath };
        }

        public void ImportFile(string srcFile, string targetName, string targetDir)
        {
            string[] output = this.GetOutputFiles(srcFile, targetName, targetDir);
            TmxMap tmxMap = new TmxMap();
            tmxMap.LoadMapData(srcFile);
            tmxMap.Save(output[0]);
        }

        public bool IsUsingSrcFile(ContentRef<Resource> r, string srcFile)
        {
            ContentRef<TmxMap> tmxMap = r.As <TmxMap>();
            return tmxMap != null && tmxMap.Res.SourcePath == srcFile;
        }

        public void ReImportFile(ContentRef<Resource> r, string srcFile)
        {
            TmxMap tmxMap = r.Res as TmxMap;
            tmxMap.SourcePath = srcFile;
            tmxMap.LoadMapData(srcFile);
        }
    }
}
