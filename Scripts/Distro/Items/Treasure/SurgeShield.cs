using System;
using System.Collections.Generic;
using Server.Engines.Craft;

namespace Server.Items
{
	public enum SurgeEffect
	{
		HitPoint,
		Mana,
		Stamina
	}

	public class SurgeShield : BronzeShield
	{
		public static readonly TimeSpan Duration = TimeSpan.FromSeconds( 20.0 );
		public static readonly TimeSpan Cooldown = TimeSpan.FromSeconds( 120.0 );

		public override int LabelNumber { get { return 1116232; } } // Surge Shield

		public override int InitMinHits { get { return 75; } }
		public override int InitMaxHits { get { return 75; } }

		public override bool Brittle { get { return true; } }

		private int m_Charges;
		private SurgeEffect m_SurgeEffect;

		[CommandProperty( AccessLevel.GameMaster )]
		public int Charges
		{
			get { return m_Charges; }
			set
			{
				m_Charges = value;
				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public SurgeEffect SurgeEffect
		{
			get { return m_SurgeEffect; }
			set
			{
				m_SurgeEffect = value;
				InvalidateProperties();
			}
		}

		[Constructable]
		public SurgeShield()
		{
			Hue = 0x1C0;
			m_Charges = 50;

			switch ( Utility.Random( 3 ) )
			{
				case 0:
					m_SurgeEffect = SurgeEffect.HitPoint;
					break;
				case 1:
					m_SurgeEffect = SurgeEffect.Mana;
					break;
				case 2:
					m_SurgeEffect = SurgeEffect.Stamina;
					break;
			}

			switch ( Utility.Random( 2 ) )
			{
				case 0:
					Attributes.AttackChance = 5;
					break;
				case 1:
					Attributes.LowerManaCost = 4;
					break;
			}

			switch ( Utility.Random( 2 ) )
			{
				case 0:
					Attributes.CastSpeed = 1;
					break;
				case 1:
					Attributes.CastSpeed = 1;
					Attributes.SpellChanneling = 1;
					break;
			}

			switch ( Utility.Random( 5 ) )
			{
				case 0:
					Resistances.Physical = 5;
					break;
				case 1:
					Resistances.Fire = 5;
					break;
				case 2:
					Resistances.Cold = 5;
					break;
				case 3:
					Resistances.Poison = 5;
					break;
				case 4:
					Resistances.Energy = 5;
					break;
			}
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			// charges: ~1_val~
			list.Add( 1060741, Charges.ToString() );

			// Surge HP/Mana/Stamina Regeneration
			list.Add( 1116177 + (int) m_SurgeEffect );
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( Parent != from )
			{
				// That must be equipped before you can use it.
				from.SendLocalizedMessage( 1116250 );
			}
			else if ( Charges == 0 )
			{
				// This magic item is out of charges.
				from.SendLocalizedMessage( 501250 );
			}
			else if ( m_Cooldown.Contains( from ) )
			{
				// You must wait for the energy to recharge before using the warding effect again.
				from.SendLocalizedMessage( 1116170 );
			}
			else if ( !m_UnderEffect.ContainsKey( from ) )
			{
				var removeTimer = Timer.DelayCall( Duration, () => RemoveEffect( from ) );

				var context = new SurgeContext( m_SurgeEffect, removeTimer );

				m_UnderEffect.Add( from, context );
				Charges--;

				// You feel magical energy surging through your body.
				from.SendLocalizedMessage( 1116241 );
			}
		}

		public override void OnRemoved( object parent )
		{
			base.OnRemoved( parent );

			var from = parent as Mobile;

			if ( from != null )
				RemoveEffect( from );
		}

		private void RemoveEffect( Mobile from )
		{
			if ( !m_UnderEffect.ContainsKey( from ) )
				return;

			var context = m_UnderEffect[from];
			context.RemoveTimer.Stop();

			m_UnderEffect.Remove( from );

			m_Cooldown.Add( from );
			Timer.DelayCall( Cooldown, () => m_Cooldown.Remove( from ) );

			// The magical energy surging through your body subsides.
			from.SendLocalizedMessage( 1116242 );
		}

		public static bool IsUnderSurgeEffect( Mobile m, SurgeEffect effect )
		{
			SurgeContext context;

			if ( !m_UnderEffect.TryGetValue( m, out context ) )
				return false;

			return context.Effect == effect;
		}

		private static IDictionary<Mobile, SurgeContext> m_UnderEffect = new Dictionary<Mobile, SurgeContext>();
		private static ISet<Mobile> m_Cooldown = new HashSet<Mobile>();

		private class SurgeContext
		{
			public SurgeEffect Effect { get; private set; }
			public Timer RemoveTimer { get; private set; }

			public SurgeContext( SurgeEffect effect, Timer removeTimer )
			{
				Effect = effect;
				RemoveTimer = removeTimer;
			}
		}

		public SurgeShield( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.WriteEncodedInt( (int) m_Charges );
			writer.WriteEncodedInt( (int) m_SurgeEffect );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_Charges = reader.ReadEncodedInt();
			m_SurgeEffect = (SurgeEffect) reader.ReadEncodedInt();
		}
	}

	public class GargishSurgeShield : SurgeShield
	{
		public override Race RequiredRace { get { return Race.Gargoyle; } }

		public override int StrengthReq { get { return 20; } }

		[Constructable]
		public GargishSurgeShield()
		{
			ItemID = 0x4200;
			Weight = 5.0;
		}

		public override void AlterFrom( BaseArmor orig )
		{
			base.AlterFrom( orig );

			var shield = orig as GargishSurgeShield;

			if ( shield != null )
			{
				Charges = shield.Charges;
				SurgeEffect = shield.SurgeEffect;
			}
		}

		public GargishSurgeShield( Serial serial )
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