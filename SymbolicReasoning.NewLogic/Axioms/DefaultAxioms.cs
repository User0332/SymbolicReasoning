using SymbolicReasoning.NewLogic.Postulates;

namespace SymbolicReasoning.NewLogic.Axioms;

public static class DefaultAxioms
{
	public static readonly BasicAxiom<MatchPostulate> AxiomOfContrapositivity = new(
		(inPostulate) => {
			return new(inPostulate.Result.Negate(), inPostulate.Match.Negate());
		}
	);
}