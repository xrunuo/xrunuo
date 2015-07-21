using System;
using Server;

namespace Server.Items
{
	public class SingularityShrineMantra : BaseBook
	{
		public override double BookWeight { get { return 0.0; } }

		[Constructable]
		public SingularityShrineMantra()
			: base( 0xFF2, "Singularity Shrine Mantra", "Unknown", 20, false )
		{
			Hue = 0x2F3;

			Pages[0].Lines = new string[]
				{
					"unorus"
				};
		}

		public SingularityShrineMantra( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}
}