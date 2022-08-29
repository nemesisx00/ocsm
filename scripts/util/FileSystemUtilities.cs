using System;
using System.IO;

namespace OCSM
{
	public class FileSystemUtilities
	{
		private const string App = "/OCSM/";
		private const string Sheets = App + "sheets/";
		private const string Metadata = App + "metadata/";
		
		public static string DefaultSheetDirectory { get { return CreatePathIfNotExists(AutoLower(GetFinalPath(Environment.SpecialFolder.ApplicationData, Sheets))); } }
		public static string DefaultMetadataDirectory { get { return CreatePathIfNotExists(AutoLower(GetFinalPath(Environment.SpecialFolder.CommonApplicationData, Metadata))); } }
		
		public static string ReadString(string path)
		{
			var finalPath = Path.GetFullPath(path);
			if(File.Exists(finalPath))
				return File.ReadAllText(finalPath);
			return null;
		}
		
		public static void WriteString(string path, string data)
		{
			var finalPath = CreatePathIfNotExists(Path.GetFullPath(path));
			File.WriteAllText(finalPath, data);
		}
		
		private static string AutoLower(string path)
		{
			if(Environment.OSVersion.Platform.Equals(PlatformID.Unix) || Environment.OSVersion.Platform.Equals(PlatformID.MacOSX))
				return path.ToLower();
			return path;
		}
		
		private static string GetFinalPath(Environment.SpecialFolder folder, string pathFragment)
		{
			return Path.GetFullPath(Environment.GetFolderPath(folder) + pathFragment);
		}
		
		private static string CreatePathIfNotExists(string path)
		{
			Directory.CreateDirectory(Path.GetDirectoryName(path));
			return path;
		}
	}
}
