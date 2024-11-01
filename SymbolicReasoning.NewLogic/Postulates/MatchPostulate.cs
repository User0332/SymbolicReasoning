using SymbolicReasoning.NewLogic.Objects;
using SymbolicReasoning.NewLogic.Statements;

namespace SymbolicReasoning.NewLogic.Postulates;

public class MatchPostulate(
	Statement predicate, Statement result
) : IPostulate
{
	public readonly Statement Predicate = predicate;
	public readonly Statement Result = result;

	public Statement? ApplyTo(Statement statement)
	{
		if (!statement.SchemaMatches(Predicate)) return null;

		var argRef = statement.GetArgRef();

		var predicateArgRef = Predicate.GetArgRef();

		Dictionary<string, LogicalEntity> matches = [];

		for (int i = 0; i < argRef.Length; i++)
		{
			if (predicateArgRef[i] is MatchEntity match)
			{
				matches[match.Identifier] = argRef[0];
				continue;
			}
			
			if (!argRef[i].Equals(predicateArgRef[i])) return null; // statement signatures don't match
		}

		var resultArgRef = Result.GetArgRef();
		List<LogicalEntity> newArgRef = [];

		for (int i = 0; i < resultArgRef.Length; i++)
		{
			if (resultArgRef[i] is MatchEntity match)
			{
				matches.Remove(match.Identifier, out var matchedEntity);

				if (matchedEntity is null) return null;

				newArgRef.Add(matchedEntity);

				continue;
			}

			newArgRef.Add(resultArgRef[i]);
		}

		return Result.WithArgRef([..newArgRef]);
	}

	public virtual bool Equals(IPostulate? otherPostulate)
	{
		return otherPostulate is MatchPostulate other && Predicate == other.Predicate && Result == other.Result;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(GetType(), Predicate, Result);
	}
}