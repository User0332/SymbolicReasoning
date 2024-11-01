using SymbolicReasoning.NewLogic.Statements;

namespace SymbolicReasoning.NewLogic.Postulates;

public interface IPostulate
{
	public IStatement? ApplyTo(IStatement statement);
}