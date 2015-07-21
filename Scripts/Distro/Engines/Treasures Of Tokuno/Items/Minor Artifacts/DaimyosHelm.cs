using System;
using Server;

namespace Server.Items
{
	public class DaimyosHelm : PlateBattleKabuto
	{
		public override int LabelNumber { get { return 1070920; } } // Daimyo's Helm

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public DaimyosHelm()
		{
			ArmorAttributes.SelfRepair = 3;
			Attributes.WeaponSpeed = 10;
			ArmorAttributes.LowerStatReq = 100;
			ArmorAttributes.MageArmor = 1;
			Resistances.Cold = 8;
		}

		public DaimyosHelm( Serial serial )
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
				Resistances.Cold = 8;
		}
	}
}
