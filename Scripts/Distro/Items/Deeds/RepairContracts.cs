using System;
using System.Collections;
using Server.Mobiles;
using Server.Items;
using Server.Engines.Craft;

namespace Server.Items
{
	public enum ContractType
	{
		Blacksmith,
		Carpenter,
		Fletcher,
		Tailor,
		Tinker
	}

	public class RepairContract : Item
	{
		private ContractType m_Type;
		private double m_Level;
		private string m_Crafter;

		[CommandProperty( AccessLevel.GameMaster )]
		public ContractType Type
		{
			get { return m_Type; }

			set
			{
				m_Type = value;
				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public double Level
		{
			get { return m_Level; }

			set
			{
				m_Level = value;
				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public string Crafter
		{
			get { return m_Crafter; }

			set
			{
				m_Crafter = value;
				InvalidateProperties();
			}
		}

		[Constructable]
		public RepairContract( ContractType m_type, double m_level, string m_Crafter )
			: base( 0x14F0 )
		{
			Crafter = m_Crafter;

			Hue = 0x1BC;

			Level = m_level;

			LootType = LootType.Blessed;

			Type = m_type;

			Weight = 1.0;
		}

		public RepairContract( Serial serial )
			: base( serial )
		{
		}

		public string GetTitle()
		{
			if ( Type == ContractType.Blacksmith )
			{
				return "Blacksmith";
			}
			else if ( Type == ContractType.Carpenter )
			{
				return "Carpenter";
			}
			else if ( Type == ContractType.Fletcher )
			{
				return "Bowyer";
			}
			else if ( Type == ContractType.Tailor )
			{
				return "Tailor";
			}
			else if ( Type == ContractType.Tinker )
			{
				return "Tinker";
			}
			else
			{
				return null;
			}
		}

		public string GetLevel()
		{
			if ( Level >= 120.0 )
			{
				return "A Legendary";
			}
			else if ( Level >= 110.0 )
			{
				return "An Elder";
			}
			else if ( Level >= 100.0 )
			{
				return "A Grandmaster";
			}
			else if ( Level >= 90.0 )
			{
				return "A Master";
			}
			else if ( Level >= 80.0 )
			{
				return "An Adept";
			}
			else if ( Level >= 70.0 )
			{
				return "An Expert";
			}
			else if ( Level >= 60.0 )
			{
				return "A Journeyman";
			}
			else if ( Level >= 50.0 )
			{
				return "A Apprentice";
			}
			else
			{
				return null;
			}
		}

		public CraftSystem GetCraftSystem()
		{
			if ( Type == ContractType.Blacksmith )
			{
				return DefBlacksmithy.CraftSystem;
			}
			else if ( Type == ContractType.Carpenter )
			{
				return DefCarpentry.CraftSystem;
			}
			else if ( Type == ContractType.Fletcher )
			{
				return DefBowFletching.CraftSystem;
			}
			else if ( Type == ContractType.Tailor )
			{
				return DefTailoring.CraftSystem;
			}
			else if ( Type == ContractType.Tinker )
			{
				return DefTinkering.CraftSystem;
			}
			else
			{
				return null;
			}
		}

		public bool GetVendor( Mobile from )
		{
			Map map = from.Map;

			var eable = map.GetMobilesInRange( from.Location, 5 );

			foreach ( Mobile p in eable )
			{
				if ( Type == ContractType.Blacksmith && ( p is Blacksmith || p is Weaponsmith || p is Armorer || p is IronWorker ) )
				{
					return true;
				}
				else if ( Type == ContractType.Carpenter && ( p is Carpenter || p is StoneCrafter ) )
				{
					return true;
				}
				else if ( Type == ContractType.Fletcher && ( p is Bowyer || p is Bower ) )
				{
					return true;
				}
				else if ( Type == ContractType.Tailor && ( p is Tailor || p is Weaver ) )
				{
					return true;
				}
				else if ( Type == ContractType.Tinker && ( p is Tinker || p is GolemCrafter || p is Vagabond ) )
				{
					return true;
				}
			}

			return false;
		}

		public void GetVendorMessage( Mobile from )
		{
			if ( Type == ContractType.Blacksmith )
			{
				from.SendLocalizedMessage( 1047013 ); // You must be near a blacksmith shop to use the repair contract.	
			}
			else if ( Type == ContractType.Carpenter )
			{
				from.SendLocalizedMessage( 1061135 ); // You must be near a carpenter shop to use the repair contract.
			}
			else if ( Type == ContractType.Fletcher )
			{
				from.SendLocalizedMessage( 1061134 ); // You must be near a bowyer's shop to use the repair contract.
			}
			else if ( Type == ContractType.Tailor )
			{
				from.SendLocalizedMessage( 1061132 ); // You must be near a tailor shop to use the repair contract.
			}
			else if ( Type == ContractType.Tinker )
			{
				from.SendLocalizedMessage( 1061166 ); // You must be near a tinker's shop to use the repair contract.
			}
			else
			{
				return;
			}
		}

		public override LocalizedText GetNameProperty()
		{
			string title = GetTitle();

			string level = GetLevel();

			return new LocalizedText( 1061133, String.Format( "{0}\t{1}", level, title ) ); // A repair service contract from ~1_SKILL_TITLE~ ~2_SKILL_NAME~.
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1050043, String.Format( "{0}", Crafter ) ); // crafted by ~1_NAME~

			list.Add( 1060636 ); // exceptional
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( IsChildOf( from.Backpack ) )
			{
				bool IsFound = GetVendor( from );

				if ( IsFound )
				{
					CraftSystem cs = GetCraftSystem();

					Repair.Do( from, cs, m_Level, this );
				}
				else
				{
					GetVendorMessage( from );
				}
			}
			else
			{
				from.SendLocalizedMessage( 1047012 ); // The contract must be in your backpack to use it.
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version 

			writer.Write( (int) m_Type );

			writer.Write( (double) m_Level );

			writer.Write( (string) m_Crafter );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_Type = (ContractType) reader.ReadInt();

			m_Level = reader.ReadDouble();

			m_Crafter = (string) reader.ReadString();

			if ( Weight == 0.1 )
			{
				Weight = 1.0;
			}
		}
	}
}