using System.ComponentModel;
using System.Reflection;

namespace SymbolicReasoning.NewLogic.Statements;

public enum BinaryRelation
{
	[Description("E")]
	Belongs,
	[Description("==")]
	Identity,
}

public static class BinaryRelationExtensions
{
	public static string GetRepresentation(this BinaryRelation rel)
	{
		var memberInfo = typeof(BinaryRelation).GetMember(rel.ToString()).FirstOrDefault();

		if (memberInfo is null) return string.Empty;
		
		return memberInfo.GetCustomAttribute<DescriptionAttribute>()!.Description;
	}
}