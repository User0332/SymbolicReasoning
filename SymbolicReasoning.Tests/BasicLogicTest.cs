using SymbolicReasoning.Logic.Statements;
using SymbolicReasoning.Objects;

namespace SymbolicReasoning.Tests;

class BasicLogicTest
{
	public static void Run()
	{
		LogicalSet programmers = new("Programmers");
		LogicalSet humans = new("Humans");
		LogicalSet historians = new("Historians");
		LogicalSet biologists = new("Biologists");
		LogicalSet scientists = new("Scientists");
		LogicalObject george = new("George");
		LogicalObject john = new("John");

		LogicalObject jerry = new("Jerry");

		LogicalSet mathematicians = new("Mathematicians");
		LogicalSet computerScientists = new("Computer Scientists");

		List<IStatement> baseKnowledge = [
			new ObjectBelongingStatement(
				george, programmers
			),
			new ObjectBelongingStatement(
				george, biologists
			),
			new ObjectBelongingStatement(
				john, biologists
			),
			new ObjectBelongingStatement(
				jerry, computerScientists
			),
			new SetAssociationStatement(computerScientists, programmers),
			new SetAssociationStatement(computerScientists, mathematicians),
			new SetAssociationStatement(mathematicians, humans),
			new SetAssociationStatement(computerScientists, scientists),

			new SetAssociationStatement(programmers, humans),
			new SetAssociationStatement(historians, humans),
			new SetAssociationStatement(biologists, scientists),
			new SetAssociationStatement(scientists, humans)
		];

		List<IStatement> tests = [
			new ObjectBelongingStatement(george, humans),
			new ObjectBelongingStatement(george, historians),
			new ObjectBelongingStatement(john, humans),
			new ObjectBelongingStatement(john, historians),
			new ObjectBelongingStatement(john, historians).Negate()
		];

		SymbolicReasoner reasoningEngine = new();

		foreach (var axiom in baseKnowledge)
		{
			reasoningEngine.AddAxiom(axiom);
		}

		Console.WriteLine("Original knowledge base: ");
		Console.WriteLine(reasoningEngine.KnowledgeBaseToString());

		foreach (var test in tests)
		{
			var res = reasoningEngine.BackwardChain(test);

			Console.WriteLine($"{test} -> {res}");
		}

		Console.WriteLine();

		Console.WriteLine("Knowledge base after backward chaining: ");
		Console.WriteLine(reasoningEngine.KnowledgeBaseToString());

		reasoningEngine.ForwardChain(1);

		Console.WriteLine("Knowledge base after forward chaining (iter 1): ");
		Console.WriteLine(reasoningEngine.KnowledgeBaseToString());

		reasoningEngine.ForwardChain(1);

		Console.WriteLine("Knowledge base after forward chaining (iter 2 (1+1 = 2 total iterations)): ");
		Console.WriteLine(reasoningEngine.KnowledgeBaseToString());	
	}
}