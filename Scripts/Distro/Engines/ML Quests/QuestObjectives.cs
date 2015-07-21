using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Server;
using Server.Items;
using Server.Mobiles;
using Server.Regions;

namespace Server.Engines.Quests
{
	public class BaseObjective
	{
		private BaseQuest m_Quest;
		private int m_MaxProgress;
		private int m_CurProgress;
		private int m_Seconds;
		private bool m_Timed;

		public BaseQuest Quest
		{
			get { return m_Quest; }
			set { m_Quest = value; }
		}

		public int MaxProgress
		{
			get { return m_MaxProgress; }
			set { m_MaxProgress = value; }
		}

		public int CurProgress
		{
			get { return m_CurProgress; }
			set
			{
				m_CurProgress = value;

				if ( Completed )
					OnCompleted();

				if ( m_CurProgress == -1 )
					OnFailed();

				if ( m_CurProgress < -1 )
					m_CurProgress = -1;
			}
		}

		public int Seconds
		{
			get { return m_Seconds; }
			set
			{
				m_Seconds = value;

				if ( m_Seconds < 0 )
					m_Seconds = 0;
			}
		}

		public bool Timed
		{
			get { return m_Timed; }
			set { m_Timed = value; }
		}

		public bool Completed
		{
			get { return CurProgress >= MaxProgress; }
		}

		public bool Failed
		{
			get { return CurProgress == -1; }
		}

		public BaseObjective()
			: this( 1, 0 )
		{
		}

		public BaseObjective( int maxProgress )
			: this( maxProgress, 0 )
		{
		}

		public BaseObjective( int maxProgress, int seconds )
		{
			m_MaxProgress = maxProgress;
			m_Seconds = seconds;

			if ( seconds > 0 )
				Timed = true;
			else
				Timed = false;
		}

		public virtual void Complete()
		{
			CurProgress = MaxProgress;
		}

		public virtual void Fail()
		{
			CurProgress = -1;
		}

		public virtual void OnAccept()
		{
		}

		public virtual void OnCompleted()
		{
		}

		public virtual void OnFailed()
		{
		}

		public virtual Type Type()
		{
			return null;
		}

		public virtual bool Update( object obj )
		{
			return false;
		}

		public virtual void UpdateTime()
		{
			if ( !Timed || Failed )
				return;

			if ( Seconds > 0 )
			{
				Seconds -= 1;
			}
			else if ( !Completed )
			{
				m_Quest.Owner.SendLocalizedMessage( 1072258 ); // You failed to complete an objective in time!

				Fail();
			}
		}

		public virtual void Serialize( GenericWriter writer )
		{
			writer.WriteEncodedInt( (int) 0 ); // version

			writer.Write( (int) m_CurProgress );
			writer.Write( (int) m_Seconds );
		}

		public virtual void Deserialize( GenericReader reader )
		{
			/*int version = */
			reader.ReadEncodedInt();

			m_CurProgress = reader.ReadInt();
			m_Seconds = reader.ReadInt();
		}
	}

	public class SlayObjective : BaseObjective
	{
		private Type[] m_Creatures;
		private string m_Name;
		private Region m_Region;

		public Type[] Creatures
		{
			get { return m_Creatures; }
			set { m_Creatures = value; }
		}

		public string Name
		{
			get { return m_Name; }
			set { m_Name = value; }
		}

		public Region Region
		{
			get { return m_Region; }
			set { m_Region = value; }
		}

		public SlayObjective( Type creature, string name, int amount )
			: this( new Type[] { creature }, name, amount )
		{
		}

		public SlayObjective( Type[] creatures, string name, int amount )
			: this( creatures, name, amount, null )
		{
		}

		public SlayObjective( Type creature, string name, int amount, string region )
			: this( new Type[] { creature }, name, amount, region )
		{
		}

		public SlayObjective( Type[] creatures, string name, int amount, string region )
			: this( creatures, name, amount, region, 0 )
		{
		}

		public SlayObjective( Type creature, string name, int amount, int seconds )
			: this( new Type[] { creature }, name, amount, null, seconds )
		{
		}

		public SlayObjective( Type[] creatures, string name, int amount, int seconds )
			: this( creatures, name, amount, null, seconds )
		{
		}

		public SlayObjective( Type creature, string name, int amount, string region, int seconds )
			: this( new Type[] { creature }, name, amount, region, seconds )
		{
		}

		public SlayObjective( Type[] creatures, string name, int amount, string region, int seconds )
			: base( amount, seconds )
		{
			m_Creatures = creatures;
			m_Name = name;

			if ( region != null )
			{
				m_Region = QuestHelper.FindRegion( region );

				if ( m_Region == null )
					Console.WriteLine( String.Format( "Invalid region name ('{0}') in '{1}' objective!", region, GetType() ) );
			}
		}

		public virtual void OnKill( Mobile killed )
		{
			if ( Completed )
				Quest.Owner.SendLocalizedMessage( 1075050 ); // You have killed all the required quest creatures of this type.
			else
				Quest.Owner.SendLocalizedMessage( 1075051, ( MaxProgress - CurProgress ).ToString() ); // You have killed a quest creature. ~1_val~ more left.
		}

		public virtual bool IsObjective( Mobile mob )
		{
			if ( m_Creatures == null )
				return false;

			if ( m_Region != null && !m_Region.Contains( mob.Location ) )
				return false;

			for ( int i = 0; i < m_Creatures.Length; i++ )
			{
				Type creature = m_Creatures[i];

				if ( creature.IsAssignableFrom( mob.GetType() ) )
					return true;
			}

			return false;
		}

		public override bool Update( object obj )
		{
			if ( obj is Mobile )
			{
				Mobile mob = (Mobile) obj;

				if ( IsObjective( mob ) )
				{
					if ( !Completed )
						CurProgress += 1;

					OnKill( mob );
					return true;
				}
			}

			return false;
		}

		public override Type Type()
		{
			if ( m_Creatures == null || m_Creatures.Length == 0 )
				return null;

			return m_Creatures[0];
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}

	public class ObtainObjective : BaseObjective
	{
		private Type m_Obtain;
		private string m_Name;
		private int m_Image;

		public Type Obtain
		{
			get { return m_Obtain; }
			set { m_Obtain = value; }
		}

		public string Name
		{
			get { return m_Name; }
			set { m_Name = value; }
		}

		public int Image
		{
			get { return m_Image; }
			set { m_Image = value; }
		}

		public ObtainObjective( Type obtain, string name, int amount )
			: this( obtain, name, amount, 0, 0 )
		{
		}

		public ObtainObjective( Type obtain, string name, int amount, int image )
			: this( obtain, name, amount, image, 0 )
		{
		}

		public ObtainObjective( Type obtain, string name, int amount, int image, int seconds )
			: base( amount, seconds )
		{
			m_Obtain = obtain;
			m_Name = name;
			m_Image = image;
		}

		public virtual int ProgressMessage { get { return -1; } }

		public override bool Update( object obj )
		{
			if ( obj is Item )
			{
				Item obtained = (Item) obj;

				if ( IsObjective( obtained ) )
				{
					if ( !obtained.QuestItem )
					{
						CurProgress += obtained.Amount;

						obtained.QuestItem = true;
						Quest.Owner.SendLocalizedMessage( 1072353 ); // You set the item to Quest Item status

						if ( ProgressMessage != -1 )
							Quest.Owner.SendLocalizedMessage( ProgressMessage );
					}
					else
					{
						CurProgress -= obtained.Amount;

						obtained.QuestItem = false;
						Quest.Owner.SendLocalizedMessage( 1072354 ); // You remove Quest Item status from the item
					}

					return true;
				}
			}

			return false;
		}

		public virtual bool IsObjective( Item item )
		{
			if ( m_Obtain == null )
				return false;

			if ( m_Obtain.IsAssignableFrom( item.GetType() ) )
				return true;

			return false;
		}

		public override Type Type()
		{
			return m_Obtain;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}

	public class DeliverObjective : BaseObjective
	{
		private Type m_Delivery;
		private String m_DeliveryName;

		private Type m_Destination;
		private String m_DestName;

		public Type Delivery
		{
			get { return m_Delivery; }
			set { m_Delivery = value; }
		}

		public String DeliveryName
		{
			get { return m_DeliveryName; }
			set { m_DeliveryName = value; }
		}

		public Type Destination
		{
			get { return m_Destination; }
			set { m_Destination = value; }
		}

		public String DestName
		{
			get { return m_DestName; }
			set { m_DestName = value; }
		}

		public DeliverObjective( Type delivery, String deliveryName, int amount, Type destination, String destName )
			: this( delivery, deliveryName, amount, destination, destName, 0 )
		{
		}

		public DeliverObjective( Type delivery, String deliveryName, int amount, Type destination, String destName, int seconds )
			: base( amount, seconds )
		{
			m_Delivery = delivery;
			m_DeliveryName = deliveryName;

			m_Destination = destination;
			m_DestName = destName;
		}

		public override void OnAccept()
		{
			if ( Quest.StartingItem != null )
			{
				Quest.StartingItem.QuestItem = true;
				return;
			}

			int amount = MaxProgress;

			while ( amount > 0 && !Failed )
			{
				Item item = QuestHelper.Construct( m_Delivery ) as Item;

				if ( item == null )
				{
					Fail();
					break;
				}

				if ( item.Stackable )
				{
					item.Amount = amount;
					amount = 1;
				}

				if ( !Quest.Owner.PlaceInBackpack( item ) )
				{
					Quest.Owner.SendLocalizedMessage( 503200 ); // You do not have room in your backpack for this.
					Quest.Owner.SendLocalizedMessage( 1075574 ); // Could not create all the necessary items. Your quest has not advanced.

					Fail();

					break;
				}
				else
					item.QuestItem = true;

				amount -= 1;
			}

			if ( Failed )
			{
				QuestHelper.DeleteItems( Quest.Owner, m_Delivery, MaxProgress - amount, false );

				Quest.RemoveQuest();
			}
		}

		public override bool Update( object obj )
		{
			if ( m_Delivery == null || m_Destination == null )
				return false;

			if ( Failed )
			{
				Quest.Owner.SendLocalizedMessage( 1074813 );  // You have failed to complete your delivery.
				return false;
			}

			if ( obj is BaseVendor )
			{
				if ( Quest.StartingItem != null )
				{
					Complete();
					return true;
				}
				else if ( m_Destination.IsAssignableFrom( obj.GetType() ) )
				{
					if ( MaxProgress < QuestHelper.CountQuestItems( Quest.Owner, Delivery ) )
					{
						Quest.Owner.SendLocalizedMessage( 1074813 );  // You have failed to complete your delivery.						
						Fail();
					}
					else
						Complete();

					return true;
				}
			}

			return false;
		}

		public override Type Type()
		{
			return m_Delivery;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}

	public class EscortObjective : BaseObjective
	{
		private Region m_Region;
		private int m_Fame;
		private int m_Compassion;

		public Region Region
		{
			get { return m_Region; }
			set { m_Region = value; }
		}

		public int Fame
		{
			get { return m_Fame; }
			set { m_Fame = value; }
		}

		public int Compassion
		{
			get { return m_Compassion; }
			set { m_Compassion = value; }
		}

		public EscortObjective( string region )
			: this( region, 10, 200, 0 )
		{
		}

		public EscortObjective( string region, int fame )
			: this( region, fame, 200 )
		{
		}

		public EscortObjective( string region, int fame, int compassion )
			: this( region, fame, compassion, 0 )
		{
		}

		public EscortObjective( string region, int fame, int compassion, int seconds )
			: base( 1, seconds )
		{
			m_Region = QuestHelper.FindRegion( region );
			m_Fame = fame;
			m_Compassion = compassion;

			if ( m_Region == null )
				Console.WriteLine( String.Format( "Invalid region name ('{0}') in '{1}' objective!", region, GetType() ) );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}

	public class ApprenticeObjective : BaseObjective
	{
		private SkillName m_Skill;
		private Region m_Region;
		private object m_Enter;
		private object m_Leave;

		public SkillName Skill
		{
			get { return m_Skill; }
			set { m_Skill = value; }
		}

		public Region Region
		{
			get { return m_Region; }
			set { m_Region = value; }
		}

		public object Enter
		{
			get { return m_Enter; }
			set { m_Enter = value; }
		}

		public object Leave
		{
			get { return m_Leave; }
			set { m_Leave = value; }
		}

		public ApprenticeObjective( SkillName skill, int cap )
			: this( skill, cap, null, null, null )
		{
		}

		public ApprenticeObjective( SkillName skill, int cap, string region, object enterRegion, object leaveRegion )
			: base( cap )
		{
			m_Skill = skill;

			if ( region != null )
			{
				m_Region = QuestHelper.FindRegion( region );
				m_Enter = enterRegion;
				m_Leave = leaveRegion;

				if ( m_Region == null )
					Console.WriteLine( String.Format( "Invalid region name ('{0}') in '{1}' objective!", region, GetType() ) );
			}
		}

		public override bool Update( object obj )
		{
			if ( Completed )
				return false;

			if ( obj is Skill )
			{
				Skill skill = (Skill) obj;

				if ( skill.SkillName != m_Skill )
					return false;

				if ( Quest.Owner.Skills[m_Skill].Base >= MaxProgress )
				{
					Complete();
					return true;
				}
			}

			return false;
		}

		public override void OnAccept()
		{
			Region region = Quest.Owner.Region;

			while ( region != null )
			{
				if ( region is ApprenticeRegion )
					region.OnEnter( Quest.Owner );

				region = region.Parent;
			}
		}

		public override void OnCompleted()
		{
			QuestHelper.RemoveAcceleratedSkillgain( Quest.Owner );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}

	public abstract class BaseBardObjective : BaseObjective
	{
		public abstract int ProgressMessage { get; }

		private int m_Cliloc;
		private Type[] m_TargetTypes;
		private List<Mobile> m_Targets;

		public int Cliloc
		{
			get { return m_Cliloc; }
			set { m_Cliloc = value; }
		}

		public Type[] TargetTypes
		{
			get { return m_TargetTypes; }
			set { m_TargetTypes = value; }
		}

		public BaseBardObjective( Type targetType, int cliloc, int amount )
			: this( new Type[] { targetType }, cliloc, amount )
		{
		}

		public BaseBardObjective( Type[] targetTypes, int cliloc, int amount )
			: base( amount, 0 )
		{
			m_TargetTypes = targetTypes;
			m_Cliloc = cliloc;
			m_Targets = new List<Mobile>();
		}

		public override bool Update( object obj )
		{
			if ( obj is Mobile )
			{
				Mobile target = (Mobile) obj;

				if ( IsObjective( target ) )
				{
					CurProgress++;

					m_Targets.Add( target );

					if ( ProgressMessage != -1 )
					{
						int remaining = Math.Max( 0, ( MaxProgress - CurProgress ) );
						Quest.Owner.SendLocalizedMessage( 1116560, string.Format( "#{0}\t{1}", ProgressMessage, remaining.ToString() ) ); // ~1_val~ ~2_val~
					}

					return true;
				}
			}

			return false;
		}

		public bool IsObjective( Mobile m )
		{
			if ( !m_TargetTypes.Any( t => t.IsAssignableFrom( m.GetType() ) ) )
				return false;

			if ( m_Targets.Contains( m ) )
				return false;

			return true;
		}

		public override Type Type()
		{
			return m_TargetTypes[0];
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( (int) 0 ); // version

			writer.Write( m_Targets );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();

			m_Targets = reader.ReadStrongMobileList();
		}
	}

	public class DiscordObjective : BaseBardObjective
	{
		// Creatures remaining to be discorded: 
		public override int ProgressMessage { get { return 1115749; } }

		public DiscordObjective( Type discord, int cliloc, int amount )
			: base( discord, cliloc, amount )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}

	public class InciteObjective : BaseBardObjective
	{
		// Conflicts remaining to be incited: 
		public override int ProgressMessage { get { return 1115748; } }

		private Type[] m_SourceTypes;

		public Type[] SourceTypes
		{
			get { return m_SourceTypes; }
			set { m_SourceTypes = value; }
		}

		public InciteObjective( Type[] sourceTypes, Type targetType, int cliloc, int amount )
			: base( targetType, cliloc, amount )
		{
			m_SourceTypes = sourceTypes;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}

	public class CalmObjective : BaseBardObjective
	{
		// Creatures remaining to be calmed: 
		public override int ProgressMessage { get { return 1115747; } }

		public CalmObjective( Type[] targetTypes, int cliloc, int amount )
			: base( targetTypes, cliloc, amount )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}
}