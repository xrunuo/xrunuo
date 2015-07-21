using System;
using Server.Items;
using Server.Engines.Harvest;

namespace Server.Items
{
	public static class LogExtensions
	{
		public static void ConvertToBoards( Item log, Mobile from )
		{
			if ( !log.IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1054107 ); // This item must be in your backpack to be used.
			}
			else
			{
				double skillBase = from.Skills[Lumberjacking.System.Definition.Skill].Base;
				double reqSkill = 0.0;

				HarvestResource[] lumberResources = Lumberjacking.System.Definition.Resources;
				for ( int i = 0; i < lumberResources.Length; i++ )
				{
					for ( int j = 0; j < lumberResources[i].Types.Length; j++ )
					{
						if ( lumberResources[i].Types[j] == log.GetType() )
						{
							reqSkill = lumberResources[i].ReqSkill;
						}
					}
				}


				if ( skillBase >= reqSkill )
				{
					Item board;

					if ( log.GetType() == typeof( OakLog ) ) board = new OakBoard( log.Amount );
					else if ( log.GetType() == typeof( AshLog ) ) board = new AshBoard( log.Amount );
					else if ( log.GetType() == typeof( YewLog ) ) board = new YewBoard( log.Amount );
					else if ( log.GetType() == typeof( HeartwoodLog ) ) board = new HeartwoodBoard( log.Amount );
					else if ( log.GetType() == typeof( BloodwoodLog ) ) board = new BloodwoodBoard( log.Amount );
					else if ( log.GetType() == typeof( FrostwoodLog ) ) board = new FrostwoodBoard( log.Amount );
					else board = new Board( log.Amount );

					log.Delete();
					from.AddToBackpack( board );
				}
				else
				{
					from.SendLocalizedMessage( 1072652 ); // You cannot work this strange and unusual wood.
				}
			}
		}
	}

	[FlipableAttribute( 0x1bdd, 0x1be0 )]
	public class Log : Item, ICommodity, IChopable
	{
		public override int LabelNumber { get { return 1027138; } } // logs

		[Constructable]
		public Log()
			: this( 1 )
		{
		}

		[Constructable]
		public Log( int amount )
			: base( 0x1BDD )
		{
			Stackable = true;
			Weight = 2.0;
			Amount = amount;
		}

		public Log( Serial serial )
			: base( serial )
		{
		}

		public override LocalizedText GetNameProperty()
		{
			if ( Amount > 1 )
			{
				return new LocalizedText( 1050039, "{0}\t#{1}", Amount, 1027138 ); // ~1_NUMBER~ ~2_ITEMNAME~
			}
			else
			{
				return new LocalizedText( 1027138 ); // logs
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}

		public void OnChop( Mobile from )
		{
			LogExtensions.ConvertToBoards( this, from );
		}
	}

	[FlipableAttribute( 0x1bdd, 0x1be0 )]
	public class OakLog : Item, ICommodity, IChopable
	{
		public override int LabelNumber { get { return 1075063; } } // oak logs

		[Constructable]
		public OakLog()
			: this( 1 )
		{
		}

		[Constructable]
		public OakLog( int amount )
			: base( 0x1BDD )
		{
			Stackable = true;
			Weight = 2.0;
			Amount = amount;
			Hue = 2010;
		}

		public OakLog( Serial serial )
			: base( serial )
		{
		}

		public override LocalizedText GetNameProperty()
		{
			if ( Amount > 1 )
				return new LocalizedText( 1050039, "{0}\t#{1}", Amount, 1027138 ); // ~1_NUMBER~ ~2_ITEMNAME~
			else
				return new LocalizedText( 1027138 ); // logs
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1072533 );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( version >= 1 )
				reader.ReadInt();
		}

		public void OnChop( Mobile from )
		{
			LogExtensions.ConvertToBoards( this, from );
		}
	}

	[FlipableAttribute( 0x1bdd, 0x1be0 )]
	public class AshLog : Item, ICommodity, IChopable
	{
		public override int LabelNumber { get { return 1075064; } } // ash logs

		[Constructable]
		public AshLog()
			: this( 1 )
		{
		}

		[Constructable]
		public AshLog( int amount )
			: base( 0x1BDD )
		{
			Stackable = true;
			Weight = 2.0;
			Amount = amount;
			Hue = 1191;
		}

		public AshLog( Serial serial )
			: base( serial )
		{
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1072534 );
		}

		public override LocalizedText GetNameProperty()
		{
			if ( Amount > 1 )
				return new LocalizedText( 1050039, "{0}\t#{1}", Amount, 1027138 ); // ~1_NUMBER~ ~2_ITEMNAME~
			else
				return new LocalizedText( 1027138 ); // logs
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( version >= 1 )
				reader.ReadInt();
		}

		public void OnChop( Mobile from )
		{
			LogExtensions.ConvertToBoards( this, from );
		}
	}

	[FlipableAttribute( 0x1bdd, 0x1be0 )]
	public class YewLog : Item, ICommodity, IChopable
	{
		public override int LabelNumber { get { return 1075065; } } // yew logs

		[Constructable]
		public YewLog()
			: this( 1 )
		{
		}

		[Constructable]
		public YewLog( int amount )
			: base( 0x1BDD )
		{
			Stackable = true;
			Weight = 2.0;
			Amount = amount;
			Hue = 1192;
		}

		public YewLog( Serial serial )
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
				return new LocalizedText( 1050039, "{0}\t#{1}", Amount, 1027138 ); // ~1_NUMBER~ ~2_ITEMNAME~
			else
				return new LocalizedText( 1027138 ); // logs
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( version >= 1 )
				reader.ReadInt();
		}

		public void OnChop( Mobile from )
		{
			LogExtensions.ConvertToBoards( this, from );
		}
	}

	[FlipableAttribute( 0x1bdd, 0x1be0 )]
	public class HeartwoodLog : Item, ICommodity, IChopable
	{
		public override int LabelNumber { get { return 1075066; } } // heartwood logs

		[Constructable]
		public HeartwoodLog()
			: this( 1 )
		{
		}

		[Constructable]
		public HeartwoodLog( int amount )
			: base( 0x1BDD )
		{
			Stackable = true;
			Weight = 2.0;
			Amount = amount;
			Hue = 1193;
		}

		public HeartwoodLog( Serial serial )
			: base( serial )
		{
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1072536 );
		}

		public override LocalizedText GetNameProperty()
		{
			if ( Amount > 1 )
				return new LocalizedText( 1050039, "{0}\t#{1}", Amount, 1027138 ); // ~1_NUMBER~ ~2_ITEMNAME~
			else
				return new LocalizedText( 1027138 ); // logs
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( version >= 1 )
				reader.ReadInt();
		}

		public void OnChop( Mobile from )
		{
			LogExtensions.ConvertToBoards( this, from );
		}
	}

	[FlipableAttribute( 0x1bdd, 0x1be0 )]
	public class BloodwoodLog : Item, ICommodity, IChopable
	{
		public override int LabelNumber { get { return 1075067; } } // bloodwood logs

		[Constructable]
		public BloodwoodLog()
			: this( 1 )
		{
		}

		[Constructable]
		public BloodwoodLog( int amount )
			: base( 0x1BDD )
		{
			Stackable = true;
			Weight = 2.0;
			Amount = amount;
			Hue = 1194;
		}

		public BloodwoodLog( Serial serial )
			: base( serial )
		{
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1072538 );
		}

		public override LocalizedText GetNameProperty()
		{
			if ( Amount > 1 )
				return new LocalizedText( 1050039, "{0}\t#{1}", Amount, 1027138 ); // ~1_NUMBER~ ~2_ITEMNAME~
			else
				return new LocalizedText( 1027138 ); // logs
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( version >= 1 )
				reader.ReadInt();
		}

		public void OnChop( Mobile from )
		{
			LogExtensions.ConvertToBoards( this, from );
		}
	}

	[FlipableAttribute( 0x1bdd, 0x1be0 )]
	public class FrostwoodLog : Item, ICommodity, IChopable
	{
		public override int LabelNumber { get { return 1075068; } } // frostwood logs

		[Constructable]
		public FrostwoodLog()
			: this( 1 )
		{
		}

		[Constructable]
		public FrostwoodLog( int amount )
			: base( 0x1BDD )
		{
			Stackable = true;
			Weight = 2.0;
			Amount = amount;
			Hue = 1151;
		}

		public FrostwoodLog( Serial serial )
			: base( serial )
		{
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1072539 );
		}

		public override LocalizedText GetNameProperty()
		{
			if ( Amount > 1 )
				return new LocalizedText( 1050039, "{0}\t#{1}", Amount, 1027138 ); // ~1_NUMBER~ ~2_ITEMNAME~
			else
				return new LocalizedText( 1027138 ); // logs
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( version >= 1 )
				reader.ReadInt();
		}

		public void OnChop( Mobile from )
		{
			LogExtensions.ConvertToBoards( this, from );
		}
	}
}