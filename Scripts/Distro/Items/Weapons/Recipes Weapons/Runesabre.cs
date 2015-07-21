using System;
using Server;

namespace Server.Items
{
	public class Runesabre : RuneBlade
	{
		public override int LabelNumber { get { return 1073537; } } //Runesabre
		[Constructable]
		public Runesabre()
		{
			WeaponAttributes.MageWeapon = 1;
			SkillBonuses.SetValues( 0, SkillName.MagicResist, 5.0 );

		}


		public Runesabre( Serial serial )
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