using System;
using System.Collections;
using Server;
using Server.Items;
using Server.SkillHandlers;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "an enslaved satyr corpse" )]
	public class EnslavedSatyr : Satyr
	{
		public override int SuccessSound { get { return 0x58B; } }
		public override int FailureSound { get { return 0x58C; } }

		[Constructable]
		public EnslavedSatyr()
			: base()
		{
			FightMode = FightMode.Closest;
			Name = "an enslaved satyr";
		}

		public EnslavedSatyr( Serial serial )
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