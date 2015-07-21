using System;
using Server;
using Server.Engines.Plants;
using Server.Targeting;
using Server.Engines.Craft;

namespace Server.Items
{
	public class NaturalDye : Item, IDyeTub, IUsesRemaining
	{
		#region IDyeTub members
		public int DyedHue { get { return Hue; } }
		public int FailMessage { get { return 1042083; } } // You can not dye that.
		#endregion

		private PlantHue m_PlantHue;
		private int m_UsesRemaining;

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

		[CommandProperty( AccessLevel.GameMaster )]
		public int UsesRemaining
		{
			get { return m_UsesRemaining; }
			set
			{
				m_UsesRemaining = value;
				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool ShowUsesRemaining
		{
			get { return true; }
			set { }
		}

		[Constructable]
		public NaturalDye()
			: this( PlantHue.Plain )
		{
		}

		[Constructable]
		public NaturalDye( PlantHue plantHue )
			: this( 1, plantHue )
		{
		}

		[Constructable]
		public NaturalDye( int amount, PlantHue plantHue )
			: base( 0x182B )
		{
			PlantHue = plantHue;
			Stackable = false;
			Weight = 1.0;
			Amount = amount;
			m_UsesRemaining = 5;
		}

		public override void OnAfterDuped( Item newItem )
		{
			NaturalDye newDye = newItem as NaturalDye;

			if ( newDye != null )
				newDye.PlantHue = PlantHue;
		}

		public NaturalDye( Serial serial )
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
			else
			{
				// Select the item you wish to dye.
				from.SendLocalizedMessage( 1112139 );
				from.BeginTarget( -1, false, TargetFlags.None, new TargetCallback( Target ) );
			}
		}

		public static bool IsValidItem( Item i )
		{
			// As per UO Herald Playguide:
			// "These dyes essentially combine the ability of all currently available dye tubs and Tokuno dyes."

			if ( i.Stackable )
				return false;

			if ( i is BaseArmor )
			{
				BaseArmor armor = i as BaseArmor;

				// Leather Armor
				if ( armor.MaterialType == ArmorMaterialType.Leather || armor.MaterialType == ArmorMaterialType.Studded )
					return true;

				// Metal Armor
				if ( armor.MaterialType == ArmorMaterialType.Plate )
					return true;
			}

			// Weapons
			if ( i is BaseWeapon )
				return true;

			// Spellbooks (including other non-mage spellbooks) & Runebooks
			if ( i is Spellbook || i is Runebook || i is RecallRune )
				return true;

			// Statuettes
			if ( i is MonsterStatuette )
				return true;

			// Books
			if ( i is BaseBook )
				return true;

			// Jewelry
			if ( i is BaseJewel )
				return true;

			// Artifacts and magical items dyable with Tokuno dyes
			if ( PigmentsOfTokuno.CanHue( i ) )
				return true;

			// Talismans
			if ( i is Talisman )
				return true;

			if ( i is DeerMask || i is TribalMask || i is BearMask )
				return true;

			if ( i is SnakeSkinBoots )
				return true;

			return false;
		}

		protected void Target( Mobile from, object targeted )
		{
			Item toDye = targeted as Item;

			if ( toDye == null )
			{
				// You cannot dye that.
				from.SendLocalizedMessage( 1042083 );
			}
			else if ( !IsChildOf( from.Backpack ) )
			{
				// You must have the object in your backpack to use it.
				from.SendLocalizedMessage( 1042010 );
			}
			else if ( toDye.Parent is Mobile )
			{
				// Can't Dye clothing that is being worn.
				from.SendLocalizedMessage( 500861 );
			}
			else if ( !toDye.IsChildOf( from.Backpack ) )
			{
				// The item must be in your backpack to use it.
				from.SendLocalizedMessage( 1060640 );
			}
			else if ( !toDye.Movable )
			{
				// You may not dye items which are locked down.
				from.SendLocalizedMessage( 1010093 );
			}
			else
			{
				if ( IsValidItem( toDye ) )
				{
					toDye.Hue = Hue;
				}
				else if ( toDye is IDyable )
				{
					if ( !( (IDyable) toDye ).Dye( from, this ) )
						return;
				}
				else
				{
					from.SendLocalizedMessage( 1042083 ); // You cannot dye that.
					return;
				}

				from.PlaySound( 0x23E );

				UsesRemaining--;

				if ( UsesRemaining <= 0 )
				{
					// You used up the dye.
					from.SendLocalizedMessage( 500858 );

					Delete();
				}
			}
		}

		public override LocalizedText GetNameProperty()
		{
			PlantHueInfo info = PlantHueInfo.GetInfo( m_PlantHue );

			if ( Amount != 1 )
			{
				return new LocalizedText( info.IsBright()
						? 1113277 // ~1_AMOUNT~ bright ~2_COLOR~ natural dyes
						: 1113276 // ~1_AMOUNT~ ~2_COLOR~ natural dyes
					, String.Format( "{0}\t#{1}", Amount, info.Name ) );
			}
			else
			{
				return new LocalizedText( info.IsBright()
						? 1112138 // bright ~1_COLOR~ natural dye
						: 1112137 // ~1_COLOR~ natural dye
					, String.Format( "#{0}", info.Name ) );
			}
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( ShowUsesRemaining )
				list.Add( 1060584, m_UsesRemaining.ToString() ); // uses remaining: ~1_val~
		}

		public static bool ShouldChooseHue( Mobile m )
		{
			if ( m.Backpack == null )
				return false;

			PlantHue hue = PlantHue.None;

			foreach ( PlantPigment pigments in m.Backpack.FindItemsByType<PlantPigment>() )
			{
				if ( hue == PlantHue.None )
					hue = pigments.PlantHue;
				else if ( hue != pigments.PlantHue )
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
					if ( from.Backpack.GetAmount( typeof( ColorFixative ) ) < 1 || from.Backpack.GetAmount( typeof( PlantPigment ) ) < 1 )
					{
						from.EndAction( typeof( CraftSystem ) );

						// You don't have the components needed to make that.
						from.SendGump( new CraftGump( from, craftSystem, tool, 1044253 ) );
					}
					else if ( ShouldChooseHue( from ) )
					{
						from.SendLocalizedMessage( 1074794 ); // Target the material to use:
						from.Target = new PigmentsTarget( craftSystem, typeRes, tool, craftItem );
					}
					else
					{
						from.EndAction( typeof( CraftSystem ) );
						DoCraft( from, craftSystem, typeRes, tool, craftItem, from.Backpack.FindItemByType<PlantPigment>() );
					}
				}
			) );
		}

		public static void DoCraft( Mobile from, CraftSystem system, Type typeRes, BaseTool tool, CraftItem craftItem, PlantPigment pigments )
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
				pigments.Consume();

				if ( chance < Utility.RandomDouble() )
				{
					from.SendGump( new CraftGump( from, system, tool, 1044043 ) ); // You failed to create the item, and some of your materials are lost.
				}
				else
				{
					from.Backpack.ConsumeTotal( typeof( ColorFixative ), 1 );

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

					from.AddToBackpack( new NaturalDye( pigments.PlantHue ) );
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
			writer.Write( (int) m_UsesRemaining );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_PlantHue = (PlantHue) reader.ReadInt();
			m_UsesRemaining = reader.ReadInt();

			if ( Stackable )
			{
				Stackable = false;
				Amount = 1;
			}
		}

		private class PigmentsTarget : Target
		{
			private CraftSystem m_CraftSystem;
			private Type m_TypeRes;
			private BaseTool m_Tool;
			private CraftItem m_CraftItem;

			public PigmentsTarget( CraftSystem system, Type typeRes, BaseTool tool, CraftItem craftItem )
				: base( -1, false, TargetFlags.None )
			{
				m_CraftSystem = system;
				m_TypeRes = typeRes;
				m_Tool = tool;
				m_CraftItem = craftItem;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( !( targeted is PlantPigment ) || !( (PlantPigment) targeted ).IsChildOf( from.Backpack ) )
				{
					from.SendLocalizedMessage( 1046439 ); // That is not a valid target.
					from.SendLocalizedMessage( 1074794 ); // Target the material to use:

					from.Target = new PigmentsTarget( m_CraftSystem, m_TypeRes, m_Tool, m_CraftItem );
				}
				else
				{
					from.EndAction( typeof( CraftSystem ) );

					PlantPigment pigment = targeted as PlantPigment;

					if ( from.Backpack == null || from.Backpack.GetAmount( typeof( ColorFixative ) ) < 1 )
					{
						// You don't have the components needed to make that.
						from.SendGump( new CraftGump( from, m_CraftSystem, m_Tool, 1044253 ) );
					}
					else
					{
						NaturalDye.DoCraft( from, m_CraftSystem, m_TypeRes, m_Tool, m_CraftItem, pigment );
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