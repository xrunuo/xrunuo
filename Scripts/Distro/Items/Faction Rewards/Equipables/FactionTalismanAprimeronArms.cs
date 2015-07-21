using System;
using Server;
using Server.Factions;

namespace Server.Items
{
	public class FactionTalismanAprimeronArms : BaseTalisman, IFactionArtifact
	{
		public override int LabelNumber { get { return 1074887; } } // Library Talisman - A Primer on Arms Damage Removal

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		[Constructable]
		public FactionTalismanAprimeronArms()
			: base( 0x2F59 )
		{
			Weight = 1.0;
			Attributes.BonusStr = 1;
			Attributes.RegenHits = 2;
			Attributes.WeaponDamage = 20;
			Attributes.AttackChance = 10;
			TalismanType = TalismanType.DamageRemoval;
			Charges = -1;
		}

		public FactionTalismanAprimeronArms( Serial serial )
			: base( serial )
		{
		}

		public override void AddNameProperties( ObjectPropertyList list )
		{
			base.AddNameProperties( list );

			list.Add( 1041350 ); // faction item
			list.Add( 1061640, m_Owner == null ? "No one" : m_Owner.Name ); // Owner: ~1_OWNER~

			Moonbind.GetProperties( this, list );
		}

		#region IFactionArtifact Members
		private Mobile m_Owner;

		Mobile IFactionArtifact.Owner
		{
			get { return m_Owner; }
			set { m_Owner = value; }
		}
		#endregion

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 );

			writer.Write( m_Owner );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			m_Owner = reader.ReadMobile();

			if ( version < 1 )
				HitPoints = MaxHitPoints = Utility.RandomMinMax( InitMinHits, InitMaxHits );
		}
	}
}
