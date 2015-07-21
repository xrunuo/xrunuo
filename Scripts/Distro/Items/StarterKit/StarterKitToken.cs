using System;
using Server;
using Server.Accounting;

namespace Server.Items
{
	public class StarterKitToken : Item
	{
		private Mobile m_Owner;

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Owner { get { return m_Owner; } set { m_Owner = value; } }

		[Constructable]
		public StarterKitToken()
			: base( 0x14F0 )
		{
			Name = "New Player Starter Kit Token";
			Label1 = "Double click me to claim your gift!";
			Hue = 1161;
			Weight = 1.0;
			LootType = LootType.Blessed;
		}

		public StarterKitToken( Serial serial )
			: base( serial )
		{
		}

		public override LocalizedText GetNameProperty()
		{
			return new LocalizedText( 1116260, m_Owner != null ? m_Owner.Name : "no one" ); // New Player Starter Kit Token for ~1_val~.
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !this.IsChildOf( from.Backpack ) )
			{
				// This item must be in your backpack to be used.
				from.SendLocalizedMessage( 1062334 );
				return;
			}

			if ( from != Owner )
			{
				from.SendLocalizedMessage( 1116257 ); // This token does not belong to this character.
				return;
			}

			var acct = from.Account as Account;
			if ( acct.Trial )
			{
				from.SendLocalizedMessage( 1116258 ); // Trial account cannot use this token.
				return;
			}

			var totalTime = ( DateTime.Now - acct.Created );
			if ( totalTime >= TimeSpan.FromDays( 30.0 ) )
			{
				from.SendLocalizedMessage( 1116259 ); // This can only be used by accounts less than 1 month old.
				return;
			}

			BankBox bankbox = from.BankBox;

			WoodenBox box = new WoodenBox();
			box.Name = "A Starter Kit";
			box.Hue = 66;
			box.ItemID = 3709;

			if ( from.Race == Race.Gargoyle )
			{
				box.DropItem( new GargishInitiationEarrings() );
				box.DropItem( new GargishInitiationKilt() );
				box.DropItem( new GargishInitiationNecklace() );
				box.DropItem( new GargishInitiationLeggings() );
				box.DropItem( new GargishInitiationArms() );
				box.DropItem( new GargishInitiationChest() );
			}
			else
			{
				box.DropItem( new InitiationCap() );
				box.DropItem( new InitiationGloves() );
				box.DropItem( new InitiationGorget() );
				box.DropItem( new InitiationLeggings() );
				box.DropItem( new InitiationSleeves() );
				box.DropItem( new InitiationTunic() );
			}

			box.DropItem( CreateStarterRunebook() );
			box.DropItem( new Gold( 20001 ) );

			if ( bankbox.TryDropItem( from, box, false ) )
			{
				this.Delete();
				from.SendLocalizedMessage( 1042672, true, " 20001" );
				from.SendLocalizedMessage( 1116286 ); // Your new player starter kit has been placed in your bank box.
			}
		}

		private static Item CreateStarterRunebook()
		{
			Runebook book = new Runebook();
			book.CurCharges = book.MaxCharges = 20;

			book.AddEntry( CreateRunebookEntry( "New Haven Bank", new Point3D( 3487, 2572, 21 ), Map.Trammel ) );
			book.AddEntry( CreateRunebookEntry( "Britain Bank", new Point3D( 1428, 1693, 0 ), Map.Trammel ) );
			book.AddEntry( CreateRunebookEntry( "Luna Bank", new Point3D( 983, 515, -50 ), Map.Malas ) );
			book.AddEntry( CreateRunebookEntry( "Umbra Bank", new Point3D( 2045, 1342, -85 ), Map.Malas ) );
			book.AddEntry( CreateRunebookEntry( "Change Tokuno Arties", new Point3D( 678, 1275, 48 ), Map.Tokuno ) );
			book.AddEntry( CreateRunebookEntry( "Taming: Trinsic Area", new Point3D( 2103, 2794, 5 ), Map.Trammel ) );
			book.AddEntry( CreateRunebookEntry( "Taming: Bulls Delucia", new Point3D( 5208, 3952, 37 ), Map.Trammel ) );
			book.AddEntry( CreateRunebookEntry( "Minoc Mines", new Point3D( 2558, 503, 0 ), Map.Trammel ) );
			book.AddEntry( CreateRunebookEntry( "Easy: Old Haven", new Point3D( 3637, 2499, 0 ), Map.Trammel ) );
			book.AddEntry( CreateRunebookEntry( "Easy: New Haven Mine", new Point3D( 3511, 2778, 27 ), Map.Trammel ) );
			book.AddEntry( CreateRunebookEntry( "Medium: Red Solen Hive", new Point3D( 2611, 762, 0 ), Map.Trammel ) );
			book.AddEntry( CreateRunebookEntry( "Medium: Black Solen Hive", new Point3D( 1458, 988, 0 ), Map.Trammel ) );
			book.AddEntry( CreateRunebookEntry( "Medium: Covetous", new Point3D( 2499, 918, 0 ), Map.Trammel ) );
			book.AddEntry( CreateRunebookEntry( "Medium: Britain Graveyard", new Point3D( 1385, 1496, 10 ), Map.Trammel ) );
			book.AddEntry( CreateRunebookEntry( "Hard: Hythloth", new Point3D( 4722, 3823, 0 ), Map.Trammel ) );
			book.AddEntry( CreateRunebookEntry( "Stygian Abyss: Underworld", new Point3D( 4194, 3268, 0 ), Map.Trammel ) );

			return book;
		}

		private static RunebookEntry CreateRunebookEntry( string name, Point3D loc, Map map )
		{
			return new RunebookEntry( loc, map, name, null, RecallRune.CalculateHue( map ) );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

			writer.Write( (Mobile) m_Owner );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 1:
					{
						m_Owner = reader.ReadMobile();
						goto case 0;
					}
				case 0:
					{
						break;
					}
			}

			if ( string.IsNullOrEmpty( Label1 ) )
				Label1 = "Double click me for claiming your gift!";
		}
	}
}
