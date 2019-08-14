using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using uStableObject.Data.Localization;
using uStableObject.Utilities.Converters;

namespace                               uStableObject.Utilities.Editor
{
    public static class                 EditorSelectionTools
    {
        [MenuItem("Selection/Child Texts #&t")]
        public static void              SelectChildUIText()
        {
            List<Text> childTexts = new List<Text>();
            foreach (var go in Selection.gameObjects)
            {
                childTexts.AddRange(go.GetComponentsInChildren<Text>());
            }
            Selection.objects = childTexts.Select(ct => ct.gameObject).ToArray();
        }

        [MenuItem("Selection/Child Texts #&t", true)]
        public static bool              SelectChildUITextValidation()
        {
            return (Selection.gameObjects != null && Selection.gameObjects.Length > 0);
        }

        [MenuItem("Selection/Child Texts NO LOC #&n")]
        public static void              SelectChildUITextNoLoc()
        {
            List<Text> childTexts = new List<Text>();
            foreach (var go in Selection.gameObjects)
            {
                foreach (var label in go.GetComponentsInChildren<Text>())
                {
                    if (!label.GetComponent<LabelLocalization>()
                        && !label.GetComponentInParent<InputField>()
                        && !label.GetComponentInParent<Dropdown>()
                        && !label.GetComponentInParent<Slider>())
                    {
                        childTexts.Add(label);
                    }
                }
            }
            Selection.objects = childTexts.Select(ct => ct.gameObject).ToArray();
        }

        [MenuItem("Selection/Child Texts NO LOC #&n", true)]
        public static bool              SelectChildUITextNoLocValidation()
        {
            return (Selection.gameObjects != null && Selection.gameObjects.Length > 0);
        }

        [MenuItem("Selection/Not set LabelLocalization #&l")]
        public static void              SelectClearLocalizedLabels()
        {
            List<LabelLocalization> labelLocs = new List<LabelLocalization>();
            if (Selection.activeObject == null)
            {
                foreach (var lloc in GameObject.FindObjectsOfType<LabelLocalization>())
                {
                    if (lloc.Localization == null && !lloc.Ignore
                        && !lloc.GetComponent<IntValueStringConverter>()
                        && !lloc.GetComponent<GameVarWatcherString>())
                    {
                        labelLocs.Add(lloc);
                    }
                }
            }
            else
            {
                foreach (var go in Selection.gameObjects)
                {
                    var llocs = go.GetComponentsInChildren<LabelLocalization>();
                    foreach (var lloc in llocs)
                    {
                        if (lloc.Localization == null && !lloc.Ignore
                            && !lloc.GetComponent<IntValueStringConverter>()
                            && !lloc.GetComponent<GameVarWatcherString>())
                        {
                            labelLocs.Add(lloc);
                        }
                    }
                }
            }
            Selection.objects = labelLocs.Select(ct => ct.gameObject).ToArray();
        }
    }
}

