using System;
using Server;
using Server.Gumps;

namespace Server.Prompts
{
	public abstract class Prompt
	{
		public IEntity Sender { get; }

		public String MessageArgs { get; }

		public virtual int MessageCliloc => 1042971;

		public virtual int MessageHue => 0;

		public virtual int TypeId => GetType().FullName.GetHashCode();

		public Prompt()
			: this( null )
		{
		}

		public Prompt( IEntity sender )
			: this( sender, String.Empty )
		{
		}

		public Prompt( IEntity sender, String args )
		{
			Sender = sender;
			MessageArgs = args;
		}

		public virtual void OnCancel( Mobile from )
		{
		}

		public virtual void OnResponse( Mobile from, string text )
		{
		}
	}
}