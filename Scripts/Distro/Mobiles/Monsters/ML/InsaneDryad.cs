namespace Server.Mobiles
{
	[CorpseName( "an insane dryad corpse" )]
	public class InsaneDryad : Dryad
	{
		public override bool InitialInnocent { get { return false; } }

		[Constructable]
		public InsaneDryad()
			: base()
		{
			Name = "an insane dryad";
			FightMode = FightMode.Closest;
		}

		public InsaneDryad( Serial serial )
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
