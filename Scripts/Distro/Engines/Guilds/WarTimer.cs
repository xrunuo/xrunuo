using System;
using Server.Guilds;

namespace Server.Engines.Guilds
{
	public class WarTimer : Timer
	{
		public WarTimer()
			: base( TimeSpan.FromMinutes( 1.0 ), TimeSpan.FromMinutes( 1.0 ) )
		{
		}

		protected override void OnTick()
		{
			uint time;

			Guild a_Guild, wa_Guild, e_guild, g;

			int j;

			BaseGuild[] guilds = Guild.Search( "" );

			for ( int i = 0; i < guilds.Length; i++ )
			{
				g = guilds[i] as Guild;

				if ( g != null )
				{
					for ( int e = 0; e < g.Enemies.Count; e++ )
					{
						e_guild = g.Enemies[e] as Guild;

						if ( g.DeltaTime( e_guild ) )
						{
							time = g.GetExpTime( e_guild );

							if ( time <= 0 )
							{
								g.RemoveEnemy( e_guild );

								// need test localized
								g.GuildMessage( 1018018, "{0} ({1})", e_guild.Name, e_guild.Abbreviation ); // Guild Message: You are now at peace with this guild:

								for ( int a = 0; a < g.Allies.Count; a++ )
								{
									a_Guild = g.Allies[a] as Guild;

									a_Guild.RemoveEnemy( e_guild );

									for ( j = 0; j < e_guild.Allies.Count; j++ )
									{
										wa_Guild = g.Allies[j] as Guild;

										a_Guild.RemoveEnemy( wa_Guild );
									}
								}

								for ( j = 0; j < e_guild.Allies.Count; j++ )
								{
									wa_Guild = e_guild.Allies[j] as Guild;

									g.RemoveEnemy( wa_Guild );
								}
							}
						}
					}
				}
			}
		}
	}
}