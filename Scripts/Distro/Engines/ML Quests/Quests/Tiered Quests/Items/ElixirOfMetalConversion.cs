using System;
using Server;
using Server.Targeting;

namespace Server.Items
{
	public abstract class BaseElixirOfMetalConversion : Item
	{
		protected abstract Type AcceptedType { get; }
		protected abstract Item ConstructIngots();

		[Constructable]
		public BaseElixirOfMetalConversion( int hue )
			: base( 0xE2C )
		{
			Weight = 1.0;
			Hue = hue;
		}

		public override void OnDoubleClick( Mobile from )
		{
			from.SendLocalizedMessage( 1113045 ); // Select the ingots you wish to convert.
			from.BeginTarget( -1, false, TargetFlags.None, SelectIngots_Callback );
		}

		private void SelectIngots_Callback( Mobile from, object targeted )
		{
			Item item = targeted as Item;

			if ( item == null )
				return;

			if ( !this.IsChildOf( from.Backpack ) )
				from.SendLocalizedMessage( 1062334 ); // This item must be in your backpack to be used.
			else if ( !item.IsChildOf( from.Backpack ) )
				from.SendLocalizedMessage( 1062334 ); // This item must be in your backpack to be used.
			else if ( item.Amount != 500 )
				from.SendLocalizedMessage( 1113046 ); // You can only convert five hundred ingots at a time.
			else if ( item.GetType() != AcceptedType )
				from.SendLocalizedMessage( 1113047 ); // This elixir isn't made for that type of metal.
			else
			{
				Item ingots = ConstructIngots();

				from.AddToBackpack( ingots );
				ingots.Location = item.Location;

				item.Delete();
				this.Delete();

				from.SendLocalizedMessage( 1113048 ); // You've successfully converted the metal.
			}
		}

		public BaseElixirOfMetalConversion( Serial serial )
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

	public class ElixirOfGoldConversion : BaseElixirOfMetalConversion
	{
		public override int LabelNumber { get { return 1113007; } } // Elixir of Gold Conversion

		protected override Type AcceptedType { get { return typeof( DullCopperIngot ); } }

		protected override Item ConstructIngots()
		{
			return new GoldIngot( 500 );
		}

		[Constructable]
		public ElixirOfGoldConversion()
			: base( 0x8A5 )
		{
		}

		public ElixirOfGoldConversion( Serial serial )
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

	public class ElixirOfAgapiteConversion : BaseElixirOfMetalConversion
	{
		public override int LabelNumber { get { return 1113008; } } // Elixir of Agapite Conversion

		protected override Type AcceptedType { get { return typeof( ShadowIronIngot ); } }

		protected override Item ConstructIngots()
		{
			return new AgapiteIngot( 500 );
		}

		[Constructable]
		public ElixirOfAgapiteConversion()
			: base( 0x979 )
		{
		}

		public ElixirOfAgapiteConversion( Serial serial )
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

	public class ElixirOfVeriteConversion : BaseElixirOfMetalConversion
	{
		public override int LabelNumber { get { return 1113009; } } // Elixir of Verite Conversion

		protected override Type AcceptedType { get { return typeof( CopperIngot ); } }

		protected override Item ConstructIngots()
		{
			return new VeriteIngot( 500 );
		}

		[Constructable]
		public ElixirOfVeriteConversion()
			: base( 0x89F )
		{
		}

		public ElixirOfVeriteConversion( Serial serial )
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

	public class ElixirOfValoriteConversion : BaseElixirOfMetalConversion
	{
		public override int LabelNumber { get { return 1113010; } } // Elixir of Valorite Conversion

		protected override Type AcceptedType { get { return typeof( BronzeIngot ); } }

		protected override Item ConstructIngots()
		{
			return new ValoriteIngot( 500 );
		}

		[Constructable]
		public ElixirOfValoriteConversion()
			: base( 0x89F )
		{
		}

		public ElixirOfValoriteConversion( Serial serial )
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

	public class ElixirOfMetalConversion : BaseElixirOfMetalConversion
	{
		public override int LabelNumber { get { return 1113011; } } // Elixir of Metal Conversion

		protected override Type AcceptedType { get { return typeof( IronIngot ); } }

		protected override Item ConstructIngots()
		{
			switch ( Utility.Random( 4 ) )
			{
				default:
				case 0: return new DullCopperIngot( 500 );
				case 1: return new ShadowIronIngot( 500 );
				case 2: return new CopperIngot( 500 );
				case 3: return new BronzeIngot( 500 );
			}
		}

		[Constructable]
		public ElixirOfMetalConversion()
			: base( 0x89F )
		{
		}

		public ElixirOfMetalConversion( Serial serial )
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