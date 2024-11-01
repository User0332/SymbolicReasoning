using SymbolicReasoning.NewLogic.Objects;

namespace SymbolicReasoning.NewLogic.Statements;

public interface IStatement
{
	public static readonly IStatement Truth = new StatementOfTruth();

	public int ArgsConsumed { get; }

	public IStatement WithArgRef(ILogicalEntity[] args);
	public ILogicalEntity[] GetArgRef();
	public IStatement Simplify() => this;
	public bool SchemaMatches(IStatement other) => other.GetType() == GetType();
}

class StatementOfTruth : IStatement
{
	public int ArgsConsumed => 0;

	public ILogicalEntity[] GetArgRef() => [];

	public IStatement WithArgRef(ILogicalEntity[] args) => this;
}