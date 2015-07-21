using System;
using Server;

namespace Server.Items
{
	public class Bonesmasher : DiamondMace
	{
		public override int LabelNumber { get { return 1075030; } } // Bonesmasher

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public Bonesmasher()
		{
			Weight = 10.0;
			Hue = 1154;

			WeaponAttributes.SelfRepair = 2;
			WeaponAttributes.HitLeechStam = 40;
			SkillBonuses.SetValues( 0, SkillName.Macing, 10.0 );
		}

		public Bonesmasher( Serial serial )
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