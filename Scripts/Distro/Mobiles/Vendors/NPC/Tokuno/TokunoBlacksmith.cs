using System;
using System.Collections;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class TokunoBlacksmith : Blacksmith
	{
		private ArrayList m_SBInfos = new ArrayList();
		protected override ArrayList SBInfos { get { return m_SBInfos; } }

		[Constructable]
		public TokunoBlacksmith()
		{
		}

		public override VendorShoeType ShoeType { get { return VendorShoeType.SamuraiTabi; } }

		public override void InitSBInfo()
		{
			m_SBInfos.Add( new SBAxeWeapon() );
			m_SBInfos.Add( new SBKnifeWeapon() );
			m_SBInfos.Add( new SBMaceWeapon() );
			m_SBInfos.Add( new SBSmithTools() );
			m_SBInfos.Add( new SBPoleArmWeapon() );
			m_SBInfos.Add( new SBSpearForkWeapon() );
			m_SBInfos.Add( new SBSwordWeapon() );

			m_SBInfos.Add( new SBMetalShields() );

			m_SBInfos.Add( new SBHelmetArmor() );
			m_SBInfos.Add( new SBPlateArmor() );
			m_SBInfos.Add( new SBChainmailArmor() );
			m_SBInfos.Add( new SBRingmailArmor() );
			m_SBInfos.Add( new SBStuddedArmor() );
			m_SBInfos.Add( new SBLeatherArmor() );

			m_SBInfos.Add( new SBTokunoPlateArmor() );
			m_SBInfos.Add( new SBTokunoWeapon() );
		}

		public override void InitOutfit()
		{
			AddItem( new SmithHammer() );
			AddItem( new TattsukeHakama() );
			AddItem( new Bascinet() );
			AddItem( new FullApron() );
			AddItem( new HakamaShita() );
		}

		public TokunoBlacksmith( Serial serial )
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