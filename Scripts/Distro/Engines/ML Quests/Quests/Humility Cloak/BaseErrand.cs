using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Collections;

namespace Server.Engines.Quests.HumilityCloak
{
	public abstract class BaseErrand : BaseVendor
	{
		private static Type[] m_ErrandTypes = new Type[]
			{
				typeof( Deirdre ),
				typeof( Jason ),
				typeof( Kevin ),
				typeof( Maribel ),
				typeof( Nelson ),
				typeof( Walton ),
				typeof( Sean )
			};

		private static DesireDefinition[] m_Desires = new DesireDefinition[]
			{
				new DesireDefinition( 1075778, typeof( BrassRing ) ),
				new DesireDefinition( 1075774, typeof( SeasonedSkillet ) ),
				new DesireDefinition( 1075775, typeof( VillageCauldron ) ),
				new DesireDefinition( 1075776, typeof( ShortStool ) ),
				new DesireDefinition( 1075777, typeof( FriendshipMug ) ),
				new DesireDefinition( 1075779, typeof( WornHammer ) ),
				new DesireDefinition( 1075780, typeof( PairOfWorkGloves ) ),
				new DesireDefinition( 1075788, typeof( IronChain ) )
			};

		public static DesireDefinition[] Desires { get { return m_Desires; } }
		public static Type[] ErrandTypes { get { return m_ErrandTypes; } }

		public abstract int BarkMessage { get; }
		public abstract int GumpMessage { get; }
		public abstract int DesireMessage { get; }
		public abstract int GiftMessage { get; }
		public abstract int GiftGivenMessage { get; }

		public override bool IsInvulnerable { get { return true; } }
		public override bool CanTeach { get { return false; } }

		public override void InitSBInfo() { }
		private ArrayList m_SBInfos = new ArrayList();
		protected override ArrayList SBInfos { get { return m_SBInfos; } }
		public override bool IsActiveVendor { get { return false; } }

		[Constructable]
		public BaseErrand( string name, string title )
			: base( title )
		{
			Name = name;
			SpeechHue = Utility.RandomDyedHue();

			InitStats( 100, 100, 25 );
		}

		public BaseErrand( Serial serial )
			: base( serial )
		{
		}

		public void AddItem( Item item, int hue )
		{
			item.Hue = hue;
			AddItem( item );
		}

		public override void OnMovement( Mobile m, Point3D oldLocation )
		{
			if ( !CheckCloak( m ) )
				return;

			if ( m.Alive && !m.Hidden && m is PlayerMobile )
			{
				PlayerMobile pm = (PlayerMobile) m;

				if ( this.InRange( m, 3 ) && !this.InRange( oldLocation, 3 ) )
				{
					DesireInfo info = DesireInfo.GetDesireFor( pm, this );

					if ( info != null && !info.GiftGiven )
						Say( BarkMessage );
				}
			}
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !CheckCloak( from ) )
				return;

			PlayerMobile pm = from as PlayerMobile;

			if ( pm != null && this.InRange( from, 3 ) )
			{
				DesireInfo info = DesireInfo.GetDesireFor( pm, this );

				if ( info != null && !info.GiftGiven )
					OnTalk( pm, info );
			}
		}

		public void OnTalk( PlayerMobile pm, DesireInfo info )
		{
			if ( pm.Backpack == null )
				return; // sanity

			Item desired = pm.Backpack.FindItemByType( info.Desire.Type );

			if ( desired != null )
			{
				Item toGive = Activator.CreateInstance( info.Gift.Type ) as Item;

				if ( toGive != null )
				{
					desired.Delete();
					pm.Backpack.DropItem( toGive );

					info.GiftGiven = true;

					SayThanks( info.Desire.Name, info.Gift.Name );
				}
			}
			else
			{
				pm.SendGump( new GenericQuestGump( GumpMessage, GenericQuestGumpButton.Close, OnGumpAccepted ) );
			}
		}

		private void OnGumpAccepted( Mobile m )
		{
			if ( this.InRange( m, 3 ) && m is PlayerMobile )
			{
				PlayerMobile pm = m as PlayerMobile;
				DesireInfo desire = DesireInfo.GetDesireFor( pm, this );

				if ( desire != null )
				{
					if ( desire.GivesDesireInfo )
						Say( DesireMessage, String.Format( "#{0}", desire.Desire.Name ) );
					else
						Say( GiftMessage, String.Format( "#{0}", desire.Gift.Name ) );
				}
			}
		}

		protected virtual void SayThanks( int desire, int gift )
		{
			Say( GiftGivenMessage, String.Format( "#{0}\t#{1}", desire, gift ) );
		}

		public static bool CheckCloak( Mobile m )
		{
			return m.FindItemOnLayer( Layer.Cloak ) is PlainGreyCloak;
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

	public class DesireDefinition
	{
		private int m_Name;
		private Type m_Type;

		public int Name { get { return m_Name; } }
		public Type Type { get { return m_Type; } }

		public DesireDefinition( int name, Type type )
		{
			m_Name = name;
			m_Type = type;
		}

		public void Serialize( DesireDefinition[] referenceTable, GenericWriter writer )
		{
			for ( int i = 0; i < referenceTable.Length; i++ )
			{
				if ( this == referenceTable[i] )
				{
					writer.WriteEncodedInt( 0x01 );
					writer.WriteEncodedInt( i );
					return;
				}
			}

			writer.WriteEncodedInt( 0x00 );
		}

		public static DesireDefinition Deserialize( DesireDefinition[] referenceTable, GenericReader reader )
		{
			int encoding = reader.ReadEncodedInt();

			if ( encoding == 0x01 )
				return referenceTable[reader.ReadEncodedInt()];

			return null;
		}
	}

	public class DesireInfo
	{
		private DesireDefinition m_Desire, m_Gift;
		private bool m_GivesDesireInfo, m_GiftGiven;

		public DesireDefinition Desire { get { return m_Desire; } }
		public DesireDefinition Gift { get { return m_Gift; } }
		public bool GivesDesireInfo { get { return m_GivesDesireInfo; } set { m_GivesDesireInfo = value; } }
		public bool GiftGiven { get { return m_GiftGiven; } set { m_GiftGiven = value; } }

		public DesireInfo( DesireDefinition desire, DesireDefinition gift, bool givesDesireInfo )
		{
			m_Desire = desire;
			m_Gift = gift;
			m_GivesDesireInfo = givesDesireInfo;
		}

		public DesireInfo( GenericReader reader )
		{
			m_Desire = DesireDefinition.Deserialize( BaseErrand.Desires, reader );
			m_Gift = DesireDefinition.Deserialize( BaseErrand.Desires, reader );

			m_GivesDesireInfo = reader.ReadBool();
			m_GiftGiven = reader.ReadBool();
		}

		public void Serialize( GenericWriter writer )
		{
			m_Desire.Serialize( BaseErrand.Desires, writer );
			m_Gift.Serialize( BaseErrand.Desires, writer );

			writer.Write( (bool) m_GivesDesireInfo );
			writer.Write( (bool) m_GiftGiven );
		}

		public static DesireInfo GetDesireFor( PlayerMobile pm, BaseErrand errand )
		{
			DesireCollection collection = WhosMostHumbleQuest.GetDesires( pm );
			Type errandType = errand.GetType();

			if ( collection != null && collection.Desires.ContainsKey( errandType ) )
				return collection.Desires[errandType];

			return null;
		}
	}

	public class DesireCollection
	{
		private Dictionary<Type, DesireInfo> m_Desires = new Dictionary<Type, DesireInfo>();

		public Dictionary<Type, DesireInfo> Desires
		{
			get { return m_Desires; }
		}

		private DesireCollection()
		{
		}

		/* Constructor de deserialización */
		public DesireCollection( GenericReader reader )
		{
			int count = reader.ReadInt();

			for ( int i = 0; i < count; i++ )
			{
				Type errandType = ReadType( BaseErrand.ErrandTypes, reader );
				DesireInfo info = new DesireInfo( reader );

				if ( errandType != null )
					m_Desires.Add( errandType, info );
			}
		}

		public void Serialize( GenericWriter writer )
		{
			writer.Write( (int) m_Desires.Count );

			foreach ( KeyValuePair<Type, DesireInfo> kvp in m_Desires )
			{
				WriteType( kvp.Key, BaseErrand.ErrandTypes, writer );
				kvp.Value.Serialize( writer );
			}
		}

		public static void WriteType( Type type, Type[] referenceTable, GenericWriter writer )
		{
			if ( type == null )
			{
				writer.WriteEncodedInt( (int) 0x00 );
			}
			else
			{
				for ( int i = 0; i < referenceTable.Length; ++i )
				{
					if ( referenceTable[i] == type )
					{
						writer.WriteEncodedInt( (int) 0x01 );
						writer.WriteEncodedInt( (int) i );
						return;
					}
				}

				writer.WriteEncodedInt( (int) 0x02 );
				writer.Write( type.FullName );
			}
		}

		public static Type ReadType( Type[] referenceTable, GenericReader reader )
		{
			int encoding = reader.ReadEncodedInt();

			switch ( encoding )
			{
				default:
				case 0x00: // null
					{
						return null;
					}
				case 0x01: // indexed
					{
						int index = reader.ReadEncodedInt();

						if ( index >= 0 && index < referenceTable.Length )
							return referenceTable[index];

						return null;
					}
				case 0x02: // by name
					{
						string fullName = reader.ReadString();

						if ( fullName == null )
							return null;

						return ScriptCompiler.FindTypeByFullName( fullName, false );
					}
			}
		}

		public static DesireCollection Build()
		{
			DesireCollection collection = new DesireCollection();

			/* Desordenamos los deseos, pero mantenemos el Brass Ring en primer lugar,
			 * ya que es el item que nos da Gareth al principio de la Quest, y el Iron
			 * Chain en último lugar, ya que es el que debemos devolverle al final. */
			DesireDefinition[] desires = Utility.Shuffle( BaseErrand.Desires, 1, BaseErrand.Desires.Length - 2 );

			/* Desordenamos los NPCs, pero mantenemos a Sean en último lugar, ya que él
			 * da siempre el último item que debemos devolverle a Gareth. */
			Type[] errandTypes = Utility.Shuffle( BaseErrand.ErrandTypes, 0, BaseErrand.ErrandTypes.Length - 1 );

			for ( int i = 0; i < errandTypes.Length; i++ )
			{
				Type errandType = errandTypes[i];
				DesireDefinition desire = desires[i];
				DesireDefinition gift = desires[i + 1];
				bool givesDesireInfo = Utility.RandomBool();

				collection.m_Desires.Add( errandType, new DesireInfo( desire, gift, givesDesireInfo ) );
			}

			/* Sean siempre da información sobre lo que quiere, no sobre lo que da,
			 * ya que siempre da el Iron Chain. */
			collection.m_Desires[typeof( Sean )].GivesDesireInfo = true;

			return collection;
		}
	}
}