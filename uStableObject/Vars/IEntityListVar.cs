using System.Collections.Generic;

namespace uStableObject.Data
{
    public interface IEntityListVar
    {
        IEnumerable<IEntity> Entities { get; }
    }
}