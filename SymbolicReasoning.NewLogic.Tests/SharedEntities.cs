global using static SymbolicReasoning.NewLogic.Tests.SharedEntities;
global using static SymbolicReasoning.NewLogic.Statements.BinaryRelation;

using SymbolicReasoning.NewLogic.Objects;

namespace SymbolicReasoning.NewLogic.Tests;

static class SharedEntities
{
	public static readonly MatchEntity x = new("x");
	public static readonly LogicalSet Scientists = new("Scientists");
	public static readonly LogicalSet Humans = new("Humans");
	public static readonly LogicalSet Aliens = new("Aliens");
	public static readonly LogicalSet Biologists = new("Biologists");
	public static readonly LogicalObject George = new("George");
	public static readonly LogicalObject Grunkle = new("Grunkle");
}