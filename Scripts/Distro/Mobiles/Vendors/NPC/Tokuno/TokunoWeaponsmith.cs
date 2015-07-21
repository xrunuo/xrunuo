using System;
using Server;
using Server.Items;
using System.Collections;

namespace Server.Mobiles
{
	public class TokunoWeaponsmith : Weaponsmith
	{
		private ArrayList m_SBInfos = new ArrayList();
		protected override ArrayList SBInfos { get { return m_SBInfos; } }

		[Constructable]
		public TokunoWeaponsmith()
		{
		}

		public override VendorShoeType ShoeType { get { return VendorShoeType.NinjaTabi; } }

		public override void InitSBInfo()
		{
			m_SBInfos.Add( new SBAxeWeapon() );
			m_SBInfos.Add( new SBKnifeWeapon() );
			m_SBInfos.Add( new SBMaceWeapon() );
			m_SBInfos.Add( new SBPoleArmWeapon() );
			m_SBInfos.Add( new SBSpearForkWeapon() );
			m_SBInfos.Add( new SBSwordWeapon() );
			m_SBInfos.Add( new SBTokunoWeapon() );
		}

		public override void InitOutfit()
		{
			AddItem( new SmithHammer() );
			AddItem( new LeatherNinjaPants() );
			AddItem( new SkullCap() );
			AddItem( new HakamaShita() );
		}

		public TokunoWeaponsmith( Serial serial )
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