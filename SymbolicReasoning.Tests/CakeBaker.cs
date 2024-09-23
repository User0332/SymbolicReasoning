using SymbolicReasoning.Logic.Statements;
using SymbolicReasoning.Objects;

namespace SymbolicReasoning.Tests;

class CakeBaker
{
	public static void Run()
	{
		LogicalSet bakers = new("Cake Bakers", new SetConstraints(
			requiresNMembersOnly: true,
			requiredMembers: 1
		));

		LogicalObject alice = new("alice");
		LogicalObject bob = new("bob");
		LogicalObject carol = new("carol");


		List<IStatement> possiblyTrue = [
			new ObjectBelongingStatement(alice, bakers).Negate(), // Alice's statement
			new ObjectBelongingStatement(carol, bakers), // Bob's statement
			new ObjectBelongingStatement(carol, bakers).Negate(), // Carol's statement
		];

		SymbolicReasoner reasoningEngine = new();

		var trueStmt = reasoningEngine.SelectOneTruth( // TODO: fix (set constraint doesn't work)
			possiblyTrue
		)!;

		Console.WriteLine(reasoningEngine.KnowledgeBaseToString());

		Console.WriteLine($"Found that {trueStmt} is true");
	}
}