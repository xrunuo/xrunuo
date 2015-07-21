using System;
using Server;

namespace Server.Items
{
	public class Boomstick : WildStaff
	{
		public override int LabelNumber { get { return 1075032; } } // Boomstick

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public Boomstick()
		{
			Hue = 37;
			Attributes.CastSpeed = 2;
			Attributes.RegenMana = 3;
			Attributes.SpellChanneling = 1;
			Attributes.LowerRegCost = 20;
		}

		public override void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy, out int chao )
		{
			nrgy = fire = phys = pois = cold = 0;
			chao = 100;
		}

		public Boomstick( Serial serial )
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