using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells.Bard;

namespace Server.Engines.Quests
{
	public abstract class BaseBardMasteryQuest : BaseQuest
	{
		public abstract BardMastery Mastery { get; }

		public override bool CanOffer()
		{
			if ( Owner.Skills.Musicianship.Value < 90.0 || Owner.Skills[Mastery.Skill].Value < 90.0 )
			{
				Owner.SendLocalizedMessage( 1115703 ); // Your skills in this focus area are less than the required master level. (90 minimum)
				return false;
			}

			if ( QuestHelper.HasQuest<BaseBardMasteryQuest>( Owner ) )
			{
				Owner.SendLocalizedMessage( 1115702 ); // You must quit your other mastery quests before engaging on a new one.
				return false;
			}

			return true;
		}

		public override void GiveRewards()
		{
			base.GiveRewards();

			Owner.BardMastery = Mastery;

			Owner.PlaceInBackpack( new BardSpellbook() );
			Owner.SendLocalizedMessage( 1074360, "#1028794" ); // You receive a reward: Book of Bard Masteries
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

	public class WieldingTheSonicBladeQuest : BaseBardMasteryQuest
	{
		public override BardMastery Mastery { get { return BardMastery.Discordance; } }

		/* Wielding the Sonic Blade */
		public override object Title { get { return 1115696; } }

		/* This quest is the single quest required for a player to unlock the discordance mastery
		 * abilities for bards. This quest can be completed multiple times to reinstate the discordance
		 * mastery. To prove yourself worthy, you must first be a master of discordance and musicianship.
		 * You must be willing to distort your notes to bring pain to even the most indifferent ears. */
		public override object Description { get { return 1115697; } }

		/* As you wish. I extend to you the thanks of your people for your service thus far.
		 * If you change your mind, my proposal still stands. */
		public override object Refuse { get { return 1115521; } }

		/* You must strive to spread discord. */
		public override object Uncomplete { get { return 1115700; } }

		/* You have proven yourself worthy of wielding your music as a weapon. Rend the ears of your
		 * foes with your wails of discord. Let your song be feared as much as any sword. */
		public override object Complete { get { return 1115701; } }

		public WieldingTheSonicBladeQuest()
			: base()
		{
			AddObjective( new DiscordObjective( typeof( Goat ), 1115698, 5 ) ); // Discord five goats.

			AddReward( new BaseReward( 1115699 ) ); // Recognition for mastery of song wielding.
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

	public class IndoctrinationOfABattleRouserQuest : BaseBardMasteryQuest
	{
		public override BardMastery Mastery { get { return BardMastery.Provocation; } }

		/* Indoctrination of a Battle Rouser */
		public override object Title { get { return 1115657; } }

		/* This quest is the single quest required for a player to unlock the provocation mastery
		 * abilities for bards. This quest can be completed multiple times to reinstate the provocation
		 * mastery. To prove yourself worthy, you must be able to incite even the most peaceful to frenzied
		 * battle lust. */
		public override object Description { get { return 1115697; } }

		/* As you wish. I extend to you the thanks of your people for your service thus far.
		 * If you change your mind, my proposal still stands. */
		public override object Refuse { get { return 1115521; } }

		/* To inspire you must persevere. */
		public override object Uncomplete { get { return 1115660; } }

		/* You have proven yourself worthy of driving armies. Go forth, and have the blessing and curse
		 * of the War Heralds always in your heart and mind. May peace always dwell before you, may
		 * destruction mark your wake, and may fury be your constant companion. Sow the seeds of battle
		 * and glory in the music of war. */
		public override object Complete { get { return 1115661; } }

		public IndoctrinationOfABattleRouserQuest()
			: base()
		{
			// Incite rabbits into battle with 5 wandering healers.
			AddObjective( new InciteObjective( new Type[] { typeof( Rabbit ), typeof( JackRabbit ) }, typeof( WanderingHealer ), 1115658, 5 ) );

			// Recognition for mastery of battle rousing.
			AddReward( new BaseReward( 1115659 ) );
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

	public class TheBeaconOfHarmonyQuest : BaseBardMasteryQuest
	{
		public override BardMastery Mastery { get { return BardMastery.Peacemaking; } }

		/* The Beacon of Harmony */
		public override object Title { get { return 1115677; } }

		/* This quest is the single quest required for a player to unlock the peacemaking mastery
		 * abilities for bards. This quest can be completed multiple times to reinstate the peacemaking
		 * mastery. To prove yourself worthy, you must first be a master of peacemaking and musicianship.
		 * You must be able to calm even the most vicious beast into tranquility. */
		public override object Description { get { return 1115676; } }

		/* As you wish. I extend to you the thanks of your people for your service thus far.
		 * If you change your mind, my proposal still stands. */
		public override object Refuse { get { return 1115521; } }

		/* To deliver peace you must persevere. */
		public override object Uncomplete { get { return 1115680; } }

		/* You have proven yourself a beacon of peace and a bringer of harmony. Only a warrior may choose
		 * the peaceful solution, all others are condemned to it. May your message of peace flow into the
		 * world and shelter you from harm. */
		public override object Complete { get { return 1115681; } }

		public TheBeaconOfHarmonyQuest()
			: base()
		{
			// Calm five mongbats.
			AddObjective( new CalmObjective( new Type[] { typeof( Mongbat ), typeof( StrongMongbat ) }, 1115678, 5 ) );

			// Recognition for mastery of spirit soothing.
			AddReward( new BaseReward( 1115679 ) );
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
