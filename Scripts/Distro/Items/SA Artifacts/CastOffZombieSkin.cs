using System;
using Server;

namespace Server.Items
{
	public class CastOffZombieSkin : GargishLeatherArms
	{
		public override int LabelNumber { get { return 1113538; } } // Cast-off Zombie Skin

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public CastOffZombieSkin()
		{
			Hue = 846;

			SkillBonuses.SetValues( 0, SkillName.Necromancy, 5.0 );
			SkillBonuses.SetValues( 1, SkillName.SpiritSpeak, 5.0 );
			Attributes.LowerManaCost = 5;
			Attributes.LowerRegCost = 8;
			Attributes.IncreasedKarmaLoss = 5;
			Resistances.Physical = 8;
			Resistances.Fire = -10;
			Resistances.Cold = 11;
			Resistances.Poison = 12;
			Resistances.Energy = -1;
		}

		public CastOffZombieSkin( Serial serial )
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