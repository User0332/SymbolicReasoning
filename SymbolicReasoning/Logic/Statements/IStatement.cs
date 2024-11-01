using System.Runtime.CompilerServices;
using SymbolicReasoning.Objects;

namespace SymbolicReasoning.Logic.Statements;

public interface IStatement : IEquatable<IStatement>
{
	public IEnumerable<LogicalObject> ReferencedObjects { get; }
	public IEnumerable<LogicalSet> ReferencedSets { get; }

	public bool IsNegated { get; }

	public IStatement Negate();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IStatement Lie(IStatement original) => original.Negate();
}