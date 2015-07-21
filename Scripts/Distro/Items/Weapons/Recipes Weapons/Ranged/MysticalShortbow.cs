using System;
using Server;

namespace Server.Items
{
	public class MysticalShortbow : MagicalShortbow
	{
		public override int LabelNumber { get { return 1073511; } } // Mystical Shortbow

		[Constructable]
		public MysticalShortbow()
		{
			Attributes.SpellChanneling = 1;
		}


		public MysticalShortbow( Serial serial )
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

			/*int version = */
			reader.ReadInt();
		}
	}
}