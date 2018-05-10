using System;
using System.Linq;
using System.Text.RegularExpressions;

using Parameters = System.Collections.Generic.Dictionary<string, string>;

namespace Server.Engines.RestApi
{
	public class Route
	{
		private String _pattern;
		private Regex _matcher;

		public Route( string pattern )
		{
			_pattern = pattern;
			_matcher = new Regex( String.Format( "^{0}$", Regex.Replace( pattern, @"\{(\w+)\}", @"(?<$1>\w+)" ) ) );
		}

		public bool IsMatch( string uri )
		{
			return _matcher.IsMatch( uri );
		}

		public Parameters GetMatchedParameters( string uri )
		{
			if ( !IsMatch( uri ) )
				throw new ArgumentException();

			var match = _matcher.Match( uri );

			return _matcher.GetGroupNames()
				.Where( name => !name.Equals( "0" ) )
				.ToDictionary(
					name => name,
					name => match.Groups[name].Value
				);
		}
	}
}
