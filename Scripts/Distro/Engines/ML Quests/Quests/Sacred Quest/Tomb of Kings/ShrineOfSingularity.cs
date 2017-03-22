using System;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Engines.Quests;
using Server.Engines.Quests.SacredQuest;
using Server.Events;

namespace Server.Items
{
	public class ShrineOfSingularity : Item
	{
		[Constructable]
		public ShrineOfSingularity()
			: base( 0x1 )
		{
			Movable = false;
		}

		public override bool HandlesOnSpeech { get { return true; } }

		public override void OnSpeech( SpeechEventArgs e )
		{
			base.OnSpeech( e );

			PlayerMobile pm = e.Mobile as PlayerMobile;

			if ( pm != null && pm.InRange( this, 3 ) && e.Speech == "unorus" )
			{
				pm.PlaySound( 0xF9 );

				if ( pm.SacredQuest )
				{
					// You enter a state of peaceful contemplation, focusing on the meaning of Singularity.
					pm.SendLocalizedMessage( 1112697 );
				}
				else
				{
					if ( pm.SacredQuestNextChance > DateTime.UtcNow )
					{
						// You need more time to contemplate the Book of Circles before trying again.
						PublicOverheadMessage( MessageType.Regular, 0x47E, 1112685 );
					}
					else
					{
						if ( !pm.HasGump( typeof( GenericQuestGump ) ) && !pm.HasGump( typeof( QuestQuestionGump ) ) )
							pm.SendGump( new LaInsepOmGump() );
					}
				}
			}
		}

		public ShrineOfSingularity( Serial serial )
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