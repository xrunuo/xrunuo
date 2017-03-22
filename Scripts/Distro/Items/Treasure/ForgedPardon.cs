using System;
using Server;
using Server.Mobiles;
using Server.Spells;

namespace Server.Items
{
	public class ForgedPardon : Item
	{
		public override int LabelNumber { get { return 1116234; } } // A Forged Pardon

		private DateTime m_LastUsed;

		[Constructable]
		public ForgedPardon()
			: base( 0x14EE )
		{
			Weight = 1.0;
			Hue = 0x499;
		}

		public override void OnDoubleClick( Mobile from )
		{
			PlayerMobile pm = from as PlayerMobile;

			if ( pm == null )
				return;

			if ( !IsChildOf( pm.Backpack ) )
			{
				// That must be in your pack for you to use it.
				pm.SendLocalizedMessage( 1042001 );
			}
			else if ( DateTime.UtcNow - pm.LastForgedPardonUse < TimeSpan.FromDays( 1.0 ) )
			{
				// You must wait 24 hours before using another forged pardon.
				pm.SendLocalizedMessage( 1116587 );
			}
			else if ( SpellHelper.CheckCombat( pm ) )
			{
				// You cannot use a forged pardon while in combat.
				pm.SendLocalizedMessage( 1116588 );
			}
			else if ( pm.Kills == 0 )
			{
				// You are not known for having commited any murders.
				pm.SendLocalizedMessage( 1116207 );
			}
			else if ( DateTime.UtcNow - m_LastUsed > TimeSpan.FromSeconds( 15.0 ) )
			{
				m_LastUsed = DateTime.UtcNow;

				// Using this pardon again will remove one murder count from your character and consume the pardon.
				pm.SendLocalizedMessage( 1116235 );
			}
			else
			{
				pm.Kills--;
				pm.LastForgedPardonUse = DateTime.UtcNow;

				// Your murder count has been successfully updated.
				pm.SendLocalizedMessage( 1116208 );

				Delete();
			}
		}

		public override bool OnDragLift( Mobile from )
		{
			m_LastUsed = DateTime.MinValue;

			return base.OnDragLift( from );
		}

		public ForgedPardon( Serial serial )
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

			/*int version =*/
			reader.ReadInt();
		}
	}
}
