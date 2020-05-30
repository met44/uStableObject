using System.Collections.Generic;

namespace                       uStableObject.Data
{
    public interface            IDisplayableList
    {
        IEnumerable<IDisplayable>    Entities { get; }
    }
}