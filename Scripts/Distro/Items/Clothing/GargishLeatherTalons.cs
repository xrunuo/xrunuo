using System;

namespace Server.Items
{
	public class GargishLeatherTalons : BaseShoes
	{
		public override Race RequiredRace { get { return Race.Gargoyle; } }

		private bool m_CanFortity;

		public override bool CanFortify { get { return m_CanFortity; } }

		[Constructable]
		public GargishLeatherTalons()
			: this( 0 )
		{
		}

		[Constructable]
		public GargishLeatherTalons( int hue )
			: base( 0x41D8, hue )
		{
			Weight = 2.0;
		}

		public GargishLeatherTalons( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( (bool) m_CanFortity );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_CanFortity = reader.ReadBool();
		}

		public override void AlterFrom( BaseClothing orig )
		{
			base.AlterFrom( orig );

			m_CanFortity = orig.CanFortify;
		}
	}
}
