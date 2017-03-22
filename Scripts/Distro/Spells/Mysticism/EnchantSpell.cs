using System;
using System.Collections.Generic;
using System.Linq;
using Server.Network;
using Server.Gumps;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;

namespace Server.Spells.Mysticism
{
	public class EnchantSpell : MysticSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
			"Enchant", "In Ort Ylem",
			-1,
			9002,
			Reagent.SpidersSilk,
			Reagent.MandrakeRoot,
			Reagent.SulfurousAsh
		);

		public override TimeSpan CastDelayBase
		{
			get { return TimeSpan.FromSeconds( 1.0 ); }
		}

		public override double RequiredSkill
		{
			get { return 8.0; }
		}

		public override int RequiredMana
		{
			get { return 6; }
		}

		public EnchantSpell( Mobile caster, Item scroll )
			: base( caster, scroll, m_Info )
		{
		}

		public override bool ClearHandsOnCast
		{
			get { return false; }
		}

		public override bool CheckCast()
		{
			if ( !base.CheckCast() )
				return false;

			var weapon = Caster.Weapon as BaseWeapon;

			if ( weapon == null || weapon is Fists )
			{
				Caster.SendLocalizedMessage( 1060179 ); // You must be wielding a weapon to use this ability!
				return false;
			}
			else if ( weapon.Enchanted )
			{
				Caster.SendLocalizedMessage( 1005559 ); // This spell is already in effect.
				return false;
			}
			else if ( HasHitSpellEffect( weapon ) )
			{
				Caster.SendLocalizedMessage( 1080127 ); // This weapon already has a hit spell effect and cannot be enchanted.
				return false;
			}

			return true;
		}

		private static bool HasHitSpellEffect( IWeapon weapon )
		{
			return Entries.Any( entry => weapon.WeaponAttributes[entry.Attribute] != 0 );
		}

		public static EnchantEntry[] Entries { get; } =
		{
			new EnchantEntry( WeaponAttribute.HitDispel, 1079702 ),
			new EnchantEntry( WeaponAttribute.HitFireball, 1079703 ),
			new EnchantEntry( WeaponAttribute.HitHarm, 1079704 ),
			new EnchantEntry( WeaponAttribute.HitLightning, 1079705 ),
			new EnchantEntry( WeaponAttribute.HitMagicArrow, 1079706 )
		};

		public void OnGumpResponse( int buttonId )
		{
			Reset( false );

			if ( buttonId >= 1 && buttonId <= Entries.Length )
			{
				if ( !CheckCast() )
					return;

				var weapon = Caster.Weapon as BaseWeapon;

				var attribute = Entries[buttonId - 1].Attribute;

				var skillBase = GetBaseSkill( Caster );
				var skillBonus = GetBoostSkill( Caster );

				var bonus = (int) ( ( skillBase + skillBonus ) / 4.8 );
				weapon.WeaponAttributes[attribute] = bonus;

				var spellChanneling = weapon.Attributes.SpellChanneling == 0 && skillBase >= 80.0 && skillBonus >= 80.0;
				if ( spellChanneling )
					weapon.Attributes.SpellChanneling = 1;

				Caster.PlaySound( 0x64E );
				Effects.SendTargetParticles( Caster, 0x3728, 1, 13, 0x489, 7, 0x1596, EffectLayer.Head, 0 );
				Effects.SendTargetParticles( Caster, 0x3779, 1, 15, 0x3F, 7, 0x251E, EffectLayer.Head, 0 );

				var seconds = (int) ( ( skillBase + skillBonus ) * 2 / 3 );
				var timer = Timer.DelayCall( TimeSpan.FromSeconds( seconds ),
					new TimerStateCallback<BaseWeapon>( RemoveEnchantContext ), weapon );

				weapon.EnchantContext = new EnchantContext( Caster, attribute, spellChanneling, timer );
			}
			else
			{
				Caster.SendLocalizedMessage( 1080132 ); // You decide not to enchant your weapon.
			}
		}

		public override void OnCast()
		{
			if ( CheckSequence() )
			{
				Reset( false );
				Caster.SendGump( new EnchantGump( this ) );

				m_ResetTable[Caster] = Timer.DelayCall( TimeSpan.FromSeconds( 30.0 ), new TimerCallback( Reset ) );
			}

			FinishSequence();
		}

		private void Reset()
		{
			Reset( true );
		}

		private void Reset( bool message )
		{
			if ( !m_ResetTable.ContainsKey( Caster ) )
				return;

			Caster.CloseGump( typeof( EnchantGump ) );

			m_ResetTable[Caster].Stop();
			m_ResetTable.Remove( Caster );

			if ( message )
				Caster.SendLocalizedMessage( 1080132 ); // You decide not to enchant your weapon.
		}

		private static Dictionary<Mobile, Timer> m_ResetTable = new Dictionary<Mobile, Timer>();

		public static bool UnderEffect( Mobile m )
		{
			return m.Weapon is BaseWeapon && ( (BaseWeapon) m.Weapon ).Enchanted;
		}

		public static void RemoveEffect( Mobile m )
		{
			if ( m.Weapon is BaseWeapon )
				RemoveEnchantContext( m.Weapon as BaseWeapon, false );
		}

		public static void RemoveEnchantContext( BaseWeapon weapon )
		{
			RemoveEnchantContext( weapon, true );
		}

		public static void RemoveEnchantContext( BaseWeapon weapon, bool effects )
		{
			if ( !weapon.Enchanted )
				return;

			var context = weapon.EnchantContext;

			context.Timer?.Stop();

			weapon.WeaponAttributes[context.Attribute] = 0;

			if ( context.SpellChanneling )
				weapon.Attributes.SpellChanneling = 0;

			if ( effects )
				context.Owner.PlaySound( 0x1ED );

			weapon.EnchantContext = null;
		}
	}

	public class EnchantEntry
	{
		public WeaponAttribute Attribute { get; }
		public int Cliloc { get; }

		public EnchantEntry( WeaponAttribute attribute, int cliloc )
		{
			Attribute = attribute;
			Cliloc = cliloc;
		}
	}

	public class EnchantGump : Gump
	{
		public override int TypeID
		{
			get { return 0xF3E91; }
		}

		private readonly EnchantSpell m_Spell;

		public EnchantGump( EnchantSpell spell )
			: base( 200, 100 )
		{
			m_Spell = spell;

			AddPage( 0 );

			AddBackground( 10, 10, 250, 178, 0x2436 );
			AddAlphaRegion( 20, 20, 230, 158 );
			AddImage( 220, 20, 0x28E0 );
			AddImage( 220, 72, 0x28E0 );
			AddImage( 220, 124, 0x28E0 );
			AddItem( 188, 16, 0x1AE3 );
			AddItem( 198, 168, 0x1AE1 );
			AddItem( 8, 15, 0x1AE2 );
			AddItem( 2, 168, 0x1AE0 );

			AddHtmlLocalized( 30, 26, 200, 20, 1080133, 0xEF9, false, false ); // Select Enchant

			for ( var i = 0; i < EnchantSpell.Entries.Length; i++ )
			{
				AddButton( 27, 53 + ( 21 * i ), 0x25E6, 0x25E7, i + 1, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 50, 51 + ( 21 * i ), 150, 20, EnchantSpell.Entries[i].Cliloc, 0xEF9, false, false );
			}
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			m_Spell?.OnGumpResponse( info.ButtonID );
		}
	}

	public class EnchantContext
	{
		public Mobile Owner { get; }
		public WeaponAttribute Attribute { get; }
		public bool SpellChanneling { get; }
		public Timer Timer { get; }

		public EnchantContext( Mobile owner, WeaponAttribute attribute, bool spellChanneling, Timer timer )
		{
			Owner = owner;
			Attribute = attribute;
			SpellChanneling = spellChanneling;
			Timer = timer;
		}

		public EnchantContext( GenericReader reader, BaseWeapon weapon )
		{
			Owner = reader.ReadMobile();
			Attribute = (WeaponAttribute) reader.ReadInt();
			SpellChanneling = reader.ReadBool();
			Timer = Timer.DelayCall( reader.ReadTimeSpan(), EnchantSpell.RemoveEnchantContext, weapon );

			weapon.EnchantContext = this;
		}

		public void Serialize( GenericWriter writer )
		{
			writer.Write( (Mobile) Owner );
			writer.Write( (int) Attribute );
			writer.Write( (bool) SpellChanneling );
			writer.Write( (TimeSpan) ( Timer.Next - DateTime.Now ) );
		}
	}
}
