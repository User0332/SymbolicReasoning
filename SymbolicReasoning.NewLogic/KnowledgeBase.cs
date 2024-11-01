using SymbolicReasoning.NewLogic.Postulates;
using SymbolicReasoning.NewLogic.Statements;

namespace SymbolicReasoning.NewLogic;

public class KnowledgeBase
{
	public readonly HashSet<Statement> Statements = [];
	public readonly HashSet<IPostulate> Postulates = [];
}