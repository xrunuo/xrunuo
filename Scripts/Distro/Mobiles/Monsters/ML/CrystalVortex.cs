using Server;
using System;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a crystal vortex corpse" )]
	public class CrystalVortex : EnergyVortex
	{
		[Constructable]
		public CrystalVortex()
			: base()
		{
			Name = "a crystal vortex";
			Hue = 690; // TODO: Correct
		}

		public CrystalVortex( Serial serial )
			: base( serial )
		{
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average, 2 );
		}

        protected override void OnAfterDeath(Container c)
        {
            base.OnAfterDeath(c);

            if (0.2 > Utility.RandomDouble())
                c.DropItem(new JaggedCrystals());
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