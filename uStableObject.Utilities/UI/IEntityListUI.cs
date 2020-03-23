using uStableObject.Data;

namespace uStableObject.UI
{
    public interface IEntityListUI<T> where T : IEntity
    {
        T SelectedEntity { get; set; }
    }
}