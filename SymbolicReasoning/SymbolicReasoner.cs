using System.Text;
using SymbolicReasoning.Logic.Statements;
using SymbolicReasoning.Objects;

namespace SymbolicReasoning;

public class SymbolicReasoner
{
	readonly HashSet<IStatement> knowledgeBase = [];

	public void AddAxiom(IStatement axoim) => knowledgeBase.Add(axoim);

	public string KnowledgeBaseToString()
	{
		StringBuilder builder = new(knowledgeBase.Count*10);

		foreach (var stmt in knowledgeBase)
		{
			builder.AppendLine(stmt.ToString());
		}

		return builder.ToString();
	}

	public IStatement? SelectOneTruth(IEnumerable<IStatement> possibleTruths, Func<HashSet<IStatement>, bool>? extraSelector = null)
	{
		extraSelector ??= _ => true;

		HashSet<IStatement> oldKnowledge = new(knowledgeBase); 
	
		foreach (var possibleTruth in possibleTruths)
		{
			knowledgeBase.Add(possibleTruth); // assume this as true, try to find a contradiction
			
			foreach (var otherStmt in possibleTruths) // assume everything else is false
			{
				if (otherStmt == possibleTruth) continue;

				knowledgeBase.Add(otherStmt.Negate());
			}

			int lastNumTruths = 0;
			bool contradiction = false;

			while (lastNumTruths != knowledgeBase.Count) // if the number of truths did not change after forward chaining, then we have deduced all truths
			{
				lastNumTruths = knowledgeBase.Count;

				ForwardChain(1);

				foreach (var stmt in knowledgeBase) // try to find a contradiction, TODO: optimize
				{
					if (knowledgeBase.Contains(stmt.Negate())) 
					{
						contradiction = true;
						break;
					}
				}
			}

			if (!contradiction && extraSelector(knowledgeBase)) // if this is true, then keep everything in the knowledge base, don't remove it (code to reset knowledge base is below)
			{
				return possibleTruth;
			}

			knowledgeBase.Clear();
			knowledgeBase.UnionWith(oldKnowledge);
		}

		return null;
	}

	public List<IStatement> SelectTruths(IEnumerable<IStatement> possibleTruths, Func<HashSet<IStatement>, bool>? extraSelector = null)
	{
		extraSelector ??= _ => true;

		HashSet<IStatement> oldKnowledge = new(knowledgeBase); 
	
		List<IStatement> trueStatements = [];

		foreach (var possibleTruth in possibleTruths)
		{
			knowledgeBase.Add(possibleTruth); // assume this as true, try to find a contradiction

			int lastNumTruths = 0;
			bool contradiction = false;

			while (lastNumTruths != knowledgeBase.Count) // if the number of truths did not change after forward chaining, then we have deduced all truths
			{
				lastNumTruths = knowledgeBase.Count;

				ForwardChain(1);

				if (knowledgeBase.Contains(possibleTruth.Negate())) // try to find a contradiction
				{
					contradiction = true;
					break;
				}
			}

			if (!contradiction && extraSelector(knowledgeBase)) // if this is true, then keep everything in the knowledge base by adding these conclusions to oldKnowledge
			{
				trueStatements.Add(possibleTruth);
				oldKnowledge.UnionWith(knowledgeBase);
				continue;
			}

			knowledgeBase.Clear();
			knowledgeBase.UnionWith(oldKnowledge);
		}

		return trueStatements;
	}

	// TODO: remove the nesting hell
	public void ForwardChain(int iterations = 1) // ForwardChain makes depth of 1 searches for new knowledge, therefore, more iterations are needed to gain deeper knowledge
	{
		HashSet<IStatement> additionalKnowledge = [];

		for (int i = 0; i < iterations; i++)
		{
			foreach (var stmt in knowledgeBase)
			{
				if (stmt is ObjectBelongingStatement belongingStmt)
				{
					IEnumerable<IStatement> related =
						knowledgeBase
							.Where(
								stmt => stmt.ReferencedObjects.Contains(belongingStmt.Object) ||
								stmt.ReferencedSets.Contains(belongingStmt.Set)
							);

					foreach (var supplStmt in related)
					{
						if (supplStmt is SetAssociationStatement setAssociationStmt)
						{
							if (setAssociationStmt.First != belongingStmt.Set) continue;

							additionalKnowledge.Add(new ObjectBelongingStatement(belongingStmt.Object, setAssociationStmt.Second));
						}
						else if (supplStmt is SetIdentityStatement setIdentityStmt)
						{
							var other = belongingStmt.Set == setIdentityStmt.First ? setIdentityStmt.Second : setIdentityStmt.First;

							additionalKnowledge.Add(new ObjectBelongingStatement(belongingStmt.Object, other));
						}
					}
				}

				if (stmt is SetAssociationStatement setAssocStmt)
				{
					IEnumerable<IStatement> related =
						knowledgeBase
							.Where(
								stmt => stmt.ReferencedSets.Contains(setAssocStmt.First) ||
								stmt.ReferencedSets.Contains(setAssocStmt.Second)
							);

					foreach (var supplStmt in related)
					{
						if (supplStmt is SetAssociationStatement setAssociationStmt)
						{
							if (setAssocStmt.Second == setAssociationStmt.First)
							{
								additionalKnowledge.Add(new SetAssociationStatement(setAssocStmt.First, setAssociationStmt.Second));
							}
							else if (setAssociationStmt.Second == setAssocStmt.First)
							{
								additionalKnowledge.Add(new SetAssociationStatement(setAssociationStmt.First, setAssocStmt.Second));
							}
						}
						else if (supplStmt is SetIdentityStatement setIdentityStmt)
						{
							LogicalSet other = setIdentityStmt.First == setAssocStmt.First ? setIdentityStmt.Second : setIdentityStmt.First;

							if (setIdentityStmt.Second == setAssocStmt.Second)
							{
								additionalKnowledge.Add(new SetAssociationStatement(setAssocStmt.First, setIdentityStmt.First));
							}
							else if (setIdentityStmt.First == setAssocStmt.Second)
							{
								additionalKnowledge.Add(new SetAssociationStatement(setAssocStmt.First, setIdentityStmt.Second));
							}
							else if (setIdentityStmt.Second == setAssocStmt.First)
							{
								additionalKnowledge.Add(new SetAssociationStatement(setIdentityStmt.First, setAssocStmt.Second));
							}
							else if (setIdentityStmt.First == setAssocStmt.First)
							{
								additionalKnowledge.Add(new SetAssociationStatement(setIdentityStmt.Second, setAssocStmt.First));
							}
						}
					}
				}
			}

			knowledgeBase.UnionWith(additionalKnowledge);
			additionalKnowledge.Clear();
		}
	}

	public bool BackwardChain(IStatement query)
	{
		if (knowledgeBase.Contains(query))
		{
			Console.WriteLine($"{query} is a fact in the knowledge base; it must be true");
			return true;
		}

		if (query.IsNegated) return !BackwardChain(query.Negate());

		if (BackwardChainBase(query))
		{
			knowledgeBase.Add(query);
			return true;
		}

		return false;
	}

	// TODO: modularize
	// Idea: make inference of conclusion that will lead to res, then use recursion to propagate that thru
	public bool BackwardChainBase(IStatement query) // note that this statement is not necessarily true (could we use proof by contradiction here?)
	{
		Console.WriteLine($"Trying to figure out if {query}");

		if (query is ObjectBelongingStatement belongingQuery)
		{
			IEnumerable<IStatement> related =
				knowledgeBase
					.Where(
						stmt => stmt.ReferencedObjects.Contains(belongingQuery.Object) ||
						stmt.ReferencedSets.Contains(belongingQuery.Set)
					);

			foreach (var stmt in related)
			{
				if (stmt is ObjectBelongingStatement objectBelongingStmt && objectBelongingStmt.Object == belongingQuery.Object && !objectBelongingStmt.IsNegated)
				{
					SetAssociationStatement setLink = new(objectBelongingStmt.Set, belongingQuery.Set);

					Console.WriteLine($"Found that {objectBelongingStmt}, need to find that {setLink} to prove {query}");

					if (BackwardChain(setLink)) return true;
				}
			}
		}
		else if (query is SetAssociationStatement setAssocQuery)
		{
			if (knowledgeBase.Contains(new SetIdentityStatement(setAssocQuery.First, setAssocQuery.Second))) return true;

			IEnumerable<IStatement> related =
				knowledgeBase
					.Where(
						stmt => stmt.ReferencedSets.Contains(setAssocQuery.First) ||
						stmt.ReferencedSets.Contains(setAssocQuery.Second)
					);

			foreach (var stmt in related)
			{
				if (stmt is SetAssociationStatement setAssociationStmt)
				{
					if (setAssociationStmt.First == setAssocQuery.First)
					{
						SetAssociationStatement setLink = new(setAssociationStmt.Second, setAssocQuery.Second);

						Console.WriteLine($"Found that {setAssociationStmt}, need to find that {setLink} to prove {query}");

						if (BackwardChain(setLink)) return true;
					}
				}

				if (stmt is SetIdentityStatement setIdentityStmt)
				{
					LogicalSet equivalentTo = setIdentityStmt.First == setAssocQuery.First ? setIdentityStmt.Second : setIdentityStmt.First;

					SetAssociationStatement setLink;

					if (setIdentityStmt.Second == setAssocQuery.Second)
					{
						setLink = new(setAssocQuery.First, setIdentityStmt.First);
					}
					else if (setIdentityStmt.First == setAssocQuery.Second)
					{
						setLink = new(setAssocQuery.First, setIdentityStmt.Second);
					}
					else if (setIdentityStmt.Second == setAssocQuery.First)
					{
						setLink = new(setIdentityStmt.First, setAssocQuery.Second);
					}
					else // if (setIdentityStmt.First == setAssocQuery.First)
					{
						setLink = new(setIdentityStmt.Second, setAssocQuery.First);
					}
					
					Console.WriteLine($"Found that {setIdentityStmt}, need to find that {setLink} to prove {query}");

					if (BackwardChain(setLink)) return true;
				}
			}

			// TODO: more searching
		}
		else if (query is SetIdentityStatement setIdentStmt)
		{
			// TODO: more searching
		}

		Console.WriteLine($"Could not conclude that {query}");

		return false;
	}
}

