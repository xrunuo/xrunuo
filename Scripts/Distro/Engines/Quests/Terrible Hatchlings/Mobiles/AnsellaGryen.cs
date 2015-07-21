using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.Quests;

namespace Server.Engines.Quests.SE
{
	public class AnsellaGryen : BaseQuester
	{
		public override bool ClickTitle { get { return true; } }

		[Constructable]
		public AnsellaGryen()
			: base( "" )
		{
		}

		public override void InitBody()
		{
			InitStats( 100, 100, 25 );

			Hue = 0x83EA;

			Female = true;
			Body = 0x191;
			Name = "Ansella Gryen";
		}

		public override void InitOutfit()
		{
			AddItem( new FemaleKimono( 0x4BF ) );
			AddItem( new SamuraiTabi( 0x8FD ) );
			AddItem( new Obi( 0x52D ) );
			AddItem( new GoldBracelet() );

			HairItemID = 0x203B;
			HairHue = 0x1BB;
		}

		public override int GetAutoTalkRange( PlayerMobile pm )
		{
			QuestSystem qs = pm.Quest;

			if ( qs == null )
			{
				return 3;
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

			if ( qs is TerribleHatchlingsQuest )
			{
				if ( qs.IsObjectiveInProgress( typeof( ReturnToAnsellaGryenObjective ) ) )
				{
					Container cont = GetNewContainer();

					cont.DropItem( new Gold( Utility.RandomMinMax( 100, 200 ) ) );

					switch ( Utility.Random( 1 ) )
					{
						case 0:
							{
								BaseWeapon weapon = Loot.RandomWeapon( true );

								BaseRunicTool.ApplyAttributesTo( weapon, 3, 10, 30 );

								cont.DropItem( weapon );

								break;
							}

						case 1:
							{
								BaseArmor armor = Loot.RandomArmor( true );

								BaseRunicTool.ApplyAttributesTo( armor, 1, 10, 20 );

								cont.DropItem( armor );

								break;
							}
					}

					player.AddToBackpack( cont );

					qs.AddConversation( new RewardsConversation() );
				}
				else
				{
					qs.AddConversation( new DeathwatchBeetlesLocationConversation() );
				}
			}
			else
			{
				QuestSystem newQuest = new TerribleHatchlingsQuest( player );
				bool inRestartPeriod = false;

				if ( qs != null )
				{
					SayTo( player, 1063322 ); // Before you can help me with the Terrible Hatchlings, you'll need to finish the quest you've already taken!
				}
				else if ( QuestSystem.CanOfferQuest( player, typeof( TerribleHatchlingsQuest ), out inRestartPeriod ) )
				{
					PlaySound( 0x2A3 );

					newQuest.SendOffer();
				}
				else if ( inRestartPeriod && contextMenu )
				{
					SayTo( player, 1049357 ); // I have nothing more for you at this time.
				}
			}
		}

		public AnsellaGryen( Serial serial )
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