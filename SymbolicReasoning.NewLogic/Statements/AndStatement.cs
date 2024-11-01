using SymbolicReasoning.NewLogic.Objects;

namespace SymbolicReasoning.NewLogic.Statements;

public sealed class AndStatement : Statement
{
	const int AndStatementHashMask = 10827;

	public override int ArgsConsumed => First.ArgsConsumed+Second.ArgsConsumed;

	public readonly Statement First;
	public readonly Statement Second;

	readonly int hashCode;

	public AndStatement(Statement left, Statement right)
	{
		var leftSimpl = left.Simplify();
		var rightSimpl = right.Simplify();

		var leftHashCode = leftSimpl.GetHashCode();
		var rightHashCode = rightSimpl.GetHashCode();

		if (leftHashCode < rightHashCode)
		{
			First = leftSimpl;
			Second = rightSimpl;

			hashCode = HashCode.Combine(leftHashCode, rightHashCode, AndStatementHashMask);
		}
		else
		{
			First = rightSimpl;
			Second = leftSimpl;
		
			hashCode = HashCode.Combine(rightHashCode, leftHashCode, AndStatementHashMask);		
		}
	}

	// Private/Already simplified constituent ctor
	AndStatement(Statement leftSimpl, Statement rightSimpl, bool _)
	{
		var leftHashCode = leftSimpl.GetHashCode();
		var rightHashCode = rightSimpl.GetHashCode();

		if (leftHashCode < rightHashCode)
		{
			First = leftSimpl;
			Second = rightSimpl;

			hashCode = HashCode.Combine(leftHashCode, rightHashCode, AndStatementHashMask);
		}
		else
		{
			First = rightSimpl;
			Second = leftSimpl;
		
			hashCode = HashCode.Combine(rightHashCode, leftHashCode, AndStatementHashMask);		
		}
	}

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
		return
			other is AndStatement andStmt && 
			(
				(andStmt.First.SchemaMatches(First) && andStmt.Second.SchemaMatches(Second)) ||
				(andStmt.Second.SchemaMatches(First) && andStmt.First.SchemaMatches(Second))
			);
	}

	public override int GetHashCode()
	{
		return hashCode;
	}

	internal List<Statement> GetConstituentStatements()
	{
		List<Statement> constituents = [First, Second];

		if (First is AndStatement firstStmt)
		{
			constituents.AddRange(firstStmt.GetConstituentStatements());
			constituents.Remove(First);
		}

		if (Second is AndStatement secondStmt)
		{
			constituents.AddRange(secondStmt.GetConstituentStatements());
			constituents.Remove(Second);
		}

		return constituents;
	}

	public override string ToString()
	{
		return $"({First}) && ({Second})";
	}

	public override Statement Simplify() // computationally heavy right now
	{
		if ((First is not AndStatement) && (Second is not AndStatement)) return this;

		var constituents = GetConstituentStatements();
		var distinctConstituents = constituents.Distinct().ToArray();

		if (constituents.Count == distinctConstituents.Length) return this;

		AndStatement buildingStmt = new(distinctConstituents[0], distinctConstituents[1], true);

		for (int i = 2; i < distinctConstituents.Length; i++)
		{
			buildingStmt = new(buildingStmt, distinctConstituents[i], true);
		}

		return buildingStmt;
	}
}