using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
	public class DabblingOnTheDarkSideQuest : TieredQuest
	{
		public override QuestTier Tier { get { return QuestTier.ZosilemTier1; } }

		/* Dabbling on the Dark Side */
		public override object Title { get { return 1112778; } }

		/* Who dares disturb me? Ah, yes, I know who you are. Thepem has told me that you do a great
		 * deal of business with us, and are to be trusted with some of our more, shall we say...
		 * exotic... mixtures. So, let's get down to business, shall we?
		 * 
		 * Tasty treats are the weakest of our pet enhancing mixtures, but through much experimenting,
		 * I have discovered that more powerful aids can be created. However, the collection of these
		 * ingredients is not exactly healthy for one's karma. You see, in order to create a batch of
		 * deliciously tasty treats, sure to inspire your pet beyond what it is normally capable of, I
		 * will need you to bring me five boura skin, five fairy dragon wings and some dough. Hmm...
		 * no, make that ten fairy dragon wings, as I believe that to be a fair price for my time and
		 * knowledge. */
		public override object Description { get { return 1112963; } }

		/* You are not interested? That is fine, but understand that I shall vigorously deny it if
		 * word of my exotic elixirs gets out because of you. */
		public override object Refuse { get { return 1112964; } }

		/* You need to bring me five boura skin, ten fairy dragon wings and some dough. */
		public override object Uncomplete { get { return 1112965; } }

		/* You have returned, and from the bulge in your pack, I see that you have something for me.
		 * Hand it over, and I will give you a batch of deliciously tasty treats in return. Return
		 * in a bit if you wish to do business with me again. */
		public override object Complete { get { return 1112966; } }

		public DabblingOnTheDarkSideQuest()
		{
			AddObjective( new ObtainObjective( typeof( BouraSkin ), "boura skin", 5 ) );
			AddObjective( new ObtainObjective( typeof( FairyDragonWing ), "fairy dragon wings", 10 ) );
			AddObjective( new ObtainObjective( typeof( Dough ), "dough", 1 ) );

			AddReward( new BaseReward( typeof( DeliciouslyTastyTreat ), 1113004 ) ); // Deliciously Tasty Treat
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

	public class TheBrainyAlchemistQuest : TieredQuest
	{
		public override QuestTier Tier { get { return QuestTier.ZosilemTier1; } }

		/* The Brainy Alchemist */
		public override object Title { get { return 1112779; } }

		/* Who dares disturb me? Ah, yes, I know who you are. Thepem has told me that you do a great
		 * deal of business with us, and are to be trusted with some of our more, shall we say...
		 * exotic... mixtures. So, let's get down to business, shall we?
		 * 
		 * Are you perhaps interested in bettering your knowledge of the fine art of alchemy? If so,
		 * I have discovered a method in which to impart a small bit of alchemy knowledge into an
		 * arcane gem. You will need to bring me an arcane gem, and ten undead gargoyle horns for
		 * the mixture, as well as two kegs of potions for my time and knowledge. How about a keg
		 * of total refreshment potions and a keg of greater poison potions? I'll need to inspect
		 * them first, of course, so just hand them to me before we complete the transaction. Shall
		 * we make a deal? */
		public override object Description { get { return 1112967; } }

		/* You are not interested? That is fine. */
		public override object Refuse { get { return 1112968; } }

		/* Bring me an arcane gem and five undead gargoyle horns for the mixture, as well as a keg
		 * of total refreshment potions and a keg of greater poison potions. I'll need to inspect
		 * the kegs first, of course. */
		public override object Uncomplete { get { return 1112969; } }

		/* You have returned, and from the bulge in your pack, I see that you have something for
		 * me. Hand it over, and I will give you an infused alchemist's gem. I will need to extract
		 * more essence from my... well, you do not need to worry about that. Return in a bit if
		 * you wish to obtain another one. */
		public override object Complete { get { return 1112970; } }

		public TheBrainyAlchemistQuest()
		{
			AddObjective( new ObtainObjective( typeof( ArcaneGem ), "arcane gem", 1 ) );
			AddObjective( new ObtainObjective( typeof( UndamagedUndeadGargoyleHorns ), "undamaged undead gargoyle horns", 10 ) );
			AddObjective( new ObtainObjective( typeof( InspectedKegOfTotalRefreshmentPotions ), "inspected keg of total refreshment potions", 1 ) );
			AddObjective( new ObtainObjective( typeof( InspectedKegOfGreaterPoisonPotions ), "inspected keg of greater poison potions", 1 ) );

			AddReward( new BaseReward( typeof( InfusedAlchemistsGem ), 1113006 ) ); // Infused Alchemist's Gem
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

	public class ArmorUpQuest : TieredQuest
	{
		public override QuestTier Tier { get { return QuestTier.ZosilemTier2; } }

		/* Armor Up */
		public override object Title { get { return 1112780; } }

		/* You have returned, no doubt to purchase some more of my exotic elixirs. I do not trust
		 * just anyone with these elixirs, so know that you are in a small circle of trust.
		 * 
		 * It is unfortunate when Fluffy dies fighting a chicken lizard, but accidents do happen
		 * from time to time. Perhaps you would be interested in an elixir that could prolong the
		 * life of your pet in battle? If you were to bring me five boura skins, ten leather wolf
		 * skins, ten undamaged iron beetle scales, and some dough to hold the mixture together,
		 * I could provide you with a vial of an elixir that is guaranteed to help your pet absorb
		 * damage. Are you interested? */
		public override object Description { get { return 1112971; } }

		/* I see. Well, I am quite busy, so you may show yourself out now if you please. */
		public override object Refuse { get { return 1112972; } }

		/* Bring me five boura skins, ten leather wolf skins, ten undamaged iron beetle scales,
		 * and some dough. */
		public override object Uncomplete { get { return 1112973; } }

		/* Good, you have returned with the ingredients I need. I'll take those from you in
		 * exchange for a vial of armor essence. Return in a couple hours if you wish me to
		 * whip up another batch for you. */
		public override object Complete { get { return 1112974; } }

		public ArmorUpQuest()
		{
			AddObjective( new ObtainObjective( typeof( BouraSkin ), "boura skin", 5 ) );
			AddObjective( new ObtainObjective( typeof( UndamagedIronBeetleScale ), "undamaged iron beetle scale", 10 ) );
			AddObjective( new ObtainObjective( typeof( LeatherWolfSkin ), "leather wolf skin", 10 ) );

			AddReward( new BaseReward( typeof( VialOfArmorEssence ), 1113018 ) ); // Vial of Armor Essence
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

	public class ToTurnBaseMetalIntoVeriteQuest : TieredQuest
	{
		public override QuestTier Tier { get { return QuestTier.ZosilemTier2; } }

		/* To Turn Base Metal Into Verite */
		public override object Title { get { return 1112781; } }

		/* You have returned, no doubt to purchase some more of my exotic elixirs. I do not trust
		 * just anyone with these elixirs, so know that you are in a small circle of trust.
		 * 
		 * Lets get down to why you are here. I have the ability to create a mixture that is able
		 * to turn regular copper ingots into much rarer verite. To do this, I will need twenty
		 * verite ingots and five undead gargoyle medallions. They are made with verite, but were
		 * fashioned using a dark magic that I do not understand... yet. Until then, I will need
		 * some so that I can crush them and dissolve the powder into the elixir of verite
		 * conversion. Hopefully you are strong enough to collect these.
		 * 
		 * Oh, and before I forget... I will need to inspect the ingots before I accept them, so
		 * give those to me before we complete the transaction. */
		public override object Description { get { return 1112975; } }

		/* Undead gargoyles are quite powerful foes, but if you change your mind, come and see me. */
		public override object Refuse { get { return 1112976; } }

		/* Bring me five undead gargoyle medallions and twenty verite ingots. */
		public override object Uncomplete { get { return 1112977; } }

		/* It is good to see that you have returned in one piece. After all, good customers like
		 * you do not grow on trees. Yes, that was a joke.
		 * 
		 * *Zosilem opens her mouth in what approximates a smile, though it reminds you of a
		 * mythical creature you once read about called a 'grue'*
		 * 
		 * I will take those ingredients in exchange for this elixir of verite conversion. If you
		 * come back in a couple hours, I can make more if you wish. */
		public override object Complete { get { return 1112978; } }

		public ToTurnBaseMetalIntoVeriteQuest()
		{
			AddObjective( new ObtainObjective( typeof( UndeadGargoyleMedallion ), "undead gargoyle medallions", 5 ) );
			AddObjective( new ObtainObjective( typeof( PileOfInspectedVeriteIngots ), "pile of inspected verite ingots", 1 ) );

			AddReward( new BaseReward( typeof( ElixirOfVeriteConversion ), 1113009 ) ); // Elixir of Verite Conversion
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

	public class TheForbiddenFruitQuest : TieredQuest
	{
		public override QuestTier Tier { get { return QuestTier.ZosilemTier3; } }

		/* The Forbidden Fruit */
		public override object Title { get { return 1112782; } }

		/* Welcome back, friend. It is good to see you again. Are you here to purchase the results
		 * of my latest research? I believe you find this very exotic elixir to be quite impressive.
		 * 
		 * I discovered that the very nature of the fairy dragon is the reason that deliciously
		 * tasty treats have such an impact on any pet that eats one. What is this nature, you ask?
		 * I do not know what to call it, but for lack of a better term, let us say that it is
		 * extracting its good nature that produces the results we want.
		 * 
		 * Of course, some of the more narrowminded amongst us might consider it cruel to hunt these
		 * creatures, but what do they know? Anyhow, back to what I was saying. I have nearly
		 * perfected a recipe for what I shall call the irresistibly tasty treat. While causing the
		 * pets natural attributes to increase for a short amount of time, it also has the effect of
		 * putting the pet into a frenzy, allowing it to inflict greater damage on your foes.
		 * 
		 * This new mixture requires the usual five boura skin and dough, but also ten pieces of
		 * treefellow wood, which when burned to an ash using a technique I would prefer not to
		 * mention where it can be overheard, gives the mixture its potency. Oh, also, the wood
		 * must come from a treefellow guardian. That is important. If you can obtain these things
		 * for me, I will give you one irresistibly tasty treat in return. Interested, friend? */
		public override object Description { get { return 1112979; } }

		/* Please feel free to return to my shop at any time, for any reason. It is good to talk
		 * with you, even if you do not make a purchase. */
		public override object Refuse { get { return 1112980; } }

		/* I will need five boura skins and some dough, as well as ten treefellow wood from a
		 * treefellow guardian. */
		public override object Uncomplete { get { return 1112981; } }

		/* Good, you have returned with what we need. Quickly now, hand that over before someone
		 * sees what you have, and I shall give you an irresistibly tasty treat in return. I am
		 * busy for the rest of the day, but if you come back tomorrow, I can make more if you wish. */
		public override object Complete { get { return 1112982; } }

		public TheForbiddenFruitQuest()
		{
			AddObjective( new ObtainObjective( typeof( BouraSkin ), "boura skin", 5 ) );
			AddObjective( new ObtainObjective( typeof( TreefellowWood ), "treefellow wood", 10 ) );
			AddObjective( new ObtainObjective( typeof( Dough ), "dough", 1 ) );

			AddReward( new BaseReward( typeof( IrresistiblyTastyTreat ), 1113005 ) ); // Irresistibly Tasty Treat
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

	public class PureValoriteQuest : TieredQuest
	{
		public override QuestTier Tier { get { return QuestTier.ZosilemTier3; } }

		/* Pure Valorite */
		public override object Title { get { return 1112783; } }

		/* Welcome back, friend. It is good to see you again. Are you here to purchase the results
		 * of my latest research? I believe you find this very exotic elixir to be quite impressive.
		 * 
		 * The holy grail of alchemy used to be creating gold from a lesser metal, but even my
		 * apprentice knows how to do that. No, today the ultimate goal is the conversion of lesser
		 * metal into pure valorite. My most secret recipe allows me to do just that. Bronze becomes
		 * valorite! I would not reveal this to you if I did not have confidence that you will be
		 * discreet.
		 * 
		 * The catch is that the ingredients may be hard to come by. Unfortunately, the only source
		 * that I am aware of are putrid undead gargoyles. I will need five of their infused glass
		 * staves, enough so that when I crush the thickest part of one into a fine powder, I have
		 * enough to react with the twenty valorite ingots you also need to obtain. I will not bore
		 * you with the details, but the resulting elixir is capable of turning bronze into pure
		 * valorite! Sound interesting?
		 * 
		 * Of course, I will need to inspect the ingots before I accept them, so give those to me
		 * before we complete the transaction. */
		public override object Description { get { return 1112983; } }

		/* I do not blame you, putrid undead gargoyles are not to be trifled with. Return if you
		 * change your mind, friend. */
		public override object Refuse { get { return 1112984; } }

		/* Remember, this needs to be kept secret at all costs, but I will need five infused glass
		 * staves from putrid undead gargoyles, and twenty bronze ingots. */
		public override object Uncomplete { get { return 1112985; } }

		/* Quickly now, do not just stand there holding those staves where anyone can see! I will
		 * just take those... and here is your elixir of valorite conversion. It is very valuable,
		 * so watch to make sure that you are not followed when you leave the shop. I am busy for
		 * the rest of the day, but if you come back tomorrow, I can make more if you wish. */
		public override object Complete { get { return 1112986; } }

		public PureValoriteQuest()
		{
			AddObjective( new ObtainObjective( typeof( InfusedGlassStave ), "infused glass stave", 5 ) );
			AddObjective( new ObtainObjective( typeof( PileOfInspectedValoriteIngots ), "pile of inspected valorite ingots", 1 ) );

			AddReward( new BaseReward( typeof( ElixirOfValoriteConversion ), 1113010 ) ); // Elixir of Valorite Conversion
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

	public class Zosilem : MondainQuester
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
				typeof( DabblingOnTheDarkSideQuest ),
				typeof( TheBrainyAlchemistQuest ),
				typeof( ArmorUpQuest ),
				typeof( ToTurnBaseMetalIntoVeriteQuest ),
				typeof( TheForbiddenFruitQuest ),
				typeof( PureValoriteQuest )
			};

		public override Type[] Quests { get { return m_Quests; } }

		public override void Advertise()
		{
		}

		[Constructable]
		public Zosilem()
			: base( "Zosilem", "the Alchemist" )
		{
			SetSkill( SkillName.Alchemy, 60.0, 83.0 );
			SetSkill( SkillName.TasteID, 60.0, 83.0 );
		}

		public Zosilem( Serial serial )
			: base( serial )
		{
		}

		public override void InitBody()
		{
			InitStats( 100, 100, 25 );

			Female = true;
			Race = Race.Gargoyle;

			Hue = 0x86EA;
			HairItemID = 0x4273;
			HairHue = 0x323;
		}

		public override void InitOutfit()
		{
			AddItem( new FemaleGargishClothLeggings( 0x736 ) );
			AddItem( new FemaleGargishClothKilt( 0x73D ) );
			AddItem( new FemaleGargishClothChest( 0x38B ) );
			AddItem( new FemaleGargishClothArms( 0x711 ) );
		}

		private static Type[][] m_PileTypes = new Type[][]
			{
				new Type[] {typeof( DullCopperIngot ),	typeof( PileOfInspectedDullCopperIngots )},
				new Type[] {typeof( ShadowIronIngot ),	typeof( PileOfInspectedShadowIronIngots )},
				new Type[] {typeof( BronzeIngot ),		typeof( PileOfInspectedBronzeIngots )},
				new Type[] {typeof( GoldIngot ),		typeof( PileOfInspectedGoldIngots )},
				new Type[] {typeof( AgapiteIngot ),		typeof( PileOfInspectedAgapiteIngots )},
				new Type[] {typeof( VeriteIngot ),		typeof( PileOfInspectedVeriteIngots )},
				new Type[] {typeof( ValoriteIngot ),	typeof( PileOfInspectedValoriteIngots )}
			};

		private static object[][] m_KegTypes = new object[][]
			{
				new object[] {PotionEffect.RefreshGreater,  typeof( InspectedKegOfTotalRefreshmentPotions )},
				new object[] {PotionEffect.PoisonGreater, typeof( InspectedKegOfGreaterPoisonPotions )}
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

		private Type GetKegType( PotionEffect effect )
		{
			for ( int i = 0; i < m_KegTypes.Length; i++ )
			{
				object[] pair = m_KegTypes[i];

				if ( effect == (PotionEffect) pair[0] )
					return (Type) pair[1];
			}

			return null;
		}

		public override bool OnDragDrop( Mobile from, Item item )
		{
			Type inspectedType = null;
			bool success = false;

			inspectedType = GetPileType( item );

			if ( inspectedType != null )
			{
				if ( item.Amount > NeededIngots )
					SayTo( from, 1113037 ); // That's too many.
				else if ( item.Amount < NeededIngots )
					SayTo( from, 1113036 ); // That's not enough.
				else
					success = true;
			}
			else if ( item is PotionKeg )
			{
				PotionKeg keg = (PotionKeg) item;

				inspectedType = GetKegType( keg.Type );

				if ( inspectedType == null )
					SayTo( from, 1113039 ); // It is the wrong type.
				else if ( keg.Held < 100 )
					SayTo( from, 1113038 ); // It is not full.
				else
					success = true;
			}
			else
			{
				SayTo( from, 1113035 ); // Oooh, shiney. I have no use for this, though.
			}

			if ( success )
			{
				SayTo( from, 1113040 ); // Good. I can use this.

				from.AddToBackpack( Activator.CreateInstance( inspectedType ) as Item );
				from.SendLocalizedMessage( 1113041 ); // Now mark the inspected item as a quest item to turn it in.
			}

			return success;
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