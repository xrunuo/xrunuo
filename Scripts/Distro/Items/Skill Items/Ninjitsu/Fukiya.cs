using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.ContextMenus;
using Server.Engines.Craft;
using Server.Spells.Bushido;
using Server.Spells.Ninjitsu;
using Server.Targeting;

namespace Server.Items
{
	[FlipableAttribute( 0x27AA, 0x27F5 )]
	public class Fukiya : Item, IUsesRemaining
	{
		private int m_UsesRemaining;

		private Poison m_Poison;
		private int m_PoisonCharges;

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
		public int PoisonCharges
		{
			get { return m_PoisonCharges; }
			set
			{
				m_PoisonCharges = value;
				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Poison Poison
		{
			get { return m_Poison; }
			set
			{
				m_Poison = value;
				InvalidateProperties();
			}
		}

		public bool ShowUsesRemaining
		{
			get { return true; }
			set { }
		}

		[Constructable]
		public Fukiya()
			: base( 0x27F5 )
		{
			Weight = 4.0;
			Layer = Layer.OneHanded;
		}

		public Fukiya( Serial serial )
			: base( serial )
		{
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( m_Poison != null && m_PoisonCharges > 0 )
			{
				if ( m_Poison.Name == "Darkglow" )
					list.Add( 1072853, m_PoisonCharges.ToString() ); // darkglow poison charges: ~1_val~
				else if ( m_Poison.Name == "Parasitic" )
					list.Add( 1072852, m_PoisonCharges.ToString() ); // parasitic poison charges: ~1_val~
				else
					list.Add( 1062412 + m_Poison.Level, m_PoisonCharges.ToString() );
			}

			list.Add( 1060584, m_UsesRemaining.ToString() ); // uses remaining: ~1_val~
		}

		public override bool OnEquip( Mobile from )
		{
			from.SendLocalizedMessage( 1070785 ); // Double click this item each time you wish to throw a shuriken.
			return true;
		}

		public static bool HasFreeHand( Mobile m )
		{
			Item handOne = m.FindItemOnLayer( Layer.OneHanded );
			Item handTwo = m.FindItemOnLayer( Layer.TwoHanded );

			if ( handTwo is BaseWeapon )
			{
				handOne = handTwo;

				if ( ( (BaseWeapon) handOne ).WeaponAttributes.Balanced != 0 )
					return true;
			}

			return ( handOne == null || handTwo == null );
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from ) )
				return;

			if ( m_UsesRemaining < 1 )
			{
				// You have no fukiya darts!
				from.SendLocalizedMessage( 1063325 );
			}
			else if ( m_Using.Contains( from ) )
			{
				// You are already using that fukiya.
				from.SendLocalizedMessage( 1063326 );
			}
			else if ( !HasFreeHand( from ) )
			{
				// You must have a free hand to use a fukiya.
				from.SendLocalizedMessage( 1063327 );
			}
			else
			{
				from.BeginTarget( -1, false, TargetFlags.Harmful, new TargetCallback( OnShootTarget ) );
			}
		}

		public void OnShootTarget( Mobile from, object obj )
		{
			if ( Deleted || !IsChildOf( from ) )
				return;

			if ( obj is Mobile )
				Shoot( from, (Mobile) obj );
		}

		public void OnReloadTarget( Mobile from, object obj )
		{
			if ( Deleted || !IsChildOf( from ) )
				return;

			if ( obj is FukiyaDart )
				Reload( from, (FukiyaDart) obj );
			else
				from.SendLocalizedMessage( 1063329 ); // You can only load fukiya darts
		}

		private static ISet<Mobile> m_Using = new HashSet<Mobile>();

		public void Shoot( Mobile from, Mobile target )
		{
			if ( from == target )
				return;

			if ( m_UsesRemaining < 1 )
			{
				// You have no fukiya darts!
				from.SendLocalizedMessage( 1063325 );
			}
			else if ( m_Using.Contains( from ) )
			{
				// You are already using that fukiya.
				from.SendLocalizedMessage( 1063326 );
			}
			else if ( !HasFreeHand( from ) )
			{
				// You must have a free hand to use a fukiya.
				from.SendLocalizedMessage( 1063327 );
			}
			else if ( from.GetDistanceToSqrt( target ) > 5 )
			{
				// Your target is too far!
				from.SendLocalizedMessage( 1063304 );
			}
			else if ( from.CanBeHarmful( target ) )
			{
				m_Using.Add( from );

				from.Direction = from.GetDirectionTo( target );

				from.RevealingAction();

				from.PlaySound( 0x223 );
				from.MovingEffect( target, 0x2804, 5, 0, false, false );

				if ( CheckHitChance( from, target ) )
					Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ), new TimerStateCallback( OnDartHit ), new object[] { from, target } );
				else
					ConsumeUse();

				// Shooting a fukiya dart restarts your weapon swing delay
				from.NextCombatTime = DateTime.Now + from.Weapon.GetDelay( from );

				Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ), () => m_Using.Remove( from ) );
			}
		}

		public static bool CheckHitChance( Mobile attacker, Mobile defender )
		{
			BaseWeapon defWeapon = defender.Weapon as BaseWeapon;

			double atkValue = attacker.Skills[SkillName.Ninjitsu].Value;
			double defValue = defWeapon.GetDefendSkillValue( attacker, defender );

			double chance = BaseWeapon.GetHitChance( atkValue, defValue, BaseWeapon.GetAttackChance( attacker ), BaseWeapon.GetDefendChance( defender ) );

			return attacker.CheckSkill( SkillName.Ninjitsu, chance );
		}

		private void OnDartHit( object state )
		{
			object[] states = (object[]) state;
			Mobile from = (Mobile) states[0];
			Mobile target = (Mobile) states[1];

			if ( !from.CanBeHarmful( target ) )
				return;

			from.DoHarmful( target );

			AOS.Damage( target, from, Utility.RandomMinMax( 4, 6 ), 100, 0, 0, 0, 0 );

			if ( m_Poison != null && m_PoisonCharges > 0 )
			{
				Poison p = m_Poison;
				int maxLevel = from.Skills[SkillName.Poisoning].Fixed / 200;
				if ( p.Level > maxLevel )
					p = Poison.GetPoison( maxLevel );

				if ( target.ApplyPoison( from, p ) != ApplyPoisonResult.Immune )
				{
					if ( p.Name == "Parasitic" )
						ParasiticPotion.AddInfo( target, from );
					else if ( p.Name == "Darkglow" )
						DarkglowPotion.AddInfo( target, from );
				}
			}

			ConsumeUse();
		}

		public void ConsumeUse()
		{
			if ( m_UsesRemaining < 1 )
				return;

			--m_UsesRemaining;

			if ( m_PoisonCharges > 0 )
			{
				--m_PoisonCharges;

				if ( m_PoisonCharges == 0 )
					m_Poison = null;
			}

			InvalidateProperties();
		}

		private const int MaxUses = 10;

		public void Unload( Mobile from )
		{
			if ( UsesRemaining < 1 )
				return;

			FukiyaDart darts = new FukiyaDart( UsesRemaining );

			darts.Poison = m_Poison;
			darts.PoisonCharges = m_PoisonCharges;

			from.AddToBackpack( darts );

			m_UsesRemaining = 0;
			m_PoisonCharges = 0;
			m_Poison = null;

			InvalidateProperties();
		}

		public void Reload( Mobile from, FukiyaDart darts )
		{
			int need = ( MaxUses - m_UsesRemaining );

			if ( need <= 0 )
			{
				// You cannot add anymore fukiya darts
				from.SendLocalizedMessage( 1063330 );
			}
			else if ( darts.UsesRemaining > 0 )
			{
				if ( need > darts.UsesRemaining )
					need = darts.UsesRemaining;

				if ( darts.Poison != null && darts.PoisonCharges > 0 )
				{
					if ( m_PoisonCharges <= 0 || m_Poison == null || m_Poison.Level <= darts.Poison.Level )
					{
						if ( m_Poison != null && m_Poison.Level < darts.Poison.Level )
							Unload( from );

						if ( need > darts.PoisonCharges )
							need = darts.PoisonCharges;

						if ( m_Poison == null || m_PoisonCharges <= 0 )
							m_PoisonCharges = need;
						else
							m_PoisonCharges += need;

						m_Poison = darts.Poison;

						darts.PoisonCharges -= need;

						if ( darts.PoisonCharges <= 0 )
							darts.Poison = null;

						m_UsesRemaining += need;
						darts.UsesRemaining -= need;
					}
					else
					{
						from.SendLocalizedMessage( 1070767 ); // Loaded projectile is stronger, unload it first
					}
				}
				else
				{
					m_UsesRemaining += need;
					darts.UsesRemaining -= need;
				}

				if ( darts.UsesRemaining <= 0 )
					darts.Delete();

				InvalidateProperties();
			}
		}

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
		{
			base.GetContextMenuEntries( from, list );

			if ( IsChildOf( from ) )
			{
				list.Add( new LoadEntry( this ) );
				list.Add( new UnloadEntry( this ) );
			}
		}

		private class LoadEntry : ContextMenuEntry
		{
			private Fukiya m_Fukiya;

			public LoadEntry( Fukiya fukiya )
				: base( 6224, 0 )
			{
				m_Fukiya = fukiya;
			}

			public override void OnClick()
			{
				if ( !m_Fukiya.Deleted && m_Fukiya.IsChildOf( Owner.From ) )
					Owner.From.BeginTarget( -1, false, TargetFlags.None, new TargetCallback( m_Fukiya.OnReloadTarget ) );
			}
		}

		private class UnloadEntry : ContextMenuEntry
		{
			private Fukiya m_Fukiya;

			public UnloadEntry( Fukiya fukiya )
				: base( 6225, 0 )
			{
				m_Fukiya = fukiya;

				Enabled = ( fukiya.UsesRemaining > 0 );
			}

			public override void OnClick()
			{
				if ( !m_Fukiya.Deleted && m_Fukiya.IsChildOf( Owner.From ) )
					m_Fukiya.Unload( Owner.From );
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 3 );

			Poison.Serialize( m_Poison, writer );
			writer.Write( m_PoisonCharges );

			writer.Write( (int) m_UsesRemaining );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 3:
					{
						m_Poison = Poison.Deserialize( reader );
						m_PoisonCharges = reader.ReadInt();

						m_UsesRemaining = reader.ReadInt();

						break;
					}
			}
		}
	}
}
