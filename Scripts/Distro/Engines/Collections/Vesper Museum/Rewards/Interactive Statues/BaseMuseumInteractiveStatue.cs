using System;
using Server;

namespace Server.Items
{
	public abstract class BaseMuseumInteractiveStatue : MonsterStatuette
	{
		public override bool EnableSound { get { return false; } }

		public BaseMuseumInteractiveStatue( int itemid )
			: base( MonsterStatuetteType.CollectionInteractiveStatue )
		{
			ItemID = itemid;
			LootType = LootType.Regular;
		}

		public BaseMuseumInteractiveStatue( Serial serial )
			: base( serial )
		{
		}

		public override void OnMovement( Mobile m, Point3D oldLocation )
		{
			if ( TurnedOn && IsLockedDown && ( !m.Hidden || m.AccessLevel == AccessLevel.Player ) && this.InRange( m, 2 ) && !this.InRange( oldLocation, 2 ) )
			{
				this.PublicOverheadMessage( Network.MessageType.Regular, 0, Utility.RandomMinMax( 1073266, 1073286 ) );

				Effects.PlaySound( this.Location, this.Map, Utility.RandomMinMax( 0x0, 0x16 ) );
			}

			base.OnMovement( m, oldLocation );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}