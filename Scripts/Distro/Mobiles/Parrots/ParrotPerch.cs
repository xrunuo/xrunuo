using System;
using System.Collections;
using Server;
using Server.Engines.Housing.Multis;
using Server.Items;
using Server.Targeting;
using Server.Multis;

namespace Server.Mobiles
{
	public class ParrotPerchAddonComponent : AddonComponent
	{
		public override int LabelNumber { get { return 1032214; } } // parrot perch

		[Constructable]
		public ParrotPerchAddonComponent()
			: base( 0x2FB6 )
		{
		}

		public ParrotPerchAddonComponent( Serial serial )
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

	public class ParrotPerchAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new ParrotPerchDeed(); } }
		public override int LabelNumber { get { return 1032214; } } // parrot perch

		private Parrot m_Parrot;
		private BaseHouse m_MyHouse;

		[CommandProperty( AccessLevel.GameMaster )]
		public Parrot Parrot
		{
			get { return m_Parrot; }
			set { m_Parrot = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public BaseHouse MyHouse
		{
			get { return m_MyHouse; }
			set { m_MyHouse = value; }
		}

		[Constructable]
		public ParrotPerchAddon()
		{
			AddComponent( new ParrotPerchAddonComponent(), 0, 0, 0 );
		}

		public override BaseAddonDeed Redeed()
		{
			ParrotPerchDeed deed = Deed as ParrotPerchDeed;

			if ( deed != null )
			{
				if ( m_Parrot != null )
				{
					deed.ContainsParrot = true;
					deed.ParrotName = m_Parrot.Name;
					deed.ParrotHue = m_Parrot.Hue;
					deed.ParrotBirthDate = m_Parrot.BirthDate;

					m_Parrot.Delete();
				}

				return deed;
			}

			return null;
		}

		public ParrotPerchAddon( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( m_Parrot );
			writer.Write( m_MyHouse );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_Parrot = reader.ReadMobile() as Parrot;
			m_MyHouse = reader.ReadItem() as BaseHouse;
		}
	}

	public class ParrotPerchDeed : BaseAddonDeed
	{
		private bool m_ContainsParrot;
		private string m_ParrotName;
		private int m_ParrotHue;
		private DateTime m_ParrotBirthDate;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool ContainsParrot
		{
			get { return m_ContainsParrot; }
			set { m_ContainsParrot = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public string ParrotName
		{
			get { return m_ParrotName; }
			set { m_ParrotName = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int ParrotHue
		{
			get { return m_ParrotHue; }
			set { m_ParrotHue = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime ParrotBirthDate
		{
			get { return m_ParrotBirthDate; }
			set { m_ParrotBirthDate = value; }
		}

		public override BaseAddon Addon { get { return new ParrotPerchAddon(); } }
		public override int LabelNumber { get { return 1072619; } } // A deed for a Parrot Perch

		[Constructable]
		public ParrotPerchDeed()
		{
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( m_ContainsParrot )
			{
				if ( m_ParrotName != "a pet parrot" )
					list.Add( 1072624, m_ParrotName ); // Includes a pet Parrot named ~1_NAME~
				else
					list.Add( 1072620 ); // Includes a pet Parrot

				int age = Parrot.GetAge( m_ParrotBirthDate );

				if ( age > 0 )
					list.Add( 1072627, age.ToString() ); // ~1_AGE~ weeks old
			}
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( IsChildOf( from.Backpack ) )
				from.Target = new InternalTarget( this );
			else
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
		}

		private class InternalTarget : Target
		{
			private ParrotPerchDeed m_Deed;

			public InternalTarget( ParrotPerchDeed deed )
				: base( -1, true, TargetFlags.None )
			{
				m_Deed = deed;

				CheckLOS = false;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				IPoint3D p = targeted as IPoint3D;
				Map map = from.Map;

				if ( p == null || map == null || m_Deed.Deleted )
					return;

				if ( m_Deed.IsChildOf( from.Backpack ) )
				{
					ParrotPerchAddon addon = m_Deed.Addon as ParrotPerchAddon;

					Server.Spells.SpellHelper.GetSurfaceTop( ref p );

					ArrayList houses = null;

					AddonFitResult res = addon.CouldFit( p, map, from, ref houses );

					if ( res == AddonFitResult.Valid )
						addon.MoveToWorld( new Point3D( p ), map );
					else if ( res == AddonFitResult.Blocked )
						from.SendLocalizedMessage( 500269 ); // You cannot build that there.
					else if ( res == AddonFitResult.NotInHouse )
						from.SendLocalizedMessage( 500274 ); // You can only place this in a house that you own!
					else if ( res == AddonFitResult.DoorsNotClosed )
						from.SendMessage( "You must close all house doors before placing this." );
					else if ( res == AddonFitResult.DoorTooClose )
						from.SendLocalizedMessage( 500271 ); // You cannot build near the door.
					else if ( res == AddonFitResult.NoWall )
						from.SendLocalizedMessage( 500268 ); // This object needs to be mounted on something.

					if ( res == AddonFitResult.Valid )
					{
						if ( houses != null )
						{
							foreach ( BaseHouse h in houses )
							{
								h.Addons.Add( addon );
								addon.MyHouse = h;
							}

							if ( m_Deed.m_ContainsParrot )
							{
								Parrot parrot = new Parrot( m_Deed.m_ParrotName, m_Deed.m_ParrotHue, m_Deed.m_ParrotBirthDate );

								parrot.MoveToWorld( new Point3D( addon.Location.X, addon.Location.Y, addon.Location.Z + 11 ), addon.Map );

								addon.Parrot = parrot;
								parrot.Perch = addon;
							}
						}

						m_Deed.Delete();
					}
					else
					{
						addon.Delete();
					}
				}
				else
				{
					from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
				}
			}
		}

		public ParrotPerchDeed( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( m_ContainsParrot );
			writer.Write( m_ParrotName );
			writer.Write( m_ParrotHue );
			writer.Write( m_ParrotBirthDate );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_ContainsParrot = reader.ReadBool();
			m_ParrotName = reader.ReadString();
			m_ParrotHue = reader.ReadInt();
			m_ParrotBirthDate = reader.ReadDateTime();
		}
	}
}