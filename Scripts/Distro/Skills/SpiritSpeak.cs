using System;
using Server;
using Server.Items;
using Server.Spells;
using Server.Network;

namespace Server.SkillHandlers
{
	class SpiritSpeak
	{
		public static void Initialize()
		{
			SkillInfo.Table[32].Callback = new SkillUseCallback( OnUse );
		}

		public static TimeSpan OnUse( Mobile m )
		{
			Spell spell = new SpiritSpeakSpell( m );

			spell.Cast();

			if ( spell.IsCasting )
				return TimeSpan.FromSeconds( 5.0 );

			return TimeSpan.Zero;
		}

		private class SpiritSpeakTimer : Timer
		{
			private Mobile m_Owner;
			public SpiritSpeakTimer( Mobile m )
				: base( TimeSpan.FromMinutes( 2.0 ) )
			{
				m_Owner = m;
			}

			protected override void OnTick()
			{
				m_Owner.CanHearGhosts = false;
				m_Owner.SendLocalizedMessage( 502445 ); // You feel your contact with the neitherworld fading.
			}
		}

		public class SpiritSpeakSpell : Spell
		{
			private static SpellInfo m_Info = new SpellInfo( "Spirit Speak", "", 269 );

			public override bool BlockedByHorrificBeast { get { return false; } }

			public SpiritSpeakSpell( Mobile caster )
				: base( caster, null, m_Info )
			{
			}

			public override bool ClearHandsOnCast { get { return false; } }

			public override double CastDelayFastScalar { get { return 0; } }
			public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 1.0 ); } }

			public override int GetMana()
			{
				return 0;
			}

			public override void OnCasterHurt()
			{
				if ( IsCasting )
					Disturb( DisturbType.Hurt, false, true );
			}

			public override bool ConsumeReagents()
			{
				return true;
			}

			public override bool CheckFizzle()
			{
				return true;
			}

			public override bool CheckNextSpellTime { get { return false; } }

			public override void OnDisturb( DisturbType type, bool message )
			{
				Caster.NextSkillTime = DateTime.UtcNow;

				base.OnDisturb( type, message );
			}

			public override bool CheckDisturb( DisturbType type, bool checkFirst, bool resistable )
			{
				if ( type == DisturbType.EquipRequest || type == DisturbType.UseRequest )
					return false;

				return true;
			}

			public override void SayMantra()
			{
				// Anh Mi Sah Ko
				Caster.PublicOverheadMessage( MessageType.Regular, 0x3B2, 1062074, "", false );
				Caster.PlaySound( 0x24A );
			}

			public override void OnCast()
			{
				Corpse toChannel = null;

				foreach ( Item item in Caster.GetItemsInRange( 3 ) )
				{
					if ( item is Corpse && !( (Corpse) item ).Channeled )
					{
						toChannel = (Corpse) item;
						break;
					}
				}

				int max, min, mana, number;

				if ( toChannel != null )
				{
					min = 1 + (int) ( Caster.Skills[SkillName.SpiritSpeak].Value * 0.25 );
					max = min + 4;
					mana = 0;
					number = 1061287; // You channel energy from a nearby corpse to heal your wounds.
				}
				else
				{
					min = 1 + (int) ( Caster.Skills[SkillName.SpiritSpeak].Value * 0.25 );
					max = min + 4;
					mana = 10;
					number = 1061286; // You channel your own spiritual energy to heal your wounds.
				}

				if ( Caster.Mana < mana )
				{
					Caster.SendLocalizedMessage( 1061285 ); // You lack the mana required to use this skill.
				}
				else
				{
					Caster.CheckSkill( SkillName.SpiritSpeak, 0.0, 120.0 );

					if ( Utility.RandomDouble() > ( Caster.Skills[SkillName.SpiritSpeak].Value / 100.0 ) )
					{
						Caster.SendLocalizedMessage( 502443 ); // You fail your attempt at contacting the netherworld.
					}
					else
					{
						if ( toChannel != null )
						{
							toChannel.Channeled = true;
							toChannel.Hue = 0x835;
						}

						Caster.Mana -= mana;
						Caster.SendLocalizedMessage( number );

						Utility.FixMax( ref min, max );

						int toHeal = Utility.RandomMinMax( min, max );

						Server.Spells.Spellweaving.ArcaneEmpowermentSpell.ApplyHealBonus( Caster, ref toHeal );

						Caster.Heal( toHeal, Caster );

						Caster.FixedParticles( 0x375A, 1, 15, 9501, 2100, 4, EffectLayer.Waist );
					}
				}

				FinishSequence();
			}
		}
	}
}