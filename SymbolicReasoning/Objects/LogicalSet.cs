using SymbolicReasoning.Logic.Statements;

namespace SymbolicReasoning.Objects;

public class LogicalSet : IEquatable<LogicalSet>
{
	const int HashMask = 18928773;

	public readonly string Identifier;
	public readonly SetConstraints Constraints;
	
	readonly List<LogicalObject> members = [];
	public IEnumerable<LogicalObject> Members => members;

	public LogicalSet(string identifier)
	{
		Identifier = identifier;
		Constraints = new();
	}

	public LogicalSet(string identifier, SetConstraints setConstraints)
	{
		Identifier = identifier;
		Constraints = setConstraints;
	}

	void ReloadSetConstraintViolationFlag()
	{
		var count = members.Count;
		
		Constraints.IsViolated = 
			(Constraints.RequiresNMembers && count != Constraints.RequiredMembers) ||
			(!Constraints.RequiresNMembers && count < Constraints.MinMembers) ||
			(!Constraints.RequiresNMembers && count > Constraints.MaxMembers);
	}

	public void AddMember(LogicalObject obj)
	{
		if (members.Contains(obj)) return;

		members.Add(obj);

		ReloadSetConstraintViolationFlag();
	}

	public void ClearMembers()
	{
		members.Clear();

		ReloadSetConstraintViolationFlag();
	}

	public void ReloadMembers(IEnumerable<IStatement> belongingStatements)
	{
		members.Clear();

		foreach (var stmt in belongingStatements)
		{
			var belongingStmt = (ObjectBelongingStatement) stmt;

			if (belongingStmt.Set != this) continue;

			members.Add(belongingStmt.Object);
		}

		ReloadSetConstraintViolationFlag();
	}

	public bool Equals(LogicalSet? other)
	{
		return other?.Identifier == Identifier;
	}

	public override string ToString()
	{
		return $"{Identifier}{{}}";
	}

	public override int GetHashCode()
	{
		return Identifier.GetHashCode()^HashMask;
	}

	public override bool Equals(object? obj)
	{
		return Equals(obj as LogicalSet);
	}
}