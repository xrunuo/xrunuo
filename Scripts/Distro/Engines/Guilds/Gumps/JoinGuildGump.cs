using Server.Engines.Guilds.Items;
using Server.Factions;
using Server.Guilds;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Engines.Guilds.Gumps
{
	public class JoinGuildGump : Gump
	{
		public override int TypeID { get { return 0x2D5; } }

		private Mobile m_Mobile;

		public JoinGuildGump( Mobile from, Guild guild )
			: base( 10, 10 )
		{
			Intern( "Guild Name" );
			Intern( "25000" );
			Intern( "" );

			m_Mobile = from;

			AddPage( 0 );

			AddBackground( 0, 0, 500, 300, 0x2422 );

			AddHtmlLocalized( 25, 24, 450, 25, 1062939, 0x0, true, false ); // <center>GUILD MENU</center>
			AddHtmlLocalized( 25, 60, 450, 60, 1062940, 0x0, false, false ); // As you are not a member of any guild, you can create your own by providing a unique guild name and paying the standard guild registration fee.

			AddHtmlLocalized( 25, 135, 120, 25, 1062941, 0x0, false, false ); // Registration Fee:
			AddLabelIntern( 155, 135, 0x481, 1 );

			AddHtmlLocalized( 25, 165, 120, 25, 1011140, false, false ); // Enter Guild Name:
			AddBackground( 155, 160, 320, 26, 0xBB8 );
			AddTextEntryIntern( 160, 163, 315, 21, 0x481, 444, 0 );

			AddHtmlLocalized( 25, 191, 120, 26, 1063035, false, false ); // Abbreviation:
			AddBackground( 155, 186, 320, 26, 0xBB8 );
			AddTextEntryIntern( 160, 189, 315, 21, 0x481, 555, 2 );

			AddButton( 415, 217, 0xF7, 0xF8, 200, GumpButtonType.Reply, 0 );
			AddButton( 345, 217, 0xF2, 0xF1, 0, GumpButtonType.Reply, 0 );

			if ( ( from as PlayerMobile ).AcceptGuildInvites == true )
				AddButton( 20, 260, 0xD2, 0xD3, 151, GumpButtonType.Reply, 0 );
			else
				AddButton( 20, 260, 0xD3, 0xD2, 151, GumpButtonType.Reply, 0 );

			AddHtmlLocalized( 45, 260, 200, 30, 1062943, false, false ); // Ignore Guild Invites
		}

		private static string[] m_BadWordsInGuildName = new string[] { "guild", "admin", "seer", "counselor", "gm", "lady", "lord" };

		private static bool ValidateName( Mobile from, string name )
		{
			name = name.ToLower();

			if ( name.Length <= 40 )
			{
				string[] split = name.Split( ' ' );

				for ( int k = 0; k < split.Length; ++k )
				{
					for ( int j = 0; j < m_BadWordsInGuildName.Length; ++j )
					{
						if ( m_BadWordsInGuildName[j].ToLower() == split[k] )
						{
							from.SendLocalizedMessage( 1063000 ); // That guild name is not available.
							return false;
						}
					}
				}

				BaseGuild[] guilds = Guild.Search( "" );

				for ( int i = 0; i < guilds.Length; i++ )
				{
					if ( name == ( guilds[i] as BaseGuild ).Name.ToLower() )
					{
						from.SendLocalizedMessage( 1062999, name ); // That guild name is already in use: ~1_name~
						return false;
					}
				}
			}
			else
			{
				from.SendLocalizedMessage( 1063036, "40" ); // A guild name cannot be more than ~1_val~ characters in length.
				return false;
			}
			return true;
		}

		public override void OnResponse( GameClient state, RelayInfo info )
		{
			Mobile from = state.Mobile;

			Container cont = from.BankBox;

			bool backMoney = false;

			switch ( info.ButtonID )
			{
				case 0:
					{
						break;
					}
				case 200:
					{
						int Amount = 25000;

						string name = info.GetTextEntry( 444 ).Text;

						name = name.Trim();

						if ( name == "" )
						{
							from.SendLocalizedMessage( 1070884 ); // Guild name cannot be blank.
							return;
						}

						string abbr = info.GetTextEntry( 555 ).Text;

						abbr = abbr.Trim();

						if ( abbr == "" )
						{
							from.SendLocalizedMessage( 1070885 ); // You must provide a guild abbreviation.
							return;
						}

						if ( abbr.Length > 4 )
						{
							from.SendLocalizedMessage( 1063037, "4" ); // An abbreviation cannot exceed ~1_val~ characters in length.
							return;
						}

						foreach ( Faction faction in Faction.Factions )
						{
							if ( abbr == faction.Definition.Abbreviation )
							{
								from.SendLocalizedMessage( 501153 ); // That abbreviation is not available.
								return;
							}
						}

						if ( cont != null && cont.ConsumeTotal( typeof( Gold ), Amount ) )
						{
							from.SendLocalizedMessage( 1060398, Amount.ToString() ); // ~1_AMOUNT~ gold has been withdrawn from your bank box.

							if ( !ValidateName( from, name ) )
								backMoney = true;

							BaseGuild[] guilds = Guild.Search( "" );
							for ( int i = 0; i < guilds.Length; i++ )
							{
								if ( abbr.ToLower() == ( guilds[i] as BaseGuild ).Abbreviation.ToLower() )
								{
									from.SendLocalizedMessage( 501153 ); // That abbreviation is not available.
									backMoney = true;
								}
							}

							if ( !backMoney )
							{
								Guild guild = new Guild( from, name, "none" );
								from.Guild = guild;
								( from as PlayerMobile ).GuildRank = 5;

								Guildstone gm = new Guildstone( guild );
								gm.MoveToWorld( new Point3D( 0, 0, 0 ), Map.Trammel );
								guild.Guildstone = gm;
								Guild g = from.Guild as Guild;
								g.Abbreviation = abbr;

								from.SendLocalizedMessage( 1063238 ); // Your new guild has been founded.
							}

							if ( backMoney && cont != null )
							{
								cont.AddItem( new Gold( Amount ) );
								from.SendLocalizedMessage( 1060397, Amount.ToString() ); // ~1_AMOUNT~ gold has been deposited into your bank box.
							}
						}
						else
						{
							from.SendLocalizedMessage( 1063001, Amount.ToString() ); // You do not possess the ~1_val~ gold piece fee required to create a guild.
						}

						break;
					}
				case 151:
					{
						if ( ( from as PlayerMobile ).AcceptGuildInvites == true )
						{
							( from as PlayerMobile ).AcceptGuildInvites = false;
							from.SendLocalizedMessage( 1070698 ); // You are now ignoring guild invitations. 
						}
						else
						{
							( from as PlayerMobile ).AcceptGuildInvites = true;
							from.SendLocalizedMessage( 1070699 ); // You are now accepting guild invitations.
						}

						break;
					}
			}
		}
	}
}
