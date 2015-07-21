using System;
using Server.Items;

namespace Server.Items
{
	public class SongWovenMantle : LeafArms
	{
		public override int LabelNumber { get { return 1072931; } } // Song Woven Mantle

		[Constructable]
		public SongWovenMantle()
		{
			Hue = 0x493;

			SkillBonuses.SetValues( 0, SkillName.Musicianship, 10.0 );

			Attributes.Luck = 100;
			Attributes.DefendChance = 5;

			Resistances.Physical = 12;
			Resistances.Cold = 12;
			Resistances.Energy = 12;
		}

		public SongWovenMantle( Serial serial )
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
				Resistances.Physical = 12;
				Resistances.Cold = 12;
				Resistances.Energy = 12;
			}
		}
	}
}