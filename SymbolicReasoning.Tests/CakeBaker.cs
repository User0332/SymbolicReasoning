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

		var aliceStmt = new ObjectBelongingStatement(alice, bakers).Negate();
		var bobStmt = new ObjectBelongingStatement(carol, bakers);
		var carolStmt = IStatement.Lie(bobStmt);

		List<IStatement> possiblyTrue = [ aliceStmt, bobStmt, carolStmt ];

		SymbolicReasoner reasoningEngine = new();

		var trueStmt = reasoningEngine.SelectOneTruth( // TODO: fix (set constraint doesn't work)
			possiblyTrue
		)!;

		Console.WriteLine(reasoningEngine.KnowledgeBaseToString());

		Console.WriteLine($"Found that {trueStmt} is true");

		Console.WriteLine($"Therefore, {bakers.Members.First()} baked the cake.");
	}
}