using System;

namespace Server.Items
{
	public abstract class Hair : Item
	{
		protected Hair( int itemID )
			: this( itemID, 0 )
		{
		}

		protected Hair( int itemID, int hue )
			: base( itemID )
		{
			LootType = LootType.Blessed;
			Layer = Layer.Hair;
			Hue = hue;
		}

		public Hair( Serial serial )
			: base( serial )
		{
		}

		public override bool DisplayLootType { get { return false; } }

		public override bool VerifyMove( Mobile from )
		{
			return ( from.AccessLevel >= AccessLevel.GameMaster );
		}

		public override DeathMoveResult OnParentDeath( Mobile parent )
		{
			//Dupe( Amount );

			parent.HairItemID = this.ItemID;
			parent.HairHue = this.Hue;

			return DeathMoveResult.MoveToCorpse;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			LootType = LootType.Blessed;

			int version = reader.ReadInt();
		}
	}

	public class GenericHair : Hair
	{

		private GenericHair( int itemID )
			: this( itemID, 0 )
		{
		}


		private GenericHair( int itemID, int hue )
			: base( itemID, hue )
		{
		}

		public GenericHair( Serial serial )
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

			int version = reader.ReadInt();
		}
	}

	public class Mohawk : Hair
	{

		private Mohawk()
			: this( 0 )
		{
		}


		private Mohawk( int hue )
			: base( 0x2044, hue )
		{
		}

		public Mohawk( Serial serial )
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

			int version = reader.ReadInt();
		}
	}

	public class PageboyHair : Hair
	{

		private PageboyHair()
			: this( 0 )
		{
		}


		private PageboyHair( int hue )
			: base( 0x2045, hue )
		{
		}

		public PageboyHair( Serial serial )
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

			int version = reader.ReadInt();
		}
	}

	public class BunsHair : Hair
	{

		private BunsHair()
			: this( 0 )
		{
		}


		private BunsHair( int hue )
			: base( 0x2046, hue )
		{
		}

		public BunsHair( Serial serial )
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

			int version = reader.ReadInt();
		}
	}

	public class LongHair : Hair
	{

		private LongHair()
			: this( 0 )
		{
		}


		private LongHair( int hue )
			: base( 0x203C, hue )
		{
		}

		public LongHair( Serial serial )
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

			int version = reader.ReadInt();
		}
	}

	public class ShortHair : Hair
	{

		private ShortHair()
			: this( 0 )
		{
		}


		private ShortHair( int hue )
			: base( 0x203B, hue )
		{
		}

		public ShortHair( Serial serial )
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

			int version = reader.ReadInt();
		}
	}

	public class PonyTail : Hair
	{

		private PonyTail()
			: this( 0 )
		{
		}


		private PonyTail( int hue )
			: base( 0x203D, hue )
		{
		}

		public PonyTail( Serial serial )
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

			int version = reader.ReadInt();
		}
	}

	public class Afro : Hair
	{

		private Afro()
			: this( 0 )
		{
		}


		private Afro( int hue )
			: base( 0x2047, hue )
		{
		}

		public Afro( Serial serial )
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

			int version = reader.ReadInt();
		}
	}

	public class ReceedingHair : Hair
	{

		private ReceedingHair()
			: this( 0 )
		{
		}


		private ReceedingHair( int hue )
			: base( 0x2048, hue )
		{
		}

		public ReceedingHair( Serial serial )
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

			int version = reader.ReadInt();
		}
	}

	public class TwoPigTails : Hair
	{

		private TwoPigTails()
			: this( 0 )
		{
		}


		private TwoPigTails( int hue )
			: base( 0x2049, hue )
		{
		}

		public TwoPigTails( Serial serial )
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

			int version = reader.ReadInt();
		}
	}

	public class KrisnaHair : Hair
	{

		private KrisnaHair()
			: this( 0 )
		{
		}


		private KrisnaHair( int hue )
			: base( 0x204A, hue )
		{
		}

		public KrisnaHair( Serial serial )
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

			int version = reader.ReadInt();
		}
	}

	public class MidLongHair : Hair
	{

		private MidLongHair()
			: this( 0 )
		{
		}


		private MidLongHair( int hue )
			: base( 0x2FBF, hue )
		{
		}

		public MidLongHair( Serial serial )
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
	public class LongFeatherHair : Hair
	{

		private LongFeatherHair()
			: this( 0 )
		{
		}


		private LongFeatherHair( int hue )
			: base( 0x2FC0, hue )
		{
		}

		public LongFeatherHair( Serial serial )
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
	public class ShortElfHair : Hair
	{

		private ShortElfHair()
			: this( 0 )
		{
		}


		private ShortElfHair( int hue )
			: base( 0x2FC1, hue )
		{
		}

		public ShortElfHair( Serial serial )
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
	public class LongElfHair : Hair
	{

		private LongElfHair()
			: this( 0 )
		{
		}


		private LongElfHair( int hue )
			: base( 0x2FCD, hue )
		{
		}

		public LongElfHair( Serial serial )
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
	public class FlowerHair : Hair
	{

		private FlowerHair()
			: this( 0 )
		{
		}


		private FlowerHair( int hue )
			: base( 0x2FCC, hue )
		{
		}

		public FlowerHair( Serial serial )
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
	public class LongBigKnobHair : Hair
	{

		private LongBigKnobHair()
			: this( 0 )
		{
		}


		private LongBigKnobHair( int hue )
			: base( 0x2FCE, hue )
		{
		}

		public LongBigKnobHair( Serial serial )
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
	public class Mullet : Hair
	{

		private Mullet()
			: this( 0 )
		{
		}


		private Mullet( int hue )
			: base( 0x2FC2, hue )
		{
		}

		public Mullet( Serial serial )
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
	public class LongBigBraidHair : Hair
	{

		private LongBigBraidHair()
			: this( 0 )
		{
		}


		private LongBigBraidHair( int hue )
			: base( 0x2FCF, hue )
		{
		}

		public LongBigBraidHair( Serial serial )
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
	public class LongBigBunHair : Hair
	{

		private LongBigBunHair()
			: this( 0 )
		{
		}


		private LongBigBunHair( int hue )
			: base( 0x2FD0, hue )
		{
		}

		public LongBigBunHair( Serial serial )
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
	public class SpikedHair : Hair
	{

		private SpikedHair()
			: this( 0 )
		{
		}


		private SpikedHair( int hue )
			: base( 0x2FD1, hue )
		{
		}

		public SpikedHair( Serial serial )
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
	public class LongElfTwoHair : Hair
	{

		private LongElfTwoHair()
			: this( 0 )
		{
		}


		private LongElfTwoHair( int hue )
			: base( 0x2FD2, hue )
		{
		}

		public LongElfTwoHair( Serial serial )
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
