using SymbolicReasoning.NewLogic.Postulates;
using SymbolicReasoning.NewLogic.Statements;

namespace SymbolicReasoning.NewLogic.Tests;

class ForwardChaining
{
	public static void Run()
	{
		List<IPostulate> postulates = [
			new MatchPostulate(
				predicate: new BinaryRelationStatement(x, BelongsTo, Scientists),
				result: new BinaryRelationStatement(x, BelongsTo, Humans)
			),

			new BiconditionalMatchPostulate(
				first: new BinaryRelationStatement(x, BelongsTo, Humans).Negate(),
				second: new BinaryRelationStatement(x, BelongsTo, Aliens)
			),

			new MatchPostulate(
				predicate: new BinaryRelationStatement(x, BelongsTo, Biologists),
				result: new BinaryRelationStatement(x, BelongsTo, Scientists)
			),

			new MatchPostulate(
				predicate: new BinaryRelationStatement(George, BelongsTo, Humans).And(
					new BinaryRelationStatement(Grunkle, BelongsTo, Aliens)
				),
				result: new BinaryRelationStatement(Grunkle, IsIdenticalTo, George).Negate()
			)
		];

		List<Statement> statements = [
			new BinaryRelationStatement(George, BelongsTo, Biologists),
			new BinaryRelationStatement(Grunkle, BelongsTo, Aliens)
		];

		var engine = new SimpleReasoningEngine();

		engine.KnowledgeBase.Postulates.UnionWith(postulates);
		engine.KnowledgeBase.Statements.UnionWith(statements);

		foreach (var postulate in engine.KnowledgeBase.Postulates)
		{
			Console.WriteLine(postulate);
		}

		Console.WriteLine('\n');

		engine.ForwardChainPostulatesAllGens();
		// engine.ForwardChainPostulatesOneGen();

		Console.WriteLine('\n');

		foreach (var postulate in engine.KnowledgeBase.Postulates)
		{
			Console.WriteLine(postulate);
		}

		Console.WriteLine('\n');

		foreach (var stmt in engine.KnowledgeBase.Statements)
		{
			Console.WriteLine(stmt);
		}

		engine.ForwardChainStatementsAllGens();

		Console.WriteLine('\n');

		foreach (var stmt in engine.KnowledgeBase.Statements)
		{
			Console.WriteLine(stmt);
		}
	}
}