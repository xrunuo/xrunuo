/*
Script Name: GMHidingStone.cs
Author: CEO
Version: 1.0
Public Release: 06/05/04
Purpose: A stone that allows for multiple hide/appear effects for counselors and above.
*/
using System;
using Server;
using Server.Misc;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
	public enum StoneEffect
	{
		FlameStrike1, FlameStrike3, FlameStrikeLightningBolt, Sparkle1, Sparkle3, Explosion, ExplosionLightningBolt, DefaultRunUO
	}

	public class GMHidingStone : Item
	{

		public StoneEffect mAppearEffect;
		public StoneEffect mHideEffect;

		[CommandProperty( AccessLevel.Counselor )]
		public StoneEffect AppearEffect
		{
			get
			{
				return mAppearEffect;
			}
			set
			{
				if ( ( value <= StoneEffect.DefaultRunUO ) && ( value >= StoneEffect.FlameStrike1 ) )
				{
					mAppearEffect = value;
				}
				else
				{
					return;
				};
			}
		}
		[CommandProperty( AccessLevel.Counselor )]
		public StoneEffect HideEffect
		{
			get
			{
				return mHideEffect;
			}
			set
			{
				if ( ( value <= StoneEffect.DefaultRunUO ) && ( value >= StoneEffect.FlameStrike1 ) )
				{
					mHideEffect = value;
				}
				else
				{
					return;
				};
			}
		}

		[Constructable]
		public GMHidingStone()
			: base( 0x1870 )
		{
			Weight = 1.0;
			Hue = 0x0;
			Name = "GM hiding stone";
			LootType = LootType.Blessed;
			mAppearEffect = StoneEffect.DefaultRunUO;
			mHideEffect = StoneEffect.DefaultRunUO;
		}

		public override void OnDoubleClick( Mobile m )
		{
			if ( m.AccessLevel > AccessLevel.Player )
			{
				if ( m.Hidden )
				{
					m.Hidden = false;
					SendStoneEffects( mAppearEffect, m );
				}
				else
				{
					SendStoneEffects( mHideEffect, m );
					m.Hidden = true;
				}
			}
			else
			{
				m.SendMessage( "You are unable to use that!" );
			}
		}

		public void SendStoneEffects( StoneEffect mStoneEffect, Mobile m )
		{
			switch ( mStoneEffect )
			{
				case StoneEffect.FlameStrike1:
					Effects.SendLocationEffect( new Point3D( m.X, m.Y, m.Z + 1 ), m.Map, 0x3709, 15 );
					Effects.PlaySound( new Point3D( m.X, m.Y, m.Z ), m.Map, 0x208 );
					break;
				case StoneEffect.FlameStrike3:
					Effects.SendLocationEffect( new Point3D( m.X, m.Y, m.Z + 1 ), m.Map, 0x3709, 15 );
					Effects.SendLocationEffect( new Point3D( m.X + 1, m.Y, m.Z + 6 ), m.Map, 0x3709, 15 );
					Effects.SendLocationEffect( new Point3D( m.X, m.Y + 1, m.Z + 6 ), m.Map, 0x3709, 15 );
					Effects.PlaySound( new Point3D( m.X, m.Y, m.Z ), m.Map, 0x208 );
					break;
				case StoneEffect.FlameStrikeLightningBolt:
					Effects.SendLocationEffect( new Point3D( m.X, m.Y, m.Z + 1 ), m.Map, 0x3709, 15 );
					Effects.PlaySound( new Point3D( m.X, m.Y, m.Z ), m.Map, 0x208 );
					Effects.SendBoltEffect( m, true, 0 );
					break;
				case StoneEffect.Sparkle1:
					Effects.SendLocationEffect( new Point3D( m.X, m.Y, m.Z + 1 ), m.Map, 0x375A, 15 );
					Effects.PlaySound( new Point3D( m.X, m.Y, m.Z ), m.Map, 0x213 );
					break;
				case StoneEffect.Sparkle3:
					Effects.SendLocationEffect( new Point3D( m.X, m.Y + 1, m.Z ), m.Map, 0x373A, 15 );
					Effects.SendLocationEffect( new Point3D( m.X + 1, m.Y, m.Z ), m.Map, 0x373A, 15 );
					Effects.SendLocationEffect( new Point3D( m.X, m.Y, m.Z - 1 ), m.Map, 0x373A, 15 );
					Effects.PlaySound( new Point3D( m.X, m.Y, m.Z ), m.Map, 0x213 );
					break;
				case StoneEffect.Explosion:
					Effects.SendLocationEffect( new Point3D( m.X, m.Y, m.Z + 1 ), m.Map, 0x36BD, 15 );
					Effects.SendLocationEffect( new Point3D( m.X + 1, m.Y, m.Z ), m.Map, 0x36BD, 15 );
					Effects.SendLocationEffect( new Point3D( m.X + 1, m.Y + 1, m.Z ), m.Map, 0x36BD, 15 );
					Effects.SendLocationEffect( new Point3D( m.X, m.Y + 1, m.Z ), m.Map, 0x36BD, 15 );
					Effects.PlaySound( new Point3D( m.X, m.Y, m.Z ), m.Map, 0x307 );
					break;
				case StoneEffect.ExplosionLightningBolt:
					Effects.SendLocationEffect( new Point3D( m.X, m.Y, m.Z + 1 ), m.Map, 0x36BD, 15 );
					Effects.SendLocationEffect( new Point3D( m.X + 1, m.Y, m.Z ), m.Map, 0x36BD, 15 );
					Effects.SendLocationEffect( new Point3D( m.X + 1, m.Y + 1, m.Z ), m.Map, 0x36BD, 15 );
					Effects.SendLocationEffect( new Point3D( m.X, m.Y + 1, m.Z ), m.Map, 0x36BD, 15 );
					Effects.SendBoltEffect( m, true, 0 );
					Effects.PlaySound( new Point3D( m.X, m.Y, m.Z ), m.Map, 0x307 );
					break;
				case StoneEffect.DefaultRunUO:
					Effects.SendLocationEffect( new Point3D( m.X + 1, m.Y, m.Z + 4 ), m.Map, 0x3728, 13 );
					Effects.SendLocationEffect( new Point3D( m.X + 1, m.Y, m.Z ), m.Map, 0x3728, 13 );
					Effects.SendLocationEffect( new Point3D( m.X + 1, m.Y, m.Z - 4 ), m.Map, 0x3728, 13 );
					Effects.SendLocationEffect( new Point3D( m.X, m.Y + 1, m.Z + 4 ), m.Map, 0x3728, 13 );
					Effects.SendLocationEffect( new Point3D( m.X, m.Y + 1, m.Z ), m.Map, 0x3728, 13 );
					Effects.SendLocationEffect( new Point3D( m.X, m.Y + 1, m.Z - 4 ), m.Map, 0x3728, 13 );
					Effects.SendLocationEffect( new Point3D( m.X + 1, m.Y + 1, m.Z + 11 ), m.Map, 0x3728, 13 );
					Effects.SendLocationEffect( new Point3D( m.X + 1, m.Y + 1, m.Z + 7 ), m.Map, 0x3728, 13 );
					Effects.SendLocationEffect( new Point3D( m.X + 1, m.Y + 1, m.Z + 3 ), m.Map, 0x3728, 13 );
					Effects.SendLocationEffect( new Point3D( m.X + 1, m.Y + 1, m.Z - 1 ), m.Map, 0x3728, 13 );
					Effects.PlaySound( new Point3D( m.X, m.Y, m.Z ), m.Map, 0x228 );
					break;
			}
		}

		public GMHidingStone( Serial serial )
			: base( serial )
		{

		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 1 ); // version 
			writer.Write( (int) mAppearEffect );
			writer.Write( (int) mHideEffect );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			/*int version = */
			reader.ReadInt();
			mAppearEffect = (StoneEffect) reader.ReadInt();
			mHideEffect = (StoneEffect) reader.ReadInt();
		}

	}
}
