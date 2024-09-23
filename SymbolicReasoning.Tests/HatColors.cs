using SymbolicReasoning.Logic.Statements;
using SymbolicReasoning.Objects;

namespace SymbolicReasoning.Tests;

class HatColors
{
	public static void Run()
	{
		LogicalSet redHats = new("Red Hats");
		LogicalSet blueHats = new("Blue Hats");

		LogicalObject eve = new("eve");
		LogicalObject frank = new("frank");
		LogicalObject george = new("george");

		List<IStatement> truths = [ new ObjectBelongingStatement(george, redHats) ]; // George's statement

		List<IStatement> possiblyTrue = [
			new ObjectBelongingStatement(frank, redHats), // Frank's statement
			new ObjectBelongingStatement(frank, blueHats), // Frank's statement

			new ObjectBelongingStatement(eve, redHats), // Eve's statement
			new ObjectBelongingStatement(eve, blueHats), // Eve's statement
		];

		SymbolicReasoner reasoningEngine = new();

		foreach (var truth in truths)
		{
			reasoningEngine.AddAxiom(truth);
		}

		var trueStmts = reasoningEngine.SelectTruths(
			possiblyTrue,
			(knowledgeBase) => {
				int redHatCount =  knowledgeBase.Count(
					stmt => stmt is ObjectBelongingStatement { IsNegated: false } && ((stmt as ObjectBelongingStatement)!.Set == redHats)
				);
				
				int blueHatCount = knowledgeBase.Count(
					stmt => stmt is ObjectBelongingStatement { IsNegated: false } && ((stmt as ObjectBelongingStatement)!.Set == blueHats)
				);
				
				bool personWearingMultipleHats = knowledgeBase
				.Where(stmt => stmt is ObjectBelongingStatement { IsNegated: false })
				.GroupBy(stmt => (stmt as ObjectBelongingStatement)!.Object).Any(grp => grp.Count() > 1); // up to to people may wear the same color hat, no one may wear two hats
			
				return redHatCount <= 2 && blueHatCount <= 2 && !personWearingMultipleHats;
			}
		)!;

		Console.WriteLine();

		Console.WriteLine(reasoningEngine.KnowledgeBaseToString());

		foreach (var trueStmt in trueStmts)
		{
			Console.WriteLine($"Found that {trueStmt} is true");
		}
	}
}