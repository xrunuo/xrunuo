using System;
using System.Runtime.InteropServices;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Accounting;
using Server.Events;

namespace Server.Misc
{
	public class CharacterCreation
	{
		public static void Initialize()
		{
			// Register our event handler
			EventSink.Instance.CreateCharRequest += new CreateCharRequestEventHandler( EventSink_CreateCharRequest );
		}

		private static void AddBackpack( Mobile m )
		{
			Container pack = m.Backpack;

			if ( pack == null )
			{
				pack = new Backpack();
				pack.Movable = false;

				m.AddItem( pack );
			}

			PackItem( new Gold( 1000 ) ); // Starting gold can be customized here
		}

		private static void AddShirt( Mobile m, int shirtHue )
		{
			int hue = Utility.ClipDyedHue( shirtHue & 0x3FFF );

			if ( m.Race == Race.Human )
			{
				switch ( Utility.Random( 3 ) )
				{
					case 0:
						EquipItem( new Shirt( hue ), true );
						break;
					case 1:
						EquipItem( new FancyShirt( hue ), true );
						break;
					case 2:
						EquipItem( new Doublet( hue ), true );
						break;
				}
			}
			else if ( m.Race == Race.Elf )
			{
				EquipItem( new ElvenShirt( hue ), true );
			}
		}

		private static void AddPants( Mobile m, int pantsHue )
		{
			int hue = Utility.ClipDyedHue( pantsHue & 0x3FFF );

			if ( m.Race == Race.Human )
			{
				if ( m.Female )
				{
					switch ( Utility.Random( 2 ) )
					{
						case 0:
							EquipItem( new Skirt( hue ), true );
							break;
						case 1:
							EquipItem( new Kilt( hue ), true );
							break;
					}
				}
				else
				{
					switch ( Utility.Random( 2 ) )
					{
						case 0:
							EquipItem( new LongPants( hue ), true );
							break;
						case 1:
							EquipItem( new ShortPants( hue ), true );
							break;
					}
				}
			}
			else if ( m.Race == Race.Elf )
			{
				EquipItem( new ElvenPants( hue ), true );
			}
		}

		private static void AddShoes( Mobile m )
		{
			if ( m.Race == Race.Human )
				EquipItem( new Shoes( Utility.RandomYellowHue() ), true );
			else if ( m.Race == Race.Elf )
				EquipItem( new ElvenBoots( Utility.RandomYellowHue() ) );
		}

		private static Mobile CreateMobile( Account a )
		{
			if ( a.Count >= a.Limit )
				return null;

			for ( int i = 0; i < a.Length; ++i )
			{
				if ( a[i] == null )
					return ( a[i] = new PlayerMobile() );
			}

			return null;
		}

		private static bool VerifyName( string name )
		{
			if ( !NameVerification.Validate( name, 2, 16, true, false, true, 1, NameVerification.SpaceDashPeriodQuote ) )
				return false;

			return true;
		}

		private static void EventSink_CreateCharRequest( CreateCharRequestEventArgs args )
		{
			string name = args.Name.Trim();

			if ( !VerifyName( name ) )
			{
				Console.WriteLine( "Login: {0}: Character creation failed, invalid name '{1}'", args.State, args.Name );

				args.State.BlockAllPackets = false;
				args.State.Send( new PopupMessage( PMMessage.InvalidName ) );
				return;
			}

			if ( !VerifyProfession( args.Profession ) )
				args.Profession = 0;

			Mobile newChar = CreateMobile( args.Account as Account );

			if ( newChar == null )
			{
				Console.WriteLine( "Login: {0}: Character creation failed, account full", args.State );
				return;
			}

			args.Mobile = newChar;
			m_Mobile = newChar;

			newChar.IsPlayer = true;
			newChar.AccessLevel = ( (Account) args.Account ).AccessLevel;
			newChar.Female = args.Female;
			newChar.Race = args.Race;
			newChar.Hue = newChar.Race.ClipSkinHue( args.Hue & 0x3FFF ) | 0x8000;
			newChar.Hunger = 20;

			bool young = false;

			if ( newChar is PlayerMobile )
			{
				PlayerMobile pm = (PlayerMobile) newChar;

				pm.Profession = args.Profession;

				if ( pm.AccessLevel == AccessLevel.Player && ( (Account) pm.Account ).Young && ( pm.Profession != 0 ) && !( pm.Profession == 6 || pm.Profession == 7 ) )
					young = pm.Young = true;

				if ( pm.Race == Race.Gargoyle )
					pm.LoyaltyInfo.SetValue( Engines.Loyalty.LoyaltyGroup.GargoyleQueen, 2000 );
			}

			newChar.Name = name;

			AddBackpack( newChar );

			SetStats( newChar, args.Str, args.Dex, args.Int );
			SetSkills( newChar, args.Skills, args.Profession );

			Race race = newChar.Race;

			if ( race.ValidateHair( newChar, args.HairID ) )
			{
				newChar.HairItemID = args.HairID;
				newChar.HairHue = race.ClipHairHue( args.HairHue & 0x3FFF );
			}

			if ( race.ValidateFacialHair( newChar, args.BeardID ) )
			{
				newChar.FacialHairItemID = args.BeardID;
				newChar.FacialHairHue = race.ClipHairHue( args.BeardHue & 0x3FFF );
			}

			if ( args.Profession <= 3 )
			{
				AddShirt( newChar, args.ShirtHue );
				AddPants( newChar, args.PantsHue );
				AddShoes( newChar );
			}

			var acct = args.Account as Account;

			if ( acct.GetTag( "GivenStarterKit" ) == null && newChar.Backpack != null )
			{
				var token = new StarterKitToken();
				token.Owner = newChar;
				newChar.Backpack.DropItem( token );

				acct.SetTag( "GivenStarterKit", "true" );
			}

			CityInfo city = GetStartLocation( args, young );

			newChar.MoveToWorld( city.Location, city.Map );

			Console.WriteLine( "Login: {0}: New character being created (account={1})", args.State, ( (Account) args.Account ).Username );
			Console.WriteLine( " - Character: {0} (serial={1})", newChar.Name, newChar.Serial );
			Console.WriteLine( " - Started: {0} {1} in {2}", city.City, city.Location, city.Map.ToString() );
		}

		public static bool VerifyProfession( int profession )
		{
			if ( profession < 0 )
				return false;
			else if ( profession < 8 )
				return true;
			else
				return false;
		}

		private static CityInfo GetStartLocation( CreateCharRequestEventArgs args, bool isYoung )
		{
			if ( args.State.Version == null || args.State.Version < ClientVersion.Client70130 )
			{
				if ( args.Mobile.Race == Race.Gargoyle )
					return new CityInfo( "Royal City", "Royal City", 738, 3486, -19, Map.TerMur, 1150169 );
				else
					return new CityInfo( "New Haven", "New Haven Bank", 3503, 2574, 14, Map.Trammel, 1150168 );
			}
			else
			{
				return args.City;
			}
		}

		private static void FixStat( ref int stat )
		{
			if ( stat < 10 )
				stat = 10;
			else if ( stat > 60 )
				stat = 60;
		}

		private static void SetStats( Mobile m, int str, int dex, int intel )
		{
			FixStat( ref str );
			FixStat( ref dex );
			FixStat( ref intel );

			int total = str + dex + intel;

			if ( total > 90 )
				str = dex = intel = 10;

			m.InitStats( str, dex, intel );
		}

		private static bool ValidSkills( SkillNameValue[] skills )
		{
			int total = 0;

			for ( int i = 0; i < skills.Length; ++i )
			{
				if ( skills[i].Value < 0 || skills[i].Value > 50 )
					return false;

				total += skills[i].Value;

				for ( int j = i + 1; j < skills.Length; ++j )
				{
					if ( skills[j].Value > 0 && skills[j].Name == skills[i].Name )
						return false;
				}
			}

			return ( total == 100 ) || ( total == 120 );
		}

		private static Mobile m_Mobile;

		private static void SetSkills( Mobile m, SkillNameValue[] skills, int prof )
		{
			switch ( prof )
			{
				case 1: // Warrior
					{
						skills = new SkillNameValue[]
						{
							new SkillNameValue( SkillName.Anatomy, 30 ),
							new SkillNameValue( SkillName.Healing, 30 ),
							new SkillNameValue( SkillName.Swords, 30 ),
							new SkillNameValue( SkillName.Tactics, 30 )
						};

						break;
					}
				case 2: // Magician
					{
						skills = new SkillNameValue[]
						{
							new SkillNameValue( SkillName.EvalInt, 30 ),
							new SkillNameValue( SkillName.Wrestling, 30 ),
							new SkillNameValue( SkillName.Magery, 30 ),
							new SkillNameValue( SkillName.Meditation, 30 )
						};

						break;
					}
				case 3: // Blacksmith
					{
						skills = new SkillNameValue[]
						{
							new SkillNameValue( SkillName.Mining, 30 ),
							new SkillNameValue( SkillName.Tailoring, 30 ),
							new SkillNameValue( SkillName.Blacksmith, 30 ),
							new SkillNameValue( SkillName.Tinkering, 30 )
						};

						break;
					}
				case 4: // Necromancer
					{
						skills = new SkillNameValue[]
						{
							new SkillNameValue( SkillName.Necromancy, 30 ),
							new SkillNameValue( SkillName.SpiritSpeak, 30 ),
							new SkillNameValue( SkillName.Fencing, 30 ),
							new SkillNameValue( SkillName.Meditation, 30 )
						};

						break;
					}
				case 5: // Paladin
					{
						skills = new SkillNameValue[]
						{
							new SkillNameValue( SkillName.Chivalry, 30 ),
							new SkillNameValue( SkillName.Swords, 30 ),
							new SkillNameValue( SkillName.Focus, 30 ),
							new SkillNameValue( SkillName.Tactics, 30 )
						};

						break;
					}
				case 6:	// Samurai
					{
						skills = new SkillNameValue[]
						{
							new SkillNameValue( SkillName.Bushido, 30 ),
							new SkillNameValue( SkillName.Swords, 30 ),
							new SkillNameValue( SkillName.Focus, 30 ),
							new SkillNameValue( SkillName.Parry, 30 )
					};
						break;
					}
				case 7:	// Ninja
					{
						skills = new SkillNameValue[]
						{
							new SkillNameValue( SkillName.Ninjitsu, 30 ),
							new SkillNameValue( SkillName.Hiding, 30 ),
							new SkillNameValue( SkillName.Fencing, 30 ),
							new SkillNameValue( SkillName.Stealth, 30 )
						};
						break;
					}
				default:
					{
						if ( !ValidSkills( skills ) )
							return;

						break;
					}
			}

			bool addSkillItems = true;

			switch ( prof )
			{
				case 1: // Warrior
					{
						if ( m.Race == Race.Human )
							EquipItem( new LeatherChest() );
						else if ( m.Race == Race.Elf )
							EquipItem( new LeafTunic() );

						break;
					}
				case 4: // Necromancer
					{
						Container regs = new BagOfNecroReagents( 50 );

						PackItem( regs );

						regs.LootType = LootType.Regular;

						if ( m.Race == Race.Human )
						{
							EquipItem( new BoneHelm() );
							EquipItem( new BoneHarvester() );
							EquipItem( NecroHue( new LeatherChest() ) );
							EquipItem( NecroHue( new LeatherArms() ) );
							EquipItem( NecroHue( new LeatherGloves() ) );
							EquipItem( NecroHue( new LeatherGorget() ) );
							EquipItem( NecroHue( new LeatherLegs() ) );
							EquipItem( NecroHue( new Skirt() ) );
							EquipItem( new Sandals( 0x8FD ) );
						}
						else if ( m.Race == Race.Elf )
						{
							EquipItem( new BoneHelm() );
							EquipItem( new ElvenMachete() );
							EquipItem( NecroHue( new LeafTunic() ) );
							EquipItem( NecroHue( new LeafArms() ) );
							EquipItem( NecroHue( new LeafGloves() ) );
							EquipItem( NecroHue( new LeafGorget() ) );
							EquipItem( NecroHue( new LeafGorget() ) );
							EquipItem( NecroHue( new ElvenPants() ) );	//TODO: Verify the pants
							EquipItem( new ElvenBoots() );
						}

						Spellbook book = new NecromancerSpellbook( (ulong) 0x8981 ); // animate dead, evil omen, pain spike, summon familiar, wraith form

						PackItem( book );

						book.LootType = LootType.Blessed;

						addSkillItems = false;

						break;
					}
				case 5: // Paladin
					{
						if ( m.Race == Race.Human )
						{
							EquipItem( new Broadsword() );
							EquipItem( new Helmet() );
							EquipItem( new PlateGorget() );
							EquipItem( new RingmailArms() );
							EquipItem( new RingmailChest() );
							EquipItem( new RingmailLegs() );
							EquipItem( new ThighBoots( 0x748 ) );
							EquipItem( new Cloak( 0xCF ) );
							EquipItem( new BodySash( 0xCF ) );
						}
						else if ( m.Race == Race.Elf )
						{
							EquipItem( new ElvenMachete() );
							EquipItem( new WingedHelm() );
							EquipItem( new LeafGorget() );
							EquipItem( new LeafArms() );
							EquipItem( new LeafTunic() );
							EquipItem( new LeafLeggings() );
							EquipItem( new ElvenBoots() );	//Verify hue
						}

						Spellbook book = new BookOfChivalry( (ulong) 0x3FF );

						PackItem( book );

						book.LootType = LootType.Blessed;

						break;
					}
				case 6: // Samurai
					{
						addSkillItems = false;
						EquipItem( new HakamaShita( 0x2C3 ) );
						EquipItem( new Hakama( 0x2C3 ) );
						EquipItem( new SamuraiTabi( 0x2C3 ) );
						EquipItem( new TattsukeHakama( 0x22D ) );
						EquipItem( new Bokuto() );

						if ( m.Race == Race.Elf )
							EquipItem( new RavenHelm() );

						PackItem( new Scissors() );
						PackItem( new Bandage( 50 ) );

						Spellbook book = new BookOfBushido();
						PackItem( book );
						break;
					}
				case 7: // Ninja
					{
						addSkillItems = false;

						if ( m.Race != Race.Gargoyle )
							EquipItem( new Kasa() );

						int[] hues = new int[] { 0x1A8, 0xEC, 0x99, 0x90, 0xB5, 0x336, 0x89 };
						//TODO: Verify that's ALL the hues for that above.

						EquipItem( new TattsukeHakama( hues[Utility.Random( hues.Length )] ) );

						EquipItem( new HakamaShita( 0x2C3 ) );
						EquipItem( new NinjaTabi( 0x2C3 ) );

						if ( m.Race == Race.Human )
							EquipItem( new Tekagi() );
						else if ( m.Race == Race.Elf )
							EquipItem( new AssassinSpike() );

						PackItem( new SmokeBomb() );

						Spellbook book = new BookOfNinjitsu();
						PackItem( book );

						break;
					}
			}

			for ( int i = 0; i < skills.Length; ++i )
			{
				SkillNameValue snv = skills[i];

				if ( snv.Value > 0 && snv.Name != SkillName.Stealth && snv.Name != SkillName.RemoveTrap )
				{
					Skill skill = m.Skills[snv.Name];

					if ( skill != null )
					{
						skill.BaseFixedPoint = snv.Value * 10;

						if ( addSkillItems )
							AddSkillItems( m, snv.Name );
					}
				}
			}
		}

		private static void EquipItem( Item item )
		{
			EquipItem( item, false );
		}

		private static void EquipItem( Item item, bool mustEquip )
		{
			if ( m_Mobile != null && m_Mobile.EquipItem( item ) )
				return;

			Container pack = m_Mobile.Backpack;

			if ( !mustEquip && pack != null )
				pack.DropItem( item );
			else
				item.Delete();
		}

		private static void PackItem( Item item )
		{
			Container pack = m_Mobile.Backpack;

			if ( pack != null )
				pack.DropItem( item );
			else
				item.Delete();
		}

		private static void PackInstrument()
		{
			switch ( Utility.Random( 6 ) )
			{
				case 0:
					PackItem( new Drums() );
					break;
				case 1:
					PackItem( new Harp() );
					break;
				case 2:
					PackItem( new LapHarp() );
					break;
				case 3:
					PackItem( new Lute() );
					break;
				case 4:
					PackItem( new Tambourine() );
					break;
				case 5:
					PackItem( new TambourineTassel() );
					break;
			}
		}

		private static void PackScroll( int circle )
		{
			switch ( Utility.Random( 8 ) * ( circle * 8 ) )
			{
				case 0:
					PackItem( new ClumsyScroll() );
					break;
				case 1:
					PackItem( new CreateFoodScroll() );
					break;
				case 2:
					PackItem( new FeeblemindScroll() );
					break;
				case 3:
					PackItem( new HealScroll() );
					break;
				case 4:
					PackItem( new MagicArrowScroll() );
					break;
				case 5:
					PackItem( new NightSightScroll() );
					break;
				case 6:
					PackItem( new ReactiveArmorScroll() );
					break;
				case 7:
					PackItem( new WeakenScroll() );
					break;
				case 8:
					PackItem( new AgilityScroll() );
					break;
				case 9:
					PackItem( new CunningScroll() );
					break;
				case 10:
					PackItem( new CureScroll() );
					break;
				case 11:
					PackItem( new HarmScroll() );
					break;
				case 12:
					PackItem( new MagicTrapScroll() );
					break;
				case 13:
					PackItem( new MagicUnTrapScroll() );
					break;
				case 14:
					PackItem( new ProtectionScroll() );
					break;
				case 15:
					PackItem( new StrengthScroll() );
					break;
				case 16:
					PackItem( new BlessScroll() );
					break;
				case 17:
					PackItem( new FireballScroll() );
					break;
				case 18:
					PackItem( new MagicLockScroll() );
					break;
				case 19:
					PackItem( new PoisonScroll() );
					break;
				case 20:
					PackItem( new TelekinisisScroll() );
					break;
				case 21:
					PackItem( new TeleportScroll() );
					break;
				case 22:
					PackItem( new UnlockScroll() );
					break;
				case 23:
					PackItem( new WallOfStoneScroll() );
					break;
			}
		}

		private static Item NecroHue( Item item )
		{
			item.Hue = 0x2C3;

			return item;
		}

		private static void AddSkillItems( Mobile m, SkillName skill )
		{
			switch ( skill )
			{
				case SkillName.Alchemy:
					{
						PackItem( new Bottle( 4 ) );
						PackItem( new MortarPestle() );

						int hue = Utility.RandomPinkHue();

						if ( m.Race == Race.Human )
						{
							EquipItem( new Robe( hue ) );
						}
						else if ( m.Race == Race.Elf )
						{
							if ( m.Female )
								EquipItem( new FemaleElvenRobe( hue ) );
							else
								EquipItem( new ElvenRobe( hue ) );
						}
						else if ( m.Race == Race.Gargoyle )
						{
							EquipItem( new GargishFancyRobe( hue ) );
						}

						break;
					}
				case SkillName.Anatomy:
					{
						PackItem( new Bandage( 3 ) );

						int hue = Utility.RandomYellowHue();

						if ( m.Race == Race.Human )
						{
							EquipItem( new Robe( hue ) );
						}
						else if ( m.Race == Race.Elf )
						{
							if ( m.Female )
								EquipItem( new FemaleElvenRobe( hue ) );
							else
								EquipItem( new ElvenRobe( hue ) );
						}
						else if ( m.Race == Race.Gargoyle )
						{
							EquipItem( new GargishFancyRobe( hue ) );
						}

						break;
					}
				case SkillName.AnimalLore:
					{
						int hue = Utility.RandomBlueHue();

						if ( m.Race == Race.Human )
						{
							EquipItem( new ShepherdsCrook() );
							EquipItem( new Robe( hue ) );
						}
						else if ( m.Race == Race.Elf )
						{
							EquipItem( new WildStaff() );

							if ( m.Female )
								EquipItem( new FemaleElvenRobe( hue ) );
							else
								EquipItem( new ElvenRobe( hue ) );
						}
						else if ( m.Race == Race.Gargoyle )
						{
							// TODO: Gargoyles Staff?

							EquipItem( new GargishFancyRobe( hue ) );
						}

						break;
					}
				case SkillName.Archery:
					{
						PackItem( new Arrow( 25 ) );

						if ( m.Race == Race.Human )
							EquipItem( new Bow() );
						else if ( m.Race == Race.Elf )
							EquipItem( new ElvenCompositeLongBow() );

						break;
					}
				case SkillName.ArmsLore:
					{
						if ( m.Race == Race.Human )
						{
							switch ( Utility.Random( 3 ) )
							{
								case 0:
									EquipItem( new Leafblade() );
									break;
								case 1:
									EquipItem( new RuneBlade() );
									break;
								case 2:
									EquipItem( new DiamondMace() );
									break;
							}
						}
						else if ( m.Race == Race.Elf )
						{
							switch ( Utility.Random( 3 ) )
							{
								case 0:
									EquipItem( new Kryss() );
									break;
								case 1:
									EquipItem( new Katana() );
									break;
								case 2:
									EquipItem( new Club() );
									break;
							}
						}

						break;
					}
				case SkillName.Begging:
					{
						if ( m.Race == Race.Human )
							EquipItem( new WildStaff() );
						else if ( m.Race == Race.Elf )
							EquipItem( new GnarledStaff() );

						break;
					}
				case SkillName.Blacksmith:
					{
						PackItem( new Tongs() );
						PackItem( new Pickaxe() );
						PackItem( new Pickaxe() );
						PackItem( new IronIngot( 50 ) );

						if ( m.Race != Race.Gargoyle )
							EquipItem( new HalfApron( Utility.RandomYellowHue() ) );

						break;
					}
				case SkillName.Bushido:
					{
						EquipItem( new Hakama() );
						EquipItem( new Kasa() );
						EquipItem( new BookOfBushido() );

						break;
					}
				case SkillName.Fletching:
					{
						PackItem( new Board( 14 ) );
						PackItem( new Feather( 5 ) );
						PackItem( new Shaft( 5 ) );

						break;
					}
				case SkillName.Camping:
					{
						PackItem( new Bedroll() );
						PackItem( new Kindling( 5 ) );

						break;
					}
				case SkillName.Carpentry:
					{
						PackItem( new Board( 10 ) );
						PackItem( new Saw() );

						if ( m.Race != Race.Gargoyle )
							EquipItem( new HalfApron( Utility.RandomYellowHue() ) );

						break;
					}
				case SkillName.Cartography:
					{
						PackItem( new BlankMap() );
						PackItem( new BlankMap() );
						PackItem( new BlankMap() );
						PackItem( new BlankMap() );
						PackItem( new Sextant() );

						break;
					}
				case SkillName.Cooking:
					{
						PackItem( new Kindling( 2 ) );
						PackItem( new RawLambLeg() );
						PackItem( new RawChickenLeg() );
						PackItem( new RawFishSteak() );
						PackItem( new SackFlour() );
						PackItem( new Pitcher( BeverageType.Water ) );

						break;
					}
				case SkillName.Chivalry:
					{
						PackItem( new BookOfChivalry( (ulong) 0x3FF ) );

						break;
					}
				case SkillName.DetectHidden:
					{
						if ( m.Race != Race.Gargoyle )
							EquipItem( new Cloak( 0x455 ) );

						break;
					}
				case SkillName.Discordance:
					{
						PackInstrument();

						break;
					}
				case SkillName.Fencing:
					{
						if ( m.Race == Race.Human )
							EquipItem( new Kryss() );
						else if ( m.Race == Race.Elf )
							EquipItem( new Leafblade() );

						break;
					}
				case SkillName.Fishing:
					{
						EquipItem( new FishingPole() );

						int hue = Utility.RandomYellowHue();

						if ( m.Race == Race.Human )
						{
							EquipItem( new FloppyHat( Utility.RandomYellowHue() ) );
						}
						else if ( m.Race == Race.Elf )
						{
							Item i = new Circlet();
							i.Hue = hue;
							EquipItem( i );
						}

						break;
					}
				case SkillName.Healing:
					{
						PackItem( new Bandage( 50 ) );
						PackItem( new Scissors() );

						break;
					}
				case SkillName.Herding:
					{
						if ( m.Race != Race.Gargoyle )
							EquipItem( new ShepherdsCrook() );

						break;
					}
				case SkillName.Hiding:
					{
						if ( m.Race != Race.Gargoyle )
							EquipItem( new Cloak( 0x455 ) );

						break;
					}
				case SkillName.Inscribe:
					{
						PackItem( new BlankScroll( 2 ) );
						PackItem( new BlueBook() );

						break;
					}
				case SkillName.ItemID:
					{
						if ( m.Race == Race.Human )
							EquipItem( new GnarledStaff() );
						else if ( m.Race == Race.Elf )
							EquipItem( new WildStaff() );

						break;
					}
				case SkillName.Lockpicking:
					{
						PackItem( new Lockpick( 20 ) );

						break;
					}
				case SkillName.Lumberjacking:
					{
						if ( m.Race != Race.Gargoyle )
							EquipItem( new Hatchet() );

						break;
					}
				case SkillName.Macing:
					{
						if ( m.Race == Race.Human )
							EquipItem( new Club() );
						else if ( m.Race == Race.Elf )
							EquipItem( new DiamondMace() );

						break;
					}
				case SkillName.Magery:
					{
						BagOfReagents regs = new BagOfReagents( 30 );

						PackItem( regs );

						regs.LootType = LootType.Regular;

						PackScroll( 0 );
						PackScroll( 1 );
						PackScroll( 2 );

						Spellbook book = new Spellbook( (ulong) 0x382A8C38 );

						EquipItem( book );

						book.LootType = LootType.Blessed;

						if ( m.Race == Race.Human )
						{
							EquipItem( new WizardsHat() );
							EquipItem( new Robe( Utility.RandomBlueHue() ) );
						}
						if ( m.Race == Race.Elf )
						{
							EquipItem( new Circlet() );

							if ( m.Female )
								EquipItem( new FemaleElvenRobe( Utility.RandomBlueHue() ) );
							else
								EquipItem( new ElvenRobe( Utility.RandomBlueHue() ) );
						}
						else if ( m.Race == Race.Gargoyle )
						{
							// TODO: hat?

							EquipItem( new GargishFancyRobe( Utility.RandomBlueHue() ) );
						}

						break;
					}
				case SkillName.Mining:
					{
						PackItem( new Pickaxe() );

						break;
					}
				case SkillName.Musicianship:
					{
						PackInstrument();

						break;
					}
				case SkillName.Necromancy:
					{
						Container regs = new BagOfNecroReagents( 50 );

						PackItem( regs );

						regs.LootType = LootType.Regular;

						break;
					}
				case SkillName.Ninjitsu:
					{
						if ( m.Race != Race.Gargoyle )
						{
							EquipItem( new Hakama( 0x2C3 ) );	//Only ninjas get the hued one.
							EquipItem( new Kasa() );
						}

						EquipItem( new BookOfNinjitsu() );

						break;
					}
				case SkillName.Parry:
					{
						if ( m.Race != Race.Gargoyle )
							EquipItem( new WoodenShield() );

						break;
					}
				case SkillName.Peacemaking:
					{
						PackInstrument();

						break;
					}
				case SkillName.Poisoning:
					{
						PackItem( new LesserPoisonPotion() );
						PackItem( new LesserPoisonPotion() );

						break;
					}
				case SkillName.Provocation:
					{
						PackInstrument();

						break;
					}
				case SkillName.Snooping:
					{
						PackItem( new Lockpick( 20 ) );

						break;
					}
				case SkillName.SpiritSpeak:
					{
						if ( m.Race != Race.Gargoyle )
							EquipItem( new Cloak( 0x455 ) );

						break;
					}
				case SkillName.Stealing:
					{
						PackItem( new Lockpick( 20 ) );

						break;
					}
				case SkillName.Swords:
					{
						if ( m.Race == Race.Human )
							EquipItem( new Katana() );
						else if ( m.Race == Race.Elf )
							EquipItem( new RuneBlade() );

						break;
					}
				case SkillName.Tactics:
					{
						if ( m.Race == Race.Human )
							EquipItem( new Katana() );
						else if ( m.Race == Race.Elf )
							EquipItem( new RuneBlade() );

						break;
					}
				case SkillName.Tailoring:
					{
						PackItem( new BoltOfCloth() );
						PackItem( new SewingKit() );

						break;
					}
				case SkillName.Tracking:
					{
						if ( m_Mobile != null )
						{
							Item shoes = m_Mobile.FindItemOnLayer( Layer.Shoes );

							if ( shoes != null )
								shoes.Delete();
						}

						int hue = Utility.RandomYellowHue();

						if ( m.Race == Race.Human )
							EquipItem( new Boots( hue ) );
						else if ( m.Race == Race.Elf )
							EquipItem( new ElvenBoots( hue ) );

						if ( m.Race != Race.Gargoyle )
							EquipItem( new SkinningKnife() );

						break;
					}
				case SkillName.Veterinary:
					{
						PackItem( new Bandage( 5 ) );
						PackItem( new Scissors() );

						break;
					}
				case SkillName.Wrestling:
					{
						if ( m.Race == Race.Human )
							EquipItem( new LeatherGloves() );
						else if ( m.Race == Race.Elf )
							EquipItem( new LeafGloves() );

						break;
					}
			}
		}
	}
}