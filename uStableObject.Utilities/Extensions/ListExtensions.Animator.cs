using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace                       uStableObject.Utilities
{
    public static partial class ListExtensions
    {
        public static void      SetParam(this IEnumerable<Animator> animators, string paramName, bool value)
        {
            if (animators != null)
            {
                foreach (var anim in animators)
                {
                    if (anim != null)
                    {
                        anim.SetBool(paramName, value);
                    }
                }
            }
        }

        public static void      SetTrigger(this IEnumerable<Animator> animators, string paramName)
        {
            if (animators != null)
            {
                foreach (var anim in animators)
                {
                    if (anim != null)
                    {
                        anim.SetTrigger(paramName);
                    }
                }
            }
        }

        public static void      SetParam(this IEnumerable<Animator> animators, string paramName, bool value, int from, int to)
        {
            if (animators != null)
            {
                int             count = 0;

                foreach (var anim in animators)
                {
                    if (anim != null)
                    {
                        if (count >= from)
                        {
                            anim.SetBool(paramName, value);
                        }
                        if (++count > to)
                        {
                            break;
                        }
                    }
                }
            }
        }

        public static void      SetTrigger(this IEnumerable<Animator> animators, string paramName, int from, int to)
        {
            if (animators != null)
            {
                int             count = 0;

                foreach (var anim in animators)
                {
                    if (anim != null)
                    {
                        if (count >= from)
                        {
                            anim.SetTrigger(paramName);
                        }
                        if (++count > to)
                        {
                            break;
                        }
                    }
                }
            }
        }
    }
}
