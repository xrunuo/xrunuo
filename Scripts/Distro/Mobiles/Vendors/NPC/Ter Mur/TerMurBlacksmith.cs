using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Engines.BulkOrders;

namespace Server.Mobiles
{
	public class TerMurBlacksmith : Blacksmith
	{
		private ArrayList m_SBInfos = new ArrayList();
		protected override ArrayList SBInfos { get { return m_SBInfos; } }

		public override Race DefaultRace { get { return Race.Gargoyle; } }

		[Constructable]
		public TerMurBlacksmith()
		{
			Title = "the Blacksmith";

			SetSkill( SkillName.Throwing, 60.0, 93.0 );
		}

		public override void InitSBInfo()
		{
			m_SBInfos.Add( new SBTerMurBlacksmith() );
		}

		public override void InitOutfit()
		{
			AddItem( new GargishClothLeggings( Utility.RandomNeutralHue() ) );
			AddItem( new GargishClothChest( GetRandomHue() ) );
			AddItem( new GargishClothArms( Utility.RandomNeutralHue() ) );
			AddItem( new GargishClothKilt( Utility.RandomNeutralHue() ) );

			AddItem( new SmallPlateShield() );
		}

		public TerMurBlacksmith( Serial serial )
			: base( serial )
		{
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
