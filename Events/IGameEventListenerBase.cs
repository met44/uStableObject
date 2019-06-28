namespace uStableObject
{
    public interface IGameEventListenerBase<T>
    {
        void OnEventRaised(T param);
    }
}