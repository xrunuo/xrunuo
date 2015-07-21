using System;
using Server;

namespace Server.Items
{
	public class ConjurersGrimoire : Spellbook
	{
		public override int LabelNumber { get { return 1094799; } } // Conjurer's Grimoire

		[Constructable]
		public ConjurersGrimoire()
		{
			Hue = 1157;
			SkillBonuses.SetValues( 0, SkillName.Magery, 15.0 );
			Slayer = SlayerName.Undead;
			Attributes.BonusInt = 8;
			Attributes.SpellDamage = 15;
			Attributes.LowerManaCost = 10;
		}

		public ConjurersGrimoire( Serial serial )
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
