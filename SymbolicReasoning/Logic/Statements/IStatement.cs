using SymbolicReasoning.Objects;

namespace SymbolicReasoning.Logic.Statements;

public interface IStatement : IEquatable<IStatement>
{
	public IEnumerable<LogicalObject> ReferencedObjects { get; }
	public IEnumerable<LogicalSet> ReferencedSets { get; }

	public bool IsNegated { get; }

	public IStatement Negate();
}