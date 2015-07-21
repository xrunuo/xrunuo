using System;
using Server;

namespace Server.Items
{
	public class SingingAxe : OrnateAxe
	{
		public override int LabelNumber { get { return 1073546; } } // Singing Axe

		[Constructable]
		public SingingAxe()
		{
			SkillBonuses.SetValues( 0, SkillName.Musicianship, 5.0 );
		}


		public SingingAxe( Serial serial )
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