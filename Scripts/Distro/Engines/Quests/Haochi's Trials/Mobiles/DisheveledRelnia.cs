using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.Quests;

namespace Server.Engines.Quests.SE
{
	public class DisheveledRelnia : BaseQuester
	{
		public override bool ClickTitle { get { return false; } }

		[Constructable]
		public DisheveledRelnia()
			: base( "the Gypsy" )
		{
		}

		public DisheveledRelnia( Serial serial )
			: base( serial )
		{
		}

		public override bool OnDragDrop( Mobile from, Item dropped )
		{
			PlayerMobile player = from as PlayerMobile;

			if ( player != null )
			{
				QuestSystem qs = player.Quest;

				if ( qs is HaochisTrialsQuest )
				{
					if ( dropped is Gold )
					{
						QuestObjective obj = qs.FindObjective( typeof( GiveGypsyGoldOrHuntCatsObjective ) );

						if ( obj != null )
						{
							obj.Complete();
						}

						HaochisTrialsQuest htq = qs as HaochisTrialsQuest;

						htq.Choice = ChoiceType.Gold;

						Say( 1063241 ); // I thank thee.  This gold will be a great help to me and mine!

						qs.AddObjective( new MadeChoiceObjective() );

						return false;
					}
				}
			}

			return base.OnDragDrop( from, dropped );
		}

		public override void InitBody()
		{
			InitStats( 100, 100, 25 );

			Hue = 0x83FF;

			Body = 0x191;

			Name = "Disheveled Relnia";
		}

		public override void InitOutfit()
		{
			AddItem( new ThighBoots( 0x901 ) );
			AddItem( new FancyShirt( 0x600 ) );
			AddItem( new SkullCap( 0x6A5 ) );
			AddItem( new Skirt( 0x51C ) );

			HairItemID = 0x203C;
			HairHue = 0x654;
		}

		public override bool NoContextMenu( PlayerMobile pm )
		{
			return true;
		}

		public override void OnTalk( PlayerMobile player, bool contextMenu )
		{
			return;
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
