using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
	public class AllThatGlittersQuest : TieredQuest
	{
		public override QuestTier Tier { get { return QuestTier.ThepemTier1; } }

		/* All That Glitters */
		public override object Title { get { return 1112775; } }

		/* Ah, yes, welcome to our humble shop. Do you wish to buy some of our fine potions today,
		 * or perhaps have something of interest to sell?
		 * 
		 * No? Well, I do have some specialty goods for sale that may be of interest to you.
		 * Unfortunately, specialty goods require specialty ingredients, which can be harder to
		 * come by. I'm not the adventurous sort, so if you are interested, you'll have to bring
		 * them to me.
		 * 
		 * Mistress Zosilem has recently discovered an efficient method of converting lesser metals
		 * in those that are more valuable. The elixirs that convert the more valuable metals are
		 * for our long term customers. That said, perhaps you are interested in purchasing a elixir
		 * that can turn up to 500 dull copper ingots into gold ones? I will need some specialty
		 * ingredients in addition to what we have in the shop. Of course, nothing one such as
		 * yourself cannot obtain with a small bit of effort.<br><br>Bring me five portions of
		 * congealed slug acid, and twenty gold ingots. I will need to inspect the ingots before I
		 * accept them, so give those to me before we complete the transaction. */
		public override object Description { get { return 1112948; } }

		/* Ah, perhaps another time then. */
		public override object Refuse { get { return 1112949; } }

		/* I will need twenty gold ingots and some congealed slug acid, which can be found on...
		 * can you guess? Yes, that's right. Acid slugs. */
		public override object Uncomplete { get { return 1112950; } }

		/* Hello, how may I help you? Oh, wait, you were interested in the elixir of gold conversion,
		 * right? If you have the materials I asked for ready, hand them over and I'll get to work
		 * on your elixir right away. After that, I have other tasks to finish for the mistress, but
		 * you can return in a bit if you wish to make another purchase. */
		public override object Complete { get { return 1112951; } }

		public AllThatGlittersQuest()
		{
			AddObjective( new ObtainObjective( typeof( CongealedSlugAcid ), "congealed slug acid", 5 ) );
			AddObjective( new ObtainObjective( typeof( PileOfInspectedGoldIngots ), "pile of inspected gold ingots", 1 ) );

			AddReward( new BaseReward( typeof( ElixirOfGoldConversion ), 1113007 ) ); // Elixir of Gold Conversion
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

	public class TastyTreatsQuest : TieredQuest
	{
		public override QuestTier Tier { get { return QuestTier.ThepemTier1; } }

		/* Tasty Treats */
		public override object Title { get { return 1112774; } }

		/* Ah, yes, welcome to our humble shop. Do you wish to buy some of our fine potions today,
		 * or perhaps have something of interest to sell?
		 * 
		 * No? Well, I do have some specialty goods for sale that may be of interest to you.
		 * Unfortunately, specialty goods require specialty ingredients, which can be harder to come
		 * by. I'm not the adventurous sort, so if you are interested, you'll have to bring them to me.
		 * 
		 * Pets can be finicky eaters at times, but I have just the solution for that. I call them
		 * 'Tasty Treats', and they're sure to please your pet. In fact, Fluffy will be so happy after
		 * eating one of these that you'll find that Fluffy's abilities are noticeably improved! Are
		 * you interested in some Tasty Treats? */
		public override object Description { get { return 1112944; } }

		/* Ah, perhaps another time then. */
		public override object Refuse { get { return 1112945; } }

		/* You will need to bring me five boura skins and a bit of dough. You can find the boura all
		 * over Ter Mur, though I have heard that the tougher variety have skin that is more likely
		 * to stay intact during its slaughter. */
		public override object Uncomplete { get { return 1112946; } }

		/* Welcome back. Did you bring the ingredients I asked for? Ah, good. Depending on the quality
		 * of the boura skins, I usually do not need all five to produce five tasty treats. You can
		 * consider what is left over as payment for my services. The rest, I shall use... for other
		 * purposes. I have other tasks to finish right now for the master, but you can return in a
		 * bit if you wish to purchase more. */
		public override object Complete { get { return 1112947; } }

		public TastyTreatsQuest()
		{
			AddObjective( new ObtainObjective( typeof( BouraSkin ), "boura skins", 5 ) );
			AddObjective( new ObtainObjective( typeof( Dough ), "dough", 1 ) );

			AddReward( new BaseReward( typeof( TastyTreat ), 1113003 ) ); // Tasty Treat
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

	public class PinkIsTheNewBlackQuest : TieredQuest
	{
		public override QuestTier Tier { get { return QuestTier.ThepemTier2; } }

		/* Pink is the New Black */
		public override object Title { get { return 1112777; } }

		/* It is good to see you. As one of our valued customers, Mistress Zosilem has given me
		 * permission to offer you another special elixir, one able to convert the more common
		 * shadow iron into valuable agapite. I'll need twenty agapite ingots and some seared
		 * fire ant goo for the mixture. Are you interested?
		 * 
		 * I will need to inspect the ingots before I accept them, so give those to me before
		 * we complete the transaction. */
		public override object Description { get { return 1112956; } }

		/* As always, feel free to return to our shop when you find yourself in need. Farewell. */
		public override object Refuse { get { return 1112957; } }

		/* I will need twenty agapite ingots and some seared fire ant goo which, unsurprisingly,
		 * can be found on fire ants. */
		public override object Uncomplete { get { return 1112958; } }

		/* Good to see you again, have you come to bring me the ingredients for the elixir of
		 * agapite conversion? Good, I'll take those in return for this elixir I made earlier.
		 * I'll be busy the rest of the day, but come back tomorrow if you need more. */
		public override object Complete { get { return 1112959; } }

		public PinkIsTheNewBlackQuest()
		{
			AddObjective( new ObtainObjective( typeof( SearedFireAntGoo ), "seared fire ant goo", 5 ) );
			AddObjective( new ObtainObjective( typeof( PileOfInspectedAgapiteIngots ), "pile of inspected agapite ingots", 1 ) );

			AddReward( new BaseReward( typeof( ElixirOfAgapiteConversion ), 1113008 ) ); // Elixir of Agapite Conversion
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

	public class MetalHeadQuest : TieredQuest
	{
		public override QuestTier Tier { get { return QuestTier.ThepemTier2; } }

		/* Metal Head */
		public override object Title { get { return 1112776; } }

		/* Welcome back to our shop. As one of our valued customers, I assume that you are here
		 * to make a specialty purchase? Mistress Zosilem has authorized me to make available to
		 * you a very special elixir that is able to convert common iron into something a bit
		 * more valuable.
		 * 
		 * That we can make this available at all is due to some very cutting edge research that
		 * the mistress has been doing. In fact, the results are a bit unpredictable, but
		 * guaranteed to be worth your time. If you are interested, I'll need you to bring me
		 * twenty each of the lesser four colored ingots, as well as ten undamaged iron beetle
		 * scales. Does that sound good to you?
		 * 
		 * I will need to inspect the ingots before I accept them, so give those to me before
		 * we complete the transaction. */
		public override object Description { get { return 1112952; } }

		/* As you wish. Of course, stop by at any time if you find yourself in need of alchemy
		 * supplies, or for something a bit more rare. */
		public override object Refuse { get { return 1112953; } }

		/* I'll need you to bring me twenty each of the lesser four colored ingots, dull copper,
		 * shadow iron, copper and bronze, as well as ten undamaged iron beetle scales. */
		public override object Uncomplete { get { return 1112954; } }

		/* I see that you have returned. Did you still want the elixir of metal conversion? Yes?
		 * Good, just hand over the ingredients I asked for, and I'll mix this up for you immediately.
		 * I'll be busy for a couple hours, but return after that if you wish to purchase more. */
		public override object Complete { get { return 1112955; } }

		public MetalHeadQuest()
		{
			AddObjective( new ObtainObjective( typeof( UndamagedIronBeetleScale ), "undamaged iron beetle scales", 10 ) );
			AddObjective( new ObtainObjective( typeof( PileOfInspectedDullCopperIngots ), "pile of inspected dull copper ingots", 1 ) );
			AddObjective( new ObtainObjective( typeof( PileOfInspectedShadowIronIngots ), "pile of inspected shadow iron ingots", 1 ) );
			AddObjective( new ObtainObjective( typeof( PileOfInspectedCopperIngots ), "pile of inspected copper ingots", 1 ) );
			AddObjective( new ObtainObjective( typeof( PileOfInspectedBronzeIngots ), "pile of inspected bronze ingots", 1 ) );

			AddReward( new BaseReward( typeof( ElixirOfMetalConversion ), 1113011 ) ); // Elixir of Metal Conversion
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

	public class Thepem : MondainQuester
	{
		private ArrayList m_SBInfos = new ArrayList();
		protected override ArrayList SBInfos { get { return m_SBInfos; } }

		public override bool IsActiveVendor { get { return true; } }

		public override void InitSBInfo()
		{
			SBInfos.Add( new SBAlchemist() );
		}

		private static Type[] m_Quests = new Type[]
			{
				typeof( AllThatGlittersQuest ),
				typeof( TastyTreatsQuest ),
				typeof( PinkIsTheNewBlackQuest ),
				typeof( MetalHeadQuest )
			};

		public override Type[] Quests { get { return m_Quests; } }

		public override void Advertise()
		{
		}

		[Constructable]
		public Thepem()
			: base( "Thepem", "the Apprentice" )
		{
			SetSkill( SkillName.Alchemy, 60.0, 83.0 );
			SetSkill( SkillName.TasteID, 60.0, 83.0 );
		}

		public Thepem( Serial serial )
			: base( serial )
		{
		}

		public override void InitBody()
		{
			InitStats( 100, 100, 25 );

			Female = true;
			Race = Race.Gargoyle;

			Hue = 0x86E1;
			HairItemID = 0x42AB;
			HairHue = 0x385;
		}

		public override void InitOutfit()
		{
			AddItem( new FemaleGargishClothLeggings( 0x70F ) );
			AddItem( new FemaleGargishClothKilt( 0x742 ) );
			AddItem( new FemaleGargishClothChest( 0x4C3 ) );
			AddItem( new FemaleGargishClothArms( 0x738 ) );
		}

		private static Type[][] m_PileTypes = new Type[][]
			{
				new Type[] {typeof( DullCopperIngot ),	typeof( PileOfInspectedDullCopperIngots )},
				new Type[] {typeof( ShadowIronIngot ),	typeof( PileOfInspectedShadowIronIngots )},
                new Type[] {typeof( CopperIngot ),		typeof( PileOfInspectedCopperIngots )},
				new Type[] {typeof( BronzeIngot ),		typeof( PileOfInspectedBronzeIngots )},
				new Type[] {typeof( GoldIngot ),		typeof( PileOfInspectedGoldIngots )},
				new Type[] {typeof( AgapiteIngot ),		typeof( PileOfInspectedAgapiteIngots )}
			};

		private const int NeededIngots = 20;

		private Type GetPileType( Item item )
		{
			Type itemType = item.GetType();

			for ( int i = 0; i < m_PileTypes.Length; i++ )
			{
				Type[] pair = m_PileTypes[i];

				if ( itemType == pair[0] )
					return pair[1];
			}

			return null;
		}

		public override bool OnDragDrop( Mobile from, Item item )
		{
			Type pileType = GetPileType( item );

			if ( pileType != null )
			{
				if ( item.Amount > NeededIngots )
				{
					SayTo( from, 1113037 ); // That's too many.
					return false;
				}
				else if ( item.Amount < NeededIngots )
				{
					SayTo( from, 1113036 ); // That's not enough.
					return false;
				}
				else
				{
					SayTo( from, 1113040 ); // Good. I can use this.

					from.AddToBackpack( Activator.CreateInstance( pileType ) as Item );
					from.SendLocalizedMessage( 1113041 ); // Now mark the inspected item as a quest item to turn it in.					

					return true;
				}
			}
			else
			{
				SayTo( from, 1113035 ); // Oooh, shiney. I have no use for this, though.
				return false;
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