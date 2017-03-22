using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Server.Persistence
{
	public delegate object CtorDelegate( Serial serial );

	public struct EntityType
	{
		private string m_Name;
		private ConstructorInfo m_CtorInfo;
		private CtorDelegate m_CtorDelegate;

		public string Name { get { return m_Name; } }
		public ConstructorInfo Constructor { get { return m_CtorInfo; } }
		public CtorDelegate CtorDelegate { get { return m_CtorDelegate; } }

		public EntityType( string name, ConstructorInfo ctorInfo )
		{
			m_Name = name;
			m_CtorInfo = ctorInfo;
			m_CtorDelegate = CreateCtorDelegate( ctorInfo );
		}

		private static readonly Type[] m_CtorTypes = new[] { typeof( Serial ) };

		private static CtorDelegate CreateCtorDelegate( ConstructorInfo cInfo )
		{
			DynamicMethod dynamic = new DynamicMethod( string.Empty, typeof( object ), m_CtorTypes, cInfo.DeclaringType );
			ILGenerator il = dynamic.GetILGenerator();

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
		private ISerializable m_Entity;
		private int m_TypeId;
		private string m_TypeName;
		private long m_Position;
		private int m_Length;

		public ISerializable Object
		{
			get { return m_Entity; }
		}

		public Serial Serial
		{
			get { return m_Entity == null ? Serial.MinusOne : m_Entity.SerialIdentity; }
		}

		public int TypeId
		{
			get { return m_TypeId; }
		}

		public string TypeName
		{
			get { return m_TypeName; }
		}

		public long Position
		{
			get { return m_Position; }
		}

		public int Length
		{
			get { return m_Length; }
		}

		public EntityEntry( ISerializable entity, int typeId, string typeName, long pos, int length )
		{
			m_Entity = entity;
			m_TypeId = typeId;
			m_TypeName = typeName;
			m_Position = pos;
			m_Length = length;
		}

		public void Clear()
		{
			m_Entity = null;
			m_TypeName = null;
		}
	}
}
