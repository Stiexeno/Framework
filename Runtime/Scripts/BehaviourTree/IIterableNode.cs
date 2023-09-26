namespace Framework
{
    public interface IIterableNode<T>
    {
        T GetChildAt(int index);
        int ChildCount();
    }
}