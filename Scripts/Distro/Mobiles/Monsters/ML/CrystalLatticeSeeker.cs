using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a crystal lattice seeker corpse" )]
	public class CrystalLatticeSeeker : EtherealWarrior
	{
		public override bool InitialInnocent { get { return false; } }

		[Constructable]
		public CrystalLatticeSeeker()
			: base()
		{
			Name = "Crystal Lattice Seeker";
			FightMode = FightMode.Closest; // TODO: Verify
			Hue = 0x47E; // TODO: Correct

			PackSpellweavingScroll();
		}
		public override int TreasureMapLevel { get { return -1; } }

        protected override void OnAfterDeath(Container c)
        {
            base.OnAfterDeath(c);

            if (0.2 > Utility.RandomDouble())
                c.DropItem(new PiecesOfCrystal());
        }

		public CrystalLatticeSeeker( Serial serial )
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
