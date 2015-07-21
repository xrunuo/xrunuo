using System;
using Server;

namespace Server.Items
{
	public class ResillientBracer : GoldBracelet
	{
		public override int LabelNumber { get { return 1072933; } } // Resillient Bracer

		[Constructable]
		public ResillientBracer()
		{
			Hue = 1422;
			Attributes.BonusHits = 5;
			Attributes.RegenHits = 2;
			SkillBonuses.SetValues( 0, SkillName.MagicResist, 15.0 );
			Attributes.DefendChance = 10;
			Resistances.Physical = 20;
		}

		public ResillientBracer( Serial serial )
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