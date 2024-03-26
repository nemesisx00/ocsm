using System;
using System.IO;

namespace Ocsm;

/// <summary>
/// Class providing convenience methods for interacting with the file system in an OS-agnostic fashion.
/// </summary>
public static class FileSystemUtilities
{
	private const string App = "/Ocsm/";
	private const string Sheets = $"{App}sheets/";
	private const string Metadata = $"{App}metadata/";
	
	/// <summary>
	/// The path to the user-specific default storage directory for saving character sheet files.
	/// </summary>
	/// <remarks>
	/// Uses <c>System.Environment.SpecialFolder.ApplicationData</c> as a base path.
	/// </remarks>
	public static string DefaultSheetDirectory => createPathIfNotExists(
		getFinalPath(
			Environment.SpecialFolder.ApplicationData,
			Sheets
		).ToLower()
	);
	
	/// <summary>
	/// The path to the user-agnostic default storage directory for saving game system metadata files.
	/// </summary>
	/// <remarks>
	/// Uses <c>System.Environment.SpecialFolder.CommonApplicationData</c> as a base path.
	/// </remarks>
	public static string DefaultMetadataDirectory => createPathIfNotExists(
		getFinalPath(
			Environment.SpecialFolder.ApplicationData,
			Metadata
		).ToLower()
	);
	
	/// <summary>
	/// Read the contents of a file at the given <c>path</c>, if it exists.
	/// </summary>
	/// <param name="path">The file system path to the file to be read.</param>
	/// <returns>
	/// The contents of the file as a string, if the file exists.
	/// Otherwise, returns null.
	/// </returns>
	public static string ReadString(string path)
	{
		string output = null;
		
		var finalPath = Path.GetFullPath(path);
		if(File.Exists(finalPath))
			output = File.ReadAllText(finalPath);
		
		return output;
	}
	
	/// <summary>
	/// Write the given <c>data</c> to a file at the given <c>path</c>.
	/// </summary>
	/// <param name="path">The file system path to the file to be written.</param>
	/// <param name="data">The string to be written as the contents of the file.</param>
	public static void WriteString(string path, string data)
	{
		var finalPath = createPathIfNotExists(Path.GetFullPath(path));
		File.WriteAllText(finalPath, data);
	}
	
	/// <summary>
	/// Generate a fully qualified file system path to the desired directory or file.
	/// </summary>
	/// <param name="folder">The <c>System.Environment.SpecialFolder</c> to use as a base path.</param>
	/// <param name="pathFragment">The path, relative to <c>folder</c>, defining the desired directory or file.</param>
	/// <returns>The fully qualified file system path as a string.</returns>
	private static string getFinalPath(Environment.SpecialFolder folder, string pathFragment)
		=> Path.GetFullPath($"{Environment.GetFolderPath(folder)}{pathFragment}");
	
	/// <summary>
	/// Create the full directory structure of a given <c>path</c> if any
	/// part of it does not currently exist.
	/// </summary>
	/// <remarks>
	/// Returns the original <c>path</c> to allow for chaining.
	/// </remarks>
	/// <param name="path">The path whose directory structure will be created.</param>
	/// <returns>The unaltered <c>path</c> as a string.</returns>
	private static string createPathIfNotExists(string path)
	{
		Directory.CreateDirectory(Path.GetDirectoryName(path));
		return path;
	}
}
