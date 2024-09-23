using SymbolicReasoning.Objects;

namespace SymbolicReasoning.Logic.Statements;

public class ObjectBelongingStatement(LogicalObject obj, LogicalSet set, bool negated = false) : IStatement // first E second
{
	const int HashMask = 12084;

	public readonly LogicalObject Object = obj;
	public readonly LogicalSet Set = set;

	public IEnumerable<LogicalObject> ReferencedObjects { get; } = [obj];
	public IEnumerable<LogicalSet> ReferencedSets { get; } = [set];
	public bool IsNegated { get; } = negated;

	public bool Equals(IStatement? stmt)
	{
		if (stmt is ObjectBelongingStatement other)
		{
			return Object == other.Object && Set == other.Set && IsNegated == other.IsNegated;
		}

		return false;
	}

	public override string ToString()
	{
		if (IsNegated) return $"{Object} !E {Set}";

		return $"{Object} E {Set}";
	}

	public override int GetHashCode()
	{
		return ((Object.GetHashCode()+Set.GetHashCode())^HashMask)+Convert.ToByte(IsNegated);
	}

	public IStatement Negate()
	{
		return new ObjectBelongingStatement(Object, Set, !IsNegated);
	}

	public void Apply()
	{
		if (!IsNegated)
		{
			Set.AddMember(Object);
		}
	}
}