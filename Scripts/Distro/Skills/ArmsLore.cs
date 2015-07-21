using System;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.SkillHandlers
{
	public class ArmsLore
	{
		public static void Initialize()
		{
			SkillInfo.Table[(int) SkillName.ArmsLore].Callback = new SkillUseCallback( OnUse );
		}

		public static TimeSpan OnUse( Mobile m )
		{
			m.Target = new InternalTarget();

			m.SendLocalizedMessage( 500349 ); // What item do you wish to get information about?

			return TimeSpan.FromSeconds( 1.0 );
		}

		[PlayerVendorTarget]
		private class InternalTarget : Target
		{
			public InternalTarget()
				: base( 2, false, TargetFlags.None )
			{
				AllowNonlocal = true;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( targeted is BaseWeapon || targeted is BaseArmor )
				{
					if ( from.CheckTargetSkill( SkillName.ArmsLore, targeted, 0, 100 ) )
					{
						// You study the item in an attempt to learn more of its craftsmanship and use.
						from.SendLocalizedMessage( 1061657 );
					}
					else
					{
						// You are not certain...
						from.SendLocalizedMessage( 500353 );
					}
				}
				else
				{
					// This is neither weapon nor armor.
					from.SendLocalizedMessage( 500352 );
				}
			}
		}
	}
}