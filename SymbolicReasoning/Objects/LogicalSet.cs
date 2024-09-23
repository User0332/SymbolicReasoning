namespace SymbolicReasoning.Objects;

public class LogicalSet(string identifier) : IEquatable<LogicalSet>
{
	const int HashMask = 18928773;

	public readonly string Identifier = identifier;

	public bool Equals(LogicalSet? other)
	{
		return other?.Identifier == Identifier;
	}

	public override string ToString()
	{
		return $"{Identifier}{{}}";
	}

	public override int GetHashCode()
	{
		return Identifier.GetHashCode()^HashMask;
	}

	public override bool Equals(object? obj)
	{
		return Equals(obj as LogicalSet);
	}
}