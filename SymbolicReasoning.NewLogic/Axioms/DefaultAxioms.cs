using SymbolicReasoning.NewLogic.Postulates;
using SymbolicReasoning.NewLogic.Statements;

namespace SymbolicReasoning.NewLogic.Axioms;

public static class DefaultAxioms
{
	public static readonly BasicAxiom<MatchPostulate> AxiomOfContrapositivity = new(
		(inPostulate) => {
			return new(new NotStatement(inPostulate.Result), new NotStatement(inPostulate.Predicate));
		}
	);
}