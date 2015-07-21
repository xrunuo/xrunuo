using System;
using Server.Mobiles;

namespace Server.Items
{
	public class JacobsPickaxe : Pickaxe
	{
		public override int LabelNumber { get { return 1077758; } } // Jacob's Pickaxe

		[Constructable]
		public JacobsPickaxe()
			: base()
		{
			LootType = LootType.Blessed;

			SkillBonuses.SetValues( 0, SkillName.Mining, 10.0 );
			UsesRemaining = 20;

			Timer.DelayCall( TimeSpan.FromMinutes( 5 ), TimeSpan.FromMinutes( 5 ), new TimerCallback( Tick_Callback ) );
		}

		private void Tick_Callback()
		{
			int charge = UsesRemaining + 10 > 20 ? 20 - UsesRemaining : 10;

			if ( charge > 0 )
				UsesRemaining += charge;

			InvalidateProperties();
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( HarvestSystem == null )
				return;

			if ( IsChildOf( from.Backpack ) || Parent == from )
			{
				if ( UsesRemaining > 0 )
					HarvestSystem.BeginHarvesting( from, this );
				else
					from.SendLocalizedMessage( 1072306 ); // You must wait a moment for it to recharge.
			}
			else
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
		}

		public JacobsPickaxe( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();

			Timer.DelayCall( TimeSpan.FromMinutes( 5 ), TimeSpan.FromMinutes( 5 ), new TimerCallback( Tick_Callback ) );

			LootType = LootType.Blessed;
		}
	}
}