using System;
using Server;
using Server.Factions;

namespace Server.Items
{
	public class FactionMuseumOrderShield : OrderShield, IFactionArtifact
	{
		public override int LabelNumber { get { return 1073258; } } // Order Shield - Museum of Vesper Replica

		public override int InitMinHits { get { return 80; } }
		public override int InitMaxHits { get { return 80; } }

		[Constructable]
		public FactionMuseumOrderShield()
		{
			Hue = 1000;
			Attributes.SpellChanneling = 1;
			Attributes.CastSpeed = 1;
			Attributes.DefendChance = 15;
			Attributes.AttackChance = 15;
			Attributes.Luck = 80;
		}

		public FactionMuseumOrderShield( Serial serial )
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

		public Mobile Owner
		{
			get { return m_Owner; }
			set { m_Owner = value; }
		}
		#endregion

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