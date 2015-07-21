using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.Quests;

namespace Server.Engines.Quests.SE
{
	public class JedahEntille : BaseQuester
	{
		public override bool ClickTitle { get { return false; } }

		[Constructable]
		public JedahEntille()
			: base( "the Silent" )
		{
		}

		public JedahEntille( Serial serial )
			: base( serial )
		{
		}

		public override void InitBody()
		{
			InitStats( 100, 100, 25 );

			Hue = 0x83FE;

			Body = 0x191;

			Female = true;

			Name = "Jedah Entille";
		}

		public override void InitOutfit()
		{
			AddItem( new ThighBoots() );
			AddItem( new FloppyHat( 0x1 ) );
			AddItem( new PlainDress( 0x51F ) );

			HairItemID = 0x203C;
			HairHue = 0x6BE;
		}

		public override bool NoContextMenu( PlayerMobile pm )
		{
			return true;
		}

		public override int GetAutoTalkRange( PlayerMobile pm )
		{
			QuestSystem qs = pm.Quest;

			if ( qs != null )
			{
				return 4;
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

			if ( qs is EminosUndertakingQuest )
			{
				if ( qs.IsObjectiveInProgress( typeof( TakeBlueTeleporterObjective ) ) )
				{
					QuestObjective obj = qs.FindObjective( typeof( TakeBlueTeleporterObjective ) );

					if ( obj != null )
					{
						obj.Complete();
					}

					qs.AddConversation( new ApproachTheDoorConversation() );

					qs.AddObjective( new GoBackBlueTeleporterObjective() );
				}
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
