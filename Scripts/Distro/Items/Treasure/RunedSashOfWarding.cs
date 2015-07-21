using System;
using System.Collections.Generic;
using Server.Engines.Craft;

namespace Server.Items
{
	public enum WardingEffect
	{
		WeaponDamage,
		SpellDamage
	}

	[Alterable( typeof( DefTailoring ), typeof( GargishRunedSashOfWarding ), true )]
	public class RunedSashOfWarding : BodySash
	{
		public static readonly TimeSpan Duration = TimeSpan.FromSeconds( 10.0 );
		public static readonly TimeSpan Cooldown = TimeSpan.FromSeconds( 120.0 );

		public override int LabelNumber { get { return 1116231; } } // Runed Sash of Warding

		public override int InitMinHits { get { return 75; } }
		public override int InitMaxHits { get { return 75; } }

		public override bool Brittle { get { return true; } }

		private int m_Charges;
		private WardingEffect m_WardingEffect;

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
		public WardingEffect WardingEffect
		{
			get { return m_WardingEffect; }
			set
			{
				m_WardingEffect = value;
				InvalidateProperties();
			}
		}

		[Constructable]
		public RunedSashOfWarding()
			: base( 0x485 )
		{
			m_Charges = 50;
			m_WardingEffect = Utility.RandomBool() ? WardingEffect.WeaponDamage : WardingEffect.SpellDamage;
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			// charges: ~1_val~
			list.Add( 1060741, Charges.ToString() );

			// Weapon Damage Ward / Spell Damage Ward
			list.Add( 1116172 + (int) m_WardingEffect );
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

				var context = new WardingContext( m_WardingEffect, removeTimer );

				m_UnderEffect.Add( from, context );
				Charges--;

				// The runes glow and a magical warding forms around your body.
				from.SendLocalizedMessage( 1116243 );
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

			// The magical ward around you dissipates.
			from.SendLocalizedMessage( 1116244 );
		}

		public static bool IsUnderWardingEffect( Mobile m, WardingEffect effect )
		{
			WardingContext context;

			if ( !m_UnderEffect.TryGetValue( m, out context ) )
				return false;

			return context.Effect == effect;
		}

		private static IDictionary<Mobile, WardingContext> m_UnderEffect = new Dictionary<Mobile, WardingContext>();
		private static ISet<Mobile> m_Cooldown = new HashSet<Mobile>();

		private class WardingContext
		{
			public WardingEffect Effect { get; private set; }
			public Timer RemoveTimer { get; private set; }

			public WardingContext( WardingEffect effect, Timer removeTimer )
			{
				Effect = effect;
				RemoveTimer = removeTimer;
			}
		}

		public RunedSashOfWarding( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.WriteEncodedInt( (int) m_Charges );
			writer.WriteEncodedInt( (int) m_WardingEffect );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_Charges = reader.ReadEncodedInt();
			m_WardingEffect = (WardingEffect) reader.ReadEncodedInt();
		}
	}

	public class GargishRunedSashOfWarding : RunedSashOfWarding
	{
		public override Race RequiredRace { get { return Race.Gargoyle; } }

		[Constructable]
		public GargishRunedSashOfWarding()
		{
			ItemID = 0x46B4;
		}

		public override void AlterFrom( BaseClothing orig )
		{
			base.AlterFrom( orig );

			var sash = orig as RunedSashOfWarding;

			if ( sash != null )
			{
				Charges = sash.Charges;
				WardingEffect = sash.WardingEffect;
			}
		}

		public GargishRunedSashOfWarding( Serial serial )
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