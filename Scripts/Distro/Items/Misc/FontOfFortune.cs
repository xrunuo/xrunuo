using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Spells.Necromancy;
using Server.Spells.Fourth;
using Server.Spells.Chivalry;

namespace Server.Items
{
	public class FontOfFortune : BaseAddon
	{
		public override int LabelNumber { get { return 1113618; } } // Font of Fortune

		[Constructable]
		public FontOfFortune()
		{
			int itemID = 0x1731;

			AddComponent( new FontOfFortuneAddonComponent( itemID++ ), -2, +1, 0 );
			AddComponent( new FontOfFortuneAddonComponent( itemID++ ), -1, +1, 0 );
			AddComponent( new FontOfFortuneAddonComponent( itemID++ ), +0, +1, 0 );
			AddComponent( new FontOfFortuneAddonComponent( itemID++ ), +1, +1, 0 );

			AddComponent( new FontOfFortuneAddonComponent( itemID++ ), +1, +0, 0 );
			AddComponent( new FontOfFortuneAddonComponent( itemID++ ), +1, -1, 0 );
			AddComponent( new FontOfFortuneAddonComponent( itemID++ ), +1, -2, 0 );

			AddComponent( new FontOfFortuneAddonComponent( itemID++ ), +0, -2, 0 );
			AddComponent( new FontOfFortuneAddonComponent( itemID++ ), +0, -1, 0 );
			AddComponent( new FontOfFortuneAddonComponent( itemID++ ), +0, +0, 0 );

			AddComponent( new FontOfFortuneAddonComponent( itemID++ ), -1, +0, 0 );
			AddComponent( new FontOfFortuneAddonComponent( itemID++ ), -2, +0, 0 );

			AddComponent( new FontOfFortuneAddonComponent( itemID++ ), -2, -1, 0 );
			AddComponent( new FontOfFortuneAddonComponent( itemID++ ), -1, -1, 0 );

			AddComponent( new FontOfFortuneAddonComponent( itemID++ ), -1, -2, 0 );
			AddComponent( new FontOfFortuneAddonComponent( ++itemID ), -2, -2, 0 );
		}

		public FontOfFortune( Serial serial )
			: base( serial )
		{
		}

		#region Blessings
		private static readonly TimeSpan BlessingDuration = TimeSpan.FromHours( 1.0 );
		private static readonly int StatBoost = 10;

		public enum BlessingType
		{
			Strength,
			Dexterity,
			Intelligence,
			Luck,
			Protection
		}

		private static Dictionary<Mobile, BlessingType> m_Blessings = new Dictionary<Mobile, BlessingType>();

		public static bool HasAnyBlessing( Mobile m )
		{
			return m_Blessings.ContainsKey( m );
		}

		public static bool HasBlessing( Mobile m, BlessingType b )
		{
			if ( m_Blessings.ContainsKey( m ) )
			{
				BlessingType blessing = m_Blessings[m];

				if ( blessing == b )
					return true;
			}

			return false;
		}

		private delegate bool CheckForBalm( Mobile m );

		private class BlessingDefinition
		{
			private BlessingType m_Type;
			private int m_Cliloc;

			public BlessingDefinition( BlessingType type, int cliloc )
			{
				m_Type = type;
				m_Cliloc = cliloc;
			}

			public virtual void Apply( Mobile m )
			{
				m_Blessings.Add( m, m_Type );

				m.FixedParticles( 0x376A, 1, 32, 5005, EffectLayer.Waist );
				m.SendLocalizedMessage( m_Cliloc );

				Timer.DelayCall( BlessingDuration, new TimerStateCallback<Mobile>( Remove ), m );
			}

			public virtual void Remove( Mobile m )
			{
				m.SendLocalizedMessage( 1113370 ); // The Font of Fortune's blessing has faded.
				m_Blessings.Remove( m );
			}
		}

		private class StatBoostDefinition : BlessingDefinition
		{
			private StatType m_Stat;
			private CheckForBalm m_HasBalm;

			public StatBoostDefinition( BlessingType type, int cliloc, StatType stat, CheckForBalm hasBalm )
				: base( type, cliloc )
			{
				m_Stat = stat;
				m_HasBalm = hasBalm;
			}

			public override void Apply( Mobile m )
			{
				if ( m_HasBalm( m ) )
				{
					BalmOrLotion.IncreaseDuration( m );
					m.SendLocalizedMessage( 1113372 ); // The duration of your balm has been increased by an hour!
				}
				else
				{
					base.Apply( m );

					m.AddStatMod( new StatMod( m_Stat, "[FontOfFortune] Stat Offset", StatBoost, BlessingDuration ) );
				}
			}

			public override void Remove( Mobile m )
			{
				base.Remove( m );

				m.RemoveStatMod( "[FontOfFortune] Stat Offset" );
			}
		}

		private static BlessingDefinition[] m_BlessingDefs = new BlessingDefinition[]
			{
				new StatBoostDefinition( BlessingType.Strength,		1113373, StatType.Str, BalmOfStrength.UnderEffect ),
				new StatBoostDefinition( BlessingType.Dexterity,	1113374, StatType.Dex, BalmOfSwiftness.UnderEffect ),
				new StatBoostDefinition( BlessingType.Intelligence,	1113371, StatType.Int, BalmOfWisdom.UnderEffect ),
				
				new BlessingDefinition( BlessingType.Luck,			1079551 ),
				new BlessingDefinition( BlessingType.Protection,	1113375 )
			};

		public static void Bless( Mobile m )
		{
			BlessingDefinition def = m_BlessingDefs[Utility.Random( m_BlessingDefs.Length )];
			def.Apply( m );
		}
		#endregion

		#region Rewards
		private static RewardInfo[] m_Rewards = new RewardInfo[]
			{
				new RewardInfo( 1113376, typeof( SolesOfProvidence ) ),
				new RewardInfo( 1113378, typeof( GemologistsSatchel ) ),
				new RewardInfo( 1031698, typeof( EnchantedEssence ) ),
				new RewardInfo( 1031699, typeof( RelicFragment ) )	
			};

		private class RewardInfo
		{
			private int m_Cliloc;
			private Type m_Type;

			public int Cliloc { get { return m_Cliloc; } }
			public Type Type { get { return m_Type; } }

			public RewardInfo( int cliloc, Type type )
			{
				m_Cliloc = cliloc;
				m_Type = type;
			}
		}

		public static void Award( Mobile to )
		{
			RewardInfo info = m_Rewards[Utility.Random( m_Rewards.Length )];

			Item reward = (Item) Activator.CreateInstance( info.Type );

			to.PlaceInBackpack( reward );
			to.SendLocalizedMessage( 1074360, String.Format( "#{0}", info.Cliloc.ToString() ) ); // You receive a reward: ~1_REWARD~
		}
		#endregion

		#region Healing & Resurrecting
		public override bool HandlesOnMovement { get { return true; } }

		public override void OnMovement( Mobile m, Point3D oldLocation )
		{
			if ( this.InRange( m, 2 ) && !this.InRange( oldLocation, 2 ) && !m_Table.ContainsKey( m ) )
			{
				if ( !m.Alive )
				{
					m.CloseGump( typeof( ResurrectGump ) );
					m.SendGump( new FoFResurrectGump( m ) );
				}
				else if ( m.Poisoned || IsCursed( m ) )
				{
					RemoveCurseSpell.DoGraphicalEffect( m );
					RemoveCurseSpell.DoRemoveCurses( m );

					if ( m.Poisoned && m.CurePoison( m ) )
						m.SendLocalizedMessage( 1010059 ); // You have been cured of all poisons.

					AddCooldown( m );
				}
				else if ( m.Hits < ( m.HitsMax / 2.0 ) )
				{
					m.Hits = m.HitsMax;

					m.FixedParticles( 0x376A, 9, 32, 5030, EffectLayer.Waist );
					m.PlaySound( 0x202 );

					m.SendLocalizedMessage( 1113365 ); // Your wounds have been mended.

					AddCooldown( m );
				}
			}
		}

		public static void AddCooldown( Mobile m )
		{
			m_Table[m] = Timer.DelayCall( TimeSpan.FromMinutes( 10.0 ), new TimerCallback(
					delegate
					{
						m_Table.Remove( m );
					} ) );
		}

		private bool IsCursed( Mobile m )
		{
			for ( int i = 0; i < m_Effects.Length; i++ )
			{
				UnderEffect effect = m_Effects[i];

				if ( effect( m ) )
					return true;
			}

			return false;
		}

		private delegate bool UnderEffect( Mobile m );

		private static UnderEffect[] m_Effects = new UnderEffect[]
			{
				CorpseSkinSpell.UnderEffect,
				StrangleSpell.UnderEffect,
				EvilOmenSpell.UnderEffect,
				BloodOathSpell.UnderEffect,
				CurseSpell.UnderEffect,
				MortalStrike.IsWounded
			};

		private static Dictionary<Mobile, Timer> m_Table = new Dictionary<Mobile, Timer>();
		#endregion

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}

	public class FoFResurrectGump : ResurrectGump
	{
		public FoFResurrectGump( Mobile owner )
			: base( owner, ResurrectMessage.Generic )
		{
		}

		public override void OnResponse( GameClient state, RelayInfo info )
		{
			Mobile from = state.Mobile;

			from.CloseGump( typeof( ResurrectGump ) );

			if ( info.ButtonID == 2 )
			{
				from.PlaySound( 0x214 );
				from.Resurrect();

				from.Hits = (int) ( from.HitsMax * 0.8 );

				FontOfFortune.AddCooldown( from );
			}
		}
	}

	public class FontOfFortuneAddonComponent : AddonComponent
	{
		public override int LabelNumber { get { return 1113618; } } // Font of Fortune

		[Constructable]
		public FontOfFortuneAddonComponent( int itemID )
			: base( itemID )
		{

		}

		public FontOfFortuneAddonComponent( Serial serial )
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