using System;
using Server;

namespace Server.Items
{
	public class DreadsRevenge : Kryss
	{
		public override int LabelNumber { get { return 1072092; } } // Dread's Revenge

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		[Constructable]
		public DreadsRevenge()
		{
			ItemID = 0x1401;
			Hue = 0x48F;
			WeaponAttributes.HitPoisonArea = 30;
			Attributes.AttackChance = 15;
			Attributes.WeaponSpeed = 50;
			SkillBonuses.SetValues( 0, SkillName.Fencing, 20.0 );
		}

		public override void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy )
		{
			fire = cold = nrgy = phys = 0;
			pois = 100;
		}

		public DreadsRevenge( Serial serial )
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