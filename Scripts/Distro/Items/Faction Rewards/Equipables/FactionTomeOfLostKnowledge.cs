using System;
using Server;
using Server.Factions;

namespace Server.Items
{
	public class FactionTomeOfLostKnowledge : Spellbook, IFactionArtifact
	{
		public override int LabelNumber { get { return 1070971; } } // Tome of LostKnowledge

		[Constructable]
		public FactionTomeOfLostKnowledge()
		{
			Hue = 0x530;
			LootType = LootType.Regular;
			SkillBonuses.SetValues( 0, SkillName.Magery, 15.0 );
			Attributes.BonusInt = 8;
			Attributes.SpellDamage = 15;
			Attributes.LowerManaCost = 15;
			Attributes.RegenMana = 3;
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

		public FactionTomeOfLostKnowledge( Serial serial )
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

			if ( LootType == LootType.Blessed )
				LootType = LootType.Regular;
		}
	}
}
