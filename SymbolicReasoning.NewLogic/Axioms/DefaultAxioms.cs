using SymbolicReasoning.NewLogic.Postulates;
using SymbolicReasoning.NewLogic.Statements;

namespace SymbolicReasoning.NewLogic.Axioms;

public static class DefaultAxioms
{
	public static readonly BasicAxiom AxiomOfContrapositivity = new(
		(inPostulate) => {
			if (inPostulate is not MatchPostulate matchPostulate) return null; // if this is not a match postulate, we cannot safely draw any conclusions

			return new MatchPostulate(new NotStatement(matchPostulate.Result).Simplify(), new NotStatement(matchPostulate.Predicate).Simplify());
		}
	);

	public static readonly BasicAxiom AxiomOfInseparability = new(
		(inPostulate) => {
			if (inPostulate is not BiconditionalMatchPostulate matchPostulate) return null;

			return new BiconditionalMatchPostulate(matchPostulate.Result, matchPostulate.Predicate);
		}
	);

	public static readonly BasicAxiom AxiomOfBiconditionalApplication = new(
		(inPostulate) => {
			if (inPostulate is not BiconditionalMatchPostulate matchPostulate) return null;

			return new MatchPostulate(matchPostulate.Predicate, matchPostulate.Result);
		}
	);

	public static readonly IEnumerable<IAxiom> AsEnumerable = [
		AxiomOfContrapositivity,
		AxiomOfBiconditionalApplication,
		AxiomOfInseparability
	];
}