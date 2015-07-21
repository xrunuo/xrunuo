using System;
using System.Collections;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class TokunoBowyer : Bowyer
	{
		private ArrayList m_SBInfos = new ArrayList();
		protected override ArrayList SBInfos { get { return m_SBInfos; } }

		[Constructable]
		public TokunoBowyer()
		{
		}

		public override void InitSBInfo()
		{
			m_SBInfos.Add( new SBRangedWeapon() );
			m_SBInfos.Add( new SBBowyer() );
			m_SBInfos.Add( new SBTokunoRangedWeapon() );
		}

		public override VendorShoeType ShoeType { get { return VendorShoeType.SamuraiTabi; } }

		public override void InitOutfit()
		{
			AddItem( new Yumi() );
			AddItem( new TattsukeHakama() );
			AddItem( new LeatherGorget() );
			AddItem( new HakamaShita() );
		}

		public TokunoBowyer( Serial serial )
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