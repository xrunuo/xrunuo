using System;
using Server;

namespace Server.Items
{
	public class BurglarsBandana : Bandana
	{
		public override int LabelNumber { get { return 1063473; } } // Burglar's Bandana

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public BurglarsBandana()
		{
			Hue = 0x58C;

			Resistances.Physical = 10;
			Resistances.Fire = 2;
			Resistances.Cold = 2;
			Resistances.Poison = 2;
			Resistances.Energy = 2;

			SkillBonuses.SetValues( 0, SkillName.Stealing, 10.0 );
			SkillBonuses.SetValues( 1, SkillName.Stealth, 10.0 );
			SkillBonuses.SetValues( 2, SkillName.Snooping, 10.0 );

			Attributes.BonusDex = 5;
		}

		public BurglarsBandana( Serial serial )
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
				Resistances.Fire = 2;
				Resistances.Cold = 2;
				Resistances.Poison = 2;
				Resistances.Energy = 2;
			}
		}
	}
}
