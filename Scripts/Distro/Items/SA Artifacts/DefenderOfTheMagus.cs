using System;
using Server;

namespace Server.Items
{
	public class DefenderOfTheMagus : MetalShield
	{
		public override int LabelNumber { get { return 1113851; } } // Defender of the Magus

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public DefenderOfTheMagus()
		{
			Hue = 0x495;

			Attributes.SpellChanneling = 1;
			Attributes.CastSpeed = 1;
			Attributes.DefendChance = 10;
			Attributes.CastRecovery = 1;

			switch ( Utility.Random( 5 ) )
			{
				case 0:
					AbsorptionAttributes.KineticResonance = 10;
					Resistances.Physical = 10;
					break;
				case 1:
					AbsorptionAttributes.FireResonance = 10;
					Resistances.Fire = 10;
					break;
				case 2:
					AbsorptionAttributes.ColdResonance = 10;
					Resistances.Cold = 10;
					break;
				case 3:
					AbsorptionAttributes.PoisonResonance = 10;
					Resistances.Poison = 10;
					break;
				case 4:
					AbsorptionAttributes.EnergyResonance = 10;
					Resistances.Energy = 10;
					break;
			}
		}

		public DefenderOfTheMagus( Serial serial )
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