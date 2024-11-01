using SymbolicReasoning.NewLogic.Statements;

namespace SymbolicReasoning.NewLogic.Postulates;

public interface IPostulate : IEquatable<IPostulate>
{
	public Statement? ApplyTo(Statement statement);

	public IPostulate? TryChainTo(IPostulate next) => null;
}