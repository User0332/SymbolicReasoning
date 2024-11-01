using SymbolicReasoning.NewLogic.Objects;

namespace SymbolicReasoning.NewLogic.Statements;

public class BinaryRelationStatement(ILogicalEntity left, Relation op, ILogicalEntity right) : IStatement
{
	public int ArgsConsumed => 2;
	public readonly ILogicalEntity First = left;
	public readonly Relation Relation = op;
	public readonly ILogicalEntity Second = right;

	public BinaryRelationStatement(MatchEntity left, Relation op, MatchEntity right) : this((ILogicalEntity) left, op, right) { }

	public IStatement WithArgRef(ILogicalEntity[] args)
	{
		return new BinaryRelationStatement(args[0], Relation, args[1]);
	}

	public ILogicalEntity[] GetArgRef()
	{
		return [First, Second];
	}
}