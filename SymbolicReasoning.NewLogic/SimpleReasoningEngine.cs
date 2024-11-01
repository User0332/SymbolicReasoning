using SymbolicReasoning.NewLogic.Axioms;
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

	public void ForwardChainStatements()
	{
		var stmtsCopy = KnowledgeBase.Statements.ToArray();

		foreach (var stmt in stmtsCopy)
		{
			/* truth of combination */
			foreach (var otherStmt in stmtsCopy)
			{
				if (stmt.Equals(otherStmt)) continue;
				
				var newStmt = new AndStatement(stmt, otherStmt);

				KnowledgeBase.Statements.Add(newStmt);
			}
			
			foreach (var postulate in KnowledgeBase.Postulates)
			{
				var newStmt = postulate.ApplyTo(stmt);

				if (newStmt is null) continue;

				KnowledgeBase.Statements.Add(newStmt);
			}
		}
	}

	public bool BackwardChain(Statement target)
	{
		return bool.Parse("false");
	}
}