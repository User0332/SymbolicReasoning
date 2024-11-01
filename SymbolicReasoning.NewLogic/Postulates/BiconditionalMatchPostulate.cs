using SymbolicReasoning.NewLogic.Statements;

namespace SymbolicReasoning.NewLogic.Postulates;

public class BiconditionalMatchPostulate(Statement first, Statement second)
	: MatchPostulate(first, second)
{
	public override bool Equals(IPostulate? otherPostulate)
	{
		return otherPostulate is BiconditionalMatchPostulate other && Predicate == other.Predicate && Result == other.Result;
	}
}