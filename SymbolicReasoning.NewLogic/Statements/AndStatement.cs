using SymbolicReasoning.NewLogic.Objects;

namespace SymbolicReasoning.NewLogic.Statements;

public class AndStatement(Statement left, Statement right) : Statement
{
	const int AndStatementHashMask = 10827;

	public override int ArgsConsumed => First.ArgsConsumed+Second.ArgsConsumed;

	public readonly Statement First = left;
	public readonly Statement Second = right;

	public override LogicalEntity[] GetArgRef()
	{
		return [..First.GetArgRef(), ..Second.GetArgRef()];
	}

	public override Statement WithArgRef(LogicalEntity[] args)
	{
		return new AndStatement(First.WithArgRef(args[..First.ArgsConsumed]), Second.WithArgRef(args[First.ArgsConsumed..]));
	}

	public override bool SchemaMatches(Statement other)
	{
		return other is AndStatement andStmt && andStmt.First.SchemaMatches(First) && andStmt.Second.SchemaMatches(Second);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(First, Second, AndStatementHashMask);
	}
}