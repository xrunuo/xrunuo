using System;
using Server;

namespace Server.Items
{
	public class FruitBowl : Food
	{
		public override int LabelNumber { get { return 1072950; } } // fruit bowl

		[Constructable]
		public FruitBowl()
			: base( 0x2D4F )
		{
			Weight = 1.0;
			FillFactor = 20;
			Stackable = false;
		}

		public FruitBowl( Serial serial )
			: base( serial )
		{
		}

		public override bool Eat( Mobile from )
		{
			if ( FillHunger( from, FillFactor ) )
			{
				string modName = Serial.ToString();

				from.AddStatMod( new StatMod( StatType.Str, modName + "Str", (int) ( from.RawStr * 0.08 ), TimeSpan.FromSeconds( 75 ) ) );

				from.PlaySound( 0x1EA );
				from.FixedParticles( 0x373A, 10, 15, 5018, EffectLayer.Waist );

				Consume();

				return true;
			}

			return false;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}