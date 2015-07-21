using System;
using Server;

namespace Server.Items
{
	public class AegisOfGrace : Circlet
	{
		public override int LabelNumber { get { return 1075047; } } // Aegis of Grace

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override int StrengthReq { get { return 10; } }

		public override Race RequiredRace { get { return Race.Elf; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public AegisOfGrace()
			: base()
		{
			Layer = Layer.Helm;
			Weight = 2.0;

			Attributes.DefendChance = 20;
			ArmorAttributes.SelfRepair = 2;
			SkillBonuses.SetValues( 0, SkillName.MagicResist, 10.0 );

			Resistances.Physical = 9;
			Resistances.Fire = 4;
			Resistances.Cold = 5;
			Resistances.Poison = 5;
			Resistances.Energy = 10;
		}

		public AegisOfGrace( Serial serial )
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

			/*int version = */
			reader.ReadInt();
		}
	}
}