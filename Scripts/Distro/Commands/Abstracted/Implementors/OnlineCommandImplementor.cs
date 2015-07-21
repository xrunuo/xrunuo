using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Server;
using Server.Network;

namespace Server.Scripts.Commands
{
	public class OnlineCommandImplementor : BaseCommandImplementor
	{
		public OnlineCommandImplementor()
		{
			Accessors = new string[] { "Online" };
			SupportRequirement = CommandSupport.Online;
			SupportsConditionals = true;
			AccessLevel = AccessLevel.GameMaster;
			Usage = "Online <command> [condition]";
			Description = "Invokes the command on all mobiles that are currently logged in. Optional condition arguments can further restrict the set of objects.";
		}

		public override void Compile( Mobile from, BaseCommand command, ref string[] args, ref object obj )
		{
			try
			{
				ObjectConditional cond = ObjectConditional.Parse( from, ref args );

				bool items, mobiles;

				if ( !CheckObjectTypes( command, cond, out items, out mobiles ) )
					return;

				if ( !mobiles ) // sanity check
				{
					command.LogFailure( "This command does not support mobiles." );
					return;
				}

				ArrayList list = new ArrayList();

				foreach ( Mobile mob in GameServer.Instance.Clients.Select( ns => ns.Mobile ) )
				{
					if ( mob != null && cond.CheckCondition( mob ) )
						list.Add( mob );
				}

				obj = list;
			}
			catch ( Exception ex )
			{
				from.SendMessage( ex.Message );
			}
		}
	}
}