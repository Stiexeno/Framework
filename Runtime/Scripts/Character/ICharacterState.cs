namespace Framework.Character
{
	public interface ICharacterState : ICharacterComponent
	{
		State State { get; set; }
		void SetState(State state);
	}
}