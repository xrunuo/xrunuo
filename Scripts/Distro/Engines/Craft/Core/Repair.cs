using System;
using Server;
using Server.Mobiles;
using Server.Targeting;
using Server.Items;
using Server.Factions;

namespace Server.Engines.Craft
{
	public class Repair
	{
		public Repair()
		{
		}

		public static void Do( Mobile from, CraftSystem craftSystem, double skill_level, Item contract )
		{
			from.Target = new InternalTarget( craftSystem, skill_level, contract );
			from.SendLocalizedMessage( 1044276 ); // Target an item to repair.
		}

		public static void Do( Mobile from, CraftSystem craftSystem, BaseTool tool )
		{
			from.Target = new InternalTarget( craftSystem, tool );
			from.SendLocalizedMessage( 1044276 ); // Target an item to repair.
		}

		private class InternalTarget : Target
		{
			private CraftSystem m_CraftSystem;
			private BaseTool m_Tool;

			private double m_SkillLevel = 100.0;
			private Item m_Contract = null;

			public InternalTarget( CraftSystem craftSystem, double skill_level, Item contract )
				: base( 2, false, TargetFlags.None )
			{
				m_CraftSystem = craftSystem;
				m_Tool = null;
				m_SkillLevel = skill_level;
				m_Contract = contract;
			}

			private int GetWeakenChance( Mobile mob, SkillName skill, int curHits, int maxHits )
			{
				// 40% - (1% per hp lost) - (1% per 10 craft skill)
				return ( 40 + ( maxHits - curHits ) ) - (int) ( mob.Skills[skill].Value / 10 );
			}

			private bool CheckWeaken( Mobile mob, SkillName skill, int curHits, int maxHits )
			{
				return ( GetWeakenChance( mob, skill, curHits, maxHits ) > Utility.Random( 100 ) );
			}

			private int GetRepairDifficulty( int curHits, int maxHits )
			{
				return ( ( ( maxHits - curHits ) * 1250 ) / Math.Max( maxHits, 1 ) ) - 250;
			}

			private bool CheckRepairDifficulty( Mobile mob, SkillName skill, int curHits, int maxHits )
			{
				double difficulty = GetRepairDifficulty( curHits, maxHits ) * 0.1;

				if ( m_Contract != null )
				{
					double chance = ( m_SkillLevel - difficulty + 25.0 ) / 50.0;

					return ( chance >= Utility.RandomDouble() );
				}

				return mob.CheckSkill( skill, difficulty - 25.0, difficulty + 25.0 );
			}

			public InternalTarget( CraftSystem craftSystem, BaseTool tool )
				: base( 2, false, TargetFlags.None )
			{
				m_CraftSystem = craftSystem;
				m_Tool = tool;
			}

			private static void EndGolemRepair( object state )
			{
				( (Mobile) state ).EndAction( typeof( Golem ) );
			}

			private bool IsSpecialClothing( BaseClothing clothing )
			{
				// Clothing repairable but not craftable

				if ( m_CraftSystem is DefTailoring )
				{
					return ( clothing is BearMask )
						|| ( clothing is DeerMask )
						|| ( clothing is HornedTribalMask )
						|| ( clothing is TheMostKnowledgePerson )
						|| ( clothing is TheRobeOfBritanniaAri )
						|| ( clothing is EmbroideredOakLeafCloak );
				}

				return false;
			}

			private bool IsSpecialTalisman( BaseTalisman talisman )
			{
				// Talismans with durability

				if ( m_CraftSystem is DefTinkering )
				{
					return ( talisman is ManaPhasingOrb );
				}

				return false;
			}

			private bool IsSpecialWeapon( BaseWeapon weapon )
			{
				// Weapons repairable but not craftable

				if ( m_CraftSystem is DefTinkering )
				{
					return ( weapon is Cleaver ) || ( weapon is Hatchet ) || ( weapon is Pickaxe ) || ( weapon is ButcherKnife ) || ( weapon is SkinningKnife );
				}
				else if ( m_CraftSystem is DefCarpentry )
				{
					return ( weapon is Club ) || ( weapon is BlackStaff ) || ( weapon is MagicWand );
				}
				else if ( m_CraftSystem is DefBlacksmithy )
				{
					return ( weapon is Pitchfork );
				}

				return false;
			}

			private bool IsSpecialArmor( BaseArmor armor )
			{
				// Armors repairable but not craftable

				if ( m_CraftSystem is DefBlacksmithy )
					return ( armor is RingmailGlovesOfMining || armor is HelmOfSwiftness || armor is DupresShield );
				else if ( m_CraftSystem is DefTailoring )
					return ( armor is LeatherGlovesOfMining ) || ( armor is StuddedGlovesOfMining );
				else if ( m_CraftSystem is DefTinkering )
					return ( armor is BrightsightLenses || armor is ElvenGlasses || armor is GargishGlasses );

				return false;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				int number;

				if ( m_CraftSystem is DefBlacksmithy )
				{
					bool anvil, forge;
					DefBlacksmithy.CheckAnvilAndForge( from, 2, out anvil, out forge );

					if ( anvil && forge )
					{
						// You must be near a forge and and anvil to repair items.
						number = 1044282;
					}
				}

				if ( m_CraftSystem is DefTinkering && targeted is BaseCreature && ( (BaseCreature) targeted ).IsGolem )
				{
					BaseCreature g = (BaseCreature) targeted;
					int damage = g.HitsMax - g.Hits;

					if ( g.IsDeadBondedPet )
					{
						number = 500426; // You can't repair that.
					}
					else if ( damage <= 0 )
					{
						number = 500423; // That is already in full repair.
					}
					else
					{
						double skillValue = from.Skills[SkillName.Tinkering].Value;

						if ( skillValue < 60.0 )
						{
							number = 1044153; // You don't have the required skills to attempt this item.
						}
						else if ( !from.CanBeginAction( typeof( Golem ) ) )
						{
							number = 501789; // You must wait before trying again.
						}
						else
						{
							if ( damage > (int) ( skillValue * 0.3 ) )
							{
								damage = (int) ( skillValue * 0.3 );
							}

							damage += 30;

							if ( !from.CheckSkill( SkillName.Tinkering, 0.0, 100.0 ) )
							{
								damage /= 2;
							}

							Container pack = from.Backpack;

							if ( pack != null )
							{
								int v = pack.ConsumeUpTo( typeof( IronIngot ), ( damage + 4 ) / 5 );

								if ( v > 0 )
								{
									g.Hits += v * 5;

									number = 1044279; // You repair the item.

									from.BeginAction( typeof( Golem ) );
									Timer.DelayCall( TimeSpan.FromSeconds( 12.0 ), new TimerStateCallback( EndGolemRepair ), from );

									if ( m_Contract != null )
									{
										m_Contract.Delete();
									}
								}
								else
								{
									number = 1044037; // You do not have sufficient metal to make that.
								}
							}
							else
							{
								number = 1044037; // You do not have sufficient metal to make that.
							}
						}
					}
				}
				else if ( targeted is IFactionArtifact )
				{
					// That item cannot be repaired
					number = 1044277;
				}
				else if ( targeted is BaseWeapon )
				{
					BaseWeapon weapon = (BaseWeapon) targeted;
					SkillName skill = m_CraftSystem.MainSkill;
					int toWeaken = 1;

					if ( m_CraftSystem.CraftItems.SearchForSubclass( weapon.GetType() ) == null && !IsSpecialWeapon( weapon ) )
					{
						if ( m_Contract != null )
						{
							// You cannot repair that item with this type of repair contract.
							number = 1061136;
						}
						else
						{
							// You cannot repair that using this type of tool.
							number = 1061139;
						}
					}
					else if ( !weapon.IsChildOf( from.Backpack ) )
					{
						number = 1044275; // The item must be in your backpack to repair it.
					}
					else if ( weapon.MaxHitPoints <= 0 || weapon.HitPoints == weapon.MaxHitPoints )
					{
						number = 1044281; // That item is in full repair
					}
					else if ( weapon.MaxHitPoints <= toWeaken )
					{
						number = 1044278; // That item has been repaired many times, and will break if repairs are attempted again.
					}
					else
					{
						if ( CheckWeaken( from, skill, weapon.HitPoints, weapon.MaxHitPoints ) )
						{
							weapon.MaxHitPoints -= toWeaken;
							weapon.HitPoints = Math.Max( 0, weapon.HitPoints - toWeaken );
						}

						if ( CheckRepairDifficulty( from, skill, weapon.HitPoints, weapon.MaxHitPoints ) )
						{
							number = 1044279; // You repair the item.
							m_CraftSystem.PlayCraftEffect( from );
							weapon.HitPoints = weapon.MaxHitPoints;
						}
						else
						{
							if ( m_Contract != null )
							{
								// You fail to repair the item and the repair contract is destroyed.
								number = 1061137;
							}
							else
							{
								// You fail to repair the item.
								number = 1044280;
							}

							m_CraftSystem.PlayCraftEffect( from );
						}

						if ( m_Contract != null )
						{
							m_Contract.Delete();
						}
					}
				}
				else if ( targeted is BaseArmor )
				{
					BaseArmor armor = (BaseArmor) targeted;
					SkillName skill = m_CraftSystem.MainSkill;
					int toWeaken = 1;

					if ( m_CraftSystem.CraftItems.SearchForSubclass( armor.GetType() ) == null && !IsSpecialArmor( armor ) )
					{
						if ( m_Contract != null )
						{
							// You cannot repair that item with this type of repair contract.
							number = 1061136;
						}
						else
						{
							// You cannot repair that using this type of tool.
							number = 1061139;
						}
					}
					else if ( !armor.IsChildOf( from.Backpack ) )
					{
						number = 1044275; // The item must be in your backpack to repair it.
					}
					else if ( armor.MaxHitPoints <= 0 || armor.HitPoints == armor.MaxHitPoints )
					{
						number = 1044281; // That item is in full repair
					}
					else if ( armor.MaxHitPoints <= toWeaken )
					{
						number = 1044278; // That item has been repaired many times, and will break if repairs are attempted again.
					}
					else
					{
						if ( CheckWeaken( from, skill, armor.HitPoints, armor.MaxHitPoints ) )
						{
							armor.MaxHitPoints -= toWeaken;
							armor.HitPoints = Math.Max( 0, armor.HitPoints - toWeaken );
						}

						if ( CheckRepairDifficulty( from, skill, armor.HitPoints, armor.MaxHitPoints ) )
						{
							number = 1044279; // You repair the item.
							m_CraftSystem.PlayCraftEffect( from );
							armor.HitPoints = armor.MaxHitPoints;
						}
						else
						{
							if ( m_Contract != null )
							{
								// You fail to repair the item and the repair contract is destroyed.
								number = 1061137;
							}
							else
							{
								// You fail to repair the item.
								number = 1044280;
							}

							m_CraftSystem.PlayCraftEffect( from );
						}

						if ( m_Contract != null )
						{
							m_Contract.Delete();
						}
					}
				}
				else if ( targeted is BaseClothing )
				{
					BaseClothing clothing = (BaseClothing) targeted;
					SkillName skill = m_CraftSystem.MainSkill;
					int toWeaken = 1;

					if ( m_CraftSystem.CraftItems.SearchForSubclass( clothing.GetType() ) == null && !IsSpecialClothing( clothing ) )
					{
						if ( m_Contract != null )
						{
							// You cannot repair that item with this type of repair contract.
							number = 1061136;
						}
						else
						{
							// That item cannot be repaired
							number = 1044277;
						}
					}
					else if ( !clothing.IsChildOf( from.Backpack ) )
					{
						number = 1044275; // The item must be in your backpack to repair it.
					}
					else if ( clothing.MaxHitPoints <= 0 || clothing.HitPoints == clothing.MaxHitPoints )
					{
						number = 1044281; // That item is in full repair
					}
					else if ( clothing.MaxHitPoints <= toWeaken )
					{
						number = 1044278; // That item has been repaired many times, and will break if repairs are attempted again.
					}
					else
					{
						if ( CheckWeaken( from, skill, clothing.HitPoints, clothing.MaxHitPoints ) )
						{
							clothing.MaxHitPoints -= toWeaken;
							clothing.HitPoints = Math.Max( 0, clothing.HitPoints - toWeaken );
						}

						if ( CheckRepairDifficulty( from, skill, clothing.HitPoints, clothing.MaxHitPoints ) )
						{
							number = 1044279; // You repair the item.
							m_CraftSystem.PlayCraftEffect( from );
							clothing.HitPoints = clothing.MaxHitPoints;
						}
						else
						{
							if ( m_Contract != null )
							{
								// You fail to repair the item and the repair contract is destroyed.
								number = 1061137;
							}
							else
							{
								// You fail to repair the item.
								number = 1044280;
							}

							m_CraftSystem.PlayCraftEffect( from );
						}

						if ( m_Contract != null )
						{
							m_Contract.Delete();
						}
					}
				}
				else if ( targeted is BaseJewel )
				{
					BaseJewel jewel = (BaseJewel) targeted;
					SkillName skill = m_CraftSystem.MainSkill;
					int toWeaken = 1;

					if ( m_CraftSystem.CraftItems.SearchForSubclass( jewel.GetType() ) == null && !( jewel is SilverBracelet || jewel is SilverRing ) )
					{
						if ( m_Contract != null )
						{
							// You cannot repair that item with this type of repair contract.
							number = 1061136;
						}
						else
						{
							// That item cannot be repaired
							number = 1044277;
						}
					}
					else if ( !jewel.IsChildOf( from.Backpack ) )
					{
						number = 1044275; // The item must be in your backpack to repair it.
					}
					else if ( !jewel.CanLoseDurability || jewel.MaxHitPoints <= 0 || jewel.HitPoints == jewel.MaxHitPoints )
					{
						number = 1044281; // That item is in full repair
					}
					else if ( jewel.MaxHitPoints <= toWeaken )
					{
						number = 1044278; // That item has been repaired many times, and will break if repairs are attempted again.
					}
					else
					{
						if ( CheckWeaken( from, skill, jewel.HitPoints, jewel.MaxHitPoints ) )
						{
							jewel.MaxHitPoints -= toWeaken;
							jewel.HitPoints = Math.Max( 0, jewel.HitPoints - toWeaken );
						}

						if ( CheckRepairDifficulty( from, skill, jewel.HitPoints, jewel.MaxHitPoints ) )
						{
							number = 1044279; // You repair the item.
							m_CraftSystem.PlayCraftEffect( from );
							jewel.HitPoints = jewel.MaxHitPoints;
						}
						else
						{
							if ( m_Contract != null )
							{
								// You fail to repair the item and the repair contract is destroyed.
								number = 1061137;
							}
							else
							{
								// You fail to repair the item.
								number = 1044280;
							}

							m_CraftSystem.PlayCraftEffect( from );
						}

						if ( m_Contract != null )
						{
							m_Contract.Delete();
						}
					}
				}
				else if ( targeted is BaseTalisman )
				{
					BaseTalisman talisman = (BaseTalisman) targeted;
					SkillName skill = m_CraftSystem.MainSkill;
					int toWeaken = 1;

					if ( !IsSpecialTalisman( talisman ) )
					{
						if ( m_Contract != null )
						{
							// You cannot repair that item with this type of repair contract.
							number = 1061136;
						}
						else
						{
							// That item cannot be repaired
							number = 1044277;
						}
					}
					else if ( !talisman.IsChildOf( from.Backpack ) )
					{
						number = 1044275; // The item must be in your backpack to repair it.
					}
					else if ( !talisman.CanLoseDurability || talisman.MaxHitPoints <= 0 || talisman.HitPoints == talisman.MaxHitPoints )
					{
						number = 1044281; // That item is in full repair
					}
					else if ( talisman.MaxHitPoints <= toWeaken )
					{
						number = 1044278; // That item has been repaired many times, and will break if repairs are attempted again.
					}
					else
					{
						if ( CheckWeaken( from, skill, talisman.HitPoints, talisman.MaxHitPoints ) )
						{
							talisman.MaxHitPoints -= toWeaken;
							talisman.HitPoints = Math.Max( 0, talisman.HitPoints - toWeaken );
						}

						if ( CheckRepairDifficulty( from, skill, talisman.HitPoints, talisman.MaxHitPoints ) )
						{
							number = 1044279; // You repair the item.
							m_CraftSystem.PlayCraftEffect( from );
							talisman.HitPoints = talisman.MaxHitPoints;
						}
						else
						{
							if ( m_Contract != null )
							{
								// You fail to repair the item and the repair contract is destroyed.
								number = 1061137;
							}
							else
							{
								// You fail to repair the item.
								number = 1044280;
							}

							m_CraftSystem.PlayCraftEffect( from );
						}

						if ( m_Contract != null )
						{
							m_Contract.Delete();
						}
					}
				}
				else if ( targeted is Item )
				{
					number = 1044283; // You cannot repair that.

					SkillName skill = m_CraftSystem.MainSkill;

					double value = from.Skills[skill].Value;

					if ( targeted is BlankScroll )
					{
						Item contract = null;

						if ( value < 50.0 )
						{
							// You must be at least apprentice level to create a repair service contract.
							number = 1047005;
						}
						else
						{
							if ( skill == SkillName.Blacksmith )
							{
								contract = new RepairContract( ContractType.Blacksmith, value, from.Name );
							}
							else if ( skill == SkillName.Carpentry )
							{
								contract = new RepairContract( ContractType.Carpenter, value, from.Name );
							}
							else if ( skill == SkillName.Fletching )
							{
								contract = new RepairContract( ContractType.Fletcher, value, from.Name );
							}
							else if ( skill == SkillName.Tailoring )
							{
								contract = new RepairContract( ContractType.Tailor, value, from.Name );
							}
							else if ( skill == SkillName.Tinkering )
							{
								contract = new RepairContract( ContractType.Tinker, value, from.Name );
							}
						}

						if ( contract != null )
						{
							from.AddToBackpack( contract );

							number = 1044154; // You create the item.

							BlankScroll scroll = targeted as BlankScroll;

							if ( scroll.Amount >= 2 )
							{
								scroll.Amount -= 1;
							}
							else
							{
								scroll.Delete();
							}
						}
					}
				}
				else
				{
					number = 500426; // You can't repair that.
				}

				if ( m_Tool != null )
				{
					//CraftContext context = m_CraftSystem.GetContext( from );

					from.SendGump( new CraftGump( from, m_CraftSystem, m_Tool, number ) );
				}
				else
				{
					from.SendLocalizedMessage( number );
				}
			}
		}
	}
}
