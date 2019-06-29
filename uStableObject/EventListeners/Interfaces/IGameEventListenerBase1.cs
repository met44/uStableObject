namespace uStableObject
{
    public interface IGameEventListenerBase2<T1, T2>
    {
        void OnEventRaised(T1 param1, T2 param2);
    }
}