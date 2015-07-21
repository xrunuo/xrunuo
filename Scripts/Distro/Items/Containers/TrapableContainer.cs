using System;

namespace Server.Items
{
	public enum TrapType
	{
		None,
		MagicTrap,
		ExplosionTrap,
		DartTrap,
		PoisonTrap
	}

	public abstract class TrapableContainer : BaseContainer, ITelekinesisable
	{
		private TrapType m_TrapType;
		private int m_TrapPower;
		private int m_TrapLevel;
		private bool m_TrapEnabled;

		[CommandProperty( AccessLevel.GameMaster )]
		public TrapType TrapType { get { return m_TrapType; } set { m_TrapType = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int TrapPower { get { return m_TrapPower; } set { m_TrapPower = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int TrapLevel { get { return m_TrapLevel; } set { m_TrapLevel = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public bool TrapEnabled { get { return m_TrapEnabled; } set { m_TrapEnabled = value; } }

		public TrapableContainer( int itemID )
			: base( itemID )
		{
		}

		public TrapableContainer( Serial serial )
			: base( serial )
		{
		}

		private void SendMessageTo( Mobile to, int number, int hue )
		{
			if ( Deleted || !to.CanSee( this ) )
				return;

			to.Send( new Network.MessageLocalized( Serial, ItemID, Network.MessageType.Regular, hue, 3, number, "", "" ) );
		}

		private void SendMessageTo( Mobile to, string text, int hue )
		{
			if ( Deleted || !to.CanSee( this ) )
				return;

			to.Send( new Network.UnicodeMessage( Serial, ItemID, Network.MessageType.Regular, hue, 3, "ENU", "", text ) );
		}

		/// <summary>
		/// Checks whether the given mobile will activate the container's trap or not.
		/// </summary>
		/// <param name="from">The mobile who triggered the trap.</param>
		/// <returns>true if the trap will be executed, false otherwise.</returns>
		public virtual bool CheckTrap( Mobile from )
		{
			if ( !m_TrapEnabled || m_TrapType == TrapType.None )
				return false;

			if ( from.AccessLevel >= AccessLevel.GameMaster )
			{
				SendMessageTo( from, "That is trapped, but you open it with your godly powers.", 0x3B2 );
				return false;
			}

			return true;
		}

		/// <summary>
		/// Executes the trap of this container, if any.
		/// </summary>
		/// <param name="from">The mobile who triggered the trap.</param>
		public virtual void ExecuteTrap( Mobile from )
		{
			Point3D loc = this.GetWorldLocation();
			Map facet = this.Map;

			switch ( m_TrapType )
			{
				case TrapType.ExplosionTrap:
					{
						if ( from.InRange( loc, 3 ) )
						{
							int damage;

							if ( m_TrapLevel > 0 )
								damage = Utility.RandomMinMax( 10, 30 ) * m_TrapLevel;
							else
								damage = m_TrapPower;

							AOS.Damage( from, damage, 0, 100, 0, 0, 0 );

							// Your skin blisters from the heat!
							from.LocalOverheadMessage( Network.MessageType.Regular, 0x2A, 503000 );
						}

						Effects.SendLocationEffect( loc, facet, 0x36BD, 15, 10 );
						Effects.PlaySound( loc, facet, 0x307 );

						break;
					}
				case TrapType.MagicTrap:
					{
						if ( from.InRange( loc, 1 ) )
							from.Damage( m_TrapPower );

						Effects.PlaySound( loc, Map, 0x307 );

						Effects.SendLocationEffect( new Point3D( loc.X - 1, loc.Y, loc.Z ), Map, 0x36BD, 15 );
						Effects.SendLocationEffect( new Point3D( loc.X + 1, loc.Y, loc.Z ), Map, 0x36BD, 15 );

						Effects.SendLocationEffect( new Point3D( loc.X, loc.Y - 1, loc.Z ), Map, 0x36BD, 15 );
						Effects.SendLocationEffect( new Point3D( loc.X, loc.Y + 1, loc.Z ), Map, 0x36BD, 15 );

						Effects.SendLocationEffect( new Point3D( loc.X + 1, loc.Y + 1, loc.Z + 11 ), Map, 0x36BD, 15 );

						break;
					}
				case TrapType.DartTrap:
					{
						if ( from.InRange( loc, 3 ) )
						{
							int damage;

							if ( m_TrapLevel > 0 )
								damage = Utility.RandomMinMax( 5, 15 ) * m_TrapLevel;
							else
								damage = m_TrapPower;

							AOS.Damage( from, damage, 100, 0, 0, 0, 0 );

							// A dart imbeds itself in your flesh!
							from.LocalOverheadMessage( Network.MessageType.Regular, 0x62, 502998 );
						}

						Effects.PlaySound( loc, facet, 0x223 );

						break;
					}
				case TrapType.PoisonTrap:
					{
						if ( from.InRange( loc, 3 ) )
						{
							Poison poison;

							if ( m_TrapLevel > 0 )
							{
								poison = Poison.GetPoison( Math.Max( 0, Math.Min( 4, m_TrapLevel - 1 ) ) );
							}
							else
							{
								AOS.Damage( from, m_TrapPower, 0, 0, 0, 100, 0 );
								poison = Poison.Greater;
							}

							from.ApplyPoison( from, poison );
						}

						Effects.SendLocationEffect( loc, facet, 0x113A, 10, 20 );
						Effects.PlaySound( loc, facet, 0x231 );

						break;
					}
			}
		}

		public void DisarmTrap( Mobile from )
		{
			SendMessageTo( from, 502999, 0x3B2 ); // You set off a trap!

			TrapType = TrapType.None;
			TrapPower = 0;
			TrapLevel = 0;
			TrapEnabled = false;
		}

		public virtual void OnTelekinesis( Mobile from )
		{
			Effects.SendLocationParticles( EffectItem.Create( Location, Map, EffectItem.DefaultDuration ), 0x376A, 9, 32, 5022 );
			Effects.PlaySound( Location, Map, 0x1F5 );

			if ( CheckTrap( from ) )
			{
				ExecuteTrap( from );
				DisarmTrap( from );
			}

			DisplayTo( from );
		}

		public override void Open( Mobile from )
		{
			if ( CheckTrap( from ) )
			{
				ExecuteTrap( from );
				DisarmTrap( from );
			}

			base.Open( from );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 3 ); // version

			writer.Write( (int) m_TrapLevel );
			writer.Write( (bool) m_TrapEnabled );
			writer.Write( (int) m_TrapPower );
			writer.Write( (int) m_TrapType );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 3:
					{
						m_TrapLevel = reader.ReadInt();
						goto case 2;
					}
				case 2:
					{
						m_TrapEnabled = reader.ReadBool();
						goto case 1;
					}
				case 1:
					{
						m_TrapPower = reader.ReadInt();
						goto case 0;
					}
				case 0:
					{
						m_TrapType = (TrapType) reader.ReadInt();
						break;
					}
			}
		}
	}
}