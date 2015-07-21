using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells.Ninjitsu;

namespace Server.Items
{
	public class Bola : Item
	{
		[Constructable]
		public Bola()
			: this( 1 )
		{
		}

		[Constructable]
		public Bola( int amount )
			: base( 0x26AC )
		{
			Weight = 4.0;
			Stackable = true;
			Amount = amount;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
				from.SendLocalizedMessage( 1040019 ); // The bola must be in your pack to use it.
			else if ( !from.CanBeginAction( typeof( Bola ) ) )
				from.SendLocalizedMessage( 1049624 ); // You have to wait a few moments before you can use another bola!
			else if ( from.Target is BolaTarget )
				from.SendLocalizedMessage( 1049631 ); // This bola is already being used.
			else if ( from.Mounted )
				from.SendLocalizedMessage( 1040016 ); // You cannot use this while riding a mount
			else if ( from.Flying )
				from.SendLocalizedMessage( 1113414 ); // You can't use this while flying!
			else if ( AnimalForm.UnderTransformation( from ) )
				from.SendLocalizedMessage( 1070902 ); // You can't use this while in an animal form!
			else
			{
				EtherealMount.StopMounting( from );

				Item one = from.FindItemOnLayer( Layer.OneHanded );
				Item two = from.FindItemOnLayer( Layer.TwoHanded );

				if ( one != null )
					from.Backpack.DropItem( one );

				if ( two != null )
					from.Backpack.DropItem( two );

				from.Target = new BolaTarget( this );

				from.LocalOverheadMessage( MessageType.Emote, 201, 1049632 ); // * You begin to swing the bola...*
				from.NonlocalOverheadMessage( MessageType.Emote, 201, 1049633, from.Name ); // ~1_NAME~ begins to menacingly swing a bola...
			}
		}

		private static void ReleaseBolaLock( object state )
		{
			( (Mobile) state ).EndAction( typeof( Bola ) );
		}

		private static void FinishThrow( object state )
		{
			object[] states = (object[]) state;

			Mobile from = (Mobile) states[0];
			Mobile to = (Mobile) states[1];
			Item bola = (Item) states[2];

			if ( !from.Alive )
				return;
			if ( !bola.IsChildOf( from.Backpack ) )
				from.SendLocalizedMessage( 1040019 ); // The bola must be in your pack to use it.
			else if ( !from.InRange( to, 15 ) || !from.InLOS( to ) || !from.CanSee( to ) )
				from.SendLocalizedMessage( 1042060 ); // You cannot see that target!
			else if ( !to.Mounted && !to.Flying && !AnimalForm.UnderTransformation( to ) )
				from.SendLocalizedMessage( 1049628 ); // You have no reason to throw a bola at that.
			else
			{
				bola.Consume();

				from.Direction = from.GetDirectionTo( to );
				from.Animate( 11, 5, 1, true, false, 0 );
				from.MovingEffect( to, 0x26AC, 10, 0, false, false );

				new Bola().MoveToWorld( to.Location, to.Map );

				to.Damage( 1, from );

				if ( to is Neira || to is ChaosDragoon || to is ChaosDragoonElite )
				{
					from.SendLocalizedMessage( 1042047 ); // You fail to knock the rider from its mount.
				}
				else
				{
					if ( from.Flying )
						to.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1113590, from.Name ); // You have been grounded by ~1_NAME~!
					else
						to.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1049623, from.Name ); // You have been knocked off of your mount by ~1_NAME~!

					BaseMount.Dismount( to );

					if ( AnimalForm.UnderTransformation( to ) )
						AnimalForm.RemoveContext( to, true );

					to.Flying = false;

					BaseMount.SetMountPrevention( to, BlockMountType.Dazed, TimeSpan.FromSeconds( 10.0 ) );
				}
			}
		}

		private class BolaTarget : Target
		{
			private Bola m_Bola;

			public BolaTarget( Bola bola )
				: base( 15, false, TargetFlags.Harmful )
			{
				m_Bola = bola;
			}

			protected override void OnTarget( Mobile from, object obj )
			{
				if ( m_Bola.Deleted )
					return;

				if ( obj is Mobile )
				{
					Mobile to = (Mobile) obj;

					if ( !m_Bola.IsChildOf( from.Backpack ) )
						from.SendLocalizedMessage( 1040019 ); // The bola must be in your pack to use it.
					else if ( from == to )
						from.SendLocalizedMessage( 1005576 ); // You can't throw this at yourself.
					else if ( from.Mounted )
						from.SendLocalizedMessage( 1040016 ); // You cannot use this while riding a mount
					else if ( from.Flying )
						from.SendLocalizedMessage( 1113414 ); // You can't use this while flying!
					else if ( !to.Mounted && !to.Flying && !AnimalForm.UnderTransformation( to ) )
						from.SendLocalizedMessage( 1049628 ); // You have no reason to throw a bola at that.
					else if ( !from.CanBeHarmful( to ) )
					{
					}
					else if ( from.BeginAction( typeof( Bola ) ) )
					{
						from.RevealingAction();

						EtherealMount.StopMounting( from );

						Item one = from.FindItemOnLayer( Layer.OneHanded );
						Item two = from.FindItemOnLayer( Layer.TwoHanded );

						if ( one != null )
							from.AddToBackpack( one );

						if ( two != null )
							from.AddToBackpack( two );

						from.DoHarmful( to );

						BaseMount.SetMountPrevention( from, BlockMountType.BolaRecovery, TimeSpan.FromSeconds( 10.0 ) );
						Timer.DelayCall( TimeSpan.FromSeconds( 10.0 ), new TimerStateCallback( ReleaseBolaLock ), from );
						Timer.DelayCall( TimeSpan.FromSeconds( 3.0 ), new TimerStateCallback( FinishThrow ), new object[] { from, to, m_Bola } );
					}
					else
					{
						from.SendLocalizedMessage( 1049624 ); // You have to wait a few moments before you can use another bola!
					}
				}
				else
				{
					from.SendLocalizedMessage( 1049629 ); // You cannot throw a bola at that.
				}
			}
		}

		public Bola( Serial serial )
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