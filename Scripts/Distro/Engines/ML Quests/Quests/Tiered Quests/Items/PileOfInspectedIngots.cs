using System;
using Server;

namespace Server.Items
{
	public abstract class PileOfInspectedIngots : Item
	{
		[Constructable]
		public PileOfInspectedIngots( int hue )
			: base( 0x1BF0 )
		{
			Weight = 2.0;
			Hue = hue;
		}

		public PileOfInspectedIngots( Serial serial )
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

	public class PileOfInspectedDullCopperIngots : PileOfInspectedIngots
	{
		public override int LabelNumber { get { return 1113021; } } // Pile of Inspected Dull Copper Ingots

		[Constructable]
		public PileOfInspectedDullCopperIngots()
			: base( 0x973 )
		{
		}

		public PileOfInspectedDullCopperIngots( Serial serial )
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

	public class PileOfInspectedShadowIronIngots : PileOfInspectedIngots
	{
		public override int LabelNumber { get { return 1113022; } } // Pile of Inspected Shadow Iron Ingots

		[Constructable]
		public PileOfInspectedShadowIronIngots()
			: base( 0x966 )
		{
		}

		public PileOfInspectedShadowIronIngots( Serial serial )
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

	public class PileOfInspectedCopperIngots : PileOfInspectedIngots
	{
		public override int LabelNumber { get { return 1113023; } } // Pile of Inspected Copper Ingots

		[Constructable]
		public PileOfInspectedCopperIngots()
			: base( 0x96D )
		{
		}

		public PileOfInspectedCopperIngots( Serial serial )
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

	public class PileOfInspectedBronzeIngots : PileOfInspectedIngots
	{
		public override int LabelNumber { get { return 1113024; } } // Pile of Inspected Bronze Ingots

		[Constructable]
		public PileOfInspectedBronzeIngots()
			: base( 0x972 )
		{
		}

		public PileOfInspectedBronzeIngots( Serial serial )
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

	public class PileOfInspectedGoldIngots : PileOfInspectedIngots
	{
		public override int LabelNumber { get { return 1113027; } } // Pile of Inspected Gold Ingots

		[Constructable]
		public PileOfInspectedGoldIngots()
			: base( 0x8A5 )
		{
		}

		public PileOfInspectedGoldIngots( Serial serial )
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

	public class PileOfInspectedAgapiteIngots : PileOfInspectedIngots
	{
		public override int LabelNumber { get { return 1113028; } } // Pile of Inspected Agapite Ingots

		[Constructable]
		public PileOfInspectedAgapiteIngots()
			: base( 0x979 )
		{
		}

		public PileOfInspectedAgapiteIngots( Serial serial )
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

	public class PileOfInspectedVeriteIngots : PileOfInspectedIngots
	{
		public override int LabelNumber { get { return 1113029; } } // Pile of Inspected Verite Ingots

		[Constructable]
		public PileOfInspectedVeriteIngots()
			: base( 0x89F )
		{
		}

		public PileOfInspectedVeriteIngots( Serial serial )
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

	public class PileOfInspectedValoriteIngots : PileOfInspectedIngots
	{
		public override int LabelNumber { get { return 1113030; } } // Pile of Inspected Valorite Ingots

		[Constructable]
		public PileOfInspectedValoriteIngots()
			: base( 0x8AB )
		{
		}

		public PileOfInspectedValoriteIngots( Serial serial )
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