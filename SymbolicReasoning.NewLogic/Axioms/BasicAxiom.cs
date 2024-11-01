using SymbolicReasoning.NewLogic.Postulates;
using SymbolicReasoning.NewLogic.Statements;

namespace SymbolicReasoning.NewLogic.Axioms;

public class BasicAxiom(
	Func<IPostulate, IPostulate?> transformer
)
{
	public IPostulate? Apply(IPostulate postulate)
	{
		return transformer(postulate);
	}
}