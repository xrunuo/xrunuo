using System;
using Server;

namespace Server.Items
{
	public class FireDemonStatueAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new FireDemonStatueDeed(); } }
		public override int LabelNumber { get { return 1076674; } } // Fire Demon Statue

		[Constructable]
		public FireDemonStatueAddon()
		{
			AddComponent( new AddonComponent( 0x364B ), 0, 0, 0 );
		}

		public FireDemonStatueAddon( Serial serial )
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

	public class FireDemonStatueDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new FireDemonStatueAddon(); } }
		public override int LabelNumber { get { return 1076674; } } // Fire Demon Statue

		[Constructable]
		public FireDemonStatueDeed()
		{
			Hue = 1908;
		}

		public FireDemonStatueDeed( Serial serial )
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