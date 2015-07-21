using System;
using Server;
using Server.Mobiles;

namespace Server.Engines.Quests.SE
{
	public class EminosUndertakingQuest : QuestSystem
	{
		private static Type[] m_TypeReferenceTable = new Type[] { typeof( SE.FindDaimyoEminoObjective ), typeof( SE.FindEliteNinjaZoelObjective ), typeof( SE.EnterTheCaveObjective ), typeof( SE.UseNinjaTrainingsObjective ), typeof( SE.TakeGreenTeleporterObjective ), typeof( SE.BringNoteToZoelObjective ), typeof( SE.TakeBlueTeleporterObjective ), typeof( SE.GoBackBlueTeleporterObjective ), typeof( SE.TakeWhiteTeleporterObjective ), typeof( SE.WalkThroughHallwayObjective ), typeof( SE.TakeSwordObjective ), typeof( SE.KillHenchmensObjective ), typeof( SE.ReturnToDaimyoObjective ), typeof( SE.ImportantQuestInformationConversation ), typeof( SE.DaimyoEminoBeginConversation ), typeof( SE.RadarSEConversation ), typeof( SE.ZoelBeginConversation ), typeof( SE.StandsConversation ), typeof( SE.StandsWarningConversation ), typeof( SE.StrangePassageConversation ), typeof( SE.EminoSecondConversation ), typeof( SE.ZoelGrubsNoteConversation ), typeof( SE.ApproachTheDoorConversation ), typeof( SE.FrownsConversation ), typeof( SE.NarrowsConversation ), typeof( SE.OpenChestConversation ), typeof( SE.ScreamsEchoConversation ), typeof( SE.GoToEminoConversation ), typeof( SE.ContinueKillHenchmensConversation ), typeof( SE.TakeSwordAgainConversation ), typeof( SE.GiftsConversation ) };

		public override Type[] TypeReferenceTable { get { return m_TypeReferenceTable; } }

		public override object Name
		{
			get
			{
				// "Emino's Undertaking"
				return 1063173;
			}
		}

		public override object OfferMessage
		{
			get
			{
				/* Your value as a Ninja must be proven. Find
				 * Daimyo Emino and accept the test he offers.
				 */
				return 1063174;
			}
		}

		public override TimeSpan RestartDelay { get { return TimeSpan.MaxValue; } }
		public override bool IsTutorial { get { return true; } }

		public override int Picture { get { return 0x15D5; } }

		public bool HasLeftTheMansion;
		public bool AlreadySendWarning;
		public bool CanSendWarning;

		public EminosUndertakingQuest( PlayerMobile from )
			: base( from )
		{
		}

		// Serialization
		public EminosUndertakingQuest()
		{
		}

		public override void ChildDeserialize( GenericReader reader )
		{
			/*int version = */
			reader.ReadEncodedInt();

			HasLeftTheMansion = reader.ReadBool();

			AlreadySendWarning = reader.ReadBool();

			CanSendWarning = reader.ReadBool();
		}

		public override void ChildSerialize( GenericWriter writer )
		{
			writer.WriteEncodedInt( (int) 0 ); // version

			writer.Write( (bool) HasLeftTheMansion );

			writer.Write( (bool) AlreadySendWarning );

			writer.Write( (bool) CanSendWarning );
		}

		public override void Accept()
		{
			base.Accept();

			AddConversation( new ImportantQuestInformationConversation() );

			AddObjective( new FindDaimyoEminoObjective() );
		}
	}
}
