using System;
using Server;

namespace Server.Items
{
	public class PieceWhiteKing : BasePiece
	{
		public PieceWhiteKing( BaseBoard board )
			: base( 0x3587, "white king", board )
		{
		}

		public PieceWhiteKing( Serial serial )
			: base( serial )
		{
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

	public class PieceBlackKing : BasePiece
	{
		public PieceBlackKing( BaseBoard board )
			: base( 0x358E, "black king", board )
		{
		}

		public PieceBlackKing( Serial serial )
			: base( serial )
		{
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

	public class PieceWhiteQueen : BasePiece
	{
		public PieceWhiteQueen( BaseBoard board )
			: base( 0x358A, "white queen", board )
		{
		}

		public PieceWhiteQueen( Serial serial )
			: base( serial )
		{
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

	public class PieceBlackQueen : BasePiece
	{
		public PieceBlackQueen( BaseBoard board )
			: base( 0x3591, "black queen", board )
		{
		}

		public PieceBlackQueen( Serial serial )
			: base( serial )
		{
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

	public class PieceWhiteRook : BasePiece
	{
		public PieceWhiteRook( BaseBoard board )
			: base( 0x3586, "white rook", board )
		{
		}

		public PieceWhiteRook( Serial serial )
			: base( serial )
		{
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

	public class PieceBlackRook : BasePiece
	{
		public PieceBlackRook( BaseBoard board )
			: base( 0x358D, "black rook", board )
		{
		}

		public PieceBlackRook( Serial serial )
			: base( serial )
		{
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

	public class PieceWhiteBishop : BasePiece
	{
		public PieceWhiteBishop( BaseBoard board )
			: base( 0x3585, "white bishop", board )
		{
		}

		public PieceWhiteBishop( Serial serial )
			: base( serial )
		{
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

	public class PieceBlackBishop : BasePiece
	{
		public PieceBlackBishop( BaseBoard board )
			: base( 0x358C, "black bishop", board )
		{
		}

		public PieceBlackBishop( Serial serial )
			: base( serial )
		{
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

	public class PieceWhiteKnight : BasePiece
	{
		public PieceWhiteKnight( BaseBoard board )
			: base( 0x3588, "white knight", board )
		{
		}

		public PieceWhiteKnight( Serial serial )
			: base( serial )
		{
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

	public class PieceBlackKnight : BasePiece
	{
		public PieceBlackKnight( BaseBoard board )
			: base( 0x358F, "black knight", board )
		{
		}

		public PieceBlackKnight( Serial serial )
			: base( serial )
		{
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

	public class PieceWhitePawn : BasePiece
	{
		public PieceWhitePawn( BaseBoard board )
			: base( 0x3589, "white pawn", board )
		{
		}

		public PieceWhitePawn( Serial serial )
			: base( serial )
		{
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

	public class PieceBlackPawn : BasePiece
	{
		public PieceBlackPawn( BaseBoard board )
			: base( 0x3590, "black pawn", board )
		{
		}

		public PieceBlackPawn( Serial serial )
			: base( serial )
		{
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
