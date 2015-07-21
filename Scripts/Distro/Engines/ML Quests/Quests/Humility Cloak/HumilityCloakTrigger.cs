using System;
using Server;
using Server.Mobiles;
using Server.Engines.Quests.HumilityCloak;

namespace Server.Items
{
	public class HumilityCloakTrigger : Item
	{
		[Constructable]
		public HumilityCloakTrigger()
			: base( 0x1BC3 )
		{
			Name = "Humility Cloak Trigger";

			Movable = false;
			Visible = false;
		}

		public HumilityCloakTrigger( Serial serial )
			: base( serial )
		{
		}

		public override bool OnMoveOver( Mobile m )
		{
			if ( !base.OnMoveOver( m ) )
				return false;

			PlayerMobile pm = m as PlayerMobile;

			if ( pm != null && pm.Backpack != null && pm.HumilityQuestStatus == HumilityQuestStatus.RewardRefused )
			{
				PlainGreyCloak cloak = pm.FindItemOnLayer( Layer.Cloak ) as PlainGreyCloak;

				if ( cloak == null )
				{
					pm.DropHolding();
					cloak = pm.Backpack.FindItemByType<PlainGreyCloak>();
				}

				if ( cloak != null )
				{
					cloak.Delete();
					pm.PlaceInBackpack( new CloakOfHumility() );

					/* As you near the shrine a strange energy envelops you. Suddenly,
					 * your cloak is transformed into the Cloak of Humility! */
					pm.SendLocalizedMessage( 1075897 );

					pm.PlaySound( 0x244 );
					Effects.SendTargetParticles( pm, 0x376A, 1, 32, 0x13A6, EffectLayer.Waist );

					pm.HumilityQuestStatus = HumilityQuestStatus.Finished;
				}
			}

			return true;
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