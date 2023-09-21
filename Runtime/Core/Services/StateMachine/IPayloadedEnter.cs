namespace Framework.StateMachine
{
	public interface IPayloadedEnter<in TPayload>
	{
		void Enter(TPayload payload);
	}
    
	public interface IPayloadedEnter<in TPayload1, in TPayload2>
	{
		void Enter(TPayload1 payload1, TPayload2 payload2);
	}
    
	public interface IPayloadedEnter<in TPayload1, in TPayload2, in TPayload3>
	{
		void Enter(TPayload1 payload1, TPayload2 payload2, TPayload3 onCutsceneFinish);
	}
}
