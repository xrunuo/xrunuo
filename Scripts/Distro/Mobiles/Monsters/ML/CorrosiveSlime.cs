using System;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a corrosive slime corpse" )]
	public class CorrosiveSlime : Slime
	{
		[Constructable]
		public CorrosiveSlime()
		{
			Name = "a corrosive slime";
		}

		public CorrosiveSlime( Serial serial )
			: base( serial )
		{
		}

		protected override void OnAfterDeath( Container c )
		{
			base.OnAfterDeath( c );

			if ( 0.2 > Utility.RandomDouble() && c.Map != Map.TerMur )
			{
				switch ( Utility.RandomMinMax( 1, 3 ) )
				{
					case 1: c.DropItem( new GelatanousSkull() ); break;
					case 2: c.DropItem( new PartiallyDigestedTorso() ); break;
					case 3: c.DropItem( new CoagulatedLegs() ); break;
				}
			}
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