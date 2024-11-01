using SymbolicReasoning.NewLogic.Objects;

namespace SymbolicReasoning.NewLogic.Statements;

public interface IStatement
{
	public int ArgsConsumed { get; }
	public IStatement WithArgRef(ILogicalEntity[] args);
	public ILogicalEntity[] GetArgRef();
	public IStatement Simplify() => this;
	public bool SchemaMatches(IStatement other) => other.GetType() == GetType();
}