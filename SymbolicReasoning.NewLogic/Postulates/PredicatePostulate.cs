using System.Security.Cryptography.X509Certificates;
using SymbolicReasoning.NewLogic.Objects;
using SymbolicReasoning.NewLogic.Statements;

namespace SymbolicReasoning.NewLogic.Postulates;

public class MatchPostulate(
	IStatement predicate, IStatement result
) : IPostulate
{
	public readonly IStatement Predicate = predicate;
	public readonly IStatement Result = result;

	public IStatement? ApplyTo(IStatement statement)
	{
		if (statement.GetType() != Predicate.GetType()) return null;

		// match all indices

		var argRef = statement.GetArgRef();

		var predicateArgRef = Predicate.GetArgRef();

		Queue<ILogicalEntity> matches = [];

		for (int i = 0; i < argRef.Length; i++)
		{
			if (predicateArgRef[i] is MatchEntity)
			{
				matches.Enqueue(argRef[0]);
				continue;
			}
			
			if (argRef[i] != predicateArgRef[i]) return null;
		}

		var resultArgRef = Result.GetArgRef();
		List<ILogicalEntity> newArgRef = [];

		for (int i = 0; i < resultArgRef.Length; i++)
		{
			if (resultArgRef[i] is MatchEntity)
			{
				newArgRef.Add(matches.Dequeue());
				continue;
			}

			newArgRef.Add(resultArgRef[i]);
		}

		return Result.WithArgRef([..newArgRef]);
	}
}