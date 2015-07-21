using System;
using Server;

namespace Server.Items
{
	public class TokenOfHolyFavor : GoldBracelet
	{
		public override int LabelNumber { get { return 1113652; } } // Token of Holy Favor

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public TokenOfHolyFavor()
		{
			Hue = 495;

			Attributes.BonusHits = 5;
			Attributes.AttackChance = 10;
			Attributes.DefendChance = 10;
			Attributes.CastSpeed = 1;
			Attributes.CastRecovery = 2;
			Attributes.SpellDamage = 4;
			Resistances.Cold = 5;
			Resistances.Poison = 5;
		}

		public TokenOfHolyFavor( Serial serial )
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

			if ( !CanLoseDurability )
				HitPoints = MaxHitPoints = Utility.RandomMinMax( InitMinHits, InitMaxHits );
		}
	}
}
