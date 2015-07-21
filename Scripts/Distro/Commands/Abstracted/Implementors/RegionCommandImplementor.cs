using System;
using System.Collections;
using Server;

namespace Server.Scripts.Commands
{
	public class RegionCommandImplementor : BaseCommandImplementor
	{
		public RegionCommandImplementor()
		{
			Accessors = new string[] { "Region" };
			SupportRequirement = CommandSupport.Region;
			SupportsConditionals = true;
			AccessLevel = AccessLevel.Administrator;
			Usage = "Region <command> [condition]";
			Description = "Invokes the command on all appropriate mobiles in your current region. Optional condition arguments can further restrict the set of objects.";
		}

		public override void Compile( Mobile from, BaseCommand command, ref string[] args, ref object obj )
		{
			try
			{
				ObjectConditional cond = ObjectConditional.Parse( from, ref args );

				bool items, mobiles;

				if ( !CheckObjectTypes( command, cond, out items, out mobiles ) )
				{
					return;
				}

				Region reg = from.Region;

				ArrayList list = new ArrayList();

				if ( mobiles )
				{
					foreach ( Mobile mob in reg.GetMobiles() )
					{
						if ( cond.CheckCondition( mob ) )
						{
							list.Add( mob );
						}
					}
				}
				else
				{
					command.LogFailure( "This command does not support items." );
					return;
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