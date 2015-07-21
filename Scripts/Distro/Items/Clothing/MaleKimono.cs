using System;
using Server.Items;

namespace Server.Items
{
	[Flipable( 0x2782, 0x27CD )]
	public class MaleKimono : BaseOuterTorso
	{
		[Constructable]
		public MaleKimono()
			: this( 0 )
		{
		}

		[Constructable]
		public MaleKimono( int hue )
			: base( 0x2782, hue )
		{
			Weight = 3.0;
		}

		public MaleKimono( Serial serial )
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