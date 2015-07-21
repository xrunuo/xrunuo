using System;
using System.Collections;
using Server;

namespace Server.Mobiles
{
	public class TerMurProvisioner : Provisioner
	{
		private ArrayList m_SBInfos = new ArrayList();
		protected override ArrayList SBInfos { get { return m_SBInfos; } }

		public override Race DefaultRace { get { return Race.Gargoyle; } }

		[Constructable]
		public TerMurProvisioner()
		{
			Title = "the Provisioner";
		}

		public override void InitSBInfo()
		{
			m_SBInfos.Add( new SBTerMurProvisioner() );
			m_SBInfos.Add( new SBProvisioner() );
		}

		public TerMurProvisioner( Serial serial )
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