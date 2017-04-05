using System;
using Server;
using Server.Factions;
using Server.Gumps;
using Server.Network;

namespace Server.Items
{
	public class ShrineGem : Item
	{
		public override int LabelNumber { get { return 1094711; } } // Shrine Gem

		[Constructable]
		public ShrineGem()
			: base( 0xFC8 )
		{
			Hue = 0x47F;
			Weight = 1.0;
			LootType = LootType.Blessed;
		}

		public ShrineGem( Serial serial )
			: base( serial )
		{
		}

		public override void AddNameProperties( ObjectPropertyList list )
		{
			base.AddNameProperties( list );

			list.Add( 1041350 ); // faction item
		}

		public void Use( Mobile from )
		{
			Faction faction = Faction.Find( from );

			if ( faction == null )
			{
				// You may not use this unless you are a faction member!
				from.SendLocalizedMessage( 1010376, null, 0x25 );
			}
			else
			{
				from.CloseGump<ShrineGemGump>();
				from.SendGump( new ShrineGemGump( this ) );
			}
		}

		private class ShrineGemGump : Gump
		{
			public override int TypeID { get { return 0x236B; } }

			private Item m_Gem;

			public ShrineGemGump( Item gem )
				: base( 200, 200 )
			{
				m_Gem = gem;

				AddPage( 0 );

				AddBackground( 0, 0, 291, 159, 0x13BE );

				AddImageTiled( 5, 6, 280, 20, 0xA40 );
				AddHtmlLocalized( 9, 8, 280, 20, 1094715, 0x7FFF, false, false ); // Use a Shrine Gem
				AddImageTiled( 5, 31, 280, 100, 0xA40 );
				AddHtmlLocalized( 9, 35, 272, 100, 1094716, 0x7FFF, false, false ); // The power of the Shrine Gem can teleport your ghost to a shrine for resurrection. Do you wish to go?

				AddButton( 180, 133, 0xFB7, 0xFB8, 1, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 215, 135, 100, 20, 1006044, 0x7FFF, false, false ); // OK
				AddButton( 5, 133, 0xFB1, 0xFB2, 0, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 40, 135, 100, 20, 1060051, 0x7FFF, false, false ); // CANCEL
			}

			public override void OnResponse( NetState sender, RelayInfo info )
			{
				Mobile from = sender.Mobile;

				if ( info.ButtonID == 1 && !from.Alive && !m_Gem.Deleted )
				{
					if ( from.Region.IsPartOf<Regions.Jail>() )
					{
						from.SendLocalizedMessage( 1041530, "", 0x35 ); // You'll need a better jailbreak plan then that!
					}
					else
					{
						Effects.SendPacket( from.Location, from.Map, new HuedEffect( EffectType.FixedXYZ, Server.Serial.Zero, Server.Serial.Zero, 0x3709, from.Location, from.Location, 10, 30, true, false, 0x47E, 4 ) );
						from.PlaySound( 0x1FC );
						from.MoveToWorld( m_ShrineLocs[Utility.Random( m_ShrineLocs.Length )], Faction.Facet );
						from.PlaySound( 0x1FC );
					}

					m_Gem.Delete();
				}
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

		private static readonly Point3D[] m_ShrineLocs = new Point3D[]
			{
				new Point3D( 1470, 843, 0 ),
				new Point3D( 1857, 865, -1 ),
				new Point3D( 4220, 563, 36 ),
				new Point3D( 1732, 3528, 0 ),
				new Point3D( 1300, 644, 8 ),
				new Point3D( 3355, 302, 9 ),
				new Point3D( 1606, 2490, 5 ),
				new Point3D( 2500, 3931, 3 ),
				new Point3D( 4264, 3707, 0 )
			};
	}
}