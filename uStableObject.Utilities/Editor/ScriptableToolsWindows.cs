﻿using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using uStableObject.Data.Localization;
using uStableObject.Utilities.Converters;

namespace                               uStableObject.Utilities.Editor
{
    public class                        ScriptableToolsWindow : EditorWindow
    {
        ScriptableObject                _parent;
        ScriptableObject                _child;
        ScriptableObject                _splitTarget;

        [MenuItem("Tools/Scriptables Tools")]
        static void                     Init()
        {
            // Get existing open window or if none, make a new one:
            ScriptableToolsWindow window = EditorWindow.GetWindow<ScriptableToolsWindow>("Scriptables Tools");
            window.Show();
        }

        public void                     OnEnable()
        {
            Selection.selectionChanged -= this.RefreshGui;
            Selection.selectionChanged += this.RefreshGui;
        }

        public void                     OnDisable()
        {
            Selection.selectionChanged -= this.RefreshGui;
        }

        void                            RefreshGui()
        {
            this.Repaint();
        }

        void                            OnGUI()
        {
            GUILayout.BeginHorizontal();
            this.ShowMergeGUI();
            GUILayout.Space(30);
            this.ShowSplitGUI();
            GUILayout.Space(30);
            this.ShowRenameSelectedGUI();
            GUILayout.Space(30);
            this.ShowDeleteSelectedGUI();
        }

        void                            ShowMergeGUI()
        {
            GUILayout.Label("Merging", EditorStyles.boldLabel);
            if (GUILayout.Button("clear"))
            {
                this._parent = null;
                this._child = null;
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            this._parent = (ScriptableObject)EditorGUILayout.ObjectField("Parent", this._parent, typeof(ScriptableObject), false);
            this._child = (ScriptableObject)EditorGUILayout.ObjectField("Child", this._child, typeof(ScriptableObject), false);

            GUILayout.Space(10);
            if (!this._parent || !this._child)
            {
                GUILayout.Toggle(true, "Select merge targets", "Button");
            }
            else if (GUILayout.Button("Merge"))
            {
                //var childClone = Instantiate(this._child);
                //childClone.name = childClone.name.Replace("(Clone)", "");
                //AssetDatabase.AddObjectToAsset(childClone, this._parent);
                string sourcePath = AssetDatabase.GetAssetPath(this._child);
                AssetDatabase.RemoveObjectFromAsset(this._child);
                AssetDatabase.DeleteAsset(sourcePath);
                AssetDatabase.AddObjectToAsset(this._child, this._parent);
                EditorUtility.SetDirty(this._parent);
                AssetDatabase.SaveAssets();
                Selection.activeObject = this._child;
            }
        }

        void                            ShowSplitGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Splitting", EditorStyles.boldLabel);
            if (GUILayout.Button("clear"))
            {
                this._splitTarget = null;
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            this._splitTarget = (ScriptableObject)EditorGUILayout.ObjectField("Target (child)", this._splitTarget, typeof(ScriptableObject), false);

            GUILayout.Space(10);
            if (!this._splitTarget)
            {
                GUILayout.Toggle(true, "Select split target", "Button");
            }
            else
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Split"))
                {
                    this.SplitAssets(false);
                }
                if (GUILayout.Button("Split & Prepare Merge"))
                {
                    this.SplitAssets(true);
                }
                GUILayout.EndHorizontal();
            }
        }

        void                            ShowRenameSelectedGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Rename Selected", EditorStyles.boldLabel);
            if (GUILayout.Button("clear"))
            {
                Selection.activeObject = null;
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            if (Selection.objects != null)
            {
                var maxWidth = GUILayout.MaxWidth(this.position.width < 300 ? 35 : this.position.width * 0.35f);
                foreach (var obj in Selection.objects)
                {
                    if (obj is ScriptableObject)
                    {
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.ObjectField("", obj, typeof(ScriptableObject), false, maxWidth);
                        string newName = GUILayout.TextField(obj.name);
                        if (newName != obj.name)
                        {
                            obj.name = newName;
                            EditorUtility.SetDirty(obj);
                        }
                        GUILayout.EndHorizontal();
                    }
                }
            }
        }

        object readyTodelete;
        void                            ShowDeleteSelectedGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Delete Selected", EditorStyles.boldLabel);
            if (GUILayout.Button("clear"))
            {
                Selection.activeObject = null;
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            if (Selection.objects != null)
            {
                var maxWidth = GUILayout.MaxWidth(this.position.width < 300 ? 35 : this.position.width * 0.35f);
                foreach (var obj in Selection.objects)
                {
                    if (obj is ScriptableObject)
                    {
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.ObjectField("", obj, typeof(ScriptableObject), false, maxWidth);
                        if (GUILayout.Toggle(obj.Equals(readyTodelete), "Prepare delete"))
                        {
                            readyTodelete = obj;
                            if (GUILayout.Button("delete"))
                            {
                                this.DeleteAsset(obj);
                            }
                        }
                        GUILayout.EndHorizontal();
                    }
                }
            }
        }

        void                            SplitAssets(bool prepareForMergeBack)
        {
            string assetPath = AssetDatabase.GetAssetPath(this._splitTarget);
            if (prepareForMergeBack)
            {
                var mainAsset = AssetDatabase.LoadMainAssetAtPath(assetPath) as ScriptableObject;
                this._parent = mainAsset;
                this._child = this._splitTarget;
            }
            string directory = System.IO.Path.GetDirectoryName(assetPath);
            string filePath = System.IO.Path.Combine(directory, this._splitTarget.name);
            string newPath = AssetDatabase.GenerateUniqueAssetPath(filePath + ".asset");
            AssetDatabase.RemoveObjectFromAsset(this._splitTarget);
            AssetDatabase.CreateAsset(this._splitTarget, newPath);
            EditorUtility.SetDirty(this._splitTarget);
            AssetDatabase.SaveAssets();
            Selection.activeObject = this._splitTarget;
        }

        void                            DeleteAsset(Object asset)
        {
            string assetPath = AssetDatabase.GetAssetPath(asset);
            string directory = System.IO.Path.GetDirectoryName(assetPath);
            AssetDatabase.RemoveObjectFromAsset(asset);
            DestroyImmediate(asset);
            AssetDatabase.SaveAssets();
        }
    }
}
