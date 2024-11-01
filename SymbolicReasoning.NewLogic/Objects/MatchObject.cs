namespace SymbolicReasoning.NewLogic.Objects;

public class MatchObject(int num) : ILogicalObject
{
	public string Identifier => $"Postulational Matching Logical Element ({Num})";
	public readonly int Num = num;
}