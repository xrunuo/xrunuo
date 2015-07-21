using System;
using Server;
using Server.Mobiles;
using Server.Engines.Craft;
using Server.Targeting;

namespace Server.Items
{
	public abstract class WeavableBasket : BaseContainer
	{
		public abstract int NeededShafts { get; }
		public abstract int NeededReeds { get; }

		[Constructable]
		public WeavableBasket( int itemId )
			: base( itemId )
		{
		}

		public WeavableBasket( Serial serial )
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

		public static void Craft( Mobile from, CraftSystem craftSystem, Type typeRes, BaseTool tool, CraftItem craftItem )
		{
			if ( !( from is PlayerMobile ) || !( (PlayerMobile) from ).BasketWeaving )
			{
				from.EndAction( typeof( CraftSystem ) );

				// You haven't learned basket weaving. Perhaps studying a book would help!
				from.SendGump( new CraftGump( from, craftSystem, tool, 1112253 ) );
			}
			else
			{
				Timer.DelayCall( TimeSpan.FromSeconds( craftSystem.Delay ), new TimerCallback(
					delegate
					{
						from.SendLocalizedMessage( 1074794 ); // Target the material to use:
						from.Target = new ReedsTarget( craftSystem, typeRes, tool, craftItem );
					}
				) );
			}
		}

		private class ReedsTarget : Target
		{
			private CraftSystem m_CraftSystem;
			private Type m_TypeRes;
			private BaseTool m_Tool;
			private CraftItem m_CraftItem;

			public ReedsTarget( CraftSystem system, Type typeRes, BaseTool tool, CraftItem craftItem )
				: base( -1, false, TargetFlags.None )
			{
				m_CraftSystem = system;
				m_TypeRes = typeRes;
				m_Tool = tool;
				m_CraftItem = craftItem;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( !( targeted is SoftenedReeds ) || !( (SoftenedReeds) targeted ).IsChildOf( from.Backpack ) )
				{
					from.SendLocalizedMessage( 1046439 ); // That is not a valid target.
					from.SendLocalizedMessage( 1074794 ); // Target the material to use:

					from.Target = new ReedsTarget( m_CraftSystem, m_TypeRes, m_Tool, m_CraftItem );
				}
				else
				{
					from.EndAction( typeof( CraftSystem ) );

					SoftenedReeds reeds = targeted as SoftenedReeds;

					WeavableBasket basket = (WeavableBasket) Activator.CreateInstance( m_CraftItem.ItemType );
					bool delete = true;

					if ( !reeds.IsChildOf( from.Backpack ) )
					{
						// You don't have enough wooden shafts to make that.
						from.SendGump( new CraftGump( from, m_CraftSystem, m_Tool, 1112246 ) );
					}
					if ( from.Backpack == null || from.Backpack.GetAmount( typeof( Shaft ) ) < basket.NeededShafts )
					{
						// You don't have enough wooden shafts to make that.
						from.SendGump( new CraftGump( from, m_CraftSystem, m_Tool, 1112246 ) );
					}
					else if ( SoftenedReeds.GetTotalReeds( from.Backpack, reeds.PlantHue ) < basket.NeededReeds )
					{
						// You don't have enough of this type of softened reeds to make that.
						from.SendGump( new CraftGump( from, m_CraftSystem, m_Tool, 1112251 ) );
					}
					else
					{
						bool allRequiredSkills = true;

						double chance = m_CraftItem.GetSuccessChance( from, m_TypeRes, m_CraftSystem, true, ref allRequiredSkills );

						if ( chance > 0.0 )
							chance += m_CraftItem.GetTalismanBonus( from, m_CraftSystem );

						if ( allRequiredSkills )
						{
							if ( chance < Utility.RandomDouble() )
							{
								SoftenedReeds.ConsumeReeds( from.Backpack, reeds.PlantHue, basket.NeededReeds / 2 );

								from.SendGump( new CraftGump( from, m_CraftSystem, m_Tool, 1044043 ) ); // You failed to create the item, and some of your materials are lost.
							}
							else
							{
								from.Backpack.ConsumeTotal( typeof( Shaft ), basket.NeededShafts );
								SoftenedReeds.ConsumeReeds( from.Backpack, reeds.PlantHue, basket.NeededReeds );

								basket.Hue = reeds.Hue;
								from.AddToBackpack( basket );
								delete = false;

								bool toolBroken = false;

								m_Tool.UsesRemaining--;

								if ( m_Tool.UsesRemaining < 1 )
									toolBroken = true;

								if ( toolBroken )
								{
									m_Tool.Delete();

									from.SendLocalizedMessage( 1044038 ); // You have worn out your tool!
									from.SendLocalizedMessage( 1044154 ); // You create the item.
								}

								if ( !toolBroken )
								{
									// You create the item.
									from.SendGump( new CraftGump( from, m_CraftSystem, m_Tool, 1044154 ) );
								}
							}
						}
						else
						{
							// You don't have the required skills to attempt this item.
							from.SendGump( new CraftGump( from, m_CraftSystem, m_Tool, 1044153 ) );
						}
					}

					if ( delete )
						basket.Delete();
				}
			}

			protected override void OnTargetCancel( Mobile from, TargetCancelType cancelType )
			{
				from.EndAction( typeof( CraftSystem ) );
				from.SendGump( new CraftGump( from, m_CraftSystem, m_Tool, null ) );
			}
		}
	}
}