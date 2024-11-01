using SymbolicReasoning.NewLogic.Objects;

namespace SymbolicReasoning.NewLogic.Statements;

public class BinaryRelationStatement(LogicalEntity left, BinaryRelation op, LogicalEntity right) : Statement
{
	public override int ArgsConsumed => 2;
	public readonly LogicalEntity First = left;
	public readonly BinaryRelation Relation = op;
	public readonly LogicalEntity Second = right;

	public BinaryRelationStatement(MatchEntity left, BinaryRelation op, MatchEntity right) : this((LogicalEntity) left, op, right) { }

	public override Statement WithArgRef(LogicalEntity[] args)
	{
		return new BinaryRelationStatement(args[0], Relation, args[1]);
	}

	public override LogicalEntity[] GetArgRef()
	{
		return [First, Second];
	}

	public override int GetHashCode()
	{		
		return HashCode.Combine(First, Relation, Second);
	}

	public override string ToString()
	{
		return $"{First} {Relation.GetRepresentation()} {Second}";
	}
}