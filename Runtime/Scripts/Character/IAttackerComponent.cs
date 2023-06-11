namespace Framework.Character
{
	public interface IAttackerComponent : ICharacterComponent
	{
		ITarget Target { get; }
		ITarget TargetedBy { get; }
	}   
}
