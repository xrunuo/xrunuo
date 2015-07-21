using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.Quests;

namespace Server.Engines.Quests.SE
{
	public class EliteNinjaZoel : BaseQuester
	{
		public override bool ClickTitle { get { return false; } }

		[Constructable]
		public EliteNinjaZoel()
			: base( "the Masterful Tactician" )
		{
		}

		public EliteNinjaZoel( Serial serial )
			: base( serial )
		{
		}

		public override bool OnDragDrop( Mobile from, Item dropped )
		{
			PlayerMobile player = from as PlayerMobile;

			if ( player != null )
			{
				QuestSystem qs = player.Quest;

				if ( qs is EminosUndertakingQuest )
				{
					if ( dropped is NoteForZoel )
					{
						QuestObjective obj = qs.FindObjective( typeof( BringNoteToZoelObjective ) );

						if ( obj != null )
						{
							obj.Complete();
						}

						NoteForZoel note = (NoteForZoel) dropped;

						qs.AddConversation( new ZoelGrubsNoteConversation() );

						qs.AddObjective( new TakeBlueTeleporterObjective() );

						note.Delete();

						return false;
					}
				}
			}

			return base.OnDragDrop( from, dropped );
		}

		public override void InitBody()
		{
			InitStats( 100, 100, 25 );

			Hue = 0x83FE;

			Body = 0x190;

			Name = "Elite Ninja Zoel";
		}

		public override void InitOutfit()
		{
			AddItem( new Tekagi() );
			AddItem( new NinjaTabi( 0x1 ) );
			AddItem( new TattsukeHakama() );
			AddItem( new Bandana( 0x1 ) );
			AddItem( new LeatherNinjaBelt() );
			AddItem( new HakamaShita() );

			HairItemID = 0x203B;
			HairHue = 0x901;
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
				return 2;
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
				if ( qs.IsObjectiveInProgress( typeof( FindEliteNinjaZoelObjective ) ) )
				{
					QuestObjective obj = qs.FindObjective( typeof( FindEliteNinjaZoelObjective ) );

					if ( obj != null )
					{
						obj.Complete();
					}

					qs.AddConversation( new ZoelBeginConversation() );

					qs.AddObjective( new EnterTheCaveObjective() );
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
