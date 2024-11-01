using SymbolicReasoning.NewLogic.Postulates;

namespace SymbolicReasoning.NewLogic.Axioms;

public interface IAxiom
{
	public IPostulate? Apply(IPostulate postulate);
}