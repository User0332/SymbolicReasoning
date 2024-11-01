using SymbolicReasoning.NewLogic.Postulates;

namespace SymbolicReasoning.NewLogic.Axioms;

public interface IAxiom<TPostulate> where TPostulate : IPostulate
{
	public TPostulate Apply(TPostulate postulate);
}