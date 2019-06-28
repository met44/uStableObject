namespace uStableObject.Data
{
    public interface IBaseTypeVar<T>
    {
        T Value { get; set; }
        bool HasRuntimeValue();
    }
}