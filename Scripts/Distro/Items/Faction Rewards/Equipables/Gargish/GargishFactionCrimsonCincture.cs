using System;
using Server;
using Server.Factions;

namespace Server.Items
{
	public class GargishFactionCrimsonCincture : GargishHalfApron, IFactionArtifact
	{
		public override int LabelNumber { get { return 1075043; } } // Crimson Cincture

		public override bool CanBeBlessed { get { return false; } }

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		[Constructable]
		public GargishFactionCrimsonCincture()
			: base( 0xE8 )
		{
			Weight = 1.0;

			Attributes.BonusDex = 10;
			Attributes.BonusHits = 10;
			Attributes.RegenHits = 2;
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

		public GargishFactionCrimsonCincture( Serial serial )
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

		public override void AlterFrom( BaseClothing orig )
		{
			base.AlterFrom( orig );

			if ( orig is IFactionArtifact )
				m_Owner = ( (IFactionArtifact) orig ).Owner;
		}
	}
}