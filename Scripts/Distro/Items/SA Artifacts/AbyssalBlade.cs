using System;
using Server;

namespace Server.Items
{
	public class AbyssalBlade : StoneWarSword
	{
		public override int LabelNumber { get { return 1113520; } } // Abyssal Blade

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public AbyssalBlade()
		{
			Hue = 997;

			WeaponAttributes.HitManaDrain = 50;
			WeaponAttributes.HitFatigue = 50;
			WeaponAttributes.HitLeechStam = 60;
			WeaponAttributes.HitLeechMana = 60;
			Attributes.WeaponSpeed = 20;
			Attributes.WeaponDamage = 60;
		}

		public override void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy, out int chao )
		{
			phys = fire = cold = pois = nrgy = 0;
			chao = 100;
		}

		public AbyssalBlade( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}
}