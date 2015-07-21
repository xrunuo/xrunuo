using System;
using Server;
using Server.Factions;

namespace Server.Items
{
	public class FactionRingOfTheVile : GoldRing, IFactionArtifact
	{
		public override int LabelNumber { get { return 1061102; } } // Ring of the Vile
		public override int ArtifactRarity { get { return 11; } }

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		[Constructable]
		public FactionRingOfTheVile()
		{
			Hue = 0x4F7;
			Attributes.BonusDex = 8;
			Attributes.RegenStam = 6;
			Attributes.AttackChance = 25;
			Resistances.Poison = 20;
		}

		public FactionRingOfTheVile( Serial serial )
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
