using SymbolicReasoning.Objects;

namespace SymbolicReasoning.Logic.Statements;

public class SetIdentityStatement(LogicalSet first, LogicalSet second, bool negated = false) : IStatement // first <=> second
{
	const int HashMask = 975972021;
	
	public readonly LogicalSet First = first;
	public readonly LogicalSet Second = second;

	public IEnumerable<LogicalObject> ReferencedObjects { get; } = [];
	public IEnumerable<LogicalSet> ReferencedSets { get; } = [first, second];
	public bool IsNegated { get; } = negated;

	public bool Equals(IStatement? stmt)
	{
		if (stmt is SetIdentityStatement other)
		{
			return ((First == other.First && Second == other.Second) || (First == other.Second && Second == other.First)) && (IsNegated == other.IsNegated);
		}

		return false;
	}

	public override string ToString()
	{
		if (IsNegated) return $"{First} != {Second}";

		return $"{First} <=> {Second}";
	}

	public override int GetHashCode()
	{
		return ((First.GetHashCode()+Second.GetHashCode())^HashMask)+Convert.ToByte(IsNegated);
	}

	public IStatement Negate()
	{
		return new SetIdentityStatement(First, Second, !IsNegated);
	}
}