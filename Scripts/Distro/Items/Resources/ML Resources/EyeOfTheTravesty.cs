using System;

namespace Server.Items
{
	public class EyeOfTheTravesty : Item, ICommodity
	{
		public override int LabelNumber { get { return 1032685; } } // Eye Of The Travesty

		[Constructable]
		public EyeOfTheTravesty()
			: this( 1 )
		{
		}

		[Constructable]
		public EyeOfTheTravesty( int amount )
			: base( 0x318D )
		{
			Stackable = true;
			Weight = 1;
			Amount = amount;
		}

		public EyeOfTheTravesty( Serial serial )
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