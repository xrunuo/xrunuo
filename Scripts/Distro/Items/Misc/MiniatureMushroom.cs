namespace Server.Items
{
	public class MiniatureMushroom : Food
	{
		public override int LabelNumber { get { return 1073138; } } // Miniature mushroom

		[Constructable]
		public MiniatureMushroom()
			: this( 1 )
		{
		}

		[Constructable]
		public MiniatureMushroom( int amount )
			: base( amount, 0xD16 )
		{
			LootType = LootType.Blessed;
			Weight = 1;
		}

		public MiniatureMushroom( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}
