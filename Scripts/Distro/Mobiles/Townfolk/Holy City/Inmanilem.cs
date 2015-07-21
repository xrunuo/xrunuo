using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class Inmanilem : BaseHealer
	{
		[Constructable]
		public Inmanilem()
		{
			Name = "Inmanilem";
			Title = "the Healer";
			Race = Race.Gargoyle;
			Blessed = true;
			Hue = 0x86ED;
			HairItemID = 0x425C;
			HairHue = 0x387;
		}

		public override void InitOutfit()
		{
			AddItem( new GargishLeatherArms( 0x8FD ) );
			AddItem( new GargishFancyRobe( 0x8FD ) );
		}

		public override bool CanTeach { get { return false; } }
		public override bool ClickTitle { get { return false; } }

		public override bool CheckResurrect( Mobile m )
		{
			if ( m.Criminal )
			{
				Say( 501222 ); // Thou art a criminal.  I shall not resurrect thee.
				return false;
			}

			return true;
		}

		public Inmanilem( Serial serial )
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