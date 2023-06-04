using System;
using JetBrains.Annotations;

namespace Framework
{
	[AttributeUsage(AttributeTargets.Method)]
	[MeansImplicitUse(ImplicitUseKindFlags.Assign)]
	public class InjectAttribute : Attribute
	{
	}
}