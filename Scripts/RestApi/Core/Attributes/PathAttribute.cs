using System;

namespace Server.Engines.RestApi
{
	public class PathAttribute : Attribute
	{
		public string Path { get; private set; }

		public PathAttribute( string path )
		{
			Path = path;
		}
	}
}
