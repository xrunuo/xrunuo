using System;
using Server.Items;
using Server.Network;

namespace Server.Items
{
	[FlipableAttribute( 0x1765, 0x1767 )]
	public class UncutCloth : Item, IScissorable, IDyable, ICommodity
	{
		[Constructable]
		public UncutCloth()
			: this( 1 )
		{
		}

		[Constructable]
		public UncutCloth( int amount )
			: base( 0x1767 )
		{
			Stackable = true;
			Weight = 0.1;
			Amount = amount;
		}

		public UncutCloth( Serial serial )
			: base( serial )
		{
		}

		public bool Dye( Mobile from, IDyeTub sender )
		{
			if ( Deleted )
			{
				return false;
			}

			Hue = sender.DyedHue;

			return true;
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

		public bool Scissor( Mobile from, Scissors scissors )
		{
			if ( Deleted || !from.CanSee( this ) )
			{
				return false;
			}

			base.ScissorHelper( from, new Bandage(), 1 );

			return true;
		}
	}
}