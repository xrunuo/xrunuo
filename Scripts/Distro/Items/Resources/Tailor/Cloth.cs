using System;
using Server.Items;
using Server.Network;

namespace Server.Items
{
	[FlipableAttribute( 0x1766, 0x1768 )]
	public class Cloth : Item, IScissorable, IDyable, ICommodity
	{
		[Constructable]
		public Cloth()
			: this( 1 )
		{
		}

		[Constructable]
		public Cloth( int amount )
			: base( 0x1766 )
		{
			Stackable = true;
			Weight = 0.1;
			Amount = amount;
		}

		public Cloth( Serial serial )
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
				return false;

			from.SendLocalizedMessage( 1008117 ); // You cut the material into bandages and place them in your backpack.

			base.ScissorHelper( from, new Bandage(), 1 );

			return true;
		}
	}

	public class CutUpCloth : Item
	{
		[Constructable]
		public CutUpCloth()
			: base( 0x1767 )
		{
			Name = "cut-up cloth";
		}

		public CutUpCloth( Serial serial )
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

		public void CutUp( Mobile from, Item[] items )
		{
			//Container backpack = from.Backpack;

			for ( int i = 0; i < items.Length; i++ )
			{
				BoltOfCloth boc = items[i] as BoltOfCloth;

				if ( boc != null )
					boc.Scissor( from, null );
			}
		}
	}

	public class CombineCloth : Item
	{
		[Constructable]
		public CombineCloth()
			: base( 0x1767 )
		{
			Name = "combine cloth";
		}

		public CombineCloth( Serial serial )
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

		public static bool CheckHue( int hue, int[] hues, out int count )
		{
			int result = 0;
			bool success = true;

			for ( int i = 0; i < hues.Length; i++ )
			{
				if ( hues[i] == hue )
				{
					result = i;
					success = false;
				}
			}

			count = result;

			return success;
		}

		public void Combine( Mobile from, Item[] items )
		{
			Container backpack = from.Backpack;

			int[] hues = new int[backpack.Items.Count];
			int[] amounts = new int[backpack.Items.Count];

			for ( int i = 0; i < items.Length; i++ )
			{
				Cloth c = items[i] as Cloth;

				if ( c != null )
				{
					int count;

					if ( CheckHue( c.Hue, hues, out count ) )
					{
						hues[i] = c.Hue;
						amounts[i] = c.Amount;
					}
					else
					{
						amounts[count] += c.Amount;
					}

					c.Delete();
				}
			}

			for ( int i = 0; i < hues.Length; i++ )
			{
				Cloth cloth = new Cloth();
				cloth.Hue = hues[i];
				cloth.Amount = amounts[i];

				if ( cloth.Amount > 0 )
					backpack.DropItem( cloth );
				else
					cloth.Delete();
			}
		}
	}
}
