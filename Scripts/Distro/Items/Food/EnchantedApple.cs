using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Network;
using Server.Items;
using Server.Spells.Necromancy;
using Server.Spells.Fourth;
using Server.ContextMenus;
using Server.Spells.Mysticism;
using Server.Engines.BuffIcons;

namespace Server.Items
{
	public class EnchantedApple : Apple, ICommodity
	{
		public static readonly TimeSpan Cooldown = TimeSpan.FromSeconds( 30.0 );

		public override int LabelNumber { get { return 1032248; } } // enchanted apple

		[Constructable]
		public EnchantedApple()
		{
			Weight = 1.0;
			Hue = 1160;
			Stackable = true;
		}

		public EnchantedApple( Serial serial )
			: base( serial )
		{
		}

		private static string[] StatModNames = new string[] {
				"[Magic] Str Malus",
				"[Magic] Dex Malus",
				"[Magic] Int Malus"
			};

		public static int GetCursePower( Mobile m )
		{
			int power = 0;

			// 1st circle debuffs
			foreach ( string statModName in StatModNames )
			{
				if ( m.GetStatMod( statModName ) != null )
					power += 1;
			}

			// 3rd circle debuffs
			if ( EvilOmenSpell.UnderEffect( m ) )
				power += 3;

			if ( BloodOathSpell.UnderEffect( m ) )
				power += 3;

			if ( CorpseSkinSpell.UnderEffect( m ) )
				power += 3;

			if ( MindRotSpell.HasMindRotScalar( m ) )
				power += 3;

			if ( SleepSpell.IsSlept( m ) )
				power += 3;

			// 4th circle debuffs
			if ( CurseSpell.UnderEffect( m ) )
				power += 4;

			// 6th circle debuffs
			if ( StrangleSpell.UnderEffect( m ) )
				power += 6;

			// 7th circle debuffs
			if ( SpellPlagueSpell.UnderEffect( m ) )
				power += 7;

			if ( MortalStrike.IsWounded( m ) )
				power += 7;

			return power;
		}

		public static void RemoveCurses( Mobile m )
		{
			// play the sound
			m.PlaySound( 0xF6 );
			m.PlaySound( 0x1F7 );

			// do the effects
			m.FixedParticles( 0x3709, 1, 30, 9963, 13, 3, EffectLayer.Head );

			IEntity from = new DummyEntity( Serial.Zero, new Point3D( m.X, m.Y, m.Z - 10 ), m.Map );
			IEntity to = new DummyEntity( Serial.Zero, new Point3D( m.X, m.Y, m.Z + 50 ), m.Map );
			Effects.SendMovingParticles( from, to, 0x2255, 1, 0, false, false, 13, 3, 9501, 1, 0, EffectLayer.Head, 0x100 );

			// remove stat mods
			StatMod mod;

			foreach ( string statModName in StatModNames )
			{
				mod = m.GetStatMod( statModName );
				if ( mod != null && mod.Offset < 0 )
					m.RemoveStatMod( statModName );
			}

			m.Paralyzed = false;

			EvilOmenSpell.CheckEffect( m );
			StrangleSpell.RemoveCurse( m );
			CorpseSkinSpell.RemoveCurse( m );
			CurseSpell.RemoveEffect( m );
			MortalStrike.EndWound( m );
			BloodOathSpell.EndEffect( m );
			SpellPlagueSpell.RemoveEffect( m );
			SleepSpell.RemoveEffect( m );
			MindRotSpell.ClearMindRotScalar( m );

			BuffInfo.RemoveBuff( m, BuffIcon.Clumsy );
			BuffInfo.RemoveBuff( m, BuffIcon.FeebleMind );
			BuffInfo.RemoveBuff( m, BuffIcon.Weaken );
			BuffInfo.RemoveBuff( m, BuffIcon.MassCurse );
			BuffInfo.RemoveBuff( m, BuffIcon.Curse );
			BuffInfo.RemoveBuff( m, BuffIcon.EvilOmen );
			BuffInfo.RemoveBuff( m, BuffIcon.MortalStrike );
			BuffInfo.RemoveBuff( m, BuffIcon.Sleep );
			BuffInfo.RemoveBuff( m, BuffIcon.MassSleep );
			BuffInfo.RemoveBuff( m, BuffIcon.Mindrot );
		}

		private static Dictionary<Mobile, DateTime> m_CooldownTable = new Dictionary<Mobile, DateTime>();

		public override bool Eat( Mobile m )
		{
			if ( !CheckCooldown( m ) )
				return false;

			double successChance = 1.0 - ( 0.05 * GetCursePower( m ) );

			if ( successChance < Utility.RandomDouble() )
			{
				// The apple was not strong enough to purify you.
				m.SendLocalizedMessage( 1150174 );
			}
			else
			{
				// A tasty bite of the enchanted apple lifts all curses from your soul.
				m.SendLocalizedMessage( 1074846 );

				m_CooldownTable.Add( m, DateTime.Now + Cooldown );

				RemoveCurses( m );

				// Play a random "eat" sound
				m.PlaySound( Utility.Random( 0x3A, 3 ) );

				m.Animate( 6 );

				if ( Poison != null )
					m.ApplyPoison( Poisoner, Poison );
			}

			Consume();

			return true;
		}

		private static bool CheckCooldown( Mobile m )
		{
			if ( m_CooldownTable.ContainsKey( m ) )
			{
				DateTime cooldownEnd = m_CooldownTable[m];

				if ( cooldownEnd > DateTime.Now )
				{
					// You must wait ~1_seconds~ seconds before you can use this item.
					m.SendLocalizedMessage( 1079263, ( (int) ( cooldownEnd - DateTime.Now ).TotalSeconds + 1 ).ToString() );

					return false;
				}
				else
				{
					m_CooldownTable.Remove( m );
				}
			}

			return true;
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

			if ( !Stackable )
				Stackable = true;
		}
	}
}
