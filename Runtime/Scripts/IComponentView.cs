public interface IComponentView<T> : IComponentViewBase
{
	public T Component { get; set; }
}