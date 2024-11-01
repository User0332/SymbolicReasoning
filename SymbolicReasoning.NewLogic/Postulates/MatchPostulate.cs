using SymbolicReasoning.NewLogic.Statements;

namespace SymbolicReasoning.NewLogic.Postulates;

public class MatchPostulate(IStatement match, IStatement result) : IPostulate
{
	public IStatement Match => match;

	public IStatement Result => result;

	public bool AppliesTo(IStatement statement)
	{
		throw new NotImplementedException();
	}

	public IStatement ApplyTo(IStatement statement)
	{
		throw new NotImplementedException();
	}
}