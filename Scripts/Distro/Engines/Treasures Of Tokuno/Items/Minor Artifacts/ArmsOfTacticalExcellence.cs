using System;
using Server;

namespace Server.Items
{
	public class ArmsOfTacticalExcellence : LeatherHiroSode
	{
		public override int LabelNumber { get { return 1070921; } } // Arms of Tactical Excellence

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public ArmsOfTacticalExcellence()
		{
			SkillBonuses.SetValues( 0, SkillName.Tactics, 12.0 );
			Attributes.BonusDex = 5;

			Resistances.Fire = 5;
			Resistances.Cold = 10;
			Resistances.Poison = 5;
		}

		public ArmsOfTacticalExcellence( Serial serial )
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
				Resistances.Fire = 5;
				Resistances.Cold = 10;
				Resistances.Poison = 5;
			}
		}
	}
}
