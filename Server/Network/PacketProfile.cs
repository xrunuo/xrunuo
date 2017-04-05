using System;

namespace Server.Network
{
	public class PacketProfile
	{
		[CommandProperty( AccessLevel.Administrator )]
		public bool Outgoing { get; }

		[CommandProperty( AccessLevel.Administrator )]
		public int Constructed { get; private set; }

		[CommandProperty( AccessLevel.Administrator )]
		public int TotalByteLength { get; private set; }

		[CommandProperty( AccessLevel.Administrator )]
		public TimeSpan TotalProcTime { get; private set; }

		[CommandProperty( AccessLevel.Administrator )]
		public TimeSpan PeakProcTime { get; private set; }

		[CommandProperty( AccessLevel.Administrator )]
		public int Count { get; private set; }

		[CommandProperty( AccessLevel.Administrator )]
		public double AverageByteLength
		{
			get
			{
				if ( Count == 0 )
					return 0;

				return Math.Round( (double) TotalByteLength / Count, 2 );
			}
		}

		[CommandProperty( AccessLevel.Administrator )]
		public TimeSpan AverageProcTime
		{
			get
			{
				if ( Count == 0 )
					return TimeSpan.Zero;

				return TimeSpan.FromTicks( TotalProcTime.Ticks / Count );
			}
		}

		public void Record( int byteLength, TimeSpan processTime )
		{
			++Count;
			TotalByteLength += byteLength;
			TotalProcTime += processTime;

			if ( processTime > PeakProcTime )
				PeakProcTime = processTime;
		}

		public void RegConstruct()
		{
			++Constructed;
		}

		public PacketProfile( bool outgoing )
		{
			Outgoing = outgoing;
		}

		public static PacketProfile GetOutgoingProfile( int packetID )
		{
			if ( !Core.Profiling )
				return null;

			var prof = OutgoingProfiles[packetID];

			if ( prof == null )
				OutgoingProfiles[packetID] = prof = new PacketProfile( true );

			return prof;
		}

		public static PacketProfile GetIncomingProfile( int packetID )
		{
			if ( !Core.Profiling )
				return null;

			var prof = IncomingProfiles[packetID];

			if ( prof == null )
				IncomingProfiles[packetID] = prof = new PacketProfile( false );

			return prof;
		}

		public static PacketProfile[] OutgoingProfiles { get; }

		public static PacketProfile[] IncomingProfiles { get; }

		static PacketProfile()
		{
			OutgoingProfiles = new PacketProfile[0x100];
			IncomingProfiles = new PacketProfile[0x100];
		}
	}
}
