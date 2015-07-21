using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
	public class RumorsAboundQuest : BaseQuest
	{
		public override QuestChain ChainID { get { return QuestChain.TheArisen; } }
		public override Type NextQuest { get { return typeof( TheArisenQuest ); } }
		public override bool DoneOnce { get { return true; } }

		/* Rumors Abound */
		public override object Title { get { return 1112514; } }

		/* I know not the details, but from what little truth that can be separated
		 * from rumor, it seems that the Holy City is being savaged repeatedly by the
		 * Arisen. Diligence demands that you make your way with haste to the Holy
		 * City, which lies some distance to the south-east. Please take this writ
		 * and deliver it to Naxatilor so he will know that I sent you. */
		public override object Description { get { return 1112515; } }

		/* The safety of the Holy City and the Elders is at stake. Surely you cannot
		 * be refusing to help? */
		public override object Refuse { get { return 1112516; } }

		/* Make haste to the Holy City! */
		public override object Uncomplete { get { return 1112518; } }

		/* I am sorry, I am too busy to...
		 * 
		 * *You hand Naxatilor the writ*
		 * 
		 * I see that Egwexem has sent you. It is good that you have come, we could
		 * use your help. */
		public override object Complete { get { return 1112519; } }

		public RumorsAboundQuest()
		{
			AddObjective( new DeliverObjective( typeof( EgwexemsWrit ), "Egwexem's Writ", 1, typeof( Naxatilor ), "Naxatilor" ) );

			AddReward( new BaseReward( 1112731 ) ); // Knowing that you did the right thing.
		}

		public override bool CanOffer()
		{
			return !Owner.SacredQuest;
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

	public class TheArisenQuest : BaseQuest
	{
		public override QuestChain ChainID { get { return QuestChain.TheArisen; } }
		public override bool DoneOnce { get { return true; } }
		public override bool AllObjectives { get { return false; } }

		/* The Arisen */
		public override object Title { get { return 1112538; } }

		/* We need your assistance with a matter that is most grave. To the north,
		 * from within the Tomb of Kings, the Arisen are emerging each night to
		 * attack the Holy City.
		 * 
		 * Shortly after we unsealed the entrance to the Abyss, found in the depths
		 * of the Tomb, strange happenings began to occur. At first, there were only 
		 * a few reports of strange noises coming from the Tomb of Kings late at
		 * night. Investigating the Tomb during the daytime uncovered nothing unusual,
		 * so we sent someone to seek out the source of these noises one night. When
		 * morning came and he had not returned, we knew something was amiss.
		 * 
		 * We sent word to the Royal City asking for help, but the following night,
		 * unspeakable evil erupted from the entrance to the Tomb! A defense of the
		 * city was quickly marshaled, but the Arisen proved to be quite powerful.
		 * Unfortunately, they are also persistent, as every night since then, this
		 * city, the original birthplace of our people, has faced wave after wave of
		 * Arisen. We know not the cause of the attacks, nor the source, as
		 * investigations by daylight yield little. We have been hard pressed just to
		 * defend the Elders here, much less push the Arisen back into the Tomb.
		 * 
		 * We could use your help in this! Either help defend the Holy City at night,
		 * or enter the Tomb of Kings itself and seek out the Arisen at their source.
		 * It is your choice to make, as you know your own abilities best. If you
		 * decide to enter the Tomb of Kings, you'll need to speak the words "ord"
		 * and "an-ord" to pass the Serpent's Breath.<br><br>Succeed in this task,
		 * and I shall reward you well. */
		public override object Description { get { return 1112539; } }

		/* To decide not to help would bring you great shame. */
		public override object Refuse { get { return 1112540; } }

		/* Help defend the Holy City or head down into the tombs! */
		public override object Uncomplete { get { return 1112541; } }

		// Good work! Now return to Naxatilor.
		public override int CompleteMessage { get { return 1112542; } }

		/* You have proven yourself both brave and worthy! Know that you have both
		 * our gratitude and our blessing to enter the Abyss, if you so wish.
		 * 
		 * All who wish to enter must seek out the Shrine of Singularity to the
		 * North for further meditation. Only those found to be on the Sacred
		 * Quest will be allowed to enter the Abyss. I would advise that you seek
		 * out some of the ancient texts in the Holy City Museum, which you can find
		 * to the south, so that you might focus better while meditating at the
		 * Shrine. As promised, here is your reward. */
		public override object Complete { get { return 1112543; } }

		public TheArisenQuest()
		{
			AddObjective( new SlayObjective( typeof( EffeteUndeadGargoyle ), "effete undead gargoyles", 10 ) );
			AddObjective( new SlayObjective( typeof( EffetePutridGargoyle ), "effete putrid gargoyles", 10 ) );
			AddObjective( new SlayObjective( typeof( GargoyleShade ), "gargoyle shades", 10 ) );

			AddReward( new BaseReward( typeof( NecklaceOfDiligence ), 1113137 ) ); // Necklace of Diligence
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