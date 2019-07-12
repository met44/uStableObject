using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;
using uStableObject.Data;

namespace                                   uStableObject.UI
{
    public abstract class                   EntityRowBase<T, E> : MonoBehaviour
                                            where T : IEntity 
                                            where E : UnityEvent<T>
    {
        #region Input Data
        [SerializeField] protected Image[]  _icon;
        [SerializeField] protected Text     _label;
        [SerializeField] protected E        _onPreSelected;
        [SerializeField] protected E        _onClicked;
        #endregion

        #region Members
        protected T                         _entity;
        #endregion

        #region Properties
        public T                            Entity { get { return (this._entity); } }
        #endregion

        #region Triggers
        public virtual void                 SetEntity(T entity)
        {
            if (!Object.Equals(this._entity, entity))
            {
                this._entity = entity;
                this.InitIcon();
                this.InitLabel();
            }
        }

        public void                         PreSelected()
        {
            this._onPreSelected.Invoke(this._entity);
        }

        public void                         OnClicked()
        {
            this._onClicked.Invoke(this._entity);
        }
        #endregion

        #region Helpers
        protected virtual void              InitIcon()
        {
            if (this._icon != null)
            {
                foreach (var image in this._icon)
                {
                    image.sprite = this._entity.Icon;
                }
            }
        }

        protected virtual void              InitLabel()
        {
            if (this._label)
            {
                this._label.text = this._entity.Name;
            }
        }
        #endregion
    }
}
