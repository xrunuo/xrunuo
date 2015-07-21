using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class UnderworldGuard : Mobile
	{
		[Constructable]
		public UnderworldGuard()
		{
			Name = NameList.RandomName( "male" );
			Title = "the Guard";
			Race = Race.Human;

			int hairHue = Race.RandomHairHue();
			Utility.AssignRandomHair( this, hairHue );
			Utility.AssignRandomFacialHair( this, hairHue );

			Hue = Race.RandomSkinHue();

			Hits = HitsMax;

			Blessed = true;
			Frozen = true;

			AddItem( new PlateLegs() );
			AddItem( new PlateChest() );
			AddItem( new PlateArms() );
		}

		public UnderworldGuard( Serial serial )
			: base( serial )
		{
		}

		public override bool CanBeDamaged()
		{
			return false;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}
}
