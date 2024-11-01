namespace SymbolicReasoning.NewLogic.Objects;

public class MatchSet(int num) : ILogicalSet
{
	public string Identifier => $"Postulational Matching Logical Element ({Num})";
	public readonly int Num = num;
}