using System;
using Server;

namespace Server.Items
{
	public class DemonForks : Sai
	{
		public override int LabelNumber { get { return 1070917; } } // Demon Forks

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public DemonForks()
		{
			Attributes.ReflectPhysical = 10;
			Attributes.DefendChance = 10;
			Attributes.WeaponDamage = 35;
			Resistances.Fire = 10;
			Resistances.Poison = 10;
		}

		public DemonForks( Serial serial )
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

			/*int version = */
			reader.ReadInt();
		}
	}
}
