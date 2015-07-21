using System;
using Server;

namespace Server.Items
{
	[TypeAlias( "Server.Items.TotalRefreshPotion" )]
	public class GreaterRefreshPotion : BaseRefreshPotion
	{
		public override double Refresh { get { return 0.5; } }

		[Constructable]
		public GreaterRefreshPotion()
			: base( PotionEffect.RefreshGreater )
		{
		}

		public GreaterRefreshPotion( Serial serial )
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