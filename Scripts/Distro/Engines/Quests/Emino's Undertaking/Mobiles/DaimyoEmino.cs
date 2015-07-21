using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.Quests;

namespace Server.Engines.Quests.SE
{
	public class DaimyoEmino : BaseQuester
	{
		public override bool ClickTitle { get { return false; } }

		[Constructable]
		public DaimyoEmino()
			: base( "the Notorious" )
		{
		}

		public DaimyoEmino( Serial serial )
			: base( serial )
		{
		}

		public override void InitBody()
		{
			InitStats( 100, 100, 25 );

			Hue = 0x83FE;

			Body = 0x190;

			Name = "Daimyo Emino";
		}

		public override void InitOutfit()
		{
			AddItem( new Nunchaku() );
			AddItem( new SamuraiTabi() );
			AddItem( new PlateHaidate() );
			AddItem( new Bandana( 0x1 ) );
			AddItem( new PlateDo() );
			AddItem( new PlateHiroSode() );
			AddItem( new MaleKimono() );

			HairItemID = 0x203B;
			HairHue = 0x901;
		}

		public override bool NoContextMenu( PlayerMobile pm )
		{
			return true;
		}

		public override int GetAutoTalkRange( PlayerMobile pm )
		{
			QuestSystem qs = pm.Quest;

			if ( qs != null )
			{
				return 2;
			}
			else
			{
				return -1;
			}
		}

		public override void OnTalk( PlayerMobile player, bool contextMenu )
		{
			Direction = this.GetDirectionTo( player );

			QuestSystem qs = player.Quest;

			if ( qs is EminosUndertakingQuest )
			{
				if ( qs.IsObjectiveInProgress( typeof( FindDaimyoEminoObjective ) ) )
				{
					QuestObjective obj = qs.FindObjective( typeof( FindDaimyoEminoObjective ) );

					if ( obj != null )
					{
						obj.Complete();
					}

					qs.AddConversation( new DaimyoEminoBeginConversation() );

					qs.AddObjective( new FindEliteNinjaZoelObjective() );
				}

				if ( qs.IsObjectiveInProgress( typeof( TakeGreenTeleporterObjective ) ) )
				{
					QuestObjective obj = qs.FindObjective( typeof( TakeGreenTeleporterObjective ) );

					if ( obj != null )
					{
						obj.Complete();
					}

					qs.AddConversation( new EminoSecondConversation() );

					player.AddToBackpack( new NoteForZoel() );

					player.AddToBackpack( new LeatherNinjaPants() );

					player.AddToBackpack( new LeatherNinjaMitts() );

					qs.AddObjective( new BringNoteToZoelObjective() );
				}

				if ( qs.IsObjectiveInProgress( typeof( GoBackBlueTeleporterObjective ) ) )
				{
					QuestObjective obj = qs.FindObjective( typeof( GoBackBlueTeleporterObjective ) );

					if ( obj != null )
					{
						obj.Complete();
					}

					qs.AddConversation( new FrownsConversation() );

					Bag bag = new Bag();

					bag.Hue = 0x660;

					bag.DropItem( new LeatherNinjaJacket() );
					bag.DropItem( new LeatherNinjaHood() );

					for ( int i = 0; i < 10; i++ )
					{
						bag.DropItem( new LesserHealPotion() );
					}

					player.AddToBackpack( bag );

					qs.AddObjective( new TakeWhiteTeleporterObjective() );
				}

				if ( qs.IsObjectiveInProgress( typeof( KillHenchmensObjective ) ) )
				{
					qs.AddConversation( new ContinueKillHenchmensConversation() );
				}

				if ( qs.IsObjectiveInProgress( typeof( ReturnToDaimyoObjective ) ) )
				{
					List<Item> list = player.Backpack.Items;

					DaimyoEminosKatana katana = null;

					for ( int i = 0; i < list.Count; i++ )
					{
						if ( list[i] is DaimyoEminosKatana )
						{
							katana = list[i] as DaimyoEminosKatana;

							break;
						}
					}

					if ( katana == null )
					{
						qs.AddConversation( new TakeSwordAgainConversation() );
					}
					else
					{
						katana.Delete();

						QuestObjective obj = qs.FindObjective( typeof( ReturnToDaimyoObjective ) );

						if ( obj != null )
						{
							obj.Complete();
						}

						qs.AddConversation( new GiftsConversation() );

						Kama kama = new Kama();

						BaseRunicTool.ApplyAttributesTo( kama, 1, 10, 30 );

						player.AddToBackpack( kama );

						qs.Complete();
					}
				}
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
