using System;
using Server;

namespace Server.Items
{
	public class AssassinsShortbow : MagicalShortbow
	{
		public override int LabelNumber { get { return 1073512; } } // Assassin's Shortbow


		[Constructable]
		public AssassinsShortbow()
		{
			Attributes.WeaponDamage = 4;
			Attributes.AttackChance = 3;
		}


		public AssassinsShortbow( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */reader.ReadInt();
		}
	}
}