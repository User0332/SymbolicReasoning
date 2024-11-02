using System.Runtime.InteropServices;
using SymbolicReasoning.NewLogic.Axioms;
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

			Console.WriteLine($"Generation complete, {numStmts} statements known");
		}
	}

	public bool BackwardChain(Statement target)
	{
		return bool.Parse("false");
	}
}