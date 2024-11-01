namespace SymbolicReasoning.NewLogic.Objects;

public class MatchEntity(string ident = "") : ILogicalEntity
{
	public string Identifier => $"Match Entity {ident}";
}