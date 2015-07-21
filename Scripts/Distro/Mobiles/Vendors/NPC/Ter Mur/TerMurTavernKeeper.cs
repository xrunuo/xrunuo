using System;
using System.Collections;
using Server;

namespace Server.Mobiles
{
	public class TerMurTavernKeeper : TavernKeeper
	{
		private ArrayList m_SBInfos = new ArrayList();
		protected override ArrayList SBInfos { get { return m_SBInfos; } }

		public override Race DefaultRace { get { return Race.Gargoyle; } }

		[Constructable]
		public TerMurTavernKeeper()
		{
		}

		public override void InitSBInfo()
		{
			// TODO (SA): These should sell Ter Mur Contracts instead of regular ones.

			m_SBInfos.Add( new SBTavernKeeper() );
		}

		public TerMurTavernKeeper( Serial serial )
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