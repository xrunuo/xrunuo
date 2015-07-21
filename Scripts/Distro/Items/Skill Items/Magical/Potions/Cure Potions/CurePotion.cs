using System;
using Server;

namespace Server.Items
{
	public class CurePotion : BaseCurePotion
	{
		private static CureLevelInfo[] m_LevelInfo = new CureLevelInfo[]
			{
				new CureLevelInfo( 0, 1.00 ),
				new CureLevelInfo( 1, 0.95 ),
				new CureLevelInfo( 2, 0.45 ),
				new CureLevelInfo( 3, 0.25 ),
				new CureLevelInfo( 4, 0.15 )
			};

		public override CureLevelInfo[] LevelInfo { get { return m_LevelInfo; } }

		[Constructable]
		public CurePotion()
			: base( PotionEffect.Cure )
		{
		}

		public CurePotion( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}