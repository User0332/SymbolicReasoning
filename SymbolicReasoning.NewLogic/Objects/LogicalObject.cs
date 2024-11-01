namespace SymbolicReasoning.NewLogic.Objects;

public class LogicalObject(string ident) : ILogicalEntity
{
	public string Identifier => ident;
}