using SymbolicReasoning.NewLogic.Objects;

namespace SymbolicReasoning.NewLogic.Statements;

public class NotStatement(IStatement inner) : IStatement
{
	public int ArgsConsumed => Inner.ArgsConsumed;
	public readonly IStatement Inner = inner;

	public ILogicalEntity[] GetArgRef()
	{
		return Inner.GetArgRef();
	}

	public IStatement WithArgRef(ILogicalEntity[] args)
	{
		return new NotStatement(Inner.WithArgRef(args));
	}

	public IStatement Simplify()
	{
		if (Inner is NotStatement innerStmt)
		{
			return innerStmt.Inner; // the two not expressions cancel out
		}

		return this;
	}

	public bool SchemaMatches(IStatement other)
	{
		return other is NotStatement notStmt && notStmt.Inner.SchemaMatches(Inner);
	}
}