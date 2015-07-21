using System;
using Server;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
	public class ElixirOfRebirth : Item
	{
		public override int LabelNumber { get { return 1112762; } } // elixir of rebirth

		[Constructable]
		public ElixirOfRebirth()
			: base( 0x1956 )
		{
			Weight = 2.0;
			Hue = 0x48E;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
				from.SendLocalizedMessage( 1062334 ); // This item must be in your backpack to be used.
			else
			{
				from.Target = new InternalTarget( this );
				from.SendLocalizedMessage( 1112763 ); // Which pet do you wish to revive?
			}
		}

		public ElixirOfRebirth( Serial serial )
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

		private class InternalTarget : Target
		{
			private ElixirOfRebirth m_Elixir;

			public InternalTarget( ElixirOfRebirth elixir )
				: base( -1, false, TargetFlags.None )
			{
				m_Elixir = elixir;
			}

			protected override void OnTarget( Mobile from, object target )
			{
				if ( m_Elixir.Deleted )
					return;

				if ( !m_Elixir.IsChildOf( from.Backpack ) )
				{
					from.SendLocalizedMessage( 1062334 ); // This item must be in your backpack to be used.
				}
				else if ( target is BaseCreature )
				{
					BaseCreature pet = target as BaseCreature;

					if ( !pet.IsDeadBondedPet )
						from.SendLocalizedMessage( 1112764 ); // This may only be used to resurrect dead pets.
					else if ( pet.Corpse != null && !pet.Corpse.Deleted )
						from.SendLocalizedMessage( 1113279 ); // That creature's spirit lacks cohesion. Try again in a few minutes.
					else if ( !from.CanSee( pet ) || !from.InLOS( pet ) )
						from.SendLocalizedMessage( 503376 ); // Target cannot be seen.
					else if ( !from.InRange( pet, 12 ) )
						from.SendLocalizedMessage( 500643 ); // Target is too far away.
					else
					{
						pet.PlaySound( 0x214 );
						pet.ResurrectPet();
						m_Elixir.Delete();
					}
				}
				else
					from.SendLocalizedMessage( 1112764 ); // This may only be used to resurrect dead pets.
			}
		}
	}
}