using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using uStableObject;
using uStableObject.UI;
using uStableObject.Data;

namespace               uStableObject.UI
{
    // this is for cases where the specific final type doesn't matter, i.e. display-only, and fired events dont need to match the type
    public class        SimpleDisplayableListUI : DisplayableListUIBase<IDisplayable, SimpleDisplayableRow, UnityEventTypes.Displayable>
    {
    }
}
