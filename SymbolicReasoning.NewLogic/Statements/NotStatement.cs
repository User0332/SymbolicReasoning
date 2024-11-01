using SymbolicReasoning.NewLogic.Objects;

namespace SymbolicReasoning.NewLogic.Statements;

public class NotStatement(Statement inner) : Statement
{
	const int NotStatementHashMask = 12092491;
	public override int ArgsConsumed => Inner.ArgsConsumed;
	public readonly Statement Inner = inner;

	public override LogicalEntity[] GetArgRef()
	{
		return Inner.GetArgRef();
	}

	public override Statement WithArgRef(LogicalEntity[] args)
	{
		return new NotStatement(Inner.WithArgRef(args));
	}

	public override Statement Simplify()
	{
		if (Inner is NotStatement innerStmt)
		{
			return innerStmt.Inner; // the two not expressions cancel out
		}

		return this;
	}

	public override bool SchemaMatches(Statement other)
	{
		return other is NotStatement notStmt && notStmt.Inner.SchemaMatches(Inner);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(Inner, NotStatementHashMask);
	}
}