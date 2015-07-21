using System;
using Server;

namespace Server.Items
{
	public class EternalGuardianStaff : GnarledStaff
	{
		public override int LabelNumber { get { return 1112443; } } // Eternal Guardian Staff

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public EternalGuardianStaff()
		{
			Hue = 193; // TODO (SA): correct

			SkillBonuses.SetValues( 0, SkillName.Mysticism, 15.0 );
			Attributes.SpellChanneling = 1;
			Attributes.CastSpeed = 1;
			Attributes.SpellDamage = 10;
			Attributes.LowerManaCost = 5;
		}

		public EternalGuardianStaff( Serial serial )
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