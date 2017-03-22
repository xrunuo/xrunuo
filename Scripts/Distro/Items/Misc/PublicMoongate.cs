using System;
using System.Collections;
using Server;
using Server.Commands;
using Server.Gumps;
using Server.Network;
using Server.Mobiles;
using Server.Spells;

namespace Server.Items
{
	public class PublicMoongate : Item
	{
		[Constructable]
		public PublicMoongate()
			: base( 0xF6C )
		{
			Movable = false;
			Light = LightType.Circle300;
		}

		public PublicMoongate( Serial serial )
			: base( serial )
		{
		}

		public override bool ForceShowProperties { get { return Map == Map.Malas; } }

		public override void OnDoubleClick( Mobile from )
		{
			if ( !from.Player )
				return;

			if ( from.InRange( GetWorldLocation(), 1 ) )
				UseGate( from );
			else
				from.SendLocalizedMessage( 500446 ); // That is too far away.
		}

		public override bool OnMoveOver( Mobile m )
		{
			if ( !m.Player )
				return true;

			UseGate( m );

			return true;
		}

		public bool UseGate( Mobile m )
		{
			if ( m.Criminal )
			{
				m.SendLocalizedMessage( 1005561, "", 0x22 ); // Thou'rt a criminal and cannot escape so easily.
				return false;
			}
			else if ( Server.Spells.SpellHelper.CheckCombat( m ) )
			{
				m.SendLocalizedMessage( 1005564, "", 0x22 ); // Wouldst thou flee during the heat of battle??
				return false;
			}
			else if ( m.Spell != null )
			{
				m.SendLocalizedMessage( 1049616 ); // You are too busy to do that at the moment.
				return false;
			}
			else
			{
				m.CloseGump( typeof( MoongateGump ) );
				m.SendGump( new MoongateGump( m, this ) );

				if ( !m.Hidden || m.AccessLevel == AccessLevel.Player )
					Effects.PlaySound( m.Location, m.Map, 0x20E );

				return true;
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
		}

		public static void Initialize()
		{
			CommandSystem.Register( "MoonGen", AccessLevel.Developer, new CommandEventHandler( MoonGen_OnCommand ) );
		}

		[Usage( "MoonGen" )]
		[Description( "Generates public moongates. Removes all old moongates." )]
		public static void MoonGen_OnCommand( CommandEventArgs e )
		{
			DeleteAll();

			int count = 0;

			count += MoonGen( PMList.Felucca );
			count += MoonGen( PMList.Trammel );
			count += MoonGen( PMList.Ilshenar );
			count += MoonGen( PMList.Malas );
			count += MoonGen( PMList.Tokuno );
			count += MoonGen( PMList.TerMur );

			World.Broadcast( 0x35, true, "{0} moongates generated.", count );
		}

		private static void DeleteAll()
		{
			ArrayList list = new ArrayList();

			foreach ( Item item in World.Instance.Items )
			{
				if ( item is PublicMoongate )
					list.Add( item );
			}

			foreach ( Item item in list )
				item.Delete();

			if ( list.Count > 0 )
				World.Broadcast( 0x35, true, "{0} moongates removed.", list.Count );
		}

		private static int MoonGen( PMList list )
		{
			foreach ( PMEntry entry in list.Entries )
			{
				Item item = new PublicMoongate();

				item.MoveToWorld( entry.Location, list.Map );

				if ( entry.Number == 1060642 ) // Umbra
					item.Hue = 0x497;
			}

			return list.Entries.Length;
		}
	}

	public class PMEntry
	{
		private Point3D m_Location;
		private int m_Number;

		public Point3D Location { get { return m_Location; } }

		public int Number { get { return m_Number; } }

		public PMEntry( Point3D loc, int number )
		{
			m_Location = loc;
			m_Number = number;
		}
	}

	public class PMList
	{
		private int m_Number, m_SelNumber;
		private Map m_Map;
		private PMEntry[] m_Entries;

		public int Number { get { return m_Number; } }

		public int SelNumber { get { return m_SelNumber; } }

		public Map Map { get { return m_Map; } }

		public PMEntry[] Entries { get { return m_Entries; } }

		public PMList( int number, int selNumber, Map map, PMEntry[] entries )
		{
			m_Number = number;
			m_SelNumber = selNumber;
			m_Map = map;
			m_Entries = entries;
		}

		public static readonly PMList Trammel = new PMList( 1012000, 1012012, Map.Trammel, new PMEntry[]
		{
			new PMEntry( new Point3D( 4467, 1283, 5 ), 1012003 ), // Moonglow
			new PMEntry( new Point3D( 1336, 1997, 5 ), 1012004 ), // Britain
			new PMEntry( new Point3D( 1499, 3771, 5 ), 1012005 ), // Jhelom
			new PMEntry( new Point3D( 771, 752, 5 ), 1012006 ), // Yew
			new PMEntry( new Point3D( 2701, 692, 5 ), 1012007 ), // Minoc
			new PMEntry( new Point3D( 1828, 2948, -20 ), 1012008 ), // Trinsic
			new PMEntry( new Point3D( 643, 2067, 5 ), 1012009 ), // Skara Brae
			new PMEntry( new Point3D( 3563, 2139, 34 ), 1012010 ), // Magincia
			new PMEntry( new Point3D( 3450, 2677, 25 ), 1078098 ) // New Haven
		} );

		public static readonly PMList Felucca = new PMList( 1012001, 1012013, Map.Felucca, new PMEntry[]
		{
			new PMEntry( new Point3D( 4467, 1283, 5 ), 1012003 ), // Moonglow
			new PMEntry( new Point3D( 1336, 1997, 5 ), 1012004 ), // Britain
			new PMEntry( new Point3D( 1499, 3771, 5 ), 1012005 ), // Jhelom
			new PMEntry( new Point3D( 771, 752, 5 ), 1012006 ), // Yew
			new PMEntry( new Point3D( 2701, 692, 5 ), 1012007 ), // Minoc
			new PMEntry( new Point3D( 1828, 2948, -20 ), 1012008 ), // Trinsic
			new PMEntry( new Point3D( 643, 2067, 5 ), 1012009 ), // Skara Brae
			new PMEntry( new Point3D( 3563, 2139, 34 ), 1012010 ), // Magincia
			new PMEntry( new Point3D( 2711, 2234, 0 ), 1019001 ) // Buccaneer's Den
		} );

		public static readonly PMList Ilshenar = new PMList( 1012002, 1012014, Map.Ilshenar, new PMEntry[]
		{
			new PMEntry( new Point3D( 1215, 467, -13 ), 1012015 ), // Compassion
			new PMEntry( new Point3D( 722, 1366, -60 ), 1012016 ), // Honesty
			new PMEntry( new Point3D( 744, 724, -28 ), 1012017 ), // Honor
			new PMEntry( new Point3D( 281, 1016, 0 ), 1012018 ), // Humility
			new PMEntry( new Point3D( 987, 1011, -32 ), 1012019 ), // Justice
			new PMEntry( new Point3D( 1174, 1286, -30 ), 1012020 ), // Sacrifice
			new PMEntry( new Point3D( 1532, 1340, - 3 ), 1012021 ), // Spirituality
			new PMEntry( new Point3D( 528, 216, -45 ), 1012022 ), // Valor
			new PMEntry( new Point3D( 1721, 218, 96 ), 1019000 ) // Chaos
		} );

		public static readonly PMList Malas = new PMList( 1060643, 1062039, Map.Malas, new PMEntry[]
		{
			new PMEntry( new Point3D( 1015, 527, -65 ), 1060641 ), // Luna
			new PMEntry( new Point3D( 1997, 1386, -85 ), 1060642 ) // Umbra
		} );

		public static readonly PMList Tokuno = new PMList( 1063258, 1063415, Map.Tokuno, new PMEntry[]
		{
			new PMEntry( new Point3D( 1169, 998, 41 ), 1063412 ), // Isamu-Jima
			new PMEntry( new Point3D( 802, 1204, 25 ), 1063413 ), // Makoto-Jima
			new PMEntry( new Point3D( 270, 628, 15 ), 1063414 ) // Homare-Jima
		} );

		public static readonly PMList TerMur = new PMList( 1113602, 1113604, Map.TerMur, new PMEntry[]
		{
			new PMEntry( new Point3D( 850, 3525, -38 ), 1113603 ), // Royal City
		} );

		public static readonly PMList[] DefaultLists = new PMList[] { Trammel, Felucca, Ilshenar, Malas, Tokuno, TerMur };
		public static readonly PMList[] YoungLists = new PMList[] { Trammel, Ilshenar, Malas, Tokuno, TerMur };
		public static readonly PMList[] RedLists = new PMList[] { Felucca };
		public static readonly PMList[] SigilLists = new PMList[] { Felucca };
	}

	public class MoongateGump : Gump
	{
		public override int TypeID { get { return 0x258; } }

		private Mobile m_Mobile;
		private Item m_Moongate;
		private PMList[] m_Lists;

		public MoongateGump( Mobile mobile, Item moongate )
			: base( 100, 100 )
		{
			m_Mobile = mobile;
			m_Moongate = moongate;

			PMList[] checkLists;

			if ( mobile.Player )
			{
				if ( Factions.Sigil.ExistsOn( mobile ) )
					checkLists = PMList.SigilLists;
				else if ( mobile.Kills >= 5 )
					checkLists = PMList.RedLists;
				else
				{
					bool young = mobile is PlayerMobile && ( (PlayerMobile) mobile ).Young;

					checkLists = young ? PMList.YoungLists : PMList.DefaultLists;
				}
			}
			else
				checkLists = PMList.DefaultLists;

			m_Lists = new PMList[checkLists.Length];

			for ( int i = 0; i < m_Lists.Length; ++i )
				m_Lists[i] = checkLists[i];

			for ( int i = 0; i < m_Lists.Length; ++i )
			{
				if ( m_Lists[i].Map == mobile.Map )
				{
					PMList temp = m_Lists[i];

					m_Lists[i] = m_Lists[0];
					m_Lists[0] = temp;

					break;
				}
			}

			AddPage( 0 );

			AddBackground( 0, 0, 380, 280, 0x13BE );

			AddHtmlLocalized( 45, 210, 140, 25, 1011036, false, false ); // OKAY
			AddButton( 10, 210, 4005, 4007, 1, GumpButtonType.Reply, 0 );

			AddHtmlLocalized( 45, 235, 140, 25, 1011012, false, false ); // CANCEL
			AddButton( 10, 235, 4005, 4007, 0, GumpButtonType.Reply, 0 );

			AddHtmlLocalized( 5, 5, 200, 20, 1012011, false, false ); // Pick your destination:

			for ( int i = 0; i < checkLists.Length; ++i )
			{
				AddButton( 10, 35 + ( i * 25 ), 2117, 2118, 0, GumpButtonType.Page, Array.IndexOf( m_Lists, checkLists[i] ) + 1 );
				AddHtmlLocalized( 30, 35 + ( i * 25 ), 150, 20, checkLists[i].Number, false, false );
			}

			for ( int i = 0; i < m_Lists.Length; ++i )
				RenderPage( i, Array.IndexOf( checkLists, m_Lists[i] ) );
		}

		private void RenderPage( int index, int offset )
		{
			PMList list = m_Lists[index];

			AddPage( index + 1 );

			AddButton( 10, 35 + ( offset * 25 ), 2117, 2118, 0, GumpButtonType.Page, index + 1 );

			AddHtmlLocalized( 30, 35 + ( offset * 25 ), 150, 20, list.SelNumber, false, false );

			PMEntry[] entries = list.Entries;

			for ( int i = 0; i < entries.Length; ++i )
			{
				AddRadio( 200, 35 + ( i * 25 ), 210, 211, false, ( index * 100 ) + i + 1 );
				AddHtmlLocalized( 225, 35 + ( i * 25 ), 150, 20, entries[i].Number, false, false );
			}
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			if ( info.ButtonID == 0 ) // Cancel
				return;
			else if ( m_Mobile.Deleted || m_Moongate.Deleted || m_Mobile.Map == null )
				return;

			int[] switches = info.Switches;

			if ( switches.Length == 0 )
				return;

			int switchID = switches[0] - 1;
			int listIndex = switchID / 100;
			int listEntry = switchID % 100;

			if ( listIndex < 0 || listIndex >= m_Lists.Length )
				return;

			PMList list = m_Lists[listIndex];

			if ( listEntry < 0 || listEntry >= list.Entries.Length )
				return;

			PMEntry entry = list.Entries[listEntry];

			if ( !m_Mobile.InRange( m_Moongate.GetWorldLocation(), 1 ) || m_Mobile.Map != m_Moongate.Map )
			{
				m_Mobile.SendLocalizedMessage( 1019002 ); // You are too far away to use the gate.
			}
			else if ( m_Mobile.Player && m_Mobile.Kills >= 5 && list.Map != Map.Felucca )
			{
				m_Mobile.SendLocalizedMessage( 1019004 ); // You are not allowed to travel there.
			}
			else if ( Factions.Sigil.ExistsOn( m_Mobile ) && list.Map != Factions.Faction.Facet )
			{
				m_Mobile.SendLocalizedMessage( 1019004 ); // You are not allowed to travel there.
			}
			else if ( m_Mobile.Criminal )
			{
				m_Mobile.SendLocalizedMessage( 1005561, "", 0x22 ); // Thou'rt a criminal and cannot escape so easily.
			}
			else if ( Server.Spells.SpellHelper.CheckCombat( m_Mobile ) )
			{
				m_Mobile.SendLocalizedMessage( 1005564, "", 0x22 ); // Wouldst thou flee during the heat of battle??
			}
			else if ( m_Mobile.Spell != null )
			{
				m_Mobile.SendLocalizedMessage( 1049616 ); // You are too busy to do that at the moment.
			}
			else if ( m_Mobile.Map == list.Map && m_Mobile.InRange( entry.Location, 1 ) )
			{
				m_Mobile.SendLocalizedMessage( 1019003 ); // You are already there.
			}
			else
			{
				BaseCreature.TeleportPets( m_Mobile, entry.Location, list.Map );

				m_Mobile.Combatant = null;
				m_Mobile.Warmode = false;
				m_Mobile.Hidden = true;

				m_Mobile.MoveToWorld( entry.Location, list.Map );

				if ( m_Mobile.AccessLevel == AccessLevel.Player )
					Effects.PlaySound( entry.Location, list.Map, 0x1FE );
			}
		}
	}
}
