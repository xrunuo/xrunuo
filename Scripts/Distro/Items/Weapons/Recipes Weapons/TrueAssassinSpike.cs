using System;
using Server;

namespace Server.Items
{
	public class TrueAssassinSpike : AssassinSpike
	{
		public override int LabelNumber { get { return 1073517; } } // True Assassin Spike

		[Constructable]
		public TrueAssassinSpike()
		{
			Attributes.WeaponDamage = 4;
			Attributes.AttackChance = 4;
		}


		public TrueAssassinSpike( Serial serial )
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