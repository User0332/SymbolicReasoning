using SymbolicReasoning.NewLogic.Statements;

namespace SymbolicReasoning.NewLogic.Postulates;

public class BiconditionalMatchPostulate(Statement first, Statement second)
	: MatchPostulate(first, second)
{
	public override bool Equals(IPostulate? otherPostulate)
	{
		return otherPostulate is BiconditionalMatchPostulate other && Predicate.Equals(other.Predicate) && Result.Equals(other.Result);
	}

	public override string ToString()
	{
		return $"{Predicate} <-> {Result}";
	}
}