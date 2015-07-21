using System;
using Server;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	public enum LevelType
	{
		First = 1,
		Hybrid = 2,
		Second = 3,
		Third = 4,
		Fourth = 5
	}

	public class DungeonTreasureChest : LockableContainer
	{
		private LevelType m_Level;

		public override bool Decays { get { return true; } }
		public override TimeSpan DecayTime { get { return TimeSpan.FromMinutes( 10.0 ); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public LevelType Level
		{
			get { return m_Level; }
			set
			{
				m_Level = value;

				SetRequiments( m_Level );
				ClearContents();
				FillChest( m_Level );

				InvalidateProperties();
			}
		}

		public void SetRequiments( LevelType level )
		{
			switch ( m_Level )
			{
				case LevelType.First:
					{
						RequiredSkill = LockLevel = 52;
						TrapType = TrapType.DartTrap;
						TrapPower = Utility.RandomMinMax( 0, 25 );

						break;
					}
				case LevelType.Hybrid:
					{
						RequiredSkill = LockLevel = 56;
						TrapType = TrapType.PoisonTrap;
						TrapPower = Utility.RandomMinMax( 0, 35 );

						break;
					}
				case LevelType.Second:
					{
						RequiredSkill = LockLevel = 72;
						TrapType = TrapType.ExplosionTrap;
						TrapPower = Utility.RandomMinMax( 0, 45 );

						break;
					}
				case LevelType.Third:
					{
						RequiredSkill = LockLevel = 84;
						TrapType = TrapType.PoisonTrap;
						TrapPower = Utility.RandomMinMax( 0, 35 );

						break;
					}
				case LevelType.Fourth:
					{
						RequiredSkill = LockLevel = 92;
						TrapType = TrapType.ExplosionTrap;
						TrapPower = Utility.RandomMinMax( 0, 45 );

						break;
					}
			}
		}

		public static int CalculateType( LevelType level )
		{
			int[] m_ItemIDs = new int[15];

			switch ( level )
			{
				case LevelType.First:
					{
						m_ItemIDs = new int[] { 0xE7E, 0x9A9, 0xE3E, 0xE3F, 0xE3C, 0xE3D };
						break;
					}
				case LevelType.Hybrid:
					{
						m_ItemIDs = new int[] { 0xE7E, 0x9A9, 0xE3E, 0xE3F, 0xE3C, 0xE3D };
						break;
					}
				case LevelType.Second:
					{
						m_ItemIDs = new int[] { 0xE7E, 0x9A9, 0xE3E, 0xE3F, 0xE3C, 0xE3D, 0xE77, 0xE7F, 0xE43, 0xE42, 0xE41, 0xE40, 0x9AB, 0xE7C };
						break;
					}
				case LevelType.Third:
					{
						m_ItemIDs = new int[] { 0xE43, 0xE42, 0xE41, 0xE40, 0x9AB, 0xE7C };
						break;
					}
				case LevelType.Fourth:
					{
						m_ItemIDs = new int[] { 0xE43, 0xE42, 0xE41, 0xE40, 0x9AB, 0xE7C };
						break;
					}
			}

			int itemID = Utility.RandomList( m_ItemIDs );

			return itemID;
		}

		public void FillChest( LevelType level )
		{
			switch ( level )
			{
				case LevelType.First:
					{
						DropItem( new Gold( Utility.RandomMinMax( 50, 70 ) ) );

						if ( 0.3 > Utility.RandomDouble() )
						{
							int chance = Utility.Random( 3 );

							switch ( chance )
							{
								case 0:
									DropItem( new Bolt( 10 ) );
									break;
								case 1:
									DropItem( new Arrow( 10 ) );
									break;
								case 2:
									DropItem( Loot.RandomReagent() );
									break;
							}
						}

						break;
					}
				case LevelType.Hybrid:
					{
						DropItem( new Gold( Utility.RandomMinMax( 10, 40 ) ) );

						if ( 0.3 > Utility.RandomDouble() )
						{
							int chance = Utility.Random( 6 );

							switch ( chance )
							{
								case 0:
									DropItem( new Bolt( 5 ) );
									break;
								case 1:
									DropItem( new Shoes() );
									break;
								case 2:
									DropItem( new Sandals() );
									break;
								case 3:
									DropItem( new Candle() );
									break;
								case 4:
									DropItem( new BeverageBottle( Utility.RandomBool() ? BeverageType.Ale : BeverageType.Liquor ) );
									break;
								case 5:
									DropItem( new Jug( BeverageType.Cider ) );
									break;
							}
						}

						break;
					}
				case LevelType.Second:
					{
						DropItem( new Gold( Utility.RandomMinMax( 120, 160 ) ) );

						if ( 0.3 > Utility.RandomDouble() )
						{
							Item reagent = Loot.RandomReagent();
							DropItem( reagent );
						}

						break;
					}
				case LevelType.Third:
					{
						DropItem( new Gold( Utility.RandomMinMax( 300, 400 ) ) );

						if ( 0.3 > Utility.RandomDouble() )
						{
							int chance = Utility.Random( 3 );

							switch ( chance )
							{
								case 0:
									DropItem( new Bolt( 10 ) );
									break;
								case 1:
									DropItem( new Arrow( 10 ) );
									break;
								case 2:
									DropItem( Loot.RandomGem() );
									break;
							}
						}

						int chance_new = Utility.Random( 5 );

						switch ( chance_new )
						{
							case 0:
								{
									BaseWeapon weapon = Loot.RandomWeapon( this.Map == Map.Tokuno );
									BaseRunicTool.ApplyAttributesTo( weapon, 3, 10, 40 );
									DropItem( weapon );
									break;
								}
							case 1:
								{
									BaseArmor armor = Loot.RandomArmor();
									BaseRunicTool.ApplyAttributesTo( armor, 3, 10, 40 );
									DropItem( armor );
									break;
								}
							case 2:
								{
									BaseJewel jewel = Loot.RandomJewelry();
									BaseRunicTool.ApplyAttributesTo( jewel, 3, 10, 40 );
									DropItem( jewel );
									break;
								}
							case 3:
								{
									BaseHat hat = Loot.RandomHat( this.Map == Map.Tokuno );
									BaseRunicTool.ApplyAttributesTo( hat, 3, 10, 40 );
									DropItem( hat );
									break;
								}
							case 4:
								{
									BaseWand wand = Loot.RandomWand();
									DropItem( wand );
									break;
								}
						}

						break;
					}
				case LevelType.Fourth:
					{
						DropItem( new Gold( Utility.RandomMinMax( 500, 600 ) ) );

						for ( int i = 0; i < 2; i++ )
						{
							int chance = Utility.Random( 5 );

							switch ( chance )
							{
								case 0:
									{
										BaseWeapon weapon = Loot.RandomWeapon( this.Map == Map.Tokuno );
										BaseRunicTool.ApplyAttributesTo( weapon, 4, 10, 50 );
										DropItem( weapon );
										break;
									}
								case 1:
									{
										BaseArmor armor = Loot.RandomArmor( this.Map == Map.Tokuno );
										BaseRunicTool.ApplyAttributesTo( armor, 4, 10, 50 );
										DropItem( armor );
										break;
									}
								case 2:
									{
										BaseJewel jewel = Loot.RandomJewelry();
										BaseRunicTool.ApplyAttributesTo( jewel, 4, 10, 50 );
										DropItem( jewel );
										break;
									}
								case 3:
									{
										BaseHat hat = Loot.RandomHat( this.Map == Map.Tokuno );
										BaseRunicTool.ApplyAttributesTo( hat, 4, 10, 50 );
										DropItem( hat );
										break;
									}
								case 4:
									{
										BaseWand wand = Loot.RandomWand();
										DropItem( wand );
										break;
									}
							}
						}

						if ( 0.3 > Utility.RandomDouble() )
						{
							Item gem = Loot.RandomGem();
							gem.Amount = Utility.RandomMinMax( 1, 2 );
							DropItem( gem );
						}

						break;
					}
			}
		}

		public void ClearContents()
		{
			for ( int i = this.Items.Count - 1; i >= 0; --i )
			{
				if ( i < this.Items.Count )
					( (Item) this.Items[i] ).Delete();
			}
		}

		public override bool IsDecoContainer { get { return false; } }

		[Constructable]
		public DungeonTreasureChest( LevelType level )
			: base( CalculateType( level ) )
		{
			Movable = false;

			Locked = true;

			Level = level;
		}

		public DungeonTreasureChest( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version   

			writer.Write( (int) m_Level );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_Level = (LevelType) reader.ReadInt();
		}
	}

	public class DungeonTreasureChestFirst : DungeonTreasureChest
	{
		[Constructable]
		public DungeonTreasureChestFirst()
			: base( LevelType.First )
		{
		}

		public DungeonTreasureChestFirst( Serial serial )
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
	}

	public class DungeonTreasureChestHybrid : DungeonTreasureChest
	{
		[Constructable]
		public DungeonTreasureChestHybrid()
			: base( LevelType.Hybrid )
		{
		}

		public DungeonTreasureChestHybrid( Serial serial )
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
	}

	public class DungeonTreasureChestSecond : DungeonTreasureChest
	{
		[Constructable]
		public DungeonTreasureChestSecond()
			: base( LevelType.Second )
		{
		}

		public DungeonTreasureChestSecond( Serial serial )
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
	}

	public class DungeonTreasureChestThird : DungeonTreasureChest
	{
		[Constructable]
		public DungeonTreasureChestThird()
			: base( LevelType.Third )
		{
		}

		public DungeonTreasureChestThird( Serial serial )
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
	}

	public class DungeonTreasureChestFourth : DungeonTreasureChest
	{
		[Constructable]
		public DungeonTreasureChestFourth()
			: base( LevelType.Fourth )
		{
		}

		public DungeonTreasureChestFourth( Serial serial )
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
	}
}
