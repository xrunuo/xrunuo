using System;

namespace Server.Testing
{
	[AttributeUsage( AttributeTargets.Class )]
	public class TestCaseAttribute : Attribute
	{
	}

	[AttributeUsage( AttributeTargets.Method )]
	public class TestAttribute : Attribute
	{
	}
}
