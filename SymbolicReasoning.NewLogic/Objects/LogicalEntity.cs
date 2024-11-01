namespace SymbolicReasoning.NewLogic.Objects;

public abstract class LogicalEntity : IEquatable<LogicalEntity>
{
	public abstract string Identifier { get; }

	public bool Equals(LogicalEntity? other)
	{
		return other is not null && GetType() == other.GetType() && Identifier == other.Identifier;
	}

	public override bool Equals(object? obj)
	{
		return Equals(obj as LogicalEntity);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(
			GetType(),
			Identifier
		);
	}
}