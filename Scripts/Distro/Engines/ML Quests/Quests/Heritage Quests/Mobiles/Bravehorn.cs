using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
	public class Bravehorn : BaseEscort
	{
		public override bool InitialInnocent { get { return true; } }
		public override bool IsInvulnerable { get { return false; } }

		public override Type[] Quests
		{
			get
			{
				return new Type[] 
		{
			typeof( DefendingTheHerdQuest )
		};
			}
		}

		[Constructable]
		public Bravehorn()
			: base()
		{
			Name = "Bravehorn";
		}

		public Bravehorn( Serial serial )
			: base( serial )
		{
		}

		public override bool CanBeDamaged()
		{
			return true;
		}

		public override void InitBody()
		{
			InitStats( 100, 100, 25 );

			Blessed = false;
			Female = false;
			Body = 0xEA;
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
}

namespace Server.Mobiles
{
	public class BravehornsMate : Hind
	{
		[Constructable]
		public BravehornsMate()
			: base()
		{
			Name = "bravehorn's mate";
			Tamable = false;
		}

		public BravehornsMate( Serial serial )
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
}