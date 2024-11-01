using SymbolicReasoning.NewLogic.Statements;

namespace SymbolicReasoning.NewLogic.Postulates;

public interface IPostulate
{
	public IStatement Match { get; }
	public IStatement Result { get; }
	public bool AppliesTo(IStatement statement);

	public IStatement ApplyTo(IStatement statement);
}