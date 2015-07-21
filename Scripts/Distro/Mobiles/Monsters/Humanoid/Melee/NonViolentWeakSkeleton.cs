using System;
using Server.Items;

namespace Server.Mobiles
{
	public class NonViolentWeakSkeleton : WeakSkeleton
	{
		[Constructable]
		public NonViolentWeakSkeleton()
		{
			FightMode = FightMode.Aggressor;
		}

		public NonViolentWeakSkeleton( Serial serial )
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