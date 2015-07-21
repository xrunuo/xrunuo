using System;

namespace Server.Items
{
	[FlipableAttribute( 0x2BE1, 0x2BE2 )]
	public class GingerbreadCookie : Food
	{
		public override int LabelNumber { get { return 1031233; } } // Gingerbread Cookie

		[Constructable]
		public GingerbreadCookie()
			: this( false )
		{
		}

		[Constructable]
		public GingerbreadCookie( bool reward )
			: base( Utility.RandomBool() ? 0x2BE1 : 0x2BE2 )
		{
			Weight = 1.0;

			if ( reward )
				LootType = LootType.Blessed;
		}

		public GingerbreadCookie( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */reader.ReadInt();
		}
	}
}