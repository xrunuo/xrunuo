using System;
using Server;
using Server.Factions;

namespace Server.Items
{
	public class FactionClaininsSpellbook : Spellbook, IFactionArtifact
	{
		public override int LabelNumber { get { return 1073262; } } // Clainin's Spellbook - Museum of Vesper Replica

		[Constructable]
		public FactionClaininsSpellbook()
		{
			Hue = 2125;
			Attributes.SpellChanneling = 1;
			Attributes.RegenMana = 3;
			Attributes.LowerRegCost = 15;
			Attributes.Luck = 80;
			Attributes.LowerManaCost = 10;
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

		public FactionClaininsSpellbook( Serial serial )
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