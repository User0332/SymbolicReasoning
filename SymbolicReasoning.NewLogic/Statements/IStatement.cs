namespace SymbolicReasoning.NewLogic.Statements;

public interface IStatement
{
	public bool Negated { get; }
	public IStatement Negate();
}