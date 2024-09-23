using SymbolicReasoning.Logic.Statements;
using SymbolicReasoning.Objects;

namespace SymbolicReasoning.Tests;

class CakeBaker
{
	public static void Run()
	{
		LogicalSet bakers = new("Cake Bakers");

		LogicalObject alice = new("alice");
		LogicalObject bob = new("bob");
		LogicalObject carol = new("carol");


		List<IStatement> possiblyTrue = [
			new ObjectBelongingStatement(alice, bakers).Negate(), // Alice's statement
			new ObjectBelongingStatement(carol, bakers), // Bob's statement
			new ObjectBelongingStatement(carol, bakers).Negate(), // Carol's statement
		];

		SymbolicReasoner reasoningEngine = new();

		var trueStmt = reasoningEngine.SelectOneTruth(
			possiblyTrue,
			(knowledgeBase) => {
				return knowledgeBase.Count(
					stmt => stmt is ObjectBelongingStatement { IsNegated: false } && ((stmt as ObjectBelongingStatement)?.Set == bakers)
				) == 1; // select that exactly one person must have baked the cake
			}
		)!;

		Console.WriteLine(reasoningEngine.KnowledgeBaseToString());

		Console.WriteLine($"Found that {trueStmt} is true");
	}
}