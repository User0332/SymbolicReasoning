using SymbolicReasoning.NewLogic.Postulates;
using SymbolicReasoning.NewLogic.Statements;

namespace SymbolicReasoning.NewLogic.Axioms;

public static class DefaultAxioms
{
	public static readonly BasicAxiom AxiomOfContrapositivity = new(
		(inPostulate) => {
			if (inPostulate is not MatchPostulate matchPostulate) return null; // if this is not a match postulate, we cannot safely draw any conclusions

			return new MatchPostulate(new NotStatement(matchPostulate.Result), new NotStatement(matchPostulate.Predicate));
		}
	);

	// public static readonly BasicAxiom AxiomOfInseparability = new(

	// );
}