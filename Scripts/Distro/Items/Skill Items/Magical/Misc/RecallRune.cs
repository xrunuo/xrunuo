using System;
using Server.Engines.Housing;
using Server.Engines.Housing.Items;
using Server.Engines.Housing.Multis;
using Server.Network;
using Server.Prompts;
using Server.Multis;
using Server.Regions;

namespace Server.Items
{
	[FlipableAttribute( 0x1f14, 0x1f15, 0x1f16, 0x1f17 )]
	public class RecallRune : Item
	{
		private string m_Description;
		private bool m_Marked;
		private Point3D m_Target;
		private Map m_TargetMap;
		private IHouse m_House;

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			if ( m_House != null && !m_House.Deleted )
			{
				writer.Write( (int) 1 ); // version

				writer.Write( (Item) m_House );
			}
			else
			{
				writer.Write( (int) 0 ); // version
			}

			writer.Write( (string) m_Description );
			writer.Write( (bool) m_Marked );
			writer.Write( (Point3D) m_Target );
			writer.Write( (Map) m_TargetMap );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 1:
					{
						m_House = reader.ReadItem() as BaseHouse;
						goto case 0;
					}
				case 0:
					{
						m_Description = reader.ReadString();
						m_Marked = reader.ReadBool();
						m_Target = reader.ReadPoint3D();
						m_TargetMap = reader.ReadMap();

						UpdateHue();

						break;
					}
			}

			FixRunes();
		}

		[CommandProperty( AccessLevel.Counselor, AccessLevel.GameMaster )]
		public IHouse House
		{
			get
			{
				if ( m_House != null && m_House.Deleted )
					House = null;

				return m_House;
			}
			set { m_House = value; UpdateHue(); InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.Counselor, AccessLevel.GameMaster )]
		public string Description
		{
			get
			{
				return m_Description;
			}
			set
			{
				m_Description = value;
				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.Counselor, AccessLevel.GameMaster )]
		public bool Marked
		{
			get
			{
				return m_Marked;
			}
			set
			{
				if ( m_Marked != value )
				{
					m_Marked = value;
					UpdateHue();
					InvalidateProperties();
				}
			}
		}

		[CommandProperty( AccessLevel.Counselor, AccessLevel.GameMaster )]
		public Point3D Target
		{
			get
			{
				return m_Target;
			}
			set
			{
				m_Target = value;
			}
		}

		[CommandProperty( AccessLevel.Counselor, AccessLevel.GameMaster )]
		public Map TargetMap
		{
			get
			{
				return m_TargetMap;
			}
			set
			{
				if ( m_TargetMap != value )
				{
					m_TargetMap = value;
					UpdateHue();
					InvalidateProperties();
				}
			}
		}

		private bool CheckDungeonDoom( Point3D p )
		{
			bool check = false;

			Region region = Region.Find( p, Map.Malas );

			if ( region.Name == "Doom" )
				check = true;

			return check;
		}

		public void FixRunes()
		{
			Region region = Region.Find( m_Target, m_TargetMap );

			if ( region.Name == "Yomotsu Mines" || region.Name == "Fan Dancer Dojo" )
			{
				m_TargetMap = null;
				m_Target = new Point3D( 0, 0, 0 );
				Description = null;
				Marked = false;
			}

			if ( m_TargetMap == Map.TerMur && Hue != 1162 )
				Hue = 1162;
		}

		private void UpdateHue()
		{
			if ( !m_Marked )
				Hue = 0;
			else
				Hue = CalculateHue( m_TargetMap, m_House != null );
		}

		public static int CalculateHue( Map targetMap, bool house = false )
		{
			if ( targetMap == Map.Trammel )
				return ( house ? 0x47F : 50 );
			else if ( targetMap == Map.Felucca )
				return ( house ? 0x66D : 0 );
			else if ( targetMap == Map.Ilshenar )
				return ( house ? 0x55F : 1102 );
			else if ( targetMap == Map.Malas )
				return ( house ? 0x55F : 1102 );
			else if ( targetMap == Map.Tokuno )
				return ( house ? 0x1F14 : 1154 );
			else if ( targetMap == Map.TerMur )
				return 1162;

			return 0;
		}

		public void Mark( Mobile m )
		{
			m_Marked = true;

			m_House = HousingHelper.FindHouseAt( m );

			if ( m_House == null )
			{
				m_Target = m.Location;
				m_TargetMap = m.Map;
			}
			else
			{
				HouseSign sign = m_House.Sign;

				if ( sign != null )
					m_Description = sign.Name;
				else
					m_Description = null;

				if ( m_Description == null || ( m_Description = m_Description.Trim() ).Length == 0 )
					m_Description = "an unnamed house";

				int x = m_House.BanLocation.X;
				int y = m_House.BanLocation.Y + 2;
				int z = m_House.BanLocation.Z;

				Map map = m_House.Map;

				if ( map != null && !map.CanFit( x, y, z, 16, false, false ) )
					z = map.GetAverageZ( x, y );

				m_Target = new Point3D( x, y, z );
				m_TargetMap = map;
			}

			UpdateHue();
			InvalidateProperties();
		}

		private const string RuneFormat = "a recall rune for {0}";

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( m_Marked )
			{
				string desc;

				if ( ( desc = m_Description ) == null || ( desc = desc.Trim() ).Length == 0 )
					desc = "an unknown location";

				if ( m_TargetMap == Map.TerMur )
					list.Add( ( House != null ? 1113206 : 1113205 ), RuneFormat, desc ); // ~1_val~ (Ter Mur)(House)
				else if ( m_TargetMap == Map.Tokuno )
					list.Add( ( House != null ? 1063260 : 1063259 ), RuneFormat, desc ); // ~1_val~ (Tokuno Islands)(House)
				else if ( m_TargetMap == Map.Malas )
				{
					if ( CheckDungeonDoom( m_Target ) )
						list.Add( "{0} ({1})", String.Format( RuneFormat, "Dungeon Doom" ), m_TargetMap );
					else
						list.Add( ( House != null ? 1062454 : 1060804 ), RuneFormat, desc ); // ~1_val~ (Malas)[(House)]
				}
				else if ( m_TargetMap == Map.Felucca )
					list.Add( ( House != null ? 1062452 : 1060805 ), RuneFormat, desc ); // ~1_val~ (Felucca)[(House)]
				else if ( m_TargetMap == Map.Trammel )
					list.Add( ( House != null ? 1062453 : 1060806 ), RuneFormat, desc ); // ~1_val~ (Trammel)[(House)]
				else
					list.Add( ( House != null ? "{0} ({1})(House)" : "{0} ({1})" ), String.Format( RuneFormat, desc ), m_TargetMap );
			}
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			else if ( House != null )
				from.SendLocalizedMessage( 1062399 ); // You cannot edit the description for this rune.
			else if ( m_Marked )
				from.Prompt = new RenamePrompt( this );
			else
				from.SendLocalizedMessage( 501805 ); // That rune is not yet marked.
		}

		private class RenamePrompt : Prompt
		{
			// Please enter a description for this marked object.
			public override int MessageCliloc { get { return 501804; } }

			private RecallRune m_Rune;

			public RenamePrompt( RecallRune rune )
				: base( rune )
			{
				m_Rune = rune;
			}

			public override void OnResponse( Mobile from, string text )
			{
				if ( m_Rune.House == null && m_Rune.Marked )
				{
					m_Rune.Description = text;
					from.SendLocalizedMessage( 1010474 ); // The etching on the rune has been changed.
				}
			}
		}

		[Constructable]
		public RecallRune()
			: base( 0x1F14 )
		{
			Weight = 1.0;
			UpdateHue();
		}

		public RecallRune( Serial serial )
			: base( serial )
		{
		}
	}
}