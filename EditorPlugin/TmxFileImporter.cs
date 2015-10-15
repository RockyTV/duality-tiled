using System;
using System.IO;
using System.Linq;

using Duality;
using Duality.Resources;
using Duality.Editor;
using Duality.Editor.AssetManagement;

using DualityTiled.Core;

namespace DualityTiled.Editor
{
    public class TmxFileImporter : IAssetImporter
    {
        public static readonly string SourceFileExtPrimary = ".tmx";
		public static readonly string SourceFileExtSecondary = ".tsx";
		private static readonly string[] SourceFileExts = new[] { SourceFileExtPrimary, SourceFileExtSecondary };

		public string Id
		{
			get { return "TmxAssetImporter"; }
		}

		public string Name
		{
			get { return "Tiled Map Importer"; }
		}

		public int Priority
		{
			get { return 0; }
		}

		

		public void PrepareImport(IAssetImportEnvironment env)
		{
			// Ask to handle all input that matches the conditions in AcceptsInput
			foreach (AssetImportInput input in env.HandleAllInput(this.AcceptsInput))
			{
				// For all handled input items, specify which Resource the importer intends to create / modify
				string fileExtension = Path.GetExtension(input.RelativePath);

				// Check if the file is a map or a tileset
				if (fileExtension == SourceFileExtPrimary)
					env.AddOutput<TmxMap>(input.AssetName, input.Path);
				else if (fileExtension == SourceFileExtSecondary)
					env.AddOutput<TmxTileset>(input.AssetName, input.Path);
			}
		}

		public void Import(IAssetImportEnvironment env)
		{
			// Handle all available input. No need to filter or ask for this anymore, as
			// the preparation step already made a selection with AcceptsInput. We won't
			// get any input here that didn't match.
			foreach (AssetImportInput input in env.Input)
			{
				string fileExtension = Path.GetExtension(input.RelativePath);

				if (fileExtension == SourceFileExtPrimary)
				{
					// Request a target Resource with a name matching the input
					ContentRef<TmxMap> targetRef = env.GetOutput<TmxMap>(input.AssetName);

					// If we successfully acquired one, proceed with the import
					if (targetRef.IsAvailable)
					{
						TmxMap target = targetRef.Res;

						// Update map data from the input file
						target.RawData = File.ReadAllBytes(input.Path);
						target.LoadMapData();

						// Add the requested output to signal that we've done something with it
						env.AddOutput(targetRef, input.Path);
					}
				}
				else if (fileExtension == SourceFileExtSecondary)
				{
					// Request a target Resource with a name matching the input
					ContentRef<TmxTileset> targetRef = env.GetOutput<TmxTileset>(input.AssetName);

					// If we successfully acquired one, proceed with the import
					if (targetRef.IsAvailable)
					{
						TmxTileset target = targetRef.Res;

						// Update map data from the input file
						target.RawData = File.ReadAllBytes(input.Path);

						// Add the requested output to signal that we've done something with it
						env.AddOutput(targetRef, input.Path);
					}
				}
			}
		}

		public void PrepareExport(IAssetExportEnvironment env)
		{
			// We can export any Resource that is a TmxMap with a XML data
			TmxMap input = env.Input as TmxMap;
			if (input != null)
			{
				// Add the file path of the exported output we'll produce.
				env.AddOutputPath(env.Input.Name + SourceFileExtPrimary);
			}
		}

		public void Export(IAssetExportEnvironment env)
		{
			// Determine input and output path
			TmxMap input = env.Input as TmxMap;
			string outputPath = env.AddOutputPath(input.Name + SourceFileExtPrimary);

			// Take the input Resource's map data and save it at the specified location
			input.SaveMapData(outputPath);
		}

		private bool AcceptsInput(AssetImportInput input)
		{
			string inputFileExt = Path.GetExtension(input.Path);
			bool matchingFileExt = SourceFileExts.Any(acceptedExt => string.Equals(inputFileExt, acceptedExt, StringComparison.CurrentCultureIgnoreCase));
			return matchingFileExt;
		}
    }
}
