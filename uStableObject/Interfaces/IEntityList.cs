using System.Collections.Generic;

namespace uStableObject.Data
{
    public interface IEntityList
    {
        IEnumerable<IEntity> Entities { get; }
    }
}