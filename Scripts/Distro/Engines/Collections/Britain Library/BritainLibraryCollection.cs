using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Collections
{
	public abstract class BritainLibraryCollection : CollectionController
	{
		public abstract int Section { get; }

		private BaseLibraryRepresentative m_Representative;

		[CommandProperty( AccessLevel.Developer )]
		public BaseLibraryRepresentative Representative
		{
			get { return m_Representative; }
			set { m_Representative = value; }
		}

		public override int PointsPerTier { get { return 10000000; } }
		public override int MaxTiers { get { return 1; } }

		public static readonly int[] ClothHues = new int[]
			{
				337, 400, 480
			};

		public static readonly int[] BookHues = new int[]
			{
				0, 450, 800, 400, 480
			};

		public BritainLibraryCollection()
		{
			Visible = false;
			Timer.DelayCall( TimeSpan.FromMilliseconds( 100.0 ), new TimerCallback( Spawn_Representative ) );
		}

		public abstract void Spawn_Representative();

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( Section );
		}

		public BritainLibraryCollection( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );

			writer.Write( m_Representative );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_Representative = reader.ReadMobile() as BaseLibraryRepresentative;
		}
	}
}