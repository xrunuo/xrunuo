using System;
using System.Collections;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class TokunoArmorer : Armorer
	{
		private ArrayList m_SBInfos = new ArrayList();
		protected override ArrayList SBInfos { get { return m_SBInfos; } }

		[Constructable]
		public TokunoArmorer()
		{
		}

		public override void InitSBInfo()
		{
			m_SBInfos.Add( new SBMetalShields() );

			m_SBInfos.Add( new SBHelmetArmor() );
			m_SBInfos.Add( new SBPlateArmor() );
			m_SBInfos.Add( new SBChainmailArmor() );
			m_SBInfos.Add( new SBRingmailArmor() );
			m_SBInfos.Add( new SBStuddedArmor() );
			m_SBInfos.Add( new SBLeatherArmor() );

			m_SBInfos.Add( new SBTokunoPlateArmor() );
			m_SBInfos.Add( new SBTokunoStuddedArmor() );
			m_SBInfos.Add( new SBTokunoLeatherArmor() );
		}

		public override VendorShoeType ShoeType { get { return VendorShoeType.SamuraiTabi; } }

		public override void InitOutfit()
		{
			AddItem( new TattsukeHakama() );
			AddItem( new HalfApron() );
			AddItem( new PlateHatsuburi() );
			AddItem( new HakamaShita() );
		}

		public TokunoArmorer( Serial serial )
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

			/*int version = */reader.ReadInt();
		}
	}
}