using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace                       uStableObject.Utilities
{
    public static partial class ListExtensions
    {
        //from https://answers.unity.com/questions/686644/smoothing-a-path.html
        public static void      Chaikin(this List<Vector3> positionList, int iterations = 1)
        {
            /*
            Vector3[] newPts = new Vector3[(positionList.Count - 2) * 2 + 2];
            newPts[0] = positionList[0];
            newPts[newPts.Length - 1] = positionList[positionList.Count - 1];
            */
            if (positionList.Count > 2)
            {
                while (iterations-- > 0)
                {
                    int j = 1;
                    Vector3 n = positionList[0];
                    positionList.Add(positionList[positionList.Count - 1] + (positionList[positionList.Count - 1] - positionList[positionList.Count - 2]));
                    for (int i = 0; i < positionList.Count - 2; i += 2)
                    {
                        Vector3 n1 = positionList[i + 1];
                        Vector3 n2 = positionList[i + 2];
                        Vector3 lostVal = n1;
                        // newPts[j] = n + (n1 - n) * 0.75f;
                        // newPts[j + 1] = n1 + (n2 - n1) * 0.25f;
                        positionList[j] = n + (n1 - n) * 0.75f;
                        positionList.Insert(j + 1, n1 + (n2 - n1) * 0.25f);
                        n = lostVal;
                        j += 2;
                    }
                    positionList.RemoveAt(positionList.Count - 1);
                    positionList.RemoveAt(positionList.Count - 1);
                    /*int c = positionList.Count;
                    if (c >= 2)
                    {
                        if (c >= 3)
                        {
                            positionList[j] = positionList[c - 3] + (positionList[c - 2] - positionList[c - 3]) * 0.75f;
                        }
                        positionList.Insert(c - 2, positionList[c - 2] + (positionList[c - 1] - positionList[c - 2]) * 0.25f);
                    }*/
                }
            }
        }

        public static Vector3   BarycentricCenter(this List<Vector3> positionList)
        {
            if (positionList.Count  > 0)
            {
                Vector3 center = positionList[0];

                for (var i = 1; i < positionList.Count; ++i)
                {
                    center += positionList[i];
                }
                center /= positionList.Count;
                return (center);
            }
            return (Vector3.zero);
        }
    }
}
