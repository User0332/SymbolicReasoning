using SymbolicReasoning.NewLogic.Postulates;
using SymbolicReasoning.NewLogic.Statements;

namespace SymbolicReasoning.NewLogic.Axioms;

public class BasicAxiom<TPostulate>(
	Func<TPostulate, TPostulate> transformer
) : IAxiom<TPostulate> where TPostulate : IPostulate
{
	public TPostulate Apply(TPostulate postulate)
	{
		return transformer(postulate);
	}
}