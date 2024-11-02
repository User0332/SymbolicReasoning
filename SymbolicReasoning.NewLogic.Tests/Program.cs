namespace SymbolicReasoning.NewLogic.Tests;

class Program
{
	static int Main(string[] args)
	{
		ForwardChaining.Run();
		BackwardChaining.Run();

		return 0;
	}
}