using System.Runtime.InteropServices;
using SymbolicReasoning.NewLogic.Axioms;
using SymbolicReasoning.NewLogic.Objects;
using SymbolicReasoning.NewLogic.Postulates;
using SymbolicReasoning.NewLogic.Statements;

namespace SymbolicReasoning.NewLogic;

public class SimpleReasoningEngine(KnowledgeBase knowledgeBase)
{
	public readonly KnowledgeBase KnowledgeBase = knowledgeBase;

	public SimpleReasoningEngine() : this(new()) { }

	public void ForwardChainPostulatesOneGen()
	{
		var postulatesCopy = KnowledgeBase.Postulates.ToArray();

		foreach (var postulate in postulatesCopy)
		{
			foreach (var axiom in DefaultAxioms.AsEnumerable)
			{
				var newPostulate = axiom.Apply(postulate);
				
				if (newPostulate is not null) KnowledgeBase.Postulates.Add(newPostulate);
			}

			foreach (var otherPostulate in postulatesCopy)
			{
				var newPostulate = postulate.TryChainTo(otherPostulate);

				if (newPostulate is not null) KnowledgeBase.Postulates.Add(newPostulate);
			}
		}
	}

	public void ForwardChainPostulatesAllGens()
	{
		int lastNumPostulates = 0;
		int numPostulates = KnowledgeBase.Postulates.Count;

		while (numPostulates > lastNumPostulates)
		{
			lastNumPostulates = numPostulates;

			ForwardChainPostulatesOneGen();
			
			numPostulates = KnowledgeBase.Postulates.Count;
		}
	}

	public void ForwardChainStatementsOneGen()
	{
		var stmtsCopy = KnowledgeBase.Statements.ToArray();

		foreach (var stmt in stmtsCopy)
		{
			/* truth of combination -- REMOVING - this generates too many permutations (n!) and therefore is too resource-consuming; there should be a matching function for postulates with AND conditions instead */
			// foreach (var otherStmt in stmtsCopy)
			// {
			// 	if (stmt.Equals(otherStmt)) continue;
				
			// 	var newStmt = new AndStatement(stmt, otherStmt).Simplify();

			// 	KnowledgeBase.Statements.Add(newStmt);
			// }
			
			foreach (var postulate in KnowledgeBase.Postulates)
			{
				if (postulate is MatchPostulate matchPostulate && matchPostulate.Predicate is AndStatement andPredicate)
				{
					var (foundMatch, matcherStmt) = MatchPostulate.GetMatchForAndPredicate(andPredicate, stmtsCopy);

					if (!foundMatch) continue;

					KnowledgeBase.Statements.Add(
						postulate.ApplyTo(matcherStmt)!
					);

					continue;
				}

				var newStmt = postulate.ApplyTo(stmt);

				if (newStmt is null) continue;

				KnowledgeBase.Statements.Add(newStmt);
			}
		}
	}

	public void ForwardChainStatementsAllGens()
	{
		int lastNumStmts = 0;
		int numStmts = KnowledgeBase.Statements.Count;

		while (numStmts > lastNumStmts)
		{
			lastNumStmts = numStmts;

			ForwardChainStatementsOneGen();

			numStmts = KnowledgeBase.Statements.Count;
		}
	}

	bool BackwardChainUsingPostulates(Statement target, Statement originalTarget)
	{
		if (KnowledgeBase.Statements.Contains(target)) return true;

		foreach (var postulate in KnowledgeBase.Postulates)
		{
			if (postulate is MatchPostulate matchPostulate)
			{
				if (MatchPostulate.MatchesPredicateLikeStatement(matchPostulate.Result, target))
				{
					MatchPostulate reversed = new(matchPostulate.Result, matchPostulate.Predicate);
					// Dictionary<string, LogicalEntity> matchPairs = [];

					// if (!matchPostulate.Result.SchemaMatches(target)) return false;

					// var argRef = target.GetArgRef();

					// var resultArgRef = matchPostulate.Result.GetArgRef();

					// for (int i = 0; i < argRef.Length; i++)
					// {
					// 	if (resultArgRef[i] is MatchEntity matchEntity)
					// 	{
					// 		matchPairs.Add(matchEntity.Identifier, argRef[i]);
					// 	}
						
					// 	if (!argRef[i].Equals(resultArgRef[i])) return false; // statement signatures don't match
					// }

					var matchedPredicate = reversed.ApplyTo(target)!;

					if (matchedPredicate.Equals(originalTarget)) continue;

					if (BackwardChainUsingPostulates(matchedPredicate, originalTarget))
					{
						KnowledgeBase.Statements.Add(target);

						return true;
					}
				}
			}
		}

		return false;
	}

	bool BackwardChainNaive(Statement target)
	{
		int lastNumStmts = 0;
		int numStmts = KnowledgeBase.Statements.Count;

		if (KnowledgeBase.Statements.Contains(target)) return true;

		while (numStmts > lastNumStmts)
		{
			lastNumStmts = numStmts;

			ForwardChainStatementsOneGen();

			if (KnowledgeBase.Statements.Contains(target)) return true;

			numStmts = KnowledgeBase.Statements.Count;
		}

		return false;
	}

	public bool BackwardChain(Statement targetStmt)
	{
		var target = targetStmt.Simplify();

		return BackwardChainUsingPostulates(target, target);
	}
}