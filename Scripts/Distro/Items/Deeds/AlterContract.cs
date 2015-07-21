using System;
using System.Collections;
using Server.Mobiles;
using Server.Items;
using Server.Engines.Craft;

namespace Server.Items
{
	public class AlterContract : Item
	{
		private ContractType m_Type;
		private string m_CrafterName;

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
		public string CrafterName
		{
			get { return m_CrafterName; }

			set
			{
				m_CrafterName = value;
				InvalidateProperties();
			}
		}

		[Constructable]
		public AlterContract( ContractType type, Mobile crafter )
			: base( 0x14F0 )
		{
			m_CrafterName = crafter.Name;
			Type = type;

			Hue = 0x1BC;
			Weight = 1.0;
		}

		public AlterContract( Serial serial )
			: base( serial )
		{
		}

		public string GetTitle()
		{
			if ( m_Type == ContractType.Blacksmith )
				return "Blacksmithing";
			else if ( m_Type == ContractType.Carpenter )
				return "Carpentry";
			else if ( m_Type == ContractType.Tailor )
				return "Tailoring";
			else if ( m_Type == ContractType.Tinker )
				return "Tinkering";
			else
				return null;
		}

		public CraftSystem GetCraftSystem()
		{
			if ( m_Type == ContractType.Blacksmith )
				return DefBlacksmithy.CraftSystem;
			else if ( m_Type == ContractType.Carpenter )
				return DefCarpentry.CraftSystem;
			else if ( m_Type == ContractType.Tailor )
				return DefTailoring.CraftSystem;
			else if ( m_Type == ContractType.Tinker )
				return DefTinkering.CraftSystem;
			else
				return null;
		}

		public override LocalizedText GetNameProperty()
		{
			return new LocalizedText( 1094795, GetTitle() ); // An alter service contract (~1_SKILL_NAME~)
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1050043, m_CrafterName ); // crafted by ~1_NAME~
			list.Add( 1060636 ); // exceptional
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				// The contract must be in your backpack to use it.
				from.SendLocalizedMessage( 1047012 );
			}
			else
			{
				CraftSystem cs = GetCraftSystem();

				Alter.Do( from, cs, this );
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version 

			writer.Write( (int) m_Type );
			writer.Write( (string) m_CrafterName );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_Type = (ContractType) reader.ReadInt();
			m_CrafterName = (string) reader.ReadString();
		}
	}
}