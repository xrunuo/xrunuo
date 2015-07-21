using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests.SE
{
	public class HonorCandle : BaseLight
	{
		public override int LitItemID { get { return 0x1430; } }
		public override int UnlitItemID { get { return 0x1433; } }

		[Constructable]
		public HonorCandle()
			: base( 0x1433 )
		{
			Movable = false;
			Duration = TimeSpan.FromSeconds( 5.0 );
			Light = LightType.Circle300;
		}

		public HonorCandle( Serial serial )
			: base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			base.OnDoubleClick( from );

			if ( !Burning )
			{
				PlayerMobile pm = from as PlayerMobile;

				QuestSystem qs = pm.Quest;

				if ( qs != null && qs is HaochisTrialsQuest )
				{
					if ( qs.IsObjectiveInProgress( typeof( LightCandleObjective ) ) )
					{
						QuestObjective obj = qs.FindObjective( typeof( LightCandleObjective ) );

						if ( obj != null )
						{
							obj.Complete();
						}

						qs.AddObjective( new CandleCompleteObjective() );
					}
				}

				//if ( BurntOut )
				from.SendLocalizedMessage( 1063251 ); // You light a candle in honor.

				BurntOut = false;
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			/*int version = */
			reader.ReadInt();
		}
	}
}
