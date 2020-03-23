namespace                   uStableObject
{
    public interface        IGameEvent
    {
        int                 ListenersCount { get; }

#if UNITY_EDITOR
        void                ShowInspector();
#endif
    }
}