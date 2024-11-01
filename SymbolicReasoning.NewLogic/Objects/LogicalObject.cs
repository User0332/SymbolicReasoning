namespace SymbolicReasoning.NewLogic.Objects;

public class LogicalObject(string ident) : LogicalEntity
{
	public override string Identifier => ident;
}