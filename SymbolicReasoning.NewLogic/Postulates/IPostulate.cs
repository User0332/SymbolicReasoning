using SymbolicReasoning.NewLogic.Statements;

namespace SymbolicReasoning.NewLogic.Postulates;

public interface IPostulate : IEquatable<IPostulate>
{
	public abstract Statement? ApplyTo(Statement statement);
}