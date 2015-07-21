using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
	public class Laifem : MondainQuester
	{
		private ArrayList m_SBInfos = new ArrayList();
		protected override ArrayList SBInfos { get { return m_SBInfos; } }

		public override bool IsActiveVendor { get { return true; } }

		public override void InitSBInfo()
		{
			SBInfos.Add( new SBCarpets() );
		}

		private static Type[] m_Quests = new Type[] { typeof( ShearingKnowledgeQuest ) };

		public override Type[] Quests { get { return m_Quests; } }

		[Constructable]
		public Laifem()
			: base( "Laifem", "the Weaver" )
		{
		}

		public Laifem( Serial serial )
			: base( serial )
		{
		}

		public override void VendorBuy( Mobile from )
		{
			if ( !( from is PlayerMobile ) || !( (PlayerMobile) from ).CanBuyCarpets )
			{
				SayTo( from, 1113266 ); // I'm sorry, but I don't have any carpets to sell you yet.
				return;
			}

			base.VendorBuy( from );
		}

		public override void Advertise()
		{
		}

		public override void InitBody()
		{
			InitStats( 100, 100, 25 );

			Female = true;
			Race = Race.Gargoyle;

			Hue = 0x86E8;
			HairItemID = 0x4261;
			HairHue = 0x31E;
		}

		public override void InitOutfit()
		{
			AddItem( new Backpack() );
			AddItem( new FemaleGargishClothChest( 0x4D9 ) );
			AddItem( new FemaleGargishClothArms( 0x66C ) );
			AddItem( new FemaleGargishClothKilt( 0x51E ) );
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