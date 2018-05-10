using System;
using System.Net;

using Parameters = System.Collections.Generic.Dictionary<string, string>;

namespace Server.Engines.RestApi
{
	public abstract class BaseLocator
	{
		public abstract BaseController Locate( Parameters parameters );
	}
}
