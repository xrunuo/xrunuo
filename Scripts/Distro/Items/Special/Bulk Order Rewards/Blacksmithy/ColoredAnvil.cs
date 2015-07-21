using System;
using Server;
using Server.Engines.Craft;

namespace Server.Items
{
	[Anvil, Flipable( 0xFAF, 0xFB0 )]
	public class ColoredAnvil : Item
	{
		[Constructable]
		public ColoredAnvil()
			: this( CraftResources.GetHue( (CraftResource) Utility.RandomMinMax( (int) CraftResource.DullCopper, (int) CraftResource.Valorite ) ) )
		{
		}

		[Constructable]
		public ColoredAnvil( int hue )
			: base( 0xFAF )
		{
			Hue = hue;
			Weight = 20;
		}

		public ColoredAnvil( Serial serial )
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

	[Flipable( 0x14F0, 0x14EF )]
	public class ColoredAnvilDeed : Item
	{
		public override int LabelNumber { get { return 1044333; } } // anvil (east)

		[Constructable]
		public ColoredAnvilDeed()
			: base( 0x14F0 )
		{
			Hue = CraftResources.GetHue( (CraftResource) Utility.RandomMinMax( (int) CraftResource.DullCopper, (int) CraftResource.Valorite ) );

			Weight = 1.0;
		}

		public ColoredAnvilDeed( Serial serial )
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

		public override void OnDoubleClick( Mobile from )
		{
			if ( IsChildOf( from.Backpack ) )
			{
				ColoredAnvil anvil = new ColoredAnvil( Hue );
				from.Backpack.DropItem( anvil );
				this.Delete();
			}
			else
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
		}
	}
}
