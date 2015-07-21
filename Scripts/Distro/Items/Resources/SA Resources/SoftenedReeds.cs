using System;
using System.Collections.Generic;
using Server;
using Server.Engines.Plants;
using Server.Engines.Craft;
using Server.Targeting;
using Server.Mobiles;

namespace Server.Items
{
	public class SoftenedReeds : Item
	{
		private PlantHue m_PlantHue;

		[CommandProperty( AccessLevel.GameMaster )]
		public PlantHue PlantHue
		{
			get { return m_PlantHue; }
			set
			{
				m_PlantHue = value;
				Hue = PlantHueInfo.GetInfo( m_PlantHue ).Hue;
				InvalidateProperties();
			}
		}

		[Constructable]
		public SoftenedReeds()
			: this( PlantHue.Plain )
		{
		}

		[Constructable]
		public SoftenedReeds( PlantHue plantHue )
			: this( 1, plantHue )
		{
		}

		[Constructable]
		public SoftenedReeds( int amount, PlantHue plantHue )
			: base( 0x4006 )
		{
			PlantHue = plantHue;
			Stackable = true;
			Weight = 1.0;
			Amount = amount;
		}

		public SoftenedReeds( Serial serial )
			: base( serial )
		{
		}

		public override void OnAfterDuped( Item newItem )
		{
			SoftenedReeds newReeds = newItem as SoftenedReeds;

			if ( newReeds != null )
				newReeds.PlantHue = PlantHue;
		}

		public override LocalizedText GetNameProperty()
		{
			PlantHueInfo info = PlantHueInfo.GetInfo( m_PlantHue );

			if ( Amount != 1 )
			{
				return new LocalizedText( info.IsBright()
						? 1113322 // ~1_AMOUNT~ bright ~2_COLOR~ softened reeds
						: 1113323 // ~1_AMOUNT~ ~2_COLOR~ softened reeds
					, String.Format( "{0}\t#{1}", Amount, info.Name ) );
			}
			else
			{
				return new LocalizedText( info.IsBright()
						? 1112347 // bright ~1_COLOR~ softened reeds
						: 1112346 // ~1_COLOR~ softened reeds
					, String.Format( "#{0}", info.Name ) );
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( (int) m_PlantHue );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_PlantHue = (PlantHue) reader.ReadInt();
		}

		public static int GetTotalReeds( Container cont, PlantHue hue )
		{
			return GetTotalReeds( GetReeds( cont, hue ), hue );
		}

		public static int GetTotalReeds( List<SoftenedReeds> reeds, PlantHue hue )
		{
			int total = 0;

			for ( int i = 0; i < reeds.Count; i++ )
				total += reeds[i].Amount;

			return total;
		}

		public static List<SoftenedReeds> GetReeds( Container cont, PlantHue hue )
		{
			return GetReeds( cont.FindItemsByType<SoftenedReeds>(), hue );
		}

		public static List<SoftenedReeds> GetReeds( List<SoftenedReeds> reeds, PlantHue hue )
		{
			List<SoftenedReeds> validReeds = new List<SoftenedReeds>();

			for ( int i = 0; i < reeds.Count; i++ )
			{
				SoftenedReeds reed = reeds[i];

				if ( reed.PlantHue == hue )
					validReeds.Add( reed );
			}

			return validReeds;
		}

		public static bool ConsumeReeds( Container cont, PlantHue hue, int amount )
		{
			List<SoftenedReeds> reeds = GetReeds( cont, hue );

			if ( GetTotalReeds( reeds, hue ) >= amount )
			{
				for ( int i = 0; i < reeds.Count; i++ )
				{
					SoftenedReeds reed = reeds[i];

					if ( amount >= reed.Amount )
					{
						amount -= reed.Amount;
						reed.Delete();
					}
					else
					{
						reed.Amount -= amount;
						break;
					}
				}

				return true;
			}
			else
			{
				return false;
			}
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
				if ( !( targeted is DryReeds ) || !( (DryReeds) targeted ).IsChildOf( from.Backpack ) )
				{
					from.SendLocalizedMessage( 1046439 ); // That is not a valid target.
					from.SendLocalizedMessage( 1074794 ); // Target the material to use:

					from.Target = new ReedsTarget( m_CraftSystem, m_TypeRes, m_Tool, m_CraftItem );
				}
				else
				{
					from.EndAction( typeof( CraftSystem ) );

					DryReeds reeds = targeted as DryReeds;

					if ( !reeds.IsChildOf( from.Backpack ) )
					{
						// You do not have enough scouring toxins to make that!
						from.SendGump( new CraftGump( from, m_CraftSystem, m_Tool, 1112326 ) );
					}
					if ( from.Backpack == null || from.Backpack.GetAmount( typeof( ScouringToxin ) ) < 1 )
					{
						// You do not have enough scouring toxins to make that!
						from.SendGump( new CraftGump( from, m_CraftSystem, m_Tool, 1112326 ) );
					}
					else if ( DryReeds.GetTotalReeds( from.Backpack, reeds.PlantHue ) < 2 )
					{
						// You don't have enough of this type of dry reeds to make that.
						from.SendGump( new CraftGump( from, m_CraftSystem, m_Tool, 1112250 ) );
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
								DryReeds.ConsumeReeds( from.Backpack, reeds.PlantHue, 1 );

								from.SendGump( new CraftGump( from, m_CraftSystem, m_Tool, 1044043 ) ); // You failed to create the item, and some of your materials are lost.
							}
							else
							{
								from.Backpack.ConsumeTotal( typeof( ScouringToxin ), 1 );
								DryReeds.ConsumeReeds( from.Backpack, reeds.PlantHue, 2 );

								from.AddToBackpack( new SoftenedReeds( reeds.PlantHue ) );

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