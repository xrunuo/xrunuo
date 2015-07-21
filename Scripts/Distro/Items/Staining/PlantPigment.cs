using System;
using Server;
using Server.Engines.Plants;
using Server.Targeting;
using Server.Engines.Craft;

namespace Server.Items
{
	public class PlantPigment : Item
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
				if ( Hue == 0 )
					Hue = 0x835;
				InvalidateProperties();
			}
		}

		[Constructable]
		public PlantPigment()
			: this( PlantHue.Plain )
		{
		}

		[Constructable]
		public PlantPigment( PlantHue plantHue )
			: this( 1, plantHue )
		{
		}

		[Constructable]
		public PlantPigment( int amount, PlantHue plantHue )
			: base( 0xF04 )
		{
			PlantHue = plantHue;
			Stackable = true;
			Weight = 1.0;
			Amount = amount;
		}

		public override void OnAfterDuped( Item newItem )
		{
			PlantPigment newPigment = newItem as PlantPigment;

			if ( newPigment != null )
				newPigment.PlantHue = PlantHue;
		}

		public PlantPigment( Serial serial )
			: base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				// You must have the object in your backpack to use it.
				from.SendLocalizedMessage( 1042010 );
			}
			else if ( PlantHueInfo.IsSaturated( m_PlantHue ) )
			{
				// This pigment is saturated and cannot be mixed further.
				from.SendLocalizedMessage( 1112125 );
			}
			else
			{
				// Which plant pigment do you wish to mix this with?
				from.SendLocalizedMessage( 1112123 );
				from.BeginTarget( -1, false, TargetFlags.None, new TargetCallback( Target ) );
			}
		}

		protected void Target( Mobile from, object targeted )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				// You must have the object in your backpack to use it.
				from.SendLocalizedMessage( 1042010 );
			}
			else if ( !( targeted is PlantPigment ) )
			{
				// You may only mix this with another non-saturated plant pigment.
				from.SendLocalizedMessage( 1112124 );
			}
			else
			{
				PlantPigment pigment = targeted as PlantPigment;

				if ( !pigment.IsChildOf( from.Backpack ) )
				{
					// The item must be in your backpack to use it.
					from.SendLocalizedMessage( 1060640 );
				}
				else
				{
					PlantPigment mixed = Mix( from, m_PlantHue, pigment.PlantHue );

					if ( mixed != null )
					{
						Consume();
						pigment.Consume();

						from.AddToBackpack( mixed );
					}
				}
			}
		}

		public static PlantPigment Mix( Mobile from, PlantHue hue1, PlantHue hue2 )
		{
			if ( PlantHueInfo.IsSaturated( hue2 ) )
			{
				// This pigment is saturated and cannot be mixed further.
				from.SendLocalizedMessage( 1112125 );
			}
			else if ( hue1 == hue2 )
			{
				// You decide not to waste pigments by mixing two identical colors.
				from.SendLocalizedMessage( 1112242 );
			}
			else if ( PlantHueInfo.GetNotBright( hue1 ) == PlantHueInfo.GetNotBright( hue2 ) )
			{
				// You decide not to waste pigments by mixing variations of the same hue.
				from.SendLocalizedMessage( 1112243 );
			}
			else
			{
				PlantHue resultant = ( hue1 | hue2 );

				if ( ( resultant & PlantHue.Plain & ~PlantHue.Crossable ) != 0 )
				{
					if ( ( resultant & ( PlantHue.Black | PlantHue.White ) ) != 0 )
					{
						/* Mixing plain pigments with the white and black mutant color
						 * pigments will turn them into the new hues, which are saturated. */
						resultant |= PlantHue.Saturated;
					}
					else
					{
						/* Mixing plain pigments with the normal color pigments will turn
						 * them into the matching bright color pigment. */
						resultant &= ~PlantHue.Plain;
						resultant |= PlantHue.Bright | PlantHue.Crossable;
					}
				}
				else if ( ( resultant & PlantHue.White ) != 0 )
				{
					resultant &= ~PlantHue.White & ~PlantHue.Bright;
					resultant |= PlantHue.Ice;
				}
				else if ( ( resultant & PlantHue.Black ) != 0 )
				{
					resultant &= ~PlantHue.Black & ~PlantHue.Bright;
					resultant |= PlantHue.Dark;
				}

				return new PlantPigment( resultant );
			}

			return null;
		}

		public override LocalizedText GetNameProperty()
		{
			PlantHueInfo info = PlantHueInfo.GetInfo( m_PlantHue );

			if ( Amount != 1 )
			{
				return new LocalizedText( info.IsBright()
						? 1113271 // ~1_AMOUNT~ bright ~2_COLOR~ plant pigments
						: 1113270 // ~1_AMOUNT~ ~2_COLOR~ plant pigments
					, String.Format( "{0}\t#{1}", Amount, info.Name ) );
			}
			else
			{
				return new LocalizedText( info.IsBright()
						? 1112134 // bright ~1_COLOR~ plant pigment
						: 1112133 // ~1_COLOR~ plant pigment
					, String.Format( "#{0}", info.Name ) );
			}
		}

		public static bool ShouldChooseHue( Mobile m )
		{
			if ( m.Backpack == null )
				return false;

			PlantHue hue = PlantHue.None;

			foreach ( PlantClippings clippings in m.Backpack.FindItemsByType<PlantClippings>() )
			{
				if ( hue == PlantHue.None )
					hue = clippings.PlantHue;
				else if ( hue != clippings.PlantHue )
					return true;
			}

			return false;
		}

		public static void Craft( Mobile from, CraftSystem craftSystem, Type typeRes, BaseTool tool, CraftItem craftItem )
		{
			if ( from.Backpack == null )
			{
				from.EndAction( typeof( CraftSystem ) );
				return;
			}

			Timer.DelayCall( TimeSpan.FromSeconds( craftSystem.Delay ), new TimerCallback(
				delegate
				{
					if ( from.Backpack.GetAmount( typeof( Bottle ) ) < 1 || from.Backpack.GetAmount( typeof( PlantClippings ) ) < 1 )
					{
						from.EndAction( typeof( CraftSystem ) );

						// You don't have the components needed to make that.
						from.SendGump( new CraftGump( from, craftSystem, tool, 1044253 ) );
					}
					else if ( ShouldChooseHue( from ) )
					{
						from.SendLocalizedMessage( 1074794 ); // Target the material to use:
						from.Target = new ClippingsTarget( craftSystem, typeRes, tool, craftItem );
					}
					else
					{
						from.EndAction( typeof( CraftSystem ) );
						DoCraft( from, craftSystem, typeRes, tool, craftItem, from.Backpack.FindItemByType<PlantClippings>() );
					}
				}
			) );
		}

		public static void DoCraft( Mobile from, CraftSystem system, Type typeRes, BaseTool tool, CraftItem craftItem, PlantClippings clippings )
		{
			CraftContext context = system.GetContext( from );

			if ( context != null )
				context.OnMade( craftItem );

			bool allRequiredSkills = true;

			double chance = craftItem.GetSuccessChance( from, typeRes, system, true, ref allRequiredSkills );

			if ( chance > 0.0 )
				chance += craftItem.GetTalismanBonus( from, system );

			if ( allRequiredSkills )
			{
				clippings.Consume();

				if ( chance < Utility.RandomDouble() )
				{
					from.SendGump( new CraftGump( from, system, tool, 1044043 ) ); // You failed to create the item, and some of your materials are lost.
				}
				else
				{
					from.Backpack.ConsumeTotal( typeof( Bottle ), 1 );

					from.AddToBackpack( new PlantPigment( clippings.PlantHue ) );

					bool toolBroken = false;

					tool.UsesRemaining--;

					if ( tool.UsesRemaining < 1 )
						toolBroken = true;

					if ( toolBroken )
					{
						tool.Delete();

						from.SendLocalizedMessage( 1044038 ); // You have worn out your tool!
						from.SendLocalizedMessage( 1044154 ); // You create the item.
					}
					else
					{
						// You create the item.
						from.SendGump( new CraftGump( from, system, tool, 1044154 ) );
					}
				}
			}
			else
			{
				// You don't have the required skills to attempt this item.
				from.SendGump( new CraftGump( from, system, tool, 1044153 ) );
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

		private class ClippingsTarget : Target
		{
			private CraftSystem m_CraftSystem;
			private Type m_TypeRes;
			private BaseTool m_Tool;
			private CraftItem m_CraftItem;

			public ClippingsTarget( CraftSystem system, Type typeRes, BaseTool tool, CraftItem craftItem )
				: base( -1, false, TargetFlags.None )
			{
				m_CraftSystem = system;
				m_TypeRes = typeRes;
				m_Tool = tool;
				m_CraftItem = craftItem;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( !( targeted is PlantClippings ) || !( (PlantClippings) targeted ).IsChildOf( from.Backpack ) )
				{
					from.SendLocalizedMessage( 1046439 ); // That is not a valid target.
					from.SendLocalizedMessage( 1074794 ); // Target the material to use:

					from.Target = new ClippingsTarget( m_CraftSystem, m_TypeRes, m_Tool, m_CraftItem );
				}
				else
				{
					from.EndAction( typeof( CraftSystem ) );

					PlantClippings clippings = targeted as PlantClippings;

					if ( from.Backpack == null || from.Backpack.GetAmount( typeof( Bottle ) ) < 1 )
					{
						// You don't have any empty bottles.
						from.SendGump( new CraftGump( from, m_CraftSystem, m_Tool, 1044558 ) );
					}
					else
					{
						PlantPigment.DoCraft( from, m_CraftSystem, m_TypeRes, m_Tool, m_CraftItem, clippings );
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