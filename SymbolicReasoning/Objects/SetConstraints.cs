namespace SymbolicReasoning.Objects;

public class SetConstraints(
	uint minMembers = 0,
	uint maxMembers = int.MaxValue,
	bool requiresNMembersOnly = false,
	uint requiredMembers = 0
)
{
	public readonly uint MinMembers = minMembers;
	public readonly uint MaxMembers = maxMembers;
	public readonly bool RequiresNMembers = requiresNMembersOnly;
	public readonly uint RequiredMembers = requiredMembers;
	public bool IsViolated = false;
}