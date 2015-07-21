using System;

using Server;
using Server.Engines.Craft;

namespace Server.Items
{
	[FlipableAttribute( 0x457E, 0x457F )]
	public class GargishLeatherWingArmor : BaseClothing, IQuiver
	{
		private int m_PhysicalDamage;
		private int m_FireDamage;
		private int m_ColdDamage;
		private int m_PoisonDamage;
		private int m_EnergyDamage;
		private int m_ChaosDamage;
		private int m_DirectDamage;
		private int m_DamageModifier;

		[CommandProperty( AccessLevel.GameMaster )]
		public int PhysicalDamage { get { return m_PhysicalDamage; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int FireDamage { get { return m_FireDamage; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int ColdDamage { get { return m_ColdDamage; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int PoisonDamage { get { return m_PoisonDamage; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int EnergyDamage { get { return m_EnergyDamage; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int ChaosDamage { get { return m_ChaosDamage; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int DirectDamage { get { return m_DirectDamage; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int DamageModifier { get { return m_DamageModifier; } }

		public override Race RequiredRace { get { return Race.Gargoyle; } }

		[Constructable]
		public GargishLeatherWingArmor()
			: base( 0x457E, Layer.Cloak )
		{
			Weight = 2.0;
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			AddElementalDamageProperties( list );

			if ( DamageModifier != 0 )
				list.Add( 1074762, DamageModifier.ToString() ); // Damage Modifier: ~1_PERCENT~%
		}

		private void AddElementalDamageProperties( ObjectPropertyList list )
		{
			if ( DirectDamage != 0 )
				list.Add( 1079978, DirectDamage.ToString() ); // Direct Damage: ~1_PERCENT~%

			if ( PhysicalDamage != 0 )
				list.Add( 1060403, PhysicalDamage.ToString() ); // physical damage ~1_val~%

			if ( FireDamage != 0 )
				list.Add( 1060405, FireDamage.ToString() ); // fire damage ~1_val~%

			if ( ColdDamage != 0 )
				list.Add( 1060404, ColdDamage.ToString() ); // cold damage ~1_val~%

			if ( PoisonDamage != 0 )
				list.Add( 1060406, PoisonDamage.ToString() ); // poison damage ~1_val~%

			if ( EnergyDamage != 0 )
				list.Add( 1060407, EnergyDamage.ToString() ); // energy damage ~1_val~%

			if ( ChaosDamage != 0 )
				list.Add( 1072846, ChaosDamage.ToString() ); // chaos damage ~1_val~%
		}

		public GargishLeatherWingArmor( Serial serial )
			: base( serial )
		{
		}

		private enum SaveFlag
		{
			None = 0x00000000,
			PhysicalDamage = 0x00000001,
			FireDamage = 0x00000002,
			ColdDamage = 0x00000004,
			PoisonDamage = 0x00000008,
			EnergyDamage = 0x00000010,
			ChaosDamage = 0x00000020,
			DirectDamage = 0x00000040,
			DamageModifier = 0x00000080,
		}

		private static void SetSaveFlag( ref SaveFlag flags, SaveFlag toSet, bool setIf )
		{
			if ( setIf )
				flags |= toSet;
		}

		private static bool GetSaveFlag( SaveFlag flags, SaveFlag toGet )
		{
			return ( ( flags & toGet ) != 0 );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );

			SaveFlag flags = SaveFlag.None;

			SetSaveFlag( ref flags, SaveFlag.PhysicalDamage, m_PhysicalDamage != 0 );
			SetSaveFlag( ref flags, SaveFlag.FireDamage, m_FireDamage != 0 );
			SetSaveFlag( ref flags, SaveFlag.ColdDamage, m_ColdDamage != 0 );
			SetSaveFlag( ref flags, SaveFlag.PoisonDamage, m_PoisonDamage != 0 );
			SetSaveFlag( ref flags, SaveFlag.EnergyDamage, m_EnergyDamage != 0 );
			SetSaveFlag( ref flags, SaveFlag.ChaosDamage, m_ChaosDamage != 0 );
			SetSaveFlag( ref flags, SaveFlag.DirectDamage, m_DirectDamage != 0 );
			SetSaveFlag( ref flags, SaveFlag.DamageModifier, m_DamageModifier != 0 );

			writer.Write( (int) flags );

			if ( GetSaveFlag( flags, SaveFlag.PhysicalDamage ) )
				writer.Write( (int) m_PhysicalDamage );

			if ( GetSaveFlag( flags, SaveFlag.FireDamage ) )
				writer.Write( (int) m_FireDamage );

			if ( GetSaveFlag( flags, SaveFlag.ColdDamage ) )
				writer.Write( (int) m_ColdDamage );

			if ( GetSaveFlag( flags, SaveFlag.PoisonDamage ) )
				writer.Write( (int) m_PoisonDamage );

			if ( GetSaveFlag( flags, SaveFlag.EnergyDamage ) )
				writer.Write( (int) m_EnergyDamage );

			if ( GetSaveFlag( flags, SaveFlag.ChaosDamage ) )
				writer.Write( (int) m_ChaosDamage );

			if ( GetSaveFlag( flags, SaveFlag.DirectDamage ) )
				writer.Write( (int) m_DirectDamage );

			if ( GetSaveFlag( flags, SaveFlag.DamageModifier ) )
				writer.Write( (int) m_DamageModifier );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			SaveFlag flags = (SaveFlag) reader.ReadInt();

			if ( GetSaveFlag( flags, SaveFlag.PhysicalDamage ) )
				m_PhysicalDamage = reader.ReadInt();

			if ( GetSaveFlag( flags, SaveFlag.FireDamage ) )
				m_FireDamage = reader.ReadInt();

			if ( GetSaveFlag( flags, SaveFlag.ColdDamage ) )
				m_ColdDamage = reader.ReadInt();

			if ( GetSaveFlag( flags, SaveFlag.PoisonDamage ) )
				m_PoisonDamage = reader.ReadInt();

			if ( GetSaveFlag( flags, SaveFlag.EnergyDamage ) )
				m_EnergyDamage = reader.ReadInt();

			if ( GetSaveFlag( flags, SaveFlag.ChaosDamage ) )
				m_ChaosDamage = reader.ReadInt();

			if ( GetSaveFlag( flags, SaveFlag.DirectDamage ) )
				m_DirectDamage = reader.ReadInt();

			if ( GetSaveFlag( flags, SaveFlag.DamageModifier ) )
				m_DamageModifier = reader.ReadInt();
		}

		public override bool Scissor( Mobile from, Scissors scissors )
		{
			from.SendLocalizedMessage( 502440 ); // Scissors can not be used on that to produce anything.
			return false;
		}

		#region Alter
		public override void AlterFrom( BaseQuiver orig )
		{
			base.AlterFrom( orig );

			m_PhysicalDamage = orig.PhysicalDamage;
			m_FireDamage = orig.FireDamage;
			m_ColdDamage = orig.ColdDamage;
			m_PoisonDamage = orig.PoisonDamage;
			m_EnergyDamage = orig.EnergyDamage;
			m_ChaosDamage = orig.ChaosDamage;
			m_DirectDamage = orig.DirectDamage;
			m_DamageModifier = orig.DamageModifier;
		}
		#endregion
	}
}