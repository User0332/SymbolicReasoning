namespace SymbolicReasoning.NewLogic.Objects;

public class MatchEntity(string ident) : LogicalEntity
{
	public override string Identifier => ident;
}