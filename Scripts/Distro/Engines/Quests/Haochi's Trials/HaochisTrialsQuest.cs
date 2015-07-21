using System;
using Server;
using Server.Mobiles;

namespace Server.Engines.Quests.SE
{
	public enum OpponentType
	{
		FierceDragon,
		DeadlyImp
	}

	public enum ChoiceType
	{
		Cats,
		Gold
	}

	public class HaochisTrialsQuest : QuestSystem
	{
		private static Type[] m_TypeReferenceTable = new Type[] { typeof( SE.SpeakToDaimyoHaochiObjective ), typeof( SE.FollowGreenPathObjective ), typeof( SE.KillRoninsOrSoulsObjective ), typeof( SE.FirstTrialCompleteObjective ), typeof( SE.FollowYellowPathObjective ), typeof( SE.ChooseOpponentObjective ), typeof( SE.SecondTrialCompleteObjective ), typeof( SE.FollowBluePathObjective ), typeof( SE.UseHonorableExecutionObjective ), typeof( SE.ThirdTrialCompleteObjective ), typeof( SE.FollowRedPathObjective ), typeof( SE.GiveGypsyGoldOrHuntCatsObjective ), typeof( SE.MadeChoiceObjective ), typeof( SE.RetrieveKatanaObjective ), typeof( SE.GiveSwordDaimyoObjective ), typeof( SE.LightCandleObjective ), typeof( SE.CandleCompleteObjective ), typeof( SE.KillNinjaObjective ), typeof( SE.ExecutionsCompleteObjective ), typeof( SE.DaimyoHaochiBeginConversation ), typeof( SE.GreenPathConversation ), typeof( SE.GainKarmaForRoninConversation ), typeof( SE.GainKarmaForSoulsConversation ), typeof( SE.ContinueSlayingSoulsConversation ), typeof( SE.ContinueSlayingRoninsConversation ), typeof( SE.ThanksForSoulsConversation ), typeof( SE.ThanksForRoninsConversation ), typeof( SE.YellowPathConversation ), typeof( SE.DragonConversation ), typeof( SE.ImpConversation ), typeof( SE.WolfsConversation ), typeof( SE.HaochiSmilesConversation ), typeof( SE.ApproachGypsyConversation ), typeof( SE.RespectForCatsConversation ), typeof( SE.RespectForGoldConversation ), typeof( SE.SpotSwordConversation ), typeof( SE.WithoutSwordConversation ), typeof( SE.ThanksForSwordConversation ), typeof( SE.WellDoneConversation ), typeof( SE.FirewellConversation ) };

		public override Type[] TypeReferenceTable { get { return m_TypeReferenceTable; } }

		public override object Name
		{
			get
			{
				// "Haochi's Trials"
				return 1063022;
			}
		}

		public override object OfferMessage
		{
			get
			{
				/* <i>As you enter the courtyard you notice a
				 * faded sign. It reads: </i><br><br>
				 * 
				 * Welcome to your new home, Samurai.<br><br>
				 * 
				 *  Though your skills are only a shadow of what
				 * they can be some day, you must prove your
				 * adherence to the code of the Bushido.<br><br>
				 * 
				 * Seek Daimyo Haochi for guidance.<br><br>
				 *
				 * <i>Will you accept the challenge?</i>
				 */
				return 1063023;
			}
		}

		public override TimeSpan RestartDelay { get { return TimeSpan.MaxValue; } }
		public override bool IsTutorial { get { return true; } }

		public override int Picture { get { return 0x15D7; } }

		public bool EnterBridge;
		public bool EnterGreenZone;
		public bool EnterYellowZone;
		public bool EnterBlueZone;
		public bool EnterRedZone;
		public bool EnterTreasureZone;

		public int KilledRonins;
		public int KilledSouls;
		public OpponentType Opponent;

		public bool SendRoninKarma;
		public bool SendSoulsKarma;

		public ChoiceType Choice;

		public HaochisTrialsQuest( PlayerMobile from )
			: base( from )
		{
		}

		// Serialization
		public HaochisTrialsQuest()
		{
		}

		public override void ChildDeserialize( GenericReader reader )
		{
			/*int version = */
			reader.ReadEncodedInt();

			EnterBridge = reader.ReadBool();
			EnterGreenZone = reader.ReadBool();
			EnterYellowZone = reader.ReadBool();
			EnterBlueZone = reader.ReadBool();
			EnterRedZone = reader.ReadBool();
			EnterTreasureZone = reader.ReadBool();

			KilledRonins = reader.ReadInt();
			KilledSouls = reader.ReadInt();
			Opponent = (OpponentType) reader.ReadInt();

			SendRoninKarma = reader.ReadBool();
			SendSoulsKarma = reader.ReadBool();

			Choice = (ChoiceType) reader.ReadInt();
		}

		public override void ChildSerialize( GenericWriter writer )
		{
			writer.WriteEncodedInt( (int) 0 ); // version

			writer.Write( (bool) EnterBridge );
			writer.Write( (bool) EnterGreenZone );
			writer.Write( (bool) EnterYellowZone );
			writer.Write( (bool) EnterBlueZone );
			writer.Write( (bool) EnterRedZone );
			writer.Write( (bool) EnterTreasureZone );

			writer.Write( (int) KilledRonins );
			writer.Write( (int) KilledSouls );
			writer.Write( (int) Opponent );

			writer.Write( (bool) SendRoninKarma );
			writer.Write( (bool) SendSoulsKarma );

			writer.Write( (int) Choice );
		}

		public override void Accept()
		{
			base.Accept();

			AddConversation( new ImportantQuestInformationConversation() );

			AddObjective( new SpeakToDaimyoHaochiObjective() );
		}
	}
}
