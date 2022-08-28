using System;

namespace OCSM
{
	public class NodePathBuilder
	{
		private static string SceneUniqueFormat = "{1}%{0}";
		
		/// <summary>
		/// Construct a node path using a Scene Unique Name.
		/// </summary>
		public static string SceneUnique(string name, string basePath = "")
		{
			var path = basePath;
			if(!String.IsNullOrEmpty(basePath) && !basePath.EndsWith("/"))
				path += "/";
			return String.Format(SceneUniqueFormat, name, path);
		}
	}
}
