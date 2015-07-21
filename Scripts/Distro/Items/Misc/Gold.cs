using System;

namespace Server.Items
{
	public class Gold : Item
	{
		[Constructable]
		public Gold()
			: this( 1 )
		{
		}

		[Constructable]
		public Gold( int amountFrom, int amountTo )
			: this( Utility.Random( amountFrom, amountTo - amountFrom ) )
		{
		}

		[Constructable]
		public Gold( int amount )
			: base( 0xEED )
		{
			Stackable = true;
			Weight = 0.02 / 3;
			Amount = amount;
		}

		public Gold( Serial serial )
			: base( serial )
		{
		}

		public override int GetDropSound()
		{
			if ( Amount <= 1 )
			{
				return 0x2E4;
			}
			else if ( Amount <= 5 )
			{
				return 0x2E5;
			}
			else
			{
				return 0x2E6;
			}
		}

		protected override void OnAmountChange( int oldValue )
		{
			TotalGold = ( TotalGold - oldValue ) + Amount;
		}

		public override void UpdateTotals()
		{
			base.UpdateTotals();

			SetTotalGold( this.Amount );
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

			Weight = 0.02 / 3;
		}
	}
}