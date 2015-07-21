using System;
using Server;

namespace Server.Items
{
	public class ChestOfHeirlooms : WoodenFootLocker
	{
		public override int LabelNumber { get { return 1070937; } } // Chest of Heirlooms

		[Constructable]
		public ChestOfHeirlooms()
		{
			TrapType = TrapType.ExplosionTrap;
			TrapPower = 100;
			TrapLevel = 10;
			TrapEnabled = true;

			Locked = true;
			LockLevel = 80;
			MaxLockLevel = 100;
			RequiredSkill = 80;

			Fill();
		}

		public void Fill()
		{
			for ( int i = 0; i < Utility.Random( 3, 5 ); i++ )
			{
				BaseWeapon weapon = Loot.RandomWeapon();

				if ( weapon != null )
					BaseRunicTool.ApplyAttributesTo( weapon, 1, 10, 30 );

				BaseArmor armor = Loot.RandomArmor();

				if ( armor != null )
					BaseRunicTool.ApplyAttributesTo( armor, 1, 10, 30 );

				BaseJewel jewel = Loot.RandomJewelry();

				if ( jewel != null )
					BaseRunicTool.ApplyAttributesTo( jewel, 1, 10, 30 );

				DropItem( weapon );
				DropItem( armor );
				DropItem( jewel );
			}
		}

		public ChestOfHeirlooms( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 2 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( version < 2 )
				TrapLevel = 10;

			if ( version < 1 )
			{
				LockLevel = 80;
				MaxLockLevel = 100;
				RequiredSkill = 80;
			}
		}
	}
}