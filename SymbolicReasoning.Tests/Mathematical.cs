using SymbolicReasoning.Logic.Statements;
using SymbolicReasoning.Objects;

class Mathematical
{
	public static void Run()
	{
		LogicalSet natural = new("Natural Numbers");
		LogicalObject zero = new("0");

		List<IStatement> truths = [
			new ObjectBelongingStatement(zero, natural)
		];
	}
}