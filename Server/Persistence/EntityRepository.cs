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
		public override string BasePath => World.MobileBasePath;

		public override string IndexPath => World.MobileIndexPath;

		public override string TypesPath => World.MobileTypesPath;

		public override string DataPath => World.MobileDataPath;

		public override void Initialize()
		{
			World.m_Mobiles = new Dictionary<Serial, Mobile>();
		}

		public override void Initialize( int count )
		{
			World.m_Mobiles = new Dictionary<Serial, Mobile>( count );
		}

		public override void AddObject( Mobile m )
		{
			World.AddMobile( m );
		}
	}

	public class ItemRepository : EntityRepository<Item>
	{
		public override string BasePath => World.ItemBasePath;

		public override string IndexPath => World.ItemIndexPath;

		public override string TypesPath => World.ItemTypesPath;

		public override string DataPath => World.ItemDataPath;

		public override void Initialize()
		{
			World.m_Items = new Dictionary<Serial, Item>();
		}

		public override void Initialize( int count )
		{
			World.m_Items = new Dictionary<Serial, Item>( count );
		}

		public override void AddObject( Item item )
		{
			World.AddItem( item );
		}
	}

	public class GuildRepository : EntityRepository<BaseGuild>
	{
		public override string BasePath => World.GuildBasePath;

		public override string IndexPath => World.GuildIndexPath;

		public override string TypesPath => World.GuildTypesPath;

		public override string DataPath => World.GuildDataPath;

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
