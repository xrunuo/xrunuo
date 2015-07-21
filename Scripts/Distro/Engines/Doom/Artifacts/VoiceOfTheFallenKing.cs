using System;
using Server;

namespace Server.Items
{
	public class VoiceOfTheFallenKing : LeatherGorget
	{
		public override int LabelNumber { get { return 1061094; } } // Voice of the Fallen King
		public override int ArtifactRarity { get { return 11; } }

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		[Constructable]
		public VoiceOfTheFallenKing()
		{
			Hue = 0x76D;

			Attributes.BonusStr = 8;
			Attributes.RegenHits = 5;
			Attributes.RegenStam = 3;
			Resistances.Cold = 15;
			Resistances.Energy = 15;
		}

		public VoiceOfTheFallenKing( Serial serial )
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