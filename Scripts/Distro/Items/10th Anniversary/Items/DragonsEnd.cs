using System;
using Server;

namespace Server.Items
{
	public class DragonsEnd : Longsword
	{
		public override int ArtifactRarity { get { return 11; } }

		public override int LabelNumber { get { return 1079791; } } // Dragon's End

		public override int InitMinHits { get { return 120; } }
		public override int InitMaxHits { get { return 120; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public DragonsEnd()
		{
			Hue = 0x52E;

			Slayer = SlayerName.Dragon;

			Attributes.AttackChance = 10;
			Attributes.WeaponDamage = 60;

			Resistances.Fire = 20;
		}

		public override void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy )
		{
			phys = fire = pois = nrgy = 0;
			cold = 100;
		}

		public DragonsEnd( Serial serial )
			: base( serial )
		{
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