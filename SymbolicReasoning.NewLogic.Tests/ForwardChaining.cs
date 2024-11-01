using SymbolicReasoning.NewLogic.Postulates;
using SymbolicReasoning.NewLogic.Statements;

namespace SymbolicReasoning.NewLogic.Tests;

class ForwardChaining
{
	public static void Run()
	{
		List<IPostulate> postulates = [
			new MatchPostulate(
				new BinaryRelationStatement(x, BelongsTo, Scientists),
				new BinaryRelationStatement(x, BelongsTo, Humans)
			),

			new BiconditionalMatchPostulate(
				new NotStatement(new BinaryRelationStatement(x, BelongsTo, Humans)),
				new BinaryRelationStatement(x, BelongsTo, Aliens)
			),

			new MatchPostulate(
				new BinaryRelationStatement(x, BelongsTo, Biologists),
				new BinaryRelationStatement(x, BelongsTo, Scientists)
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

		engine.ForwardChainStatements();

		Console.WriteLine('\n');

		foreach (var stmt in engine.KnowledgeBase.Statements)
		{
			Console.WriteLine((stmt, stmt.GetHashCode()));
		}
	}
}