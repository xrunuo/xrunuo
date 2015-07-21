using System;
using Server.Items;

namespace Server.Items
{
	public class SpellWovenBritches : LeafLeggings
	{
		public override int LabelNumber { get { return 1072929; } } // Spell Woven Britches

		[Constructable]
		public SpellWovenBritches()
		{
			Hue = 0x487;

			SkillBonuses.SetValues( 0, SkillName.Meditation, 10.0 );

			Attributes.BonusInt = 8;
			Attributes.SpellDamage = 10;
			Attributes.LowerManaCost = 10;

			Resistances.Fire = 12;
			Resistances.Poison = 12;
		}

		public SpellWovenBritches( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 1 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();

			if ( version < 1 )
			{
				Resistances.Fire = 12;
				Resistances.Poison = 12;
			}
		}
	}
}