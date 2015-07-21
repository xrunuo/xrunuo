using System;
using Server;

namespace Server.Items
{
	public class HanzosBow : Yumi
	{
		public override int LabelNumber { get { return 1070918; } } // Hanzo's Bow

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public HanzosBow()
		{
			SkillBonuses.SetValues( 0, SkillName.Ninjitsu, 10.0 );
			WeaponAttributes.HitLeechHits = 40;
			WeaponAttributes.SelfRepair = 3;
			Attributes.WeaponDamage = 50;
		}

		public HanzosBow( Serial serial )
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
