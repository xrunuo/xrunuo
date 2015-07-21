using Server.Factions;

namespace Server.Mobiles
{
	[CorpseName( "a crystal wisp corpse" )]
	public class CrystalWisp : Wisp // TODO: No info on Stratics yet.
	{
		public override Faction FactionAllegiance { get { return null; } }

		[Constructable]
		public CrystalWisp()
			: base()
		{
			Name = "a crystal wisp";
			Hue = 0x47E; // TODO: Correct

			SetDamageType( ResistanceType.Energy, 0 );
			SetDamageType( ResistanceType.Cold, 50 );

			SetResistance( ResistanceType.Fire, 5, 10 );
			SetResistance( ResistanceType.Cold, 50, 70 );
			SetResistance( ResistanceType.Poison, 10, 30 );
			SetResistance( ResistanceType.Energy, 20, 40 );
		}

		public CrystalWisp( Serial serial )
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
			/*int version = */reader.ReadInt();
		}
	}
}
