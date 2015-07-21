using System;
using Server;

namespace Server.Items
{
	public class GlovesOfThePugilist : LeatherGloves
	{
		public override int LabelNumber { get { return 1070690; } } // Gloves of the Pugilist

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public GlovesOfThePugilist()
		{
			Hue = 0x6D1;
			SkillBonuses.SetValues( 0, SkillName.Wrestling, 10.0 );
			Attributes.BonusDex = 8;
			Attributes.WeaponDamage = 15;
			Resistances.Physical = 16;
		}

		public GlovesOfThePugilist( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( version < 1 )
				Resistances.Physical = 16;
		}
	}
}