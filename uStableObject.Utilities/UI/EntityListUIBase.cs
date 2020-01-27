using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using uStableObject.Data;
using uStableObject.Utilities;
using UnityEngine.UI;

namespace                                   uStableObject.UI
{
    public abstract class                   EntityListUIBase<T, R, E> : MonoBehaviour
                                            where T : IEntity
                                            where E : UnityEvent<T>
                                            where R : EntityRowBase<T, E>
    {
        #region Input Data
        [SerializeField] EntityListVar      _list;
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
        public T                            SelectedEntity { get; set; }
        public IEntityList                  ListVar { get; set; }
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
            if (this._prevEntityCount != this._instances.Count)
            {
                if (this._centerRows)
                {
                    int diff = Mathf.Min(this._instances.Count - this._prevEntityCount, this._entitiesPerRow);
                    float rowOffset =  diff / 2f * -this._rowWidth;
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

            if (num >= this._instances.Count)
            {
                int col = num % this._entitiesPerRow;
                int row = num / this._entitiesPerRow;
                Vector3 localPos = new Vector3(col * this._rowWidth, (-row) * this._rowHeight, 0);
                rowInstance = GoPool.Spawn(this._entityRowPrefab, this._instancesRoot.TransformPoint(localPos), this._instancesRoot.rotation, this._instancesRoot);
                rowInstance.transform.SetSiblingIndex(num);
                this._instances.Add(rowInstance);
            }
            else
            {
                rowInstance = this._instances[num];
                if (!rowInstance.gameObject.activeSelf)
                {
                    rowInstance.gameObject.SetActive(true);
                }
            }
            return (rowInstance);
        }

        void                                ResizeScrollView()
        {
            float rowheight = (this._entityRowPrefab.transform as RectTransform).rect.height;
            this._instancesRoot.sizeDelta = new Vector2(this._instancesRoot.sizeDelta.x, Mathf.CeilToInt((float)this._instances.Count / this._entitiesPerRow) * rowheight);
        }
        #endregion
    }
}
