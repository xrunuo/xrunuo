using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Server
{
	[AttributeUsage( AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum )]
	public class CustomEnumAttribute : Attribute
	{
		public string[] Names { get; private set; }

		public CustomEnumAttribute( string[] names )
		{
			Names = names;
		}
	}

	[AttributeUsage( AttributeTargets.Property )]
	public class CommandPropertyAttribute : Attribute
	{
		public AccessLevel ReadLevel { get; private set; }
		public AccessLevel WriteLevel { get; private set; }

		public CommandPropertyAttribute( AccessLevel level )
			: this( level, level )
		{
		}

		public CommandPropertyAttribute( AccessLevel readLevel, AccessLevel writeLevel )
		{
			ReadLevel = readLevel;
			WriteLevel = writeLevel;
		}
	}

	[AttributeUsage( AttributeTargets.Class | AttributeTargets.Struct )]
	public class PropertyObjectAttribute : Attribute
	{
		public PropertyObjectAttribute()
		{
		}
	}

	[AttributeUsage( AttributeTargets.Class | AttributeTargets.Struct )]
	public class NoSortAttribute : Attribute
	{
		public NoSortAttribute()
		{
		}
	}

	[AttributeUsage( AttributeTargets.Class | AttributeTargets.Struct )]
	public class ParsableAttribute : Attribute
	{
		public ParsableAttribute()
		{
		}
	}

	[AttributeUsage( AttributeTargets.Property )]
	public class HueAttribute : Attribute
	{
		public HueAttribute()
		{
		}
	}

	[AttributeUsage( AttributeTargets.Property )]
	public class BodyAttribute : Attribute
	{
		public BodyAttribute()
		{
		}
	}

	[AttributeUsage( AttributeTargets.Method )]
	public class CallPriorityAttribute : Attribute
	{
		private int m_Priority;

		public int Priority
		{
			get { return m_Priority; }
			set { m_Priority = value; }
		}

		public CallPriorityAttribute( int priority )
		{
			m_Priority = priority;
		}
	}

	public class CallPriorityComparer : IComparer<MethodInfo>
	{
		public static readonly CallPriorityComparer Instance = new CallPriorityComparer();

		public int Compare( MethodInfo a, MethodInfo b )
		{
			if ( a == null && b == null )
				return 0;

			if ( a == null )
				return 1;

			if ( b == null )
				return -1;

			return GetPriority( a ) - GetPriority( b );
		}

		private int GetPriority( MethodInfo mi )
		{
			object[] objs = mi.GetCustomAttributes( typeof( CallPriorityAttribute ), true );

			if ( objs == null )
				return 0;

			if ( objs.Length == 0 )
				return 0;

			CallPriorityAttribute attr = objs[0] as CallPriorityAttribute;

			if ( attr == null )
				return 0;

			return attr.Priority;
		}
	}

	[AttributeUsage( AttributeTargets.Class )]
	public class TypeAliasAttribute : Attribute
	{
		private string[] m_Aliases;

		public string[] Aliases
		{
			get
			{
				return m_Aliases;
			}
		}

		public TypeAliasAttribute( params string[] aliases )
		{
			m_Aliases = aliases;
		}
	}

	[AttributeUsage( AttributeTargets.Constructor )]
	public class ConstructableAttribute : Attribute
	{
		public ConstructableAttribute()
		{
		}
	}
}