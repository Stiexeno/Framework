namespace Framework.Character
{
	public interface ICharacterComponent
	{
		void Process(in float deltaTime);
		void Init(Character character);
	}
}