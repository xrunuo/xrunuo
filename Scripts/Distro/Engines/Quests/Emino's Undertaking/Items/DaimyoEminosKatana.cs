using System;
using Server.Mobiles;
using Server.Items;

namespace Server.Engines.Quests.SE
{
	public class DaimyoEminosKatana : QuestItem
	{
		public override int LabelNumber { get { return 1063214; } } // Daimyo Emino's Katana

		[Constructable]
		public DaimyoEminosKatana()
			: base( 0x13FF )
		{
			Weight = 1.0;
		}

		public DaimyoEminosKatana( Serial serial )
			: base( serial )
		{
		}

		public override bool CanDrop( PlayerMobile player )
		{
			EminosUndertakingQuest qs = player.Quest as EminosUndertakingQuest;

			if ( qs == null )
			{
				return true;
			}

			return !( qs.IsObjectiveInProgress( typeof( TakeSwordObjective ) ) || qs.IsObjectiveInProgress( typeof( KillHenchmensObjective ) ) || qs.IsObjectiveInProgress( typeof( ReturnToDaimyoObjective ) ) );
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
