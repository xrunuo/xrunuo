using System;
using Server.Network;

namespace Server.Items
{
	public class BankBox : Container
	{
		public override int DefaultMaxWeight => 0;

		public BankBox( Serial serial )
			: base( serial )
		{
		}

		public Mobile Owner { get; private set; }

		public bool Opened { get; private set; }

		public void Open()
		{
			Opened = true;

			if ( Owner != null )
			{
				Owner.PrivateOverheadMessage( MessageType.Regular, 0x3B2, true, String.Format( "Bank container has {0} items, {1} stones", TotalItems, TotalWeight ), Owner.NetState );
				Owner.Send( new EquipUpdate( this ) );
				DisplayTo( Owner );
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( (Mobile) Owner );
			writer.Write( (bool) Opened );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
					{
						Owner = reader.ReadMobile();
						Opened = reader.ReadBool();

						if ( Owner == null )
							Delete();

						break;
					}
			}

			if ( ItemID == 0xE41 )
				ItemID = 0xE7C;
		}

		public static bool SendDeleteOnClose { get; set; }

		public void Close()
		{
			Opened = false;

			if ( Owner != null && SendDeleteOnClose )
				Owner.Send( this.RemovePacket );
		}

		public override void OnDoubleClick( Mobile from )
		{
		}

		public override DeathMoveResult OnParentDeath( Mobile parent )
		{
			return DeathMoveResult.RemainEquiped;
		}

		public BankBox( Mobile owner )
			: base( 0xE7C )
		{
			Layer = Layer.Bank;
			Movable = false;
			Owner = owner;
		}

		public override bool IsAccessibleTo( Mobile check )
		{
			if ( ( check == Owner && Opened ) || check.AccessLevel >= AccessLevel.GameMaster )
				return base.IsAccessibleTo( check );
			else
				return false;
		}

		public override bool OnDragDrop( Mobile from, Item dropped )
		{
			if ( ( from == Owner && Opened ) || from.AccessLevel >= AccessLevel.GameMaster )
				return base.OnDragDrop( from, dropped );
			else
				return false;
		}

		public override bool OnDragDropInto( Mobile from, Item item, Point3D p, byte gridloc )
		{
			if ( ( from == Owner && Opened ) || from.AccessLevel >= AccessLevel.GameMaster )
				return base.OnDragDropInto( from, item, p, gridloc );
			else
				return false;
		}
	}
}