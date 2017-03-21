using System;
using System.Collections;
using Server;
using Server.Engines.BuffIcons;
using Server.Gumps;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Spells.Fifth;
using Server.Spells.Seventh;
using Server.Events;

namespace Server.Spells.Ninjitsu
{
	public class AnimalForm : NinjaSpell
	{
		public static void Initialize()
		{
			EventSink.Login += new LoginEventHandler( OnLogin );
		}

		public static void OnLogin( LoginEventArgs e )
		{
			AnimalFormContext context = AnimalForm.GetContext( e.Mobile );

			if ( context != null && context.SpeedBoost )
				e.Mobile.InvalidateSpeed();
		}

		private static SpellInfo m_Info = new SpellInfo(
				"Animal Form", null,
				-1,
				9002
			);

		public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 1.0 ); } }

		public override double RequiredSkill { get { return 0.0; } }
		public override int RequiredMana { get { return 10; } }

		public override int CastRecoveryBase { get { return 10; } }

		public override bool BlockedByAnimalForm { get { return false; } }

		public AnimalForm( Mobile caster, Item scroll )
			: base( caster, scroll, m_Info )
		{
		}

		public override bool CheckCast()
		{
			if ( !Caster.CanBeginAction( typeof( PolymorphSpell ) ) )
			{
				Caster.SendLocalizedMessage( 1061628 ); // You can't do that while polymorphed.
				return false;
			}
			else if ( Necromancy.TransformationSpell.UnderTransformation( Caster ) )
			{
				Caster.SendLocalizedMessage( 1063219 ); // You cannot mimic an animal while in that form.
				return false;
			}
			else if ( !BaseMount.CheckMountAllowed( Caster, false ) )
			{
				Caster.SendLocalizedMessage( 1063108 ); // You cannot use this ability right now.
				return false;
			}
			else if ( Caster.Flying )
			{
				Caster.SendLocalizedMessage( 1113415 ); // You cannot use this ability while flying.
				return false;
			}

			return base.CheckCast();
		}

		public override bool CheckDisturb( DisturbType type, bool firstCircle, bool resistable )
		{
			return false;
		}

		public override void OnBeginCast()
		{
			base.OnBeginCast();

			Caster.FixedEffect( 0x37C4, 10, 14, 4, 3 );
		}

		public override bool CheckFizzle()
		{
			int mana = ScaleMana( RequiredMana );

			if ( Caster.Skills[CastSkill].Value < RequiredSkill )
			{
				Caster.SendLocalizedMessage( 1063352, RequiredSkill.ToString( "F1" ) ); // You need ~1_SKILL_REQUIREMENT~ Ninjitsu skill to perform that attack!
				return false;
			}
			else if ( Caster.Mana < mana )
			{
				Caster.SendLocalizedMessage( 1060174, mana.ToString() ); // You must have at least ~1_MANA_REQUIREMENT~ Mana to use this ability.
				return false;
			}

			return true;
		}

		public override void OnCast()
		{
			if ( !Caster.CanBeginAction( typeof( PolymorphSpell ) ) )
			{
				Caster.SendLocalizedMessage( 1061628 ); // You can't do that while polymorphed.
			}
			else if ( Necromancy.TransformationSpell.UnderTransformation( Caster ) )
			{
				Caster.SendLocalizedMessage( 1063219 ); // You cannot mimic an animal while in that form.
			}
			else if ( !BaseMount.CheckMountAllowed( Caster, false ) )
			{
				Caster.SendLocalizedMessage( 1063108 ); // You cannot use this ability right now.
			}
			else if ( !Caster.CanBeginAction( typeof( IncognitoSpell ) ) || ( Caster.IsBodyMod && GetContext( Caster ) == null ) )
			{
				DoFizzle();
			}
			else if ( CheckSequence() )
			{
				AnimalFormContext context = GetContext( Caster );

				if ( context != null )
				{
					RemoveContext( Caster, context, true );

					Effects.SendLocationParticles( EffectItem.Create( Caster.Location, Caster.Map, EffectItem.DefaultDuration ), 0x3728, 1, 13, 0x7F3 );

					ConsumeMana();
				}
				else
				{
					if ( Caster.IsPlayer )
					{
						Caster.CloseGump( typeof( AnimalFormGump ) );
						Caster.SendGump( new AnimalFormGump( Caster, m_Entries, this ) );
					}
					else
					{
						if ( Morph( Caster, GetLastAnimalForm( Caster ) ) == MorphResult.Fail )
							DoFizzle();
						else
							ConsumeMana();
					}
				}
			}

			FinishSequence();
		}

		private bool ConsumeMana()
		{
			int mana = ScaleMana( RequiredMana );

			if ( Caster.Mana < mana )
			{
				Caster.SendLocalizedMessage( 1060174, mana.ToString() ); // You must have at least ~1_MANA_REQUIREMENT~ Mana to use this ability.
				return false;
			}

			Caster.Mana -= mana;

			return true;
		}

		private static Hashtable m_LastAnimalForms = new Hashtable();

		public int GetLastAnimalForm( Mobile m )
		{
			if ( m_LastAnimalForms.Contains( m ) )
				return (int) m_LastAnimalForms[m];

			return -1;
		}

		public enum MorphResult
		{
			Success,
			Fail,
			NoSkill
		}

		public static MorphResult Morph( Mobile m, int entryID )
		{
			if ( entryID < 0 || entryID >= m_Entries.Length )
				return MorphResult.Fail;

			AnimalFormEntry entry = m_Entries[entryID];

			m_LastAnimalForms[m] = entryID;	// On OSI, it's the last /attempted/ one not the last succeeded one

			if ( m.Skills.Ninjitsu.Value < entry.ReqSkill )
			{
				string args = String.Format( "{0}\t{1}\t ", entry.ReqSkill.ToString( "F1" ), SkillName.Ninjitsu );
				m.SendLocalizedMessage( 1063013, args ); // You need at least ~1_SKILL_REQUIREMENT~ ~2_SKILL_NAME~ skill to use that ability.
				return MorphResult.NoSkill;
			}

			double minSkill = entry.ReqSkill - 12.5;
			double maxSkill = entry.ReqSkill + 37.5;

			if ( !m.CheckSkill( SkillName.Ninjitsu, minSkill, maxSkill ) )
				return MorphResult.Fail;

			BaseMount.Dismount( m );

			m.BodyMod = entry.BodyMod;

			if ( entry.HueMod >= 0 )
				m.HueMod = entry.HueMod;

			if ( entry.SpeedBoost )
				m.ForcedRun = true;

			SkillMod mod = null;

			if ( entry.StealthBonus )
			{
				mod = new DefaultSkillMod( SkillName.Stealth, true, 20.0 );
				mod.ObeyCap = true;
				m.AddSkillMod( mod );
			}

			#region Heritage Items
			/*
			else if ( entry.StealingBonus )
			{
				mod = new DefaultSkillMod( SkillName.Stealing, true, 10.0 );
				mod.ObeyCap = true;
				m.AddSkillMod( mod );
			}
			*/
			#endregion

			Timer timer = new AnimalFormTimer( m, entry.BodyMod, m.HueMod );
			timer.Start();

			AddContext( m, new AnimalFormContext( timer, mod, entry.SpeedBoost, entry.Type ) );

			Effects.SendLocationParticles( EffectItem.Create( m.Location, m.Map, EffectItem.DefaultDuration ), 0x3728, 1, 13, 0x7F3 );

			BuffInfo.AddBuff( m, new BuffInfo( BuffIcon.AnimalForm, 1075822, 1075823, String.Format( "{0}\t{1}", entry.ArticleCliloc, entry.FormCliloc ) ) );

			m.Target = null;

			return MorphResult.Success;
		}

		private static Hashtable m_Table = new Hashtable();

		public static void AddContext( Mobile m, AnimalFormContext context )
		{
			m_Table[m] = context;

			if ( context.Type == typeof( BakeKitsune ) || context.Type == typeof( GreyWolf ) )
				m.Hits += 20;
		}

		public static void RemoveContext( Mobile m )
		{
			RemoveContext( m, true );
		}

		public static void RemoveContext( Mobile m, bool resetGraphics )
		{
			AnimalFormContext context = GetContext( m );

			if ( context != null )
				RemoveContext( m, context, resetGraphics );
		}

		public static void RemoveContext( Mobile m, AnimalFormContext context, bool resetGraphics )
		{
			m_Table.Remove( m );

			if ( context.SpeedBoost )
				m.ForcedRun = false;

			SkillMod mod = context.Mod;

			if ( mod != null )
				m.RemoveSkillMod( mod );

			if ( context.Type == typeof( BakeKitsune ) || context.Type == typeof( GreyWolf ) )
				m.Hits -= 20;

			if ( resetGraphics )
			{
				m.HueMod = -1;
				m.BodyMod = 0;
			}

			BuffInfo.RemoveBuff( m, BuffIcon.AnimalForm );

			context.Timer.Stop();
		}

		public static AnimalFormContext GetContext( Mobile m )
		{
			return ( m_Table[m] as AnimalFormContext );
		}

		public static bool UnderTransformationNotTalisman( Mobile m )
		{
			AnimalFormContext context = GetContext( m );

			return ( context != null && !context.Type.IsSubclassOf( typeof( BaseFormTalisman ) ) );
		}

		public static bool UnderTransformation( Mobile m )
		{
			return ( GetContext( m ) != null );
		}

		public static bool UnderTransformation( Mobile m, Type type )
		{
			AnimalFormContext context = GetContext( m );

			return ( context != null && context.Type == type );
		}

		public class AnimalFormEntry
		{
			private Type m_Type;
			private TextDefinition m_Name;
			private int m_ItemID;
			private int m_Hue;
			private int m_Tooltip;
			private double m_ReqSkill;
			private int m_BodyMod;
			private Func<int> m_HueMod;
			private bool m_StealthBonus;
			private bool m_SpeedBoost;

			#region Heritage Items
			/*
			private int m_X, m_Y;
			private bool m_StealingBonus;

			public int X { get { return m_X; } }
			public int Y { get { return m_Y; } }
			public bool StealingBonus { get { return m_StealingBonus; } }
			*/
			#endregion

			private TextDefinition m_ArticleCliloc;
			private TextDefinition m_FormCliloc;
			private int m_TileArtWidth;
			private int m_TileArtHeight;

			public Type Type { get { return m_Type; } }
			public TextDefinition Name { get { return m_Name; } }
			public int ItemID { get { return m_ItemID; } }
			public int Hue { get { return m_Hue; } }
			public int Tooltip { get { return m_Tooltip; } }
			public double ReqSkill { get { return m_ReqSkill; } }
			public int BodyMod { get { return m_BodyMod; } }
			public int HueMod { get { return m_HueMod(); } }
			public bool StealthBonus { get { return m_StealthBonus; } }
			public bool SpeedBoost { get { return m_SpeedBoost; } }
			public TextDefinition ArticleCliloc { get { return m_ArticleCliloc; } }
			public TextDefinition FormCliloc { get { return m_FormCliloc; } }
			public int TileArtWidth { get { return m_TileArtWidth; } }
			public int TileArtHeight { get { return m_TileArtHeight; } }

			public AnimalFormEntry( Type type, TextDefinition name, int itemID, int hue, int tooltip, double reqSkill, int bodyMod, Func<int> hueMod, bool stealthBonus, bool speedBoost, TextDefinition articlecliloc, TextDefinition formcliloc, int tileartwidth, int tileartheight )
			{
				m_Type = type;
				m_Name = name;
				m_ItemID = itemID;
				m_Hue = hue;
				m_Tooltip = tooltip;
				m_ReqSkill = reqSkill;
				m_BodyMod = bodyMod;
				m_HueMod = hueMod;
				m_StealthBonus = stealthBonus;
				m_SpeedBoost = speedBoost;
				m_ArticleCliloc = articlecliloc;
				m_FormCliloc = formcliloc;
				m_TileArtWidth = tileartwidth;
				m_TileArtHeight = tileartheight;
			}

			public AnimalFormEntry( Type type, TextDefinition name, int itemID, int hue, int tooltip, double reqSkill, int bodyMod, int hueMod, bool stealthBonus, bool speedBoost, TextDefinition articlecliloc, TextDefinition formcliloc, int tileartwidth, int tileartheight )
				: this( type, name, itemID, hue, tooltip, reqSkill, bodyMod, () => hueMod, stealthBonus, speedBoost, articlecliloc, formcliloc, tileartwidth, tileartheight )
			{
			}
		}

		private static AnimalFormEntry[] m_Entries = new AnimalFormEntry[]
			{
				new AnimalFormEntry( typeof( Kirin ),        1029632,  9632,     0, 1070811, 99.0, 0x84,     0, false,  true,  "a", 1029632, 6,  10 ),
				new AnimalFormEntry( typeof( Unicorn ),      1018214,  9678,     0, 1070812, 99.0, 0x7A,     0, false,  true, "an", 1018214, 20, 10 ),
				new AnimalFormEntry( typeof( BakeKitsune ),  1030083, 10083,     0, 1070810, 85.0, 0xF6,     0, false,  true,  "a", 1030083, 15, 15 ),
				new AnimalFormEntry( typeof( GreyWolf ),     1028482,  9681, 0x905, 1070810, 85.0, 0x19, 0x905, false,  true,  "a", 1018120, 25, 10 ),
				new AnimalFormEntry( typeof( Llama ),        1028438,  8438,     0, 1070809, 70.0, 0xDC,     0, false,  true,  "a", 1028438, 15, 8  ),
				new AnimalFormEntry( typeof( ForestOstard ), 1018273,  8503, 0x8A4, 1070809, 70.0, 0xDB, ForestOstard.GetRandomHue, false,  true,  "a", 1018281, 12, 10 ),
				new AnimalFormEntry( typeof( BullFrog ),     1028496,  8496, 0x7D3, 1070807, 50.0, 0x51, 0x7D3, false, false,  "a", 1028496, 15, 20 ),
				new AnimalFormEntry( typeof( GiantSerpent ), 1018114,  9663, 0x7D9, 1070808, 50.0, 0x15, 0x7D9, false, false,  "a", 1018114, 8,  7  ),
				new AnimalFormEntry( typeof( Dog ),          1018280,  8476, 0x905, 1070806, 40.0, 0xD9, 0x905, false, false,  "a", 1028476, 16, 17 ),
				new AnimalFormEntry( typeof( Cat ),          1018264,  8475, 0x905, 1070806, 40.0, 0xC9, 0x905, false, false,  "a", 1028475, 18, 17 ),
				new AnimalFormEntry( typeof( Rat ),          1018294,  8483, 0x905, 1070805,  0.0, 0xEE, 0x905,  true, false,  "a", 1018294, 15, 20 ),
				new AnimalFormEntry( typeof( Rabbit ),       1028485,  8485, 0x905, 1070805,  0.0, 0xCD, 0x905,  true, false,  "a", 1028485, 19, 20 ),

                #region Heritage Items
				new AnimalFormEntry( typeof( SquirrelFormTalisman ),	1031671,  0x2D97, 0x905,       0,  20.0, 0x116, 0x905, false, false, "a", 1031671, 15, 15 ), // squirrel
				new AnimalFormEntry( typeof( FerretFormTalisman ),		1031672,  0x2D98, 0x905, 1075220,  40.0, 0x117, 0x905, false, true,  "a", 1031672, 15, 15 ), // ferret
				new AnimalFormEntry( typeof( CuSidheFormTalisman ),		1031670,  0x2D96, 0x905, 1075221,  60.0, 0x115, 0x905, false, false, "a", 1031670, 19, 12 ), // cu sidhe
				new AnimalFormEntry( typeof( ReptalonFormTalisman ),	1075202,  0x2D95, 0x905, 1075222,  90.0, 0x114, 0x905, false, false, "a", 1075202, -2, 0  )  // reptalon
				#endregion
			};

		public static AnimalFormEntry[] Entries { get { return m_Entries; } }

		public class AnimalFormGump : Gump
		{
			public override int TypeID { get { return 0x2336; } }

			private Mobile m_Caster;
			private AnimalForm m_Spell;

			public AnimalFormGump( Mobile caster, AnimalFormEntry[] entries, AnimalForm spell )
				: base( 60, 36 )
			{
				m_Caster = caster;
				m_Spell = spell;

				AddPage( 0 );

				AddBackground( 0, 0, 520, 404, 0x13BE );
				AddImageTiled( 10, 10, 500, 20, 0xA40 );
				AddImageTiled( 10, 40, 500, 324, 0xA40 );
				AddImageTiled( 10, 374, 500, 20, 0xA40 );
				AddAlphaRegion( 10, 10, 500, 384 );
				AddButton( 10, 374, 0xFB1, 0xFB2, 0, GumpButtonType.Reply, 0 );
				AddHtmlLocalized( 45, 376, 450, 20, 1060051, 0x7FFF, false, false ); // CANCEL
				AddHtmlLocalized( 14, 12, 500, 20, 1063394, 0x7FFF, false, false ); // <center>Animal Form Selection Menu</center>

				AddPage( 1 );

				int idx = 0;

				for ( int i = 0; i < entries.Length; i++ )
				{
					if ( ( i >= 12 && i <= entries.Length ) && ( m_Caster.Talisman == null || m_Caster.Talisman.GetType() != entries[i].Type ) )
					{

					}
					else
					{
						AnimalFormEntry entry = entries[i];

						if ( entry.Type == typeof( Rat ) || entry.Type == typeof( Rabbit ) || entry.ReqSkill < m_Caster.Skills[SkillName.Ninjitsu].Value )
						{
							idx++;

							if ( idx == 11 )
							{
								AddButton( 400, 374, 0xFA5, 0xFA7, 0, GumpButtonType.Page, 2 );
								AddHtmlLocalized( 440, 376, 60, 20, 1043353, 0x7FFF, false, false ); // Next

								AddPage( 2 );

								AddButton( 300, 374, 0xFAE, 0xFB0, 0, GumpButtonType.Page, 1 );
								AddHtmlLocalized( 340, 376, 60, 20, 1011393, 0x7FFF, false, false ); // Back

								idx = 1;
							}

							if ( ( idx % 2 ) != 0 )
							{
								AddButtonTileArt( 14, 44 + ( 64 * ( idx - 1 ) / 2 ), 0x918, 0x919, GumpButtonType.Reply, 0x64 + i, 100 + i, entry.ItemID, entry.Hue, entry.TileArtWidth, entry.TileArtHeight );
								AddTooltip( entry.Tooltip );
								AddHtmlLocalized( 98, 44 + ( 64 * ( idx - 1 ) / 2 ), 250, 60, entry.Name, 0x7FFF, false, false );
							}
							else
							{
								AddButtonTileArt( 264, 44 + ( 64 * ( idx - 2 ) / 2 ), 0x918, 0x919, GumpButtonType.Reply, 0x64 + i, 100 + i, entry.ItemID, entry.Hue, entry.TileArtWidth, entry.TileArtHeight );
								AddTooltip( entry.Tooltip );
								AddHtmlLocalized( 348, 44 + ( 64 * ( idx - 2 ) / 2 ), 250, 60, entry.Name, 0x7FFF, false, false );
							}
						}
					}
				}
			}


			public override void OnResponse( GameClient sender, RelayInfo info )
			{
				if ( !m_Spell.CheckCast() )
					return;

				if ( info.ButtonID >= 100 )
				{
					int entryID = info.ButtonID - 100;

					if ( AnimalForm.Morph( m_Caster, entryID ) == MorphResult.Fail )
					{
						m_Caster.LocalOverheadMessage( MessageType.Regular, 0x3B2, 502632 ); // The spell fizzles.
						m_Caster.FixedParticles( 0x3735, 1, 30, 9503, EffectLayer.Waist );
						m_Caster.PlaySound( 0x5C );
					}
					else
					{
						m_Spell.ConsumeMana();
					}
				}
			}
		}
	}

	public class AnimalFormContext
	{
		private Timer m_Timer;
		private SkillMod m_Mod;
		private bool m_SpeedBoost;
		private Type m_Type;

		public Timer Timer { get { return m_Timer; } }
		public SkillMod Mod { get { return m_Mod; } }
		public bool SpeedBoost { get { return m_SpeedBoost; } }
		public Type Type { get { return m_Type; } }

		public AnimalFormContext( Timer timer, SkillMod mod, bool speedBoost, Type type )
		{
			m_Timer = timer;
			m_Mod = mod;
			m_SpeedBoost = speedBoost;
			m_Type = type;
		}
	}

	public class AnimalFormTimer : Timer
	{
		private Mobile m_Mobile;
		private int m_Body;
		private int m_Hue;

		#region Heritage Items
		private int m_Delay = 10;
		private Mobile m_Target;
		#endregion

		public AnimalFormTimer( Mobile from, int body, int hue )
			: base( TimeSpan.FromSeconds( 1.0 ), TimeSpan.FromSeconds( 1.0 ) )
		{
			m_Mobile = from;
			m_Body = body;
			m_Hue = hue;

		}

		protected override void OnTick()
		{
			#region Heritage Items
			if ( !m_Mobile.Deleted && m_Mobile.Alive )
			{
				if ( m_Body == 0x115 ) // Cu Sidhe
				{
					if ( m_Mobile.Hits < m_Mobile.HitsMax || m_Delay < 9 )
					{
						if ( m_Delay-- <= 0 )
						{
							m_Mobile.Hits += Utility.RandomMinMax( 28, 32 );
							m_Delay = 10;
						}
					}
				}
				else if ( m_Body == 0x114 ) // Reptalon
				{
					if ( m_Mobile.Combatant != null && m_Mobile.Combatant != m_Target )
					{
						m_Delay = 0;
						m_Target = m_Mobile.Combatant;
					}

					if ( m_Target != null && m_Mobile.InRange( m_Target.Location, 1 ) )
						m_Delay -= 8;

					if ( m_Target != null && m_Delay-- <= 0 )
					{
						if ( m_Target.Alive && !m_Target.IsDeadBondedPet && m_Mobile.CanBeHarmful( m_Target ) && m_Target.Map == m_Mobile.Map && m_Target.InRange( m_Mobile.Location, BaseCreature.DefaultRangePerception ) && m_Mobile.InLOS( m_Target ) )
						{
							m_Mobile.Direction = m_Mobile.GetDirectionTo( m_Target );
							m_Mobile.Freeze( TimeSpan.FromSeconds( 1 ) );
							m_Mobile.PlaySound( 0x16A );
							m_Mobile.Animate( 12, 5, 1, true, false, 0 );

							Timer.DelayCall( TimeSpan.FromSeconds( 1.3 ), new TimerStateCallback( BreathEffect_Callback ), m_Target );
						}

						m_Delay = 10;
					}
				}
			}
			#endregion

			if ( m_Mobile.Deleted || !m_Mobile.Alive || m_Mobile.Body != m_Body || ( m_Hue != 0 && m_Mobile.Hue != m_Hue ) )
			{
				AnimalForm.RemoveContext( m_Mobile, true );
				Stop();
			}
		}

		#region Heritage Items
		public virtual void BreathEffect_Callback( object state )
		{
			Mobile target = (Mobile) state;

			if ( target.Alive && m_Mobile.CanBeHarmful( target ) )
			{
				m_Mobile.PlaySound( 0x227 );
				Effects.SendMovingEffect( m_Mobile, target, 0x36D4, 5, 0, false, false, 0, 0 );

				Timer.DelayCall( TimeSpan.FromSeconds( 1 ), new TimerStateCallback( BreathDamage_Callback ), target );
			}
		}

		public virtual void BreathDamage_Callback( object state )
		{
			Mobile target = (Mobile) state;

			if ( target.Alive && m_Mobile.CanBeHarmful( target ) )
				AOS.Damage( target, m_Mobile, 20, 0, 100, 0, 0, 0 );
		}
		#endregion
	}
}