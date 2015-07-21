using System;
using Server;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using System.Collections;

namespace Server.Items
{
	public class LyricalReadingGlasses : ElvenGlasses, ICollectionItem
	{
		public override int LabelNumber { get { return 1073382; } } // Lyrical Reading Glasses

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		[Constructable]
		public LyricalReadingGlasses()
		{
			Hue = 1151;

			HitLowerDefend = 20;
			Attributes.NightSight = 1;
			Attributes.ReflectPhysical = 15;

			Resistances.Physical = 8;
			Resistances.Fire = 6;
			Resistances.Cold = 7;
			Resistances.Poison = 7;
			Resistances.Energy = 7;
		}

		public LyricalReadingGlasses( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( version < 1 )
			{
				Resistances.Physical = 8;
				Resistances.Fire = 6;
				Resistances.Cold = 7;
				Resistances.Poison = 7;
				Resistances.Energy = 7;
			}
		}

		private bool CheckUse( Mobile from )
		{
			if ( !this.IsAccessibleTo( from ) )
				return false;

			if ( from.Map != this.Map || !from.InRange( GetWorldLocation(), 2 ) )
			{
				from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1019045 ); // I can't reach that.
				return false;
			}

			if ( !from.CanBeginAction( typeof( FireHorn ) ) )
			{
				from.SendLocalizedMessage( 1049615 ); // You must take a moment to catch your breath.
				return false;
			}

			return true;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( CheckUse( from ) )
			{
				from.SendLocalizedMessage( 1049620 ); // Select an area to incinerate.
				from.Target = new InternalTarget( this );
			}
		}

		public void Use( Mobile from, IPoint3D loc )
		{
			if ( !CheckUse( from ) )
				return;

			from.BeginAction( typeof( FireHorn ) );
			Timer.DelayCall( TimeSpan.FromSeconds( 6.0 ), new TimerStateCallback( EndAction ), from );

			int music = from.Skills[SkillName.Musicianship].Fixed;

			int sucChance = 500 + ( music - 775 ) * 2;
			double dSucChance = ( (double) sucChance ) / 1000.0;

			if ( !from.CheckSkill( SkillName.Musicianship, dSucChance ) )
			{
				from.SendLocalizedMessage( 1049618 ); // The horn emits a pathetic squeak.
				from.PlaySound( 0x18A );
				return;
			}

			from.PlaySound( 0x15F );
			Effects.SendPacket( from, from.Map, new HuedEffect( EffectType.Moving, from.Serial, Serial.Zero, 0x36D4, from.Location, loc, 5, 0, false, true, 0, 0 ) );

			ArrayList targets = new ArrayList();
			bool playerVsPlayer = false;

			var eable = from.Map.GetMobilesInRange( new Point3D( loc ), 2 );

			foreach ( Mobile m in eable )
			{
				if ( from != m && SpellHelper.ValidIndirectTarget( from, m ) && from.CanBeHarmful( m, false ) )
				{
					if ( !from.InLOS( m ) )
						continue;

					targets.Add( m );

					if ( m.IsPlayer )
						playerVsPlayer = true;
				}
			}


			if ( targets.Count > 0 )
			{
				int prov = from.Skills[SkillName.Provocation].Fixed;
				int disc = from.Skills[SkillName.Discordance].Fixed;
				int peace = from.Skills[SkillName.Peacemaking].Fixed;

				int minDamage, maxDamage;

				int musicScaled = music + Math.Max( 0, music - 900 ) * 2;
				int provScaled = prov + Math.Max( 0, prov - 900 ) * 2;
				int discScaled = disc + Math.Max( 0, disc - 900 ) * 2;
				int peaceScaled = peace + Math.Max( 0, peace - 900 ) * 2;

				int weightAvg = ( musicScaled + provScaled * 3 + discScaled * 3 + peaceScaled ) / 80;

				int avgDamage;
				if ( playerVsPlayer )
					avgDamage = weightAvg / 3;
				else
					avgDamage = weightAvg / 2;

				minDamage = ( avgDamage * 9 ) / 10;
				maxDamage = ( avgDamage * 10 ) / 9;

				double damage = Utility.RandomMinMax( minDamage, maxDamage );

				if ( targets.Count > 1 )
					damage = ( damage * 2 ) / targets.Count;

				for ( int i = 0; i < targets.Count; ++i )
				{
					Mobile m = (Mobile) targets[i];

					double toDeal = damage;

					from.DoHarmful( m );
					SpellHelper.Damage( TimeSpan.Zero, m, from, toDeal, 0, 100, 0, 0, 0 );

					Effects.SendTargetEffect( m, 0x3709, 10, 30 );
				}
			}

			double breakChance = 0.01;

			if ( Utility.RandomDouble() < breakChance )
			{
				from.SendLocalizedMessage( 1049619 ); // The fire horn crumbles in your hands.
				this.Delete();
			}
		}

		private static void EndAction( object state )
		{
			Mobile m = (Mobile) state;

			m.EndAction( typeof( FireHorn ) );
			m.SendLocalizedMessage( 1049621 ); // You catch your breath.
		}

		private class InternalTarget : Target
		{
			private LyricalReadingGlasses m_Horn;

			public InternalTarget( LyricalReadingGlasses horn )
				: base( 3, true, TargetFlags.Harmful )
			{
				m_Horn = horn;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( m_Horn.Deleted )
					return;

				IPoint3D loc;
				if ( targeted is Item )
					loc = ( (Item) targeted ).GetWorldLocation();
				else
					loc = targeted as IPoint3D;

				m_Horn.Use( from, loc );
			}
		}
	}
}
