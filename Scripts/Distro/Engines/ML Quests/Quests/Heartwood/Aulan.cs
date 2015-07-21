using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
	public class CreepyCrawliesQuest : BaseQuest
	{
		/* Creepy Crawlies */
		public override object Title { get { return 1072987; } }

		/* Disgusting!  The way they scuttle on those hairy legs just makes me want to gag. I hate 
		spiders!  Rid the world of twelve and I'll find something nice to give you in thanks. */
		public override object Description { get { return 1073016; } }

		/* Well, okay. But if you decide you are up for it after all, c'mon back and see me. */
		public override object Refuse { get { return 1072270; } }

		/* You're not quite done yet.  Get back to work! */
		public override object Uncomplete { get { return 1072271; } }

		public CreepyCrawliesQuest()
			: base()
		{
			AddObjective( new SlayObjective( typeof( GiantSpider ), "giant spiders", 12 ) );

			AddReward( new BaseReward( typeof( TrinketBag ), 1072341 ) );
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

	public class VoraciousPlantsQuest : BaseQuest
	{
		/* Voracious Plants */
		public override object Title { get { return 1073001; } }

		/* I bet you can't tangle with those nasty plants ... say eight corpsers and two swamp 
		tentacles!  I bet they're too much for you. You may as well confess you can't ...*/
		public override object Description { get { return 1073024; } }

		/* Hahahaha!  I knew it! */
		public override object Refuse { get { return 1073019; } }

		/* You're not quite done yet.  Get back to work! */
		public override object Uncomplete { get { return 1072271; } }

		public VoraciousPlantsQuest()
			: base()
		{
			AddObjective( new SlayObjective( typeof( Corpser ), "corpsers", 8 ) );
			AddObjective( new SlayObjective( typeof( SwampTentacle ), "swamp tentacles", 2 ) );

			AddReward( new BaseReward( typeof( TrinketBag ), 1072341 ) );
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

	public class GibberJabberQuest : BaseQuest
	{
		/* Gibber Jabber */
		public override object Title { get { return 1073004; } }

		/* I bet you can't kill ... ten gibberlings!  I bet they're too much for you.  You may as 
		well confess you can't ... */
		public override object Description { get { return 1073023; } }

		/* Hahahaha!  I knew it! */
		public override object Refuse { get { return 1073019; } }

		/* You're not quite done yet.  Get back to work! */
		public override object Uncomplete { get { return 1072271; } }

		public GibberJabberQuest()
			: base()
		{
			AddObjective( new SlayObjective( typeof( Gibberling ), "gibberlings", 10 ) );

			AddReward( new BaseReward( typeof( TrinketBag ), 1072341 ) );
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

	public class AnimatedMonstrosityQuest : BaseQuest
	{
		/* Animated Monstrosity */
		public override object Title { get { return 1072990; } }

		/* I bet you can't kill ... say twelve ... flesh golems!  I bet 
		they're too much for you.  You may as well confess you can't ... */
		public override object Description { get { return 1073020; } }

		/* Hahahaha!  I knew it! */
		public override object Refuse { get { return 1073019; } }

		/* You're not quite done yet.  Get back to work! */
		public override object Uncomplete { get { return 1072271; } }

		public AnimatedMonstrosityQuest()
			: base()
		{
			AddObjective( new SlayObjective( typeof( FleshGolem ), "flesh golems", 12 ) );

			AddReward( new BaseReward( typeof( TrinketBag ), 1072341 ) );
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

	public class BirdsOfAFeatherQuest : BaseQuest
	{
		/* Birds of a Feather */
		public override object Title { get { return 1073007; } }

		/* I bet you can't kill ... ten harpies!  I bet they're too much 
		for you.  You may as well confess you can't ... */
		public override object Description { get { return 1073022; } }

		/* Hahahaha!  I knew it! */
		public override object Refuse { get { return 1073019; } }

		/* You're not quite done yet.  Get back to work! */
		public override object Uncomplete { get { return 1072271; } }

		public BirdsOfAFeatherQuest()
			: base()
		{
			AddObjective( new SlayObjective( typeof( Harpy ), "harpies", 10 ) );

			AddReward( new BaseReward( typeof( TrinketBag ), 1072341 ) );
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

	public class FrightmaresQuest : BaseQuest
	{
		/* Frightmares */
		public override object Title { get { return 1073000; } }

		/* I bet you can't handle ten plague spawns!  I bet they're too 
		much for you.  You may as well confess you can't ... */
		public override object Description { get { return 1073036; } }

		/* Hahahaha!  I knew it! */
		public override object Refuse { get { return 1073019; } }

		/* You're not quite done yet.  Get back to work! */
		public override object Uncomplete { get { return 1072271; } }

		public FrightmaresQuest()
			: base()
		{
			AddObjective( new SlayObjective( typeof( PlagueSpawn ), "plague spawns", 10 ) );

			AddReward( new BaseReward( typeof( TrinketBag ), 1072341 ) );
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

	public class MoltenReptilesQuest : BaseQuest
	{
		/* Molten Reptiles */
		public override object Title { get { return 1072989; } }

		/* I bet you can't kill ... say ten ... lava lizards!  I bet they're 
		too much for you.  You may as well confess you can't ... */
		public override object Description { get { return 1073018; } }

		/* Hahahaha!  I knew it! */
		public override object Refuse { get { return 1073019; } }

		/* You're not quite done yet.  Get back to work! */
		public override object Uncomplete { get { return 1072271; } }

		public MoltenReptilesQuest()
			: base()
		{
			AddObjective( new SlayObjective( typeof( LavaLizard ), "lava lizards", 10 ) );

			AddReward( new BaseReward( typeof( TrinketBag ), 1072341 ) );
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

	public class BloodyNuisanceQuest : BaseQuest
	{
		/* Bloody Nuisance */
		public override object Title { get { return 1072992; } }

		/* I bet you can't kill ... ten gore fiends!  I bet they're too much 
		for you.  You may as well confess you can't ... */
		public override object Description { get { return 1073021; } }

		/* Hahahaha!  I knew it! */
		public override object Refuse { get { return 1073019; } }

		/* You're not quite done yet.  Get back to work! */
		public override object Uncomplete { get { return 1072271; } }

		public BloodyNuisanceQuest()
			: base()
		{
			AddObjective( new SlayObjective( typeof( GoreFiend ), "gore fiends", 10 ) );

			AddReward( new BaseReward( typeof( TrinketBag ), 1072341 ) );
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

	public class BloodSuckersQuest : BaseQuest
	{
		/* Blood Suckers */
		public override object Title { get { return 1072997; } }

		/* I bet you can't tangle with those bloodsuckers ... say around ten vampire bats!  I bet 
		they're too much for you.  You may as well confess you can't ... */
		public override object Description { get { return 1073025; } }

		/* Hahahaha!  I knew it! */
		public override object Refuse { get { return 1073019; } }

		/* You're not quite done yet.  Get back to work! */
		public override object Uncomplete { get { return 1072271; } }

		public BloodSuckersQuest()
			: base()
		{
			AddObjective( new SlayObjective( typeof( VampireBat ), "vampire bats", 10 ) );

			AddReward( new BaseReward( typeof( TrinketBag ), 1072341 ) );
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

	public class Aulan : MondainQuester
	{
		public override Type[] Quests
		{
			get
			{
				return new Type[] 
			{ 
				typeof( CreepyCrawliesQuest ),
				typeof( VoraciousPlantsQuest ),
				typeof( GibberJabberQuest ),
				typeof( AnimatedMonstrosityQuest ),
				typeof( BirdsOfAFeatherQuest ),
				typeof( FrightmaresQuest ),
				typeof( MoltenReptilesQuest ),
				typeof( BloodyNuisanceQuest ),
				typeof( BloodSuckersQuest )
			};
			}
		}

		[Constructable]
		public Aulan()
			: base( "Aulan", "the expeditionist" )
		{
			SetSkill( SkillName.Meditation, 60.0, 83.0 );
			SetSkill( SkillName.Focus, 60.0, 83.0 );
		}

		public Aulan( Serial serial )
			: base( serial )
		{
		}

		public override void InitBody()
		{
			InitStats( 100, 100, 25 );

			Female = false;
			Race = Race.Elf;

			Hue = 0x847E;
			HairItemID = 0x2FC1;
			HairHue = 0x852;
		}

		public override void InitOutfit()
		{
			AddItem( new ElvenBoots( 0x725 ) );
			AddItem( new ElvenPants( 0x3B3 ) );
			AddItem( new Cloak( 0x16A ) );
			AddItem( new Circlet() );

			Item item;

			item = new HideGloves();
			item.Hue = 0x224;
			AddItem( item );

			item = new HideTunic();
			item.Hue = 0x224;
			AddItem( item );

			item = new HidePauldrons();
			item.Hue = 0x224;
			AddItem( item );
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