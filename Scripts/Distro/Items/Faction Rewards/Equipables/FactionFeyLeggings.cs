using System;
using Server;
using Server.Factions;

namespace Server.Items
{
	public class FactionFeyLeggings : ChainLegs, IFactionArtifact
	{
		public override int LabelNumber { get { return 1075041; } } // Fey Leggings

		public override int BasePhysicalResistance { get { return 15; } }
		public override int BaseFireResistance { get { return 11; } }
		public override int BaseColdResistance { get { return 10; } }
		public override int BasePoisonResistance { get { return 4; } }
		public override int BaseEnergyResistance { get { return 22; } }

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		[Constructable]
		public FactionFeyLeggings()
		{
			Attributes.BonusHits = 6;
			Attributes.DefendChance = 20;
			ArmorAttributes.MageArmor = 1;
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

		public FactionFeyLeggings( Serial serial )
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