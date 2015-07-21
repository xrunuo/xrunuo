using System;
using Server;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x1E15, 0x1E16 )]
	public class TravestysSushiPreparation : Item
	{
		public override int LabelNumber { get { return 1075093; } } // Travesty's Sushi Preparations

		[Constructable]
		public TravestysSushiPreparation()
			: base( Utility.RandomBool() ? 0x1E15 : 0x1E16 )
		{
			Weight = 1.0;
		}

		public TravestysSushiPreparation( Serial serial )
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