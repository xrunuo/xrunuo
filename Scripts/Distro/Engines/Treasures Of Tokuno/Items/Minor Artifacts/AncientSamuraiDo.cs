using System;
using Server;

namespace Server.Items
{
	public class AncientSamuraiDo : PlateDo
	{
		public override int LabelNumber { get { return 1070926; } } // Ancient Samurai Do

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public AncientSamuraiDo()
		{
			SkillBonuses.SetValues( 0, SkillName.Parry, 10.0 );
			ArmorAttributes.LowerStatReq = 100;
			ArmorAttributes.MageArmor = 1;

			Resistances.Physical = 10;
			Resistances.Fire = 9;
			Resistances.Cold = 8;
			Resistances.Poison = 8;
			Resistances.Energy = 6;
		}

		public AncientSamuraiDo( Serial serial )
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
			{
				Resistances.Physical = 10;
				Resistances.Fire = 9;
				Resistances.Cold = 8;
				Resistances.Poison = 8;
				Resistances.Energy = 6;
			}
		}
	}
}
