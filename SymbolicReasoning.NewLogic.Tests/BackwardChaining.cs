using System.Collections.Specialized;
using SymbolicReasoning.NewLogic.Postulates;
using SymbolicReasoning.NewLogic.Statements;

namespace SymbolicReasoning.NewLogic.Tests;

class BackwardChaining
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

		List<Statement> targets = [
			new BinaryRelationStatement(George, BelongsTo, Scientists),
			new BinaryRelationStatement(Grunkle, BelongsTo, Humans),
			new BinaryRelationStatement(George, BelongsTo, Aliens).Negate()
		];

		var engine = new SimpleReasoningEngine();

		engine.KnowledgeBase.Postulates.UnionWith(postulates);
		engine.KnowledgeBase.Statements.UnionWith(statements);

		engine.ForwardChainPostulatesAllGens();

		foreach (var target in targets)
		{
			Console.WriteLine($"{target}: {engine.BackwardChain(target)}");
		}
	}
}