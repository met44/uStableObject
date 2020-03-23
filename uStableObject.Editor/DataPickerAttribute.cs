using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace                   uStableObject
{
    public class            DataPicker : PropertyAttribute
    {
        public bool         AllowBaseType { get; private set; }
        public Type         BaseType { get; private set; }
        public Color        Color { get; private set; }

        public              DataPicker()
        {
        }

        public              DataPicker(Type baseType, bool allowBaseType = false)
        {
            this.AllowBaseType = allowBaseType;
            this.BaseType = baseType;
            this.Color = Color.grey;
        }

        public              DataPicker(string colorHex)
        {
            this.AllowBaseType = false;
            this.BaseType = null;
            this.SetColor(colorHex);
        }

        public              DataPicker(Type baseType, bool allowBaseType, string colorHex)
        {
            this.AllowBaseType = allowBaseType;
            this.BaseType = baseType;
            this.SetColor(colorHex);
        }

        void               SetColor(string hex)
        {
            Color          col;

            if (ColorUtility.TryParseHtmlString(hex, out col))
            {
                this.Color = col;
            }
            else
            {
                this.Color = Color.grey;
            }
        }
    }
}
