using SymbolicReasoning.NewLogic.Objects;
using SymbolicReasoning.NewLogic.Statements;

namespace SymbolicReasoning.NewLogic.Postulates;

public class MatchPostulate(
	Statement predicate, Statement result
) : IPostulate
{
	public readonly Statement Predicate = predicate.Simplify();
	public readonly Statement Result = result.Simplify();

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
		return otherPostulate is MatchPostulate other && Predicate.Equals(other.Predicate) && Result.Equals(other.Result);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(GetType(), Predicate, Result);
	}

	public IPostulate? TryChainTo(IPostulate next)
	{
		if (next is not MatchPostulate nextMatcher) return null;

		if (nextMatcher.Result.Equals(Predicate)) return null; // prevent infinite chaining

		if (!nextMatcher.Predicate.Equals(Result)) return null; // not directly chainable

		return new MatchPostulate(Predicate, nextMatcher.Result);
	}

	public override string ToString()
	{
		return $"{Predicate} -> {Result}";
	}

	public static bool MatchesPredicate(Statement predicate, Statement stmt)
	{
		if (!predicate.SchemaMatches(stmt)) return false;

		var argRef = stmt.GetArgRef();

		var predicateArgRef = predicate.GetArgRef();

		for (int i = 0; i < argRef.Length; i++)
		{
			if (predicateArgRef[i] is MatchEntity) continue;
			
			if (!argRef[i].Equals(predicateArgRef[i])) return false; // statement signatures don't match
		}

		return true;
	}

	public static (bool Matches, AndStatement ResolvedStatement) GetMatchForAndPredicate(AndStatement predicate, IEnumerable<Statement> statements)
	{
		foreach (var stmt in statements)
		{
			Statement other;

			if (MatchesPredicate(predicate.First, stmt)) other = predicate.Second;
			else if (MatchesPredicate(predicate.Second, stmt)) other = predicate.First;
			else continue;

			if (other is AndStatement innerPredicate)
			{
				var (matches, resolvedStatement) = GetMatchForAndPredicate(innerPredicate, statements);

				if (!matches) return (false, null!);

				return (true, new(stmt, resolvedStatement));
			}
			else
			{
				foreach (var secondStmt in statements)
				{
					if (MatchesPredicate(other, secondStmt)) return (true, new(stmt, secondStmt));
				}
			}
		}

		return (false, null!);
	}
}