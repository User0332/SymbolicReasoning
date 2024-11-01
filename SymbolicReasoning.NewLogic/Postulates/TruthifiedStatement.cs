using SymbolicReasoning.NewLogic.Statements;

namespace SymbolicReasoning.NewLogic.Postulates;

public class TruthifiedStatement(IStatement result) : IPostulate
{
	public readonly IStatement Statement = result;

	public IStatement? ApplyTo(IStatement statement)
	{
		return Statement;
	}
}