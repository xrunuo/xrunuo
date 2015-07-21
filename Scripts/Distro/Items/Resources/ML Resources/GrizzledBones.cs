using System;

namespace Server.Items
{
	public class GrizzledBones : Item, ICommodity
	{
		public override int LabelNumber { get { return 1032684; } } // grizzled bones

		[Constructable]
		public GrizzledBones()
			: this( 1 )
		{
		}

		[Constructable]
		public GrizzledBones( int amount )
			: base( 0x318c )
		{
			Stackable = true;
			Weight = 1;
			Amount = amount;
		}

		public GrizzledBones( Serial serial )
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