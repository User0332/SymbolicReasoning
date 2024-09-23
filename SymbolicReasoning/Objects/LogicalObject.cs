namespace SymbolicReasoning.Objects;

public class LogicalObject(string identifier) : IEquatable<LogicalObject>
{
	const int HashMask = 983737722;
	
	public readonly string Identifier = identifier;

	public bool Equals(LogicalObject? other)
	{
		return other?.Identifier == Identifier;
	}

	public override string ToString()
	{
		return $"{Identifier}";
	}

	public override int GetHashCode()
	{
		return Identifier.GetHashCode()^HashMask;
	}

	public override bool Equals(object? obj)
	{
		return Equals(obj as LogicalObject);
	}
}