using System;
using Server;
using Server.Mobiles;
using Server.Targeting;
using Server.Items;
using Server.Engines.VeteranRewards;

namespace Server.Engines.Craft
{
	[AttributeUsage( AttributeTargets.Class )]
	public class AlterableAttribute : Attribute
	{
		public Type CraftSystem { get; private set; }
		public Type AlteredType { get; private set; }
		public bool Inherit { get; private set; }

		public AlterableAttribute( Type craftSystem, Type alteredType, bool inherit = false )
		{
			CraftSystem = craftSystem;
			AlteredType = alteredType;
			Inherit = inherit;
		}
	}

	public class Alter
	{
		public static void Do( Mobile from, CraftSystem craftSystem, Item contract )
		{
			from.Target = new InternalTarget( craftSystem, contract );
			from.SendLocalizedMessage( 1094730 ); // Target the item to alter.
		}

		public static void Do( Mobile from, CraftSystem craftSystem, BaseTool tool )
		{
			from.Target = new InternalTarget( craftSystem, tool );
			from.SendLocalizedMessage( 1094730 ); // Target the item to alter.
		}

		private class InternalTarget : Target
		{
			private CraftSystem m_CraftSystem;
			private BaseTool m_Tool;
			private Item m_Contract;

			public InternalTarget( CraftSystem craftSystem, Item contract )
				: base( 2, false, TargetFlags.None )
			{
				m_CraftSystem = craftSystem;
				m_Contract = contract;
			}

			public InternalTarget( CraftSystem craftSystem, BaseTool tool )
				: base( 2, false, TargetFlags.None )
			{
				m_CraftSystem = craftSystem;
				m_Tool = tool;
			}

			private static AlterableAttribute GetAlterableAttribute( object o, bool inherit = false )
			{
				object[] attrs = o.GetType().GetCustomAttributes( typeof( AlterableAttribute ), inherit );

				if ( attrs != null && attrs.Length > 0 )
				{
					AlterableAttribute attr = attrs[0] as AlterableAttribute;

					if ( attr != null && ( !inherit || attr.Inherit ) )
						return attr;
				}

				return null;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				int number = -1;
				Item origItem = targeted as Item;
				SkillName skill = m_CraftSystem.MainSkill;
				double value = from.Skills[skill].Value;

				var alterInfo = GetAlterableAttribute( targeted, false );

				if ( alterInfo == null )
					alterInfo = GetAlterableAttribute( targeted, true );

				if ( origItem == null || !origItem.IsChildOf( from.Backpack ) )
				{
					// The item must be in your backpack for you to alter it.
					number = 1094729;
				}
				else if ( origItem is BlankScroll )
				{
					if ( m_Contract == null )
					{
						if ( value >= 100.0 )
						{
							Item contract = null;

							if ( skill == SkillName.Blacksmith )
								contract = new AlterContract( ContractType.Blacksmith, from );
							else if ( skill == SkillName.Carpentry )
								contract = new AlterContract( ContractType.Carpenter, from );
							else if ( skill == SkillName.Tailoring )
								contract = new AlterContract( ContractType.Tailor, from );
							else if ( skill == SkillName.Tinkering )
								contract = new AlterContract( ContractType.Tinker, from );

							if ( contract != null )
							{
								from.AddToBackpack( contract );

								number = 1044154; // You create the item.

								// Consume a blank scroll
								origItem.Consume();
							}
						}
						else
						{
							// You must be at least grandmaster level to create an alter service contract.
							number = 1111869;
						}
					}
					else
					{
						// You may not alter that item.
						number = 1094728;
					}
				}
				else if ( alterInfo == null )
				{
					// You may not alter that item.
					number = 1094728;
				}
				else if ( !IsAlterable( origItem ) )
				{
					// You may not alter that item.
					number = 1094728;
				}
				else if ( alterInfo.CraftSystem != m_CraftSystem.GetType() )
				{
					if ( m_Tool != null )
					{
						// You may not alter that item.
						number = 1094728;
					}
					else
					{
						// You cannot alter that item with this type of alter contract.
						number = 1094793;
					}
				}
				else if ( !Imbuing.Soulforge.CheckProximity( from, 2 ) )
				{
					// You must be near a soulforge to alter an item.
					number = 1111867;
				}
				else if ( !Imbuing.Soulforge.CheckQueen( from ) )
				{
					// You must rise to the rank of noble in the eyes of the Gargoyle Queen before her majesty will allow you to use this soulforge.
					number = 1113736;
				}
				else if ( origItem is BaseWeapon && ( (BaseWeapon) origItem ).Enchanted )
				{
					// You cannot alter an item that is currently enchanted.
					number = 1111849;
				}
				else if ( m_Contract == null && value < 100.0 )
				{
					// You must be at least grandmaster level to alter an item.
					number = 1111870;
				}
				else
				{
					Item alteredItem = Activator.CreateInstance( alterInfo.AlteredType ) as Item;

					alteredItem.Hue = origItem.Hue;
					alteredItem.Insured = origItem.Insured;
					alteredItem.PayedInsurance = origItem.PayedInsurance;
					alteredItem.Label1 = origItem.Label1;
					alteredItem.Label2 = origItem.Label2;
					alteredItem.Label3 = origItem.Label3;
					alteredItem.LootType = origItem.LootType;

					if ( origItem.LabelNumber != origItem.DefaultLabelNumber )
						alteredItem.LabelNumber = origItem.LabelNumber;

					AlterFrom( origItem, alteredItem );

					alteredItem.Hue = origItem.PrivateHue;

					from.PlaceInBackpack( alteredItem );
					alteredItem.Location = origItem.Location;
					origItem.Delete();

					if ( m_Contract != null )
						m_Contract.Delete();

					// You have altered the item.
					number = 1094727;
				}

				if ( m_Tool != null )
					from.SendGump( new CraftGump( from, m_CraftSystem, m_Tool, number ) );
				else
					from.SendLocalizedMessage( number );
			}

			private static bool IsAlterable( Item item )
			{
				if ( item.RequiredRace == Race.Gargoyle )
					return false;

				if ( item is IRewardItem )
					return false;

				if ( item is ISetItem )
					return false;

				return true;
			}

			private static void AlterFrom( Item origItem, Item alteredItem )
			{
				if ( alteredItem is BaseWeapon )
				{
					BaseWeapon origWeapon = origItem as BaseWeapon;
					BaseWeapon alteredWeapon = alteredItem as BaseWeapon;

					alteredWeapon.AlterFrom( origWeapon );
				}
				else if ( alteredItem is BaseArmor )
				{
					BaseArmor origArmor = origItem as BaseArmor;
					BaseArmor alteredArmor = alteredItem as BaseArmor;

					alteredArmor.AlterFrom( origArmor );
				}
				else if ( alteredItem is BaseClothing )
				{
					var alteredClothing = alteredItem as BaseClothing;

					if ( origItem is BaseQuiver )
					{
						var origQuiver = origItem as BaseQuiver;
						alteredClothing.AlterFrom( origQuiver );
					}
					else if ( origItem is BaseClothing )
					{
						var origClothing = origItem as BaseClothing;
						alteredClothing.AlterFrom( origClothing );
					}
				}
			}
		}
	}
}
