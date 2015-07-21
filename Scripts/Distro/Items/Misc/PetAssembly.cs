using System;
using Server;
using Server.Mobiles;
using Server.Spells;

namespace Server.Items
{
	public class MechanicalPetReq
	{
		private Type m_Type;
		private int m_Amount;
		private int m_Message;

		public Type Type { get { return m_Type; } }
		public int Amount { get { return m_Amount; } }
		public int Message { get { return m_Message; } }

		public MechanicalPetReq( Type type, int amount, int message )
		{
			m_Type = type;
			m_Amount = amount;
			m_Message = message;
		}
	}

	public abstract class PetAssembly : Item
	{
		public abstract MechanicalPetReq[] Requirements { get; }
		public abstract Type PetType { get; }

		[Constructable]
		public PetAssembly()
			: base( 0x1EA8 )
		{
			Weight = 10.0;
		}

		public PetAssembly( Serial serial )
			: base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( from.Backpack == null || !IsChildOf( from.Backpack ) )
			{
				// That must be in your pack for you to use it.
				from.SendLocalizedMessage( 1042001 );
			}
			else if ( from.Skills[SkillName.Tinkering].Value < 60.0 )
			{
				// You must be a Journeyman or higher Tinker to construct a mechanical pet.
				from.SendLocalizedMessage( 1113054 );
			}
			else if ( ( from.Followers + 2 ) > from.FollowersMax )
			{
				// You have too many followers to control that creature.
				from.SendLocalizedMessage( 1049607 );
			}
			else
			{
				Type[] types = null;
				int[] amounts = null;

				BuildTypes( ref types, ref amounts );

				int res = from.Backpack.ConsumeTotal( types, amounts );

				if ( res != -1 )
				{
					from.SendLocalizedMessage( Requirements[res].Message );
				}
				else
				{
					BaseCreature pet = (BaseCreature) Activator.CreateInstance( PetType );
					pet.IsGolem = true;

					if ( pet.SetControlMaster( from ) )
					{
						Delete();

						pet.MoveToWorld( from.Location, from.Map );
						from.PlaySound( 0x241 );
					}
				}
			}
		}

		private void BuildTypes( ref Type[] types, ref int[] amounts )
		{
			types = new Type[Requirements.Length];
			amounts = new int[Requirements.Length];

			for ( int i = 0; i < Requirements.Length; i++ )
			{
				MechanicalPetReq req = Requirements[i];

				types[i] = req.Type;
				amounts[i] = req.Amount;
			}
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

	public class ClockworkScorpionAssembly : PetAssembly
	{
		private static MechanicalPetReq[] m_Reqs = new MechanicalPetReq[]
			{
				new MechanicalPetReq( typeof( BronzeIngot ), 100, 1113060 ),
				new MechanicalPetReq( typeof( Gears ), 100, 1113061 )
			};

		public override MechanicalPetReq[] Requirements { get { return m_Reqs; } }

		public override Type PetType { get { return typeof( ClockworkScorpion ); } }

		public override int LabelNumber { get { return 1113032; } } // clockwork scorpion assembly

		[Constructable]
		public ClockworkScorpionAssembly()
		{
		}

		public ClockworkScorpionAssembly( Serial serial )
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

	public class LeatherWolfAssembly : PetAssembly
	{
		private static MechanicalPetReq[] m_Reqs = new MechanicalPetReq[]
			{
				new MechanicalPetReq( typeof( Leather ), 100, 1113058 ),
				new MechanicalPetReq( typeof( OilFlask ), 1, 1113059 )
			};

		public override MechanicalPetReq[] Requirements { get { return m_Reqs; } }

		public override Type PetType { get { return typeof( LeatherWolf ); } }

		public override int LabelNumber { get { return 1113031; } } // leather wolf assembly

		[Constructable]
		public LeatherWolfAssembly()
		{
		}

		public LeatherWolfAssembly( Serial serial )
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

	public class VollemAssembly : PetAssembly
	{
		private static MechanicalPetReq[] m_Reqs = new MechanicalPetReq[]
			{
				new MechanicalPetReq( typeof( Leather ), 50, 1113058 ),
				new MechanicalPetReq( typeof( WhiteScales ), 25, 1113062 )
			};

		public override MechanicalPetReq[] Requirements { get { return m_Reqs; } }

		public override Type PetType { get { return typeof( Vollem ); } }

		public override int LabelNumber { get { return 1113033; } } // vollem assembly

		[Constructable]
		public VollemAssembly()
		{
		}

		public VollemAssembly( Serial serial )
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