using System;

namespace OCSM
{
	/// <summary>
	/// Class providing convenience methods for generating Godot Node path strings.
	/// </summary>
	public class NodePathBuilder
	{
		private static string SceneUniqueFormat = "{1}%{0}";
		
		/// <summary>
		/// Construct a node path using a Scene Unique Name.
		/// </summary>
		/// <param name="name">The name of the target <c>Godot.Node</c>.</param>
		/// <param name="basePath">
		/// (Optional) The base path to prepend before the Scene Unique Name
		/// generated from the given <c>name</c>. Defaults to an empty string.
		/// </param>
		/// <returns>
		/// The fully qualified Godot node path as a string in the format of
		/// a Scene Unique Name.
		/// </returns>
		public static string SceneUnique(string name, string basePath = "")
		{
			var path = basePath;
			if(!String.IsNullOrEmpty(basePath) && !basePath.EndsWith("/"))
				path += "/";
			return String.Format(SceneUniqueFormat, name, path);
		}
	}
}
