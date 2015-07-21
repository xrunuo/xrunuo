using System;
using Server;
using Server.Factions;

namespace Server.Items
{
	public class FactionHeartOfTheLion : PlateChest, IFactionArtifact
	{
		public override int LabelNumber { get { return 1070817; } } // Heart of the Lion

		public override int BasePhysicalResistance { get { return 20; } }
		public override int BaseFireResistance { get { return 15; } }
		public override int BaseColdResistance { get { return 15; } }
		public override int BasePoisonResistance { get { return 15; } }
		public override int BaseEnergyResistance { get { return 15; } }

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		[Constructable]
		public FactionHeartOfTheLion()
		{
			Hue = 0x501;
			Attributes.Luck = 95;
			Attributes.DefendChance = 15;
			ArmorAttributes.LowerStatReq = 100;
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

		public FactionHeartOfTheLion( Serial serial )
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