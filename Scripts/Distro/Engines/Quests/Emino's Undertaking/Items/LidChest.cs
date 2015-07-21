using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests.SE
{
	public class LidChest : WoodenChest
	{
		[Constructable]
		public LidChest()
		{
			GenerateJewelry();
		}

		public LidChest( Serial serial )
			: base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			PlayerMobile pm = from as PlayerMobile;

			if ( pm == null )
			{
				return;
			}

			QuestSystem qs = pm.Quest;

			if ( qs is EminosUndertakingQuest )
			{
				if ( qs.IsObjectiveInProgress( typeof( WalkThroughHallwayObjective ) ) )
				{
					ClearContents();

					GenerateJewelry();

					QuestObjective obj = qs.FindObjective( typeof( WalkThroughHallwayObjective ) );

					if ( obj != null )
					{
						obj.Complete();
					}

					pm.AddToBackpack( new DaimyoEminosKatana() );

					qs.AddConversation( new OpenChestConversation() );

					qs.AddObjective( new TakeSwordObjective() );
				}
			}

			base.OnDoubleClick( from );
		}

		public void ClearContents()
		{
			for ( int i = Items.Count - 1; i >= 0; --i )
			{
				if ( i < Items.Count )
				{
					( (Item) Items[i] ).Delete();
				}
			}
		}

		public override bool OnDragDropInto( Mobile from, Item item, Point3D p, byte gridloc )
		{
			return false;
		}

		public override bool OnDragDrop( Mobile from, Item dropped )
		{
			return false;
		}

		public void GenerateJewelry()
		{
			for ( int i = 0; i < Utility.Random( 50, 60 ); i++ )
			{
				Item item = null;

				int chance = Utility.Random( 2 );

				switch ( chance )
				{
					case 0:
						item = Loot.RandomJewelry();
						break;
					case 1:
						item = Loot.RandomGem();
						break;
				}

				DropItem( item );
			}
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
