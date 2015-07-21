using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.Quests;

namespace Server.Engines.Quests.SE
{
	public class DaimyoHaochi : BaseQuester
	{
		public override bool ClickTitle { get { return false; } }

		[Constructable]
		public DaimyoHaochi()
			: base( "the Honorable Samurai Legend" )
		{
		}

		public DaimyoHaochi( Serial serial )
			: base( serial )
		{
		}

		public override void InitBody()
		{
			InitStats( 100, 100, 25 );

			Hue = 0x8403;

			Body = 0x190;

			Name = "Daimyo Haochi";
		}

		public override void InitOutfit()
		{
			AddItem( new SamuraiTabi() );
			AddItem( new PlateHaidate() );
			AddItem( new StandardPlateKabuto() );
			AddItem( new PlateDo() );
			AddItem( new PlateHiroSode() );
			AddItem( new JinBaori() );

			HairItemID = 0x204A;
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

			if ( qs is HaochisTrialsQuest )
			{
				HaochisTrialsQuest htq = qs as HaochisTrialsQuest;

				if ( qs.IsObjectiveInProgress( typeof( SpeakToDaimyoHaochiObjective ) ) )
				{
					QuestObjective obj = qs.FindObjective( typeof( SpeakToDaimyoHaochiObjective ) );

					if ( obj != null )
					{
						obj.Complete();
					}

					qs.AddConversation( new DaimyoHaochiBeginConversation() );

					qs.AddObjective( new FollowGreenPathObjective() );
				}

				if ( qs.IsObjectiveInProgress( typeof( KillRoninsOrSoulsObjective ) ) )
				{
					bool ronins = false;

					if ( htq.KilledRonins > htq.KilledSouls )
					{
						ronins = true;
					}

					if ( ronins )
					{
						qs.AddConversation( new ContinueSlayingRoninsConversation() );
					}
					else if ( htq.KilledSouls > 0 )
					{
						qs.AddConversation( new ContinueSlayingSoulsConversation() );
					}
				}

				if ( qs.IsObjectiveInProgress( typeof( FirstTrialCompleteObjective ) ) )
				{
					QuestObjective obj = qs.FindObjective( typeof( FirstTrialCompleteObjective ) );

					if ( obj != null )
					{
						obj.Complete();
					}

					if ( htq.KilledRonins == 3 )
					{
						qs.AddConversation( new ThanksForRoninsConversation() );
					}

					if ( htq.KilledSouls == 3 )
					{
						qs.AddConversation( new ThanksForSoulsConversation() );
					}

					player.AddToBackpack( new LeatherDo() );

					qs.AddObjective( new FollowYellowPathObjective() );
				}

				if ( qs.IsObjectiveInProgress( typeof( SecondTrialCompleteObjective ) ) )
				{
					QuestObjective obj = qs.FindObjective( typeof( SecondTrialCompleteObjective ) );

					if ( obj != null )
					{
						obj.Complete();
					}

					if ( htq.Opponent == OpponentType.FierceDragon )
					{
						qs.AddConversation( new DragonConversation() );
					}

					if ( htq.Opponent == OpponentType.DeadlyImp )
					{
						qs.AddConversation( new ImpConversation() );
					}

					player.AddToBackpack( new LeatherSuneate() );

					qs.AddObjective( new FollowBluePathObjective() );
				}

				if ( qs.IsObjectiveInProgress( typeof( ThirdTrialCompleteObjective ) ) )
				{
					QuestObjective obj = qs.FindObjective( typeof( ThirdTrialCompleteObjective ) );

					if ( obj != null )
					{
						obj.Complete();
					}

					qs.AddConversation( new HaochiSmilesConversation() );

					player.AddToBackpack( new LeatherHiroSode() );

					qs.AddObjective( new FollowRedPathObjective() );
				}

				if ( qs.IsObjectiveInProgress( typeof( MadeChoiceObjective ) ) )
				{
					QuestObjective obj = qs.FindObjective( typeof( MadeChoiceObjective ) );

					if ( obj != null )
					{
						obj.Complete();
					}

					if ( htq.Choice == ChoiceType.Gold )
					{
						qs.AddConversation( new RespectForGoldConversation() );
					}

					if ( htq.Choice == ChoiceType.Cats )
					{
						qs.AddConversation( new RespectForCatsConversation() );
					}

					Bag bag = new Bag();

					bag.Hue = 0x660;

					bag.DropItem( new LeatherHiroSode() );
					bag.DropItem( new JinBaori() );

					player.AddToBackpack( bag );

					qs.AddObjective( new RetrieveKatanaObjective() );
				}

				if ( qs.IsObjectiveInProgress( typeof( GiveSwordDaimyoObjective ) ) )
				{
					List<Item> list = player.Backpack.Items;

					DaimyoHaochisKatana katana = null;

					for ( int i = 0; i < list.Count; i++ )
					{
						if ( list[i] is DaimyoHaochisKatana )
						{
							katana = list[i] as DaimyoHaochisKatana;

							break;
						}
					}

					if ( katana == null )
					{
						qs.AddConversation( new WithoutSwordConversation() );
					}
					else
					{
						katana.Delete();

						QuestObjective obj = qs.FindObjective( typeof( GiveSwordDaimyoObjective ) );

						if ( obj != null )
						{
							obj.Complete();
						}

						qs.AddConversation( new ThanksForSwordConversation() );

						qs.AddObjective( new LightCandleObjective() );
					}
				}

				if ( qs.IsObjectiveInProgress( typeof( CandleCompleteObjective ) ) )
				{
					QuestObjective obj = qs.FindObjective( typeof( CandleCompleteObjective ) );

					if ( obj != null )
					{
						obj.Complete();
					}

					qs.AddConversation( new WellDoneConversation() );

					qs.AddObjective( new KillNinjaObjective() );
				}

				if ( qs.IsObjectiveInProgress( typeof( ExecutionsCompleteObjective ) ) )
				{
					QuestObjective obj = qs.FindObjective( typeof( ExecutionsCompleteObjective ) );

					if ( obj != null )
					{
						obj.Complete();
					}

					qs.AddConversation( new FirewellConversation() );

					BaseWeapon weapon = new Daisho();

					BaseRunicTool.ApplyAttributesTo( weapon, Utility.Random( 1, 3 ), 10, 30 );

					player.AddToBackpack( weapon );

					BaseArmor armor = new LeatherDo();

					BaseRunicTool.ApplyAttributesTo( armor, Utility.Random( 1, 3 ), 10, 20 );

					player.AddToBackpack( armor );

					qs.Complete();
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
