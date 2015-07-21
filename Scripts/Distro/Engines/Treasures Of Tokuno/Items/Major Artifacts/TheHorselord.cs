using System;
using Server;

namespace Server.Items
{
	public class TheHorselord : Yumi
	{
		public override int LabelNumber { get { return 1070967; } } // The Horselord

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public TheHorselord()
		{
			Slayer = SlayerName.Elemental;
			Slayer2 = SlayerName.Reptile;
			Attributes.BonusDex = 5;
			Attributes.RegenMana = 1;
			Attributes.Luck = 125;
			Attributes.WeaponDamage = 50;
		}

		public TheHorselord( Serial serial )
			: base( serial )
		{
		}

		public void FixMods()
		{
			// old mods
			WeaponAttributes.HitLowerDefend = 0;
			WeaponAttributes.HitLeechStam = 0;
			Attributes.SpellChanneling = 0;

			// new mods
			Slayer = SlayerName.Elemental;
			Slayer2 = SlayerName.Reptile;
			Attributes.RegenMana = 1;
			Attributes.Luck = 125;
		}

		public override void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy )
		{
			phys = 100;

			fire = pois = cold = nrgy = 0;
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
			{
				FixMods();
			}
		}
	}
}