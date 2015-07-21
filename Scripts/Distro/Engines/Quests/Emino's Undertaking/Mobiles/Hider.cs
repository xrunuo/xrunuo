using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.Quests;

namespace Server.Engines.Quests.SE
{
	public class Hider : BaseQuester
	{
		public override bool ClickTitle { get { return false; } }

		private static int[] m_Messages = new int[]
			{
				1063191, // They won't find me here.
				1063192, // Ah, a quiet hideout.
				1063193, // I wonder if I can find a sharpening stone around here.
				1063194, // Who locked me in this room?
				1063195  // Must have been strong ale to land me in this place.
			};

		[Constructable]
		public Hider()
			: base( "" )
		{
		}

		public Hider( Serial serial )
			: base( serial )
		{
		}

		public override void InitBody()
		{
			InitStats( 100, 100, 25 );

			Hue = 0x8408;

			if ( Utility.RandomBool() )
			{
				Body = 0x191;

				Female = true;
			}
			else
			{
				Body = 0x190;

				Female = false;
			}

			Name = NameList.RandomName( "hider" );
		}

		public override void InitOutfit()
		{
			AddItem( new Shoes( 0x590 ) );
			AddItem( new TattsukeHakama( 0x1BB ) );
			AddItem( new Kasa() );
			AddItem( new HakamaShita( 0x901 ) );

			AddItem( new PonyTail( 0x459 ) );
		}

		public override bool CanTalkTo( PlayerMobile pm )
		{
			return false;
		}

		public override void OnTalk( PlayerMobile player, bool contextMenu )
		{
			return;
		}

		public override void OnMovement( Mobile m, Point3D oldLocation )
		{
			if ( m.Alive && m is PlayerMobile && this.InRange( m, 5 ) && !this.InRange( oldLocation, 5 ) )
			{
				if ( 0.3 >= Utility.RandomDouble() )
				{
					Say( Utility.RandomList( m_Messages ) );
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
	}
}
