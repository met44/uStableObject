using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using uStableObject.Data;
using uStableObject.Utilities;
using UnityEngine.UI;

namespace                                   uStableObject.UI
{
    public abstract class                   DisplayableListUIBase<T, R, E> : MonoBehaviour, IDisplayableListUI<T>
                                            where T : IDisplayable
                                            where E : UnityEvent<T>
                                            where R : DisplayableRowBase<T, E>
    {
        #region Input Data
        [SerializeField] DisplaybleListVar      _list;
        [SerializeField] RectTransform      _instancesRoot;
        [SerializeField] R                  _entityRowPrefab;
        [SerializeField] int                _entitiesPerRow = 1;
        [SerializeField] bool               _autoSelectIfNoPreselected;
        [SerializeField] bool               _centerRows;
        #endregion

        #region Members
        float                               _rowWidth;
        float                               _rowHeight;
        int                                 _prevEntityCount = 1;
        List<R>                             _instances = new List<R>();
        #endregion

        #region Properties
        public IReadOnlyList<R>             Instances       { get { return (this._instances); } }
        public T                            SelectedEntity  { get; set; }
        public IDisplayableList                  ListVar         { get; set; }
        #endregion

        #region Unity
        void                                Awake()
        {
            this.ListVar = this._list;
            this._rowWidth = (this._entityRowPrefab.transform as RectTransform).rect.width;
            this._rowHeight = (this._entityRowPrefab.transform as RectTransform).rect.height;
        }
        #endregion

        #region Triggers
        public void                         Refresh()
        {
            int                             i = 0;

            this.OnBeforeRefresh();
            if (this.ListVar == null)
            {
                this.Awake();
            }
            if (this.ListVar != null && this.ListVar.Entities != null)
            {
                if (this._autoSelectIfNoPreselected)
                {
                    bool                        selectedIsInList = false;

                    if (this.SelectedEntity != null) //ensure entity is in list otherwise we must select another one
                    {
                        foreach (var entity in this.ListVar.Entities)
                        {
                            if (Object.Equals(entity, this.SelectedEntity))
                            {
                                selectedIsInList = true;
                                break;
                            }
                        }
                    }
                    if (!selectedIsInList)
                    {
                        foreach (var entity in this.ListVar.Entities)
                        {
                            this.SelectedEntity = (T)entity;
                            break;
                        }
                    }
                }
                foreach (var entity in this.ListVar.Entities)
                {
                    UnityEngine.Profiling.Profiler.BeginSample("RefreshEntities");
                    i = this.InitEntityRow(i, (T)entity);
                    UnityEngine.Profiling.Profiler.EndSample();
                }
            }
            for (var n = this._instances.Count - 1; n >= i; --n)
            {
                var instance = this._instances[n];
                if (instance.gameObject.activeSelf)
                {
                    instance.gameObject.SetActive(false);
                }
                instance.transform.SetParent(this.transform);
                this._instances.RemoveAt(n);
                GoPool.Despawn(instance);
            }
            //if (this._prevEntityCount != this._instances.Count)
            {
                if (this._centerRows && this._instances.Count > 1)
                {
                    float rowOffset = (Mathf.Min(this._instances.Count, this._entitiesPerRow) - 1f) / 2f * -this._rowWidth;
                    for (var n = 0; n < this._instances.Count; ++n)
                    {
                        var instance = this._instances[n];
                        var localPos = instance.transform.localPosition;
                        localPos.x += rowOffset;
                        instance.transform.localPosition = localPos;
                    }
                }
                this._prevEntityCount = this._instances.Count;
                this.ResizeScrollView();
            }
            this.OnAfterRefresh();
        }

        public void                         ClearSelectedEntity()
        {
            if (this.SelectedEntity != null)
            {
                this.SelectedEntity = default(T);
            }
        }

        public void                         ApplySelectedEntity()
        {
            if (this.SelectedEntity != null)
            {
                foreach (var entityRow in this._instances)
                {
                    if (Object.Equals(entityRow.Entity, this.SelectedEntity))
                    {
                        entityRow.PreSelected();
                        break;
                    }
                }
            }
        }
        #endregion

        #region Helpers
        protected virtual void              OnBeforeRefresh() { }
        protected virtual void              OnAfterRefresh() { }

        protected virtual int               InitEntityRow(int entityRowNum, T entity)
        {
            var entityRow = this.GetEntityRow(entityRowNum);
            entityRow.SetEntity(entity);
            if (Object.Equals(entity, this.SelectedEntity))
            {
                entityRow.PreSelected();
            }
            return (++entityRowNum);
        }

        protected R                         GetEntityRow(int num)
        {
            R                               rowInstance;

            int col = num % this._entitiesPerRow;
            int row = num / this._entitiesPerRow;
            Vector3 localPos = new Vector3(col * this._rowWidth, (-row) * this._rowHeight, 0);
            if (num >= this._instances.Count)
            {
                rowInstance = GoPool.Spawn(this._entityRowPrefab, this._instancesRoot.TransformPoint(localPos), this._instancesRoot.rotation, this._instancesRoot);
                rowInstance.transform.SetSiblingIndex(num);
                rowInstance.List = this;
                this._instances.Add(rowInstance);
            }
            else
            {
                rowInstance = this._instances[num];
                rowInstance.transform.localPosition = localPos;
                if (!rowInstance.gameObject.activeSelf)
                {
                    rowInstance.gameObject.SetActive(true);
                }
            }
            return (rowInstance);
        }

        void                                ResizeScrollView()
        {
            int cols = Mathf.Min(this._instances.Count, this._entitiesPerRow);
            int rows = Mathf.CeilToInt((float)this._instances.Count / this._entitiesPerRow);
            float colWidth = (this._entityRowPrefab.transform as RectTransform).rect.width;
            float rowheight = (this._entityRowPrefab.transform as RectTransform).rect.height;
            this._instancesRoot.sizeDelta = new Vector2(Mathf.CeilToInt((float)cols) * colWidth, 
                                                        Mathf.CeilToInt((float)rows) * rowheight);
        }
        #endregion
    }
}
