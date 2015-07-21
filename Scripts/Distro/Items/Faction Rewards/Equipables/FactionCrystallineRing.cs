using System;
using Server.Factions;

namespace Server.Items
{
	public class FactionCrystallineRing : GoldRing, IFactionArtifact
	{
		public override int LabelNumber { get { return 1075096; } } // Crystalline Ring

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		[Constructable]
		public FactionCrystallineRing()
		{
			Hue = 0x480;

			Attributes.RegenHits = 5;
			Attributes.RegenMana = 3;
			Attributes.SpellDamage = 20;
			Attributes.CastRecovery = 3;

			SkillBonuses.SetValues( 0, SkillName.Magery, 20.0 );
			SkillBonuses.SetValues( 1, SkillName.Focus, 20.0 );
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

		public Mobile Owner
		{
			get { return m_Owner; }
			set { m_Owner = value; }
		}
		#endregion

		public FactionCrystallineRing( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );

			writer.Write( m_Owner );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_Owner = reader.ReadMobile();
		}
	}
}