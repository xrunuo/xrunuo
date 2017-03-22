using System.Collections.Generic;
using Server.Guilds;

namespace Server.Persistence
{
	public interface IEntityRepository
	{
		IEntityRepositoryLoad CreateEntityRepositoryLoad();
	}

	public abstract class EntityRepository<T> : IEntityRepository where T : class, ISerializable
	{
		public abstract string BasePath { get; }
		public abstract string IndexPath { get; }
		public abstract string TypesPath { get; }
		public abstract string DataPath { get; }

		public abstract void Initialize();
		public abstract void Initialize( int count );
		public abstract void AddObject( T entity );

		public virtual IEntityRepositoryLoad CreateEntityRepositoryLoad()
		{
			return new EntityRepositoryLoad<T>( this );
		}
	}

	public class MobileRepository : EntityRepository<Mobile>
	{
		public override string BasePath { get { return World.MobileBasePath; } }
		public override string IndexPath { get { return World.MobileIndexPath; } }
		public override string TypesPath { get { return World.MobileTypesPath; } }
		public override string DataPath { get { return World.MobileDataPath; } }

		public override void Initialize()
		{
			World.Instance.m_Mobiles = new Dictionary<Serial, Mobile>();
		}

		public override void Initialize( int count )
		{
			World.Instance.m_Mobiles = new Dictionary<Serial, Mobile>( count );
		}

		public override void AddObject( Mobile m )
		{
			World.Instance.AddMobile( m );
		}
	}

	public class ItemRepository : EntityRepository<Item>
	{
		public override string BasePath { get { return World.ItemBasePath; } }
		public override string IndexPath { get { return World.ItemIndexPath; } }
		public override string TypesPath { get { return World.ItemTypesPath; } }
		public override string DataPath { get { return World.ItemDataPath; } }

		public override void Initialize()
		{
			World.Instance.m_Items = new Dictionary<Serial, Item>();
		}

		public override void Initialize( int count )
		{
			World.Instance.m_Items = new Dictionary<Serial, Item>( count );
		}

		public override void AddObject( Item item )
		{
			World.Instance.AddItem( item );
		}
	}

	public class GuildRepository : EntityRepository<BaseGuild>
	{
		public override string BasePath { get { return World.GuildBasePath; } }
		public override string IndexPath { get { return World.GuildIndexPath; } }
		public override string TypesPath { get { return World.GuildTypesPath; } }
		public override string DataPath { get { return World.GuildDataPath; } }

		public override void Initialize()
		{
		}

		public override void Initialize( int count )
		{
		}

		public override void AddObject( BaseGuild guild )
		{
		}
	}
}
