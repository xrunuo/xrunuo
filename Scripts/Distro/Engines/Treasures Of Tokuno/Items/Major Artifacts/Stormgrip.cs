using System;
using Server;

namespace Server.Items
{
	public class Stormgrip : LeatherNinjaMitts
	{
		public override int LabelNumber { get { return 1070970; } } // Stormgrip

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public Stormgrip()
		{
			Attributes.BonusInt = 8;
			Attributes.Luck = 125;
			Attributes.WeaponDamage = 25;

			Resistances.Physical = 8;
			Resistances.Cold = 15;
			Resistances.Energy = 15;
		}

		public Stormgrip( Serial serial )
			: base( serial )
		{
		}

		public void FixMods()
		{
			// old mods
			Attributes.LowerRegCost = 0;

			// new mods
			Attributes.Luck = 125;
			Attributes.WeaponDamage = 25;
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

			if ( version < 1 )
			{
				FixMods();
			}

			if ( version < 2 )
			{
				Resistances.Physical = 8;
				Resistances.Cold = 15;
				Resistances.Energy = 15;
			}
		}
	}
}