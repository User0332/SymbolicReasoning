using System.ComponentModel;
using System.Reflection;

namespace SymbolicReasoning.NewLogic.Statements;

public enum Relation
{
	[Description("E")]
	Belongs,
	[Description("==")]
	Identity,
}

public static class RelationExtensions
{
	public static string GetRepresentation(this Relation rel)
	{
		var memberInfo = typeof(Relation).GetMember(rel.ToString()).FirstOrDefault();

		if (memberInfo is null) return string.Empty;
		
		return memberInfo.GetCustomAttribute<DescriptionAttribute>()!.Description;
	}
}