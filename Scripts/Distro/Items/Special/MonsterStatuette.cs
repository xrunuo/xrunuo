using System;
using Server;
using Server.Engines.Housing;
using Server.Multis;
using Server.Gumps;
using Server.Items;
using Server.Network;

namespace Server.Items
{
	public enum MonsterStatuetteType
	{
		Crocodile,
		Daemon,
		Dragon,
		EarthElemental,
		Ettin,
		Gargoyle,
		Gorilla,
		Lich,
		Lizardman,
		Ogre,
		Orc,
		Ratman,
		Skeleton,
		Troll,
		Cow,
		Zombie,
		Llama,
		Ophidian,
		Reaper,
		Mongbat,
		Gazer,
		FireElemental,
		Wolf,
		PhillipsWoodenSteed,
		Seahorse,
		#region Veteran Rewards
		Harrower,
		Efreet,
		TerathanMatriarch,
		FireAnt,
		#endregion
		Slime,
		PlagueBeast,
		RedDeath,
		Spider,
		OphidianArchMage,
		OphidianWarrior,
		DreadHorn,
		BlackCatStatuette,
		TormentedMinotaur,
		CollectionInteractiveStatue
	}

	public class MonsterStatuetteInfo
	{
		private int m_LabelNumber;
		private int m_ItemID;
		private int[] m_Sounds;

		public int LabelNumber { get { return m_LabelNumber; } }
		public int ItemID { get { return m_ItemID; } }
		public int[] Sounds { get { return m_Sounds; } }

		public MonsterStatuetteInfo( int labelNumber, int itemID, int baseSoundID )
		{
			m_LabelNumber = labelNumber;
			m_ItemID = itemID;
			m_Sounds = new int[] { baseSoundID, baseSoundID + 1, baseSoundID + 2, baseSoundID + 3, baseSoundID + 4 };
		}

		private static MonsterStatuetteInfo[] m_Table = new MonsterStatuetteInfo[]
			{
				/* Crocodile */			new MonsterStatuetteInfo( 1041249, 0x20DA, 660 ),
				/* Daemon */			new MonsterStatuetteInfo( 1041250, 0x20D3, 357 ),
				/* Dragon */			new MonsterStatuetteInfo( 1041251, 0x20D6, 362 ),
				/* EarthElemental */	new MonsterStatuetteInfo( 1041252, 0x20D7, 268 ),
				/* Ettin */				new MonsterStatuetteInfo( 1041253, 0x20D8, 367 ),
				/* Gargoyle */			new MonsterStatuetteInfo( 1041254, 0x20D9, 372 ),
				/* Gorilla */			new MonsterStatuetteInfo( 1041255, 0x20F5, 158 ),
				/* Lich */				new MonsterStatuetteInfo( 1041256, 0x20F8, 1001 ),
				/* Lizardman */			new MonsterStatuetteInfo( 1041257, 0x20DE, 417 ),
				/* Ogre */				new MonsterStatuetteInfo( 1041258, 0x20DF, 427 ),
				/* Orc */				new MonsterStatuetteInfo( 1041259, 0x20E0, 1114 ),
				/* Ratman */			new MonsterStatuetteInfo( 1041260, 0x20E3, 437 ),
				/* Skeleton */			new MonsterStatuetteInfo( 1041261, 0x20E7, 1165 ),
				/* Troll */				new MonsterStatuetteInfo( 1041262, 0x20E9, 461 ),
				/* Cow */				new MonsterStatuetteInfo( 1041263, 0x2103, 120 ),
				/* Zombie */			new MonsterStatuetteInfo( 1041264, 0x20EC, 471 ),
				/* Llama */				new MonsterStatuetteInfo( 1041265, 0x20F6, 1011 ),
				/* Ophidian */			new MonsterStatuetteInfo( 1049742, 0x2133, 634 ),
				/* Reaper */			new MonsterStatuetteInfo( 1049743, 0x20FA, 442 ),
				/* Mongbat */			new MonsterStatuetteInfo( 1049744, 0x20F9, 422 ),
				/* Gazer */				new MonsterStatuetteInfo( 1049768, 0x20F4, 377 ),
				/* FireElemental */		new MonsterStatuetteInfo( 1049769, 0x20F3, 838 ),
				/* Wolf */				new MonsterStatuetteInfo( 1049770, 0x2122, 229 ),
				/* Phillip's Steed */	new MonsterStatuetteInfo( 1063488, 0x3FFE, 205 ),
				/* Seahorse */			new MonsterStatuetteInfo( 1070819, 0x25BA, 138 ),
				#region Veteran Rewards
				/* Harrower */			new MonsterStatuetteInfo( 1080520, 0x25BB, 0x289 ),
				/* Efreet */			new MonsterStatuetteInfo( 1080521, 0x2590, 0x300 ),
                /* Terathan Matriarch */new MonsterStatuetteInfo( 1113800, 0x212C, 0x257 ),
                /* Fire Ant */          new MonsterStatuetteInfo( 1113801, 0x42a7, 1006 ),
				#endregion
				/* Slime */				new MonsterStatuetteInfo( 1015246, 0x20E8, 456 ),
				/* PlagueBeast */		new MonsterStatuetteInfo( 1029747, 0x2613, 0x1BF ),
				/* RedDeath */			new MonsterStatuetteInfo( 1094932, 0x2617, -1 ),
				/* Spider */			new MonsterStatuetteInfo( 1029668, 0x25C4, 1170 ),
				/* OphidianArchMage */	new MonsterStatuetteInfo( 1029641, 0x25A9, 639 ),
				/* OphidianWarrior */	new MonsterStatuetteInfo( 1029645, 0x25AD, 634 ),
				/* DreadHorn */			new MonsterStatuetteInfo( 1031651, 0x2D83, 0xA8 ),
                /* BlackCatStatuette */ new MonsterStatuetteInfo( 1096928, 0x4688, 0x069),
				/* TormentedMinotaur*/  new MonsterStatuetteInfo( 1031656, 0x2D88, 1431),                
			};

		public static MonsterStatuetteInfo GetInfo( MonsterStatuetteType type )
		{
			int v = (int) type;

			if ( v < 0 || v >= m_Table.Length )
				v = 0;

			return m_Table[v];
		}
	}

	public class MonsterStatuette : Item, Engines.VeteranRewards.IRewardItem
	{
		private MonsterStatuetteType m_Type;
		private bool m_TurnedOn;
		private bool m_IsRewardItem;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsRewardItem { get { return m_IsRewardItem; } set { m_IsRewardItem = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public bool TurnedOn
		{
			get { return m_TurnedOn; }
			set
			{
				m_TurnedOn = value;
				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public MonsterStatuetteType Type
		{
			get { return m_Type; }
			set
			{
				m_Type = value;
				ItemID = MonsterStatuetteInfo.GetInfo( m_Type ).ItemID;
				InvalidateProperties();
			}
		}

		public override int LabelNumber { get { return MonsterStatuetteInfo.GetInfo( m_Type ).LabelNumber; } }

		public virtual bool EnableSound { get { return true; } }

		[Constructable]
		public MonsterStatuette()
			: this( MonsterStatuetteType.Crocodile )
		{
		}

		[Constructable]
		public MonsterStatuette( MonsterStatuetteType type )
			: base( MonsterStatuetteInfo.GetInfo( type ).ItemID )
		{
			Weight = 1.0;
			LootType = LootType.Blessed;

			m_Type = type;
		}

		public override bool HandlesOnMovement { get { return m_TurnedOn && IsLockedDown; } }

		public override void OnMovement( Mobile m, Point3D oldLocation )
		{
			if ( EnableSound && m_TurnedOn && IsLockedDown && ( !m.Hidden || m.AccessLevel == AccessLevel.Player ) && this.InRange( m, 2 ) && !this.InRange( oldLocation, 2 ) )
			{
				int[] sounds = MonsterStatuetteInfo.GetInfo( m_Type ).Sounds;

				Effects.PlaySound( this.Location, this.Map, sounds[Utility.Random( sounds.Length )] );
			}

			base.OnMovement( m, oldLocation );
		}

		public MonsterStatuette( Serial serial )
			: base( serial )
		{
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( m_TurnedOn )
				list.Add( 502695 ); // turned on
			else
				list.Add( 502696 ); // turned off
		}

		public bool IsOwner( Mobile mob )
		{
			IHouse house = HousingHelper.FindHouseAt( this );

			return ( house != null && house.IsOwner( mob ) );
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( IsOwner( from ) )
			{
				OnOffGump onOffGump = new OnOffGump( this );
				from.SendGump( onOffGump );
			}
			else
			{
				from.SendLocalizedMessage( 502691 ); // You must be the owner to use this.
			}
		}

		private class OnOffGump : Gump
		{
			public override int TypeID { get { return 0xF3E67; } }

			private MonsterStatuette m_Statuette;

			public OnOffGump( MonsterStatuette statuette )
				: base( 150, 200 )
			{
				m_Statuette = statuette;

				AddPage( 0 );

				AddBackground( 0, 0, 300, 150, 0xA28 );

				AddHtmlLocalized( 45, 20, 300, 35, statuette.TurnedOn ? 1011035 : 1011034, false, false ); // [De]Activate this item

				AddButton( 40, 53, 0xFA5, 0xFA7, 2, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 80, 55, 65, 35, 1011036, false, false ); // OKAY

				AddButton( 150, 53, 0xFA5, 0xFA7, 1, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 190, 55, 100, 35, 1011012, false, false ); // CANCEL
			}

			public override void OnResponse( NetState sender, RelayInfo info )
			{
				Mobile from = sender.Mobile;

				if ( info.ButtonID == 2 )
				{
					bool newValue = !m_Statuette.TurnedOn;
					m_Statuette.TurnedOn = newValue;

					if ( newValue && !m_Statuette.IsLockedDown )
						from.SendLocalizedMessage( 502693 ); // Remember, this only works when locked down.
				}
				else
				{
					from.SendLocalizedMessage( 502694 ); // Cancelled action.
				}
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.WriteEncodedInt( (int) m_Type );
			writer.Write( (bool) m_TurnedOn );
			writer.Write( (bool) m_IsRewardItem );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
					{
						m_Type = (MonsterStatuetteType) reader.ReadEncodedInt();
						m_TurnedOn = reader.ReadBool();
						m_IsRewardItem = reader.ReadBool();
						break;
					}
			}
		}
	}
}