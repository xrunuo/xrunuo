using System;
using Server;

namespace Server.Items
{
	public class SmokeBomb : Item, ICommodity
	{
		public override int LabelNumber { get { return 1030248; } } // smoke bomb

		[Constructable]
		public SmokeBomb()
			: base( 0x2808 )
		{
			Weight = 1.0;

			Stackable = true;
		}

		public SmokeBomb( Serial serial )
			: base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1060640 ); // The item must be in your backpack to use it.
			}
			else if ( from.Skills[SkillName.Ninjitsu].Value < 50.0 )
			{
				string args = String.Format( "{0}\t{1}\t ", 50.ToString( "F1" ), "Ninjitsu" );

				from.SendLocalizedMessage( 1063013, args ); // You need at least ~1_SKILL_REQUIREMENT~ ~2_SKILL_NAME~ skill to use that ability.
			}
			else if ( from.NextSkillTime > DateTime.UtcNow )
			{
				from.SendLocalizedMessage( 1070772 ); // You must wait a few seconds before you can use that item.
			}
			else if ( from.Mana < 10 )
			{
				from.SendLocalizedMessage( 1049456 ); // You don't have enough mana to do that.
			}
			else
			{
				SkillHandlers.Hiding.CombatOverride = true;

				if ( from.UseSkill( SkillName.Hiding ) )
				{
					from.Mana -= 10;

					from.FixedParticles( 0x3709, 1, 30, 9904, 1108, 6, EffectLayer.RightFoot );
					from.PlaySound( 0x22F );

					Consume();
				}

				SkillHandlers.Hiding.CombatOverride = false;
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

			if ( !Stackable )
				Stackable = true;
		}
	}
}
