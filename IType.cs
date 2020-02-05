public interface IType<out T> where T : struct
{
    T Type { get; }
}