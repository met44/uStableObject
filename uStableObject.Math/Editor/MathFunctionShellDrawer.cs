using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Supyrb;
using uStableObject.Math;
using System.Reflection;

namespace                           uStableObject.Math
{
    [CustomPropertyDrawer(typeof(MathFunctionShell), true)]
    public class                    MathFunctionShellDrawer : PropertyDrawer
    {
        const float                 LineHeight = 16;

        public override float       GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            MathFunctionShell       shell = property.GetValue<MathFunctionShell>();
            SerializedProperty      mathFunctionProperty = property.FindPropertyRelative("_function");
            MathFunctionBase        function = mathFunctionProperty?.GetValue<MathFunctionBase>();

            if (shell != null && function != null)
            {
                return 100;
            }
            else
            {
                return (LineHeight);
            }
        }

        public override void        OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            MathFunctionShell       shell = property.GetValue<MathFunctionShell>();
            SerializedProperty      mathFunctionProperty = property.FindPropertyRelative("_function");
            MathFunctionBase        function = mathFunctionProperty?.GetValue<MathFunctionBase>();
            if (shell != null && function != null)
            {
                // Calculate rects
                var propertyRect = new Rect(position.x, position.y, position.width - 35, LineHeight);
                var deleteButtonRect = new Rect(propertyRect.xMax + 5, position.y, 30, LineHeight);
                if (GUI.Button(deleteButtonRect, "-"))
                {
                    property.SetValue<MathFunctionShell>(null);
                }
                mathFunctionProperty.objectReferenceValue = EditorGUI.ObjectField(propertyRect, GUIContent.none, mathFunctionProperty.objectReferenceValue, typeof(MathFunctionBase), false);
                if (this.RefreshShell(property, indent, shell, (MathFunctionBase)mathFunctionProperty.objectReferenceValue)) return;
                propertyRect.yMin += LineHeight;
                position.yMin += LineHeight;
                position.height -= LineHeight;
                this.ShowShellProperties(position, property, shell);
            }
            else
            {
                if (shell != null)
                {
                    var assetRect = new Rect(position.x, position.y, position.width - 35, LineHeight);
                    var addButtonRect = new Rect(assetRect.xMax + 5, position.y, 30, LineHeight);
                    mathFunctionProperty.objectReferenceValue = EditorGUI.ObjectField(assetRect, GUIContent.none, mathFunctionProperty.objectReferenceValue, typeof(MathFunctionBase), false);
                    if (this.RefreshShell(property, indent, shell, (MathFunctionBase)mathFunctionProperty.objectReferenceValue)) return;
                }
                else
                {
                    var assetRect = new Rect(position.x, position.y, position.width, LineHeight);
                    MathFunctionBase mathFunction = (MathFunctionBase)EditorGUI.ObjectField(assetRect, GUIContent.none, null, typeof(MathFunctionBase), false);
                    if (mathFunction)
                    {
                        shell = mathFunction.GetNewShell();
                        property.SetValue<MathFunctionShell>(shell);
                    }
                }
            }

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }

        bool                        RefreshShell(SerializedProperty property, int indent, MathFunctionShell shell, MathFunctionBase function)
        {
            //MathFunctionBase function = mathFunctionProperty.GetValue<MathFunctionBase>();
            if (function != null)
            {
                if (shell == null || shell.GetType() != function.GetShellType())
                {
                    var newShell = function.GetNewShell();
                    Debug.Log("Setting new shell => " + newShell.GetType().Name);
                    property.SetValue<MathFunctionShell>(newShell);
                    EditorGUI.indentLevel = indent;
                    EditorGUI.EndProperty();
                    return (true);
                }
            }
            return (false);
        }

        IEnumerable<FieldInfo>      GetShellChildFields(SerializedProperty shellProperty, MathFunctionShell shell)
        {
            BindingFlags bindings = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            FieldInfo[] fields = shell.GetType().GetFields(bindings);
            foreach (var field in fields)
            {
                if (!field.IsNotSerialized)
                {
                    yield return (field);
                }
            }
        }

        void                        ShowShellProperties(Rect position, SerializedProperty property, MathFunctionShell shell)
        {
            if (property.hasChildren)
            {
                Rect childRect = new Rect(position.xMin, position.yMin, position.width, LineHeight);

                IEnumerator enumerator = property.GetEnumerator();
                enumerator.MoveNext();
                while (enumerator.MoveNext())
                {
                    var childProperty = enumerator.Current as SerializedProperty;
                    EditorGUI.PropertyField(childRect, childProperty);
                    childRect.yMin += LineHeight;
                }
                /*
                foreach (var childField in this.GetShellChildFields(property, shell))
                {
                    if (childField.FieldType == typeof(int))
                    {
                        this.ShowField<int>(childRect, childField, shell, EditorGUI.IntField);
                    }
                    else if (childField.FieldType == typeof(float))
                    {
                        this.ShowField<float>(childRect, childField, shell, EditorGUI.FloatField);
                    }
                    else if (childField.FieldType == typeof(double))
                    {
                        this.ShowField<double>(childRect, childField, shell, EditorGUI.DoubleField);
                    }
                    else if (childField.FieldType == typeof(long))
                    {
                        this.ShowField<long>(childRect, childField, shell, EditorGUI.LongField);
                    }
                    else if (typeof(IList<MathFunctionShell>).IsAssignableFrom(childField.FieldType))
                    {
                        this.ShowFieldList(ref childRect, childField, shell);
                    }
                    childRect.yMin += LineHeight;
                }*/
            }
        }

        void                        ShowField<T>(Rect position, FieldInfo field, MathFunctionShell shell, System.Func<Rect, string, T, T> displayFunc) where T : System.IEquatable<T>
        {
            T currentVal = (T)field.GetValue(shell);
            T newVal = displayFunc(position, field.Name, currentVal);
            if (!currentVal.Equals(newVal))
            {
                field.SetValue(shell, newVal);
            }
        }

        void                        ShowFieldList(ref Rect position, FieldInfo field, MathFunctionShell shell)
        {
            var list = (IList<MathFunctionShell>)field.GetValue(shell);
            if (list == null)
            {
                list = new List<MathFunctionShell>();
                field.SetValue(shell, list);
            }
            foreach (var listedShell in list)
            {

                position.yMin += LineHeight;
            }
        }
    }
}
