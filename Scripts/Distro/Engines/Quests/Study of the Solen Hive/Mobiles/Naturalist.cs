using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.Plants;
using Server.Engines.Quests;

namespace Server.Engines.Quests.Naturalist
{
	public class Naturalist : BaseQuester
	{
		[Constructable]
		public Naturalist()
			: base( "the Naturalist" )
		{
		}

		public override void InitBody()
		{
			InitStats( 100, 100, 25 );

			Hue = Utility.RandomSkinHue();

			Female = false;
			Body = 0x190;
			Name = NameList.RandomName( "male" );
		}

		public override void InitOutfit()
		{
			AddItem( new Tunic( 0x598 ) );
			AddItem( new LongPants( 0x59B ) );
			AddItem( new Boots() );

			Utility.AssignRandomHair( this );
			Utility.AssignRandomFacialHair( this, HairHue );
		}

		public override void OnTalk( PlayerMobile player, bool contextMenu )
		{
			StudyOfSolenQuest qs = player.Quest as StudyOfSolenQuest;

			if ( qs != null && qs.Naturalist == this )
			{
				StudyNestsObjective study = qs.FindObjective( typeof( StudyNestsObjective ) ) as StudyNestsObjective;

				if ( study != null )
				{
					if ( !study.Completed )
					{
						PlaySound( 0x41F );
						qs.AddConversation( new NaturalistDuringStudyConversation() );
					}
					else
					{
						QuestObjective obj = qs.FindObjective( typeof( ReturnToNaturalistObjective ) );

						if ( obj != null && !obj.Completed )
						{
							Seed reward;

							PlantType type;
							switch ( Utility.Random( study.StudiedSpecialNest ? 34 : 16 ) )
							{
								case 0:
									type = PlantType.CampionFlowers;
									break;
								case 1:
									type = PlantType.Poppies;
									break;
								case 2:
									type = PlantType.Snowdrops;
									break;
								case 3:
									type = PlantType.Bulrushes;
									break;
								case 4:
									type = PlantType.Lilies;
									break;
								case 5:
									type = PlantType.PampasGrass;
									break;
								case 6:
									type = PlantType.Rushes;
									break;
								case 7:
									type = PlantType.ElephantEarPlant;
									break;
								case 8:
									type = PlantType.Fern;
									break;
								case 9:
									type = PlantType.PonytailPalm;
									break;
								case 10:
									type = PlantType.SmallPalm;
									break;
								case 11:
									type = PlantType.CenturyPlant;
									break;
								case 12:
									type = PlantType.WaterPlant;
									break;
								case 13:
									type = PlantType.SnakePlant;
									break;
								case 14:
									type = PlantType.PricklyPearCactus;
									break;
								case 15:
									type = PlantType.BarrelCactus;
									break;
								case 16:
									type = PlantType.TribarrelCactus;
									break;
								case 17:
									type = PlantType.Cactus;
									break;
								case 18:
									type = PlantType.FlaxFlowers;
									break;
								case 19:
									type = PlantType.FoxgloveFlowers;
									break;
								case 20:
									type = PlantType.HopsEast;
									break;
								case 21:
									type = PlantType.OrfluerFlowers;
									break;
								case 22:
									type = PlantType.CypressTwisted;
									break;
								case 23:
									type = PlantType.HedgeShort;
									break;
								case 24:
									type = PlantType.JuniperBush;
									break;
								case 25:
									type = PlantType.SnowdropPatch;
									break;
								case 26:
									type = PlantType.Cattails;
									break;
								case 27:
									type = PlantType.PoppyPatch;
									break;
								case 28:
									type = PlantType.SpiderTree;
									break;
								case 29:
									type = PlantType.WaterLily;
									break;
								case 30:
									type = PlantType.CypressStraight;
									break;
								case 31:
									type = PlantType.HedgeTall;
									break;
								case 32:
									type = PlantType.HopsSouth;
									break;
								default:
									type = PlantType.SugarCanes;
									break;
							}

							if ( study.StudiedSpecialNest )
							{
								PlantTypeInfo typeInfo = PlantTypeInfo.GetInfo( type );

								if ( typeInfo.PlantCategory == PlantCategory.Peculiar && type != PlantType.Cactus )
									reward = new Seed( type, Utility.RandomBool() ? PlantHue.White : PlantHue.Black, false );
								else
									reward = new Seed( type, PlantHue.FireRed, false );
							}
							else
							{
								reward = new Seed( type, Utility.RandomList( PlantHue.Pink, PlantHue.Magenta, PlantHue.Aqua ), false );
							}

							if ( player.PlaceInBackpack( reward ) )
							{
								obj.Complete();

								PlaySound( 0x449 );
								PlaySound( 0x41B );

								if ( study.StudiedSpecialNest )
									qs.AddConversation( new SpecialEndConversation() );
								else
									qs.AddConversation( new EndConversation() );
							}
							else
							{
								reward.Delete();

								qs.AddConversation( new FullBackpackConversation() );
							}
						}
					}
				}
			}
			else
			{
				QuestSystem newQuest = new StudyOfSolenQuest( player, this );

				if ( player.Quest == null && QuestSystem.CanOfferQuest( player, typeof( StudyOfSolenQuest ) ) )
				{
					PlaySound( 0x42F );
					newQuest.SendOffer();
				}
				else
				{
					PlaySound( 0x448 );
					newQuest.AddConversation( new DontOfferConversation() );
				}
			}
		}

		public Naturalist( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}
}
