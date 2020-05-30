using uStableObject.Data;

namespace uStableObject.UI
{
    public interface IDisplayableListUI<T> where T : IDisplayable
    {
        T SelectedEntity { get; set; }
    }
}