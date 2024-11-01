using SymbolicReasoning.NewLogic.Objects;

namespace SymbolicReasoning.NewLogic.Statements;

public class AndStatement(IStatement left, IStatement right) : IStatement
{
	public int ArgsConsumed => First.ArgsConsumed+Second.ArgsConsumed;

	public readonly IStatement First = left;
	public readonly IStatement Second = right;

	public ILogicalEntity[] GetArgRef()
	{
		return [..First.GetArgRef(), ..Second.GetArgRef()];
	}

	public IStatement WithArgRef(ILogicalEntity[] args)
	{
		return new AndStatement(First.WithArgRef(args[..First.ArgsConsumed]), Second.WithArgRef(args[First.ArgsConsumed..]));
	}
}