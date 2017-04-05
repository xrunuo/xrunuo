using System.Text.RegularExpressions;

namespace Server
{
	public class LocalizedText
	{
		public int Number { get; }
		public string Args { get; }

		public LocalizedText( int number )
			: this( number, null )
		{
		}

		public LocalizedText( string text )
			: this( 1042971, text )
		{
		}

		public LocalizedText( int number, string format, params object[] args )
			: this( number, string.Format( format, args ) )
		{
		}

		public LocalizedText( string textFormat, params object[] args )
			: this( 1042971, string.Format( textFormat, args ) )
		{
		}

		public LocalizedText( int number, string args )
		{
			Number = number;
			Args = args;
		}

		public void AddTo( ObjectPropertyList list )
		{
			if ( Args == null )
				list.Add( Number );
			else
				list.Add( Number, Args );
		}

		private static readonly Regex LocalizedTextExpression = new Regex( @"#(\d+)", RegexOptions.IgnoreCase );

		public string Delocalize()
		{
			string format = StringList.Localization[Number];

			if ( format == null )
				return "(empty)";

			if ( string.IsNullOrEmpty( Args ) )
				return format;

			string args = LocalizedTextExpression.Replace( Args, m => StringList.Localization[Utility.ToInt32( m.Groups[1].Value )] ?? string.Empty );
			return StringList.CombineArguments( format, args );
		}
	}
}
