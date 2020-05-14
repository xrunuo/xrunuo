using System.IO;
using System.Net;
using Newtonsoft.Json;
using Server.Accounting;
using Parameters = System.Collections.Generic.Dictionary<string, string>;

namespace Server.Engines.RestApi
{
	public class Request
	{
		private readonly HttpListenerContext _context;
		private readonly Parameters _parameters;

		public Request( HttpListenerContext context, Parameters parameters )
		{
			_context = context;
			_parameters = parameters;
		}

		public string this[string param]
		{
			get
			{
				string value;
				_parameters.TryGetValue( param, out value );

				return value;
			}
		}

		public Account GetAccount()
		{
			return Accounts.GetAccount( _context.User.Identity.Name );
		}

		/// <summary>
		/// Reads the request parameters as a json string and deserializes it
		/// into the given type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		public T AsDto<T>()
		{
			var data = "";

			using ( var body = _context.Request.InputStream )
			using ( var reader = new StreamReader( body, _context.Request.ContentEncoding ) )
				data = reader.ReadToEnd();

			return JsonConvert.DeserializeObject<T>( data );
		}

		public string HttpMethod => _context.Request.HttpMethod;
	}
}
