using System;
using Server;
using Server.Targeting;
using Server.Mobiles;
using Server.Network;
using Server.Engines.Imbuing;

namespace Server.Items
{
	public class ItemIdentification
	{
		public static void Initialize()
		{
			SkillInfo.Table[(int) SkillName.ItemID].Callback = new SkillUseCallback( OnUse );
		}

		public static TimeSpan OnUse( Mobile from )
		{
			from.SendLocalizedMessage( 500343 ); // What do you wish to appraise and identify?
			from.Target = new InternalTarget();

			return TimeSpan.FromSeconds( 1.0 );
		}

		[PlayerVendorTarget]
		private class InternalTarget : Target
		{
			public InternalTarget()
				: base( 3, false, TargetFlags.None )
			{
				AllowNonlocal = true;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is Mobile )
				{
					// It appears to be:
					from.PrivateOverheadMessage( MessageType.Regular, 0x3B2, 1041349, ( (Mobile) o ).Name, from.Client );
				}
				else if ( !from.CheckTargetSkill( SkillName.ItemID, o, 0, 100 ) )
				{
					// You have no idea how much it might be worth.
					from.SendLocalizedMessage( 1041352 );
				}
				else if ( o is Item )
				{
					Item item = o as Item;

					// TODO (SA): Display the value of the item
					// TODO (SA): When should we show this message? "You conclude that item cannot be magically unraveled. The magic in that item has been weakened due to either low durability or the imbuing process." (1111877)

					if ( item is IImbuable )
					{
						UnravelInfo info = Unraveling.GetUnravelInfo( from, item );

						if ( info == null )
						{
							// You conclude that item cannot be magically unraveled. It appears to possess little to no magic.
							from.PrivateOverheadMessage( MessageType.Regular, 0x3B2, 1111876, from.Client );
						}
						else
						{
							if ( from.Skills[SkillName.Imbuing].Value >= info.MinSkill )
							{
								// You conclude that item will magically unravel into: ~1_ingredient~
								from.PrivateOverheadMessage( MessageType.Regular, 0x3B2, 1111874, String.Format( "#{0}", info.Name.ToString() ), from.Client );
							}
							else
							{
								// Your Imbuing skill is not high enough to identify the imbuing ingredient.
								from.PrivateOverheadMessage( MessageType.Regular, 0x3B2, 1111875, from.Client );
							}
						}
					}
					else
					{
						// You conclude that item cannot be magically unraveled.
						from.PrivateOverheadMessage( MessageType.Regular, 0x3B2, 1111878, from.Client );
					}
				}
				else
				{
					// You have no idea how much it might be worth.
					from.SendLocalizedMessage( 1041352 );
				}
			}

			protected override void OnTargetOutOfRange( Mobile from, object targeted )
			{
				from.SendLocalizedMessage( 500344 ); // You can't see that object well enough to identify it.
			}
		}
	}
}