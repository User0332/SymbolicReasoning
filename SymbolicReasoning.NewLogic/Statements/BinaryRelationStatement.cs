using SymbolicReasoning.NewLogic.Objects;

namespace SymbolicReasoning.NewLogic.Statements;

public class BinaryRelationStatement(ILogicalEntity left, BinaryRelation op, ILogicalEntity right) : IStatement
{
	public int ArgsConsumed => 2;
	public readonly ILogicalEntity First = left;
	public readonly BinaryRelation Relation = op;
	public readonly ILogicalEntity Second = right;

	public BinaryRelationStatement(MatchEntity left, BinaryRelation op, MatchEntity right) : this((ILogicalEntity) left, op, right) { }

	public IStatement WithArgRef(ILogicalEntity[] args)
	{
		return new BinaryRelationStatement(args[0], Relation, args[1]);
	}

	public ILogicalEntity[] GetArgRef()
	{
		return [First, Second];
	}
}