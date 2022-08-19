using System;

namespace OCSM
{
	public class PathBuilder
	{
		private static string SceneUniqueFormat = "{1}%{0}";
		
		/// <summary>
		/// Construct a node path using a Scene Unique Name.
		/// </summary>
		public static string SceneUnique(string name, string basePath = "")
		{
			return String.Format(SceneUniqueFormat, name, basePath);
		}
	}
}
