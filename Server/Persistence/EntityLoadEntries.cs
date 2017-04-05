using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Server.Persistence
{
	public delegate object CtorDelegate( Serial serial );

	public struct EntityType
	{
		public string Name { get; }
		public ConstructorInfo Constructor { get; }
		public CtorDelegate CtorDelegate { get; }

		public EntityType( string name, ConstructorInfo ctorInfo )
		{
			Name = name;
			Constructor = ctorInfo;
			CtorDelegate = CreateCtorDelegate( ctorInfo );
		}

		private static readonly Type[] m_CtorTypes = { typeof( Serial ) };

		private static CtorDelegate CreateCtorDelegate( ConstructorInfo cInfo )
		{
			var dynamic = new DynamicMethod( string.Empty, typeof( object ), m_CtorTypes, cInfo.DeclaringType );
			var il = dynamic.GetILGenerator();

			il.DeclareLocal( cInfo.DeclaringType );
			il.Emit( OpCodes.Ldarg_0 );
			il.Emit( OpCodes.Newobj, cInfo );
			il.Emit( OpCodes.Ret );

			return (CtorDelegate) dynamic.CreateDelegate( typeof( CtorDelegate ) );
		}
	}

	public interface IEntityEntry
	{
		Serial Serial { get; }
		int TypeId { get; }
		long Position { get; }
		int Length { get; }
		ISerializable Object { get; }
		void Clear();
	}

	public struct EntityEntry : IEntityEntry
	{
		public ISerializable Object { get; private set; }
		public int TypeId { get; }
		public string TypeName { get; private set; }
		public long Position { get; }
		public int Length { get; }

		public Serial Serial => Object?.SerialIdentity ?? Serial.MinusOne;

		public EntityEntry( ISerializable entity, int typeId, string typeName, long pos, int length )
		{
			Object = entity;
			TypeId = typeId;
			TypeName = typeName;
			Position = pos;
			Length = length;
		}

		public void Clear()
		{
			Object = null;
			TypeName = null;
		}
	}
}
