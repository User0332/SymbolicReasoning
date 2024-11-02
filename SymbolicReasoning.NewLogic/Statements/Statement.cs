using SymbolicReasoning.NewLogic.Objects;

namespace SymbolicReasoning.NewLogic.Statements;

public abstract class Statement : IEquatable<Statement>
{
	public abstract int ArgsConsumed { get; }

	public abstract Statement WithArgRef(LogicalEntity[] args);
	public abstract LogicalEntity[] GetArgRef();
	public virtual Statement Simplify() => this;
	public virtual bool SchemaMatches(Statement other) => other.GetType() == GetType();
	
	public bool Equals(Statement? other)
	{
		return other is not null && SchemaMatches(other) && GetArgRef().SequenceEqual(other.GetArgRef());
	}

	public override bool Equals(object? obj)
	{
		return Equals(obj as Statement);
	}

	public abstract override int GetHashCode();

	public Statement Negate() => new NotStatement(this).Simplify();
	public Statement And(Statement other) => new AndStatement(this, other).Simplify();
}