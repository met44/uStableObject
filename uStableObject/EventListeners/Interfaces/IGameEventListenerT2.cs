namespace               uStableObject
{
    public interface    IGameEventListener<T1, T2>
    {
        void            OnEventRaised(T1 param1, T2 param2);
    }
}