namespace               uStableObject
{
    public interface    IGameEventListener<T>
    {
        void            OnEventRaised(T param);
    }
}