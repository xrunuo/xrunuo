using System;

namespace Server.Items
{
	[FlipableAttribute( 0x1BD7, 0x1BDA )]
	public class Board : Item, ICommodity
	{
		public override int LabelNumber { get { return 1027132; } } // boards

		[Constructable]
		public Board()
			: this( 1 )
		{
		}

		[Constructable]
		public Board( int amount )
			: base( 0x1BD7 )
		{
			Stackable = true;
			Weight = 1.0;
			Amount = amount;
		}

		public Board( Serial serial )
			: base( serial )
		{
		}

		public override LocalizedText GetNameProperty()
		{
			if ( Amount > 1 )
				return new LocalizedText( 1050039, "{0}\t#{1}", Amount, 1027132 ); // ~1_NUMBER~ ~2_ITEMNAME~
			else
				return new LocalizedText( 1027132 ); // boards
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

	[FlipableAttribute( 0x1BD7, 0x1BDA )]
	public class OakBoard : Item, ICommodity
	{
		public override int LabelNumber { get { return 1075052; } } // Oak Boards

		[Constructable]
		public OakBoard()
			: this( 1 )
		{
		}

		[Constructable]
		public OakBoard( int amount )
			: base( 0x1BD7 )
		{
			Stackable = true;
			Weight = 1.0;
			Amount = amount;
			Hue = 2010;
		}

		public OakBoard( Serial serial )
			: base( serial )
		{
		}

		public override LocalizedText GetNameProperty()
		{
			if ( Amount > 1 )
				return new LocalizedText( 1050039, "{0}\t#{1}", Amount, 1027132 ); // ~1_NUMBER~ ~2_ITEMNAME~
			else
				return new LocalizedText( 1027132 ); // boards
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1072533 );
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

	[FlipableAttribute( 0x1BD7, 0x1BDA )]
	public class AshBoard : Item, ICommodity
	{
		public override int LabelNumber { get { return 1075053; } } // Ash Boards

		[Constructable]
		public AshBoard()
			: this( 1 )
		{
		}

		[Constructable]
		public AshBoard( int amount )
			: base( 0x1BD7 )
		{
			Stackable = true;
			Weight = 1.0;
			Amount = amount;
			Hue = 1191;
		}

		public AshBoard( Serial serial )
			: base( serial )
		{
		}

		public override LocalizedText GetNameProperty()
		{
			if ( Amount > 1 )
				return new LocalizedText( 1050039, "{0}\t#{1}", Amount, 1027132 ); // ~1_NUMBER~ ~2_ITEMNAME~
			else
				return new LocalizedText( 1027132 ); // boards
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1072534 );
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

	[FlipableAttribute( 0x1BD7, 0x1BDA )]
	public class YewBoard : Item, ICommodity
	{
		public override int LabelNumber { get { return 1075054; } }

		[Constructable]
		public YewBoard()
			: this( 1 )
		{
		}

		[Constructable]
		public YewBoard( int amount )
			: base( 0x1BD7 )
		{
			Stackable = true;
			Weight = 1.0;
			Amount = amount;
			Hue = 1192;
		}

		public YewBoard( Serial serial )
			: base( serial )
		{
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1072535 );
		}

		public override LocalizedText GetNameProperty()
		{
			if ( Amount > 1 )
				return new LocalizedText( 1050039, "{0}\t#{1}", Amount, 1027132 ); // ~1_NUMBER~ ~2_ITEMNAME~
			else
				return new LocalizedText( 1027132 ); // boards
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

	[FlipableAttribute( 0x1BD7, 0x1BDA )]
	public class HeartwoodBoard : Item, ICommodity
	{
		public override int LabelNumber { get { return 1075062; } } // Heartwood Boards

		[Constructable]
		public HeartwoodBoard()
			: this( 1 )
		{
		}

		[Constructable]
		public HeartwoodBoard( int amount )
			: base( 0x1BD7 )
		{
			Stackable = true;
			Weight = 1.0;
			Amount = amount;
			Hue = 1193;
		}

		public HeartwoodBoard( Serial serial )
			: base( serial )
		{
		}

		public override LocalizedText GetNameProperty()
		{
			if ( Amount > 1 )
				return new LocalizedText( 1050039, "{0}\t#{1}", Amount, 1027132 ); // ~1_NUMBER~ ~2_ITEMNAME~
			else
				return new LocalizedText( 1027132 ); // boards
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1072536 );
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

	[FlipableAttribute( 0x1BD7, 0x1BDA )]
	public class BloodwoodBoard : Item, ICommodity
	{
		public override int LabelNumber { get { return 1075055; } } // Bloodwood Boards

		[Constructable]
		public BloodwoodBoard()
			: this( 1 )
		{
		}

		[Constructable]
		public BloodwoodBoard( int amount )
			: base( 0x1BD7 )
		{
			Stackable = true;
			Weight = 1.0;
			Amount = amount;
			Hue = 1194;
		}

		public BloodwoodBoard( Serial serial )
			: base( serial )
		{
		}

		public override LocalizedText GetNameProperty()
		{
			if ( Amount > 1 )
				return new LocalizedText( 1050039, "{0}\t#{1}", Amount, 1027132 ); // ~1_NUMBER~ ~2_ITEMNAME~
			else
				return new LocalizedText( 1027132 ); // boards
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1072538 );
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

	[FlipableAttribute( 0x1BD7, 0x1BDA )]
	public class FrostwoodBoard : Item, ICommodity
	{
		public override int LabelNumber { get { return 1075056; } } // Frostwood Boards

		[Constructable]
		public FrostwoodBoard()
			: this( 1 )
		{
		}

		[Constructable]
		public FrostwoodBoard( int amount )
			: base( 0x1BD7 )
		{
			Stackable = true;
			Weight = 1.0;
			Amount = amount;
			Hue = 1151;
		}

		public FrostwoodBoard( Serial serial )
			: base( serial )
		{
		}

		public override LocalizedText GetNameProperty()
		{
			if ( Amount > 1 )
				return new LocalizedText( 1050039, "{0}\t#{1}", Amount, 1027132 ); // ~1_NUMBER~ ~2_ITEMNAME~
			else
				return new LocalizedText( 1027132 ); // boards
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1072539 );
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