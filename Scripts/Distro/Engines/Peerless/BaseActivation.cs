using System;
using Server;
using Server.Engines.PartySystem;
using Server.Mobiles;
using System.Collections;
using Server.Gumps;
using Server.Network;

namespace Server.Items
{
	public class BaseActivation : TransientItem
	{
		private Map m_Mapa = Map.Felucca;

		public Map Mapa
		{
			get { return m_Mapa; }
			set { m_Mapa = value; InvalidateProperties(); }
		}

		private Point3D m_EnterPoint;

		public Point3D EnterPoint
		{
			get { return m_EnterPoint; }
			set { m_EnterPoint = value; }
		}

		public BaseActivation( int itemid )
			: base( itemid, TimeSpan.FromSeconds( 600.0 ) )
		{
			Weight = 1.0;
			LootType = LootType.Blessed;
		}

		public BaseActivation( Serial serial )
			: base( serial )
		{
		}

		public override void OnDoubleClick( Mobile m )
		{
			if ( !IsChildOf( m.Backpack ) )
			{
				m.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
			else
			{
				m.CloseGump( typeof( ConfirmPeerlessGump ) );
				m.SendGump( new ConfirmPeerlessGump( this ) );
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			if ( !this.Deleted )
				this.Delete();
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( m_Mapa == Map.Felucca )
				list.Add( 1012001 ); // Felucca
			else if ( m_Mapa == Map.Trammel )
				list.Add( 1012000 ); // Trammel
			else if ( m_Mapa == Map.Ilshenar )
				list.Add( 1012002 ); // Ilshenar
			else if ( m_Mapa == Map.Malas )
				list.Add( 1060643 ); // Malas
			else if ( m_Mapa == Map.Tokuno )
				list.Add( 1063258 ); // Tokuno Islands
		}

		public class ConfirmPeerlessGump : Gump
		{
			BaseActivation m_key;

			public ConfirmPeerlessGump( BaseActivation key )
				: base( 0, 0 )
			{
				m_key = key;
				Closable = true;
				Disposable = true;
				Dragable = true;
				Resizable = false;
				AddPage( 0 );
				AddBackground( 38, 45, 238, 134, 9250 );
				AddLabel( 78, 64, 0, @"Are you sure you want to" );
				AddLabel( 131, 84, 0, @"teleport" );
				AddLabel( 61, 104, 0, @"your party to unknown area?" );
				AddButton( 128, 140, 241, 242, 0, GumpButtonType.Reply, 0 );
				AddButton( 198, 140, 247, 248, 1, GumpButtonType.Reply, 0 );
			}

			public override void OnResponse( NetState sender, RelayInfo info )
			{
				int button = info.ButtonID;

				PlayerMobile from = sender.Mobile as PlayerMobile;

				from.CloseGump( typeof( ConfirmPeerlessGump ) );

				switch ( button )
				{
					case 0:
						{
							break;
						}
					case 1:
						{
							Party p = Party.Get( from );

							if ( p != null )
							{
								for ( int i = 0; i < p.Members.Count; ++i )
								{
									PartyMemberInfo pmi = (PartyMemberInfo) p.Members[i];
									PlayerMobile member = pmi.Mobile as PlayerMobile;

									if ( m_key.Map != Map.Felucca && member.Kills >= 5 )
										continue;

									if ( member.Map == from.Map && member.Region == from.Region )
									{
										member.CloseGump( typeof( ConfirmPeerlessPartyGump ) );
										member.SendGump( new ConfirmPeerlessPartyGump( m_key, member ) );
									}
								}
							}

							if ( m_key != null && !m_key.Deleted )
							{
								from.CloseGump( typeof( ConfirmPeerlessPartyGump ) );
								from.SendGump( new ConfirmPeerlessPartyGump( m_key, from ) );
								m_key.Delete();
							}

							break;
						}
				}
			}
		}

		public class ConfirmPeerlessPartyGump : Gump
		{
			private Timer m_Accept;
			private BaseActivation m_Key;

			public ConfirmPeerlessPartyGump( BaseActivation key, PlayerMobile from )
				: base( 0, 0 )
			{
				m_Key = key;
				Closable = true;
				Disposable = true;
				Dragable = true;
				Resizable = false;
				AddPage( 0 );
				AddBackground( 38, 45, 238, 134, 9250 );
				AddLabel( 62, 64, 0, @"Your party is teleporting to an" );
				AddLabel( 118, 84, 0, @"unknown area." );
				AddLabel( 98, 104, 0, @"Do you wish to go?" );
				AddButton( 128, 140, 241, 242, 0, GumpButtonType.Reply, 0 );
				AddButton( 198, 140, 247, 248, 1, GumpButtonType.Reply, 0 );

				m_Accept = new AcceptConfirmPeerlessPartyTimer( from );
				m_Accept.Start();
			}

			public override void OnResponse( NetState sender, RelayInfo info )
			{
				Mobile from = sender.Mobile;
				from.CloseGump( typeof( ConfirmPeerlessPartyGump ) );

				int button = info.ButtonID;
				switch ( button )
				{
					case 0:
						break;

					case 1:
						if ( m_Key.Map != Map.Felucca && from.Kills >= 5 )
							return;

						Effects.SendLocationParticles( EffectItem.Create( from.Location, from.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 2023 );

						BaseCreature.TeleportPets( from, m_Key.EnterPoint, m_Key.Mapa );
						from.MoveToWorld( m_Key.EnterPoint, m_Key.Mapa );

						break;
				}
			}
		}

		private class AcceptConfirmPeerlessPartyTimer : Timer
		{
			private Mobile m_From;

			public AcceptConfirmPeerlessPartyTimer( Mobile from )
				: base( TimeSpan.FromSeconds( 60.0 ), TimeSpan.FromSeconds( 60.0 ), 1 )
			{
				m_From = from;
			}

			protected override void OnTick()
			{
				m_From.CloseGump( typeof( ConfirmPeerlessPartyGump ) );
				Stop();
			}
		}
	}
}