using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace                           uStableObject.Utilities
{
    public static class             TextUtils
    {
        static string[]             _intStrings;
        static string[]             _intStringsNegative;

        static                      TextUtils()
        {
            int amount = 101;
            _intStrings = new string[amount];
            _intStringsNegative = new string[amount];
            for (var i = 0; i < amount; ++i)
            {
                _intStrings[i] = i.ToString();
                _intStringsNegative[i] = "-" + _intStrings[i];
            }
        }

        public static string        ToStringNonAlloc(this int val)
        {
            if (Mathf.Abs(val) <= 100)
            {
                if (val < 0)
                {
                    return (_intStringsNegative[-val]);
                }
                else
                {
                    return (_intStrings[val]);
                }
            }
            return (val.ToString());
        }

        public static string        ToStringNonAlloc(this uint val)
        {
            if (Mathf.Abs(val) <= 100)
            {
                if (val < 0)
                {
                    return (_intStringsNegative[-val]);
                }
                else
                {
                    return (_intStrings[val]);
                }
            }
            return (val.ToString());
        }

        public static string        ToStringNonAlloc(this byte val)
        {
            if (Mathf.Abs(val) <= 100)
            {
                if (val < 0)
                {
                    return (_intStringsNegative[-val]);
                }
                else
                {
                    return (_intStrings[val]);
                }
            }
            return (val.ToString());
        }
    }
}