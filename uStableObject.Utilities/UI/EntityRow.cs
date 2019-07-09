using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;
using uStableObject.Data;

namespace                           uStableObject.UI
{
    public abstract class           EntityRow<T, E> : MonoBehaviour
                                    where T : IEntity 
                                    where E : UnityEvent<T>
    {
        #region Input Data
        [SerializeField] Image[]    _icon;
        [SerializeField] Text       _label;
        [SerializeField] E          _onPreSelected;
        [SerializeField] E          _onClicked;
        #endregion

        #region Members
        T                           _entity;
        #endregion

        #region Triggers
        public void                 SetEntity(T entity)
        {
            if (!Object.Equals(this._entity, entity))
            {
                this._entity = entity;
                if (this._icon != null)
                {
                    foreach (var image in this._icon)
                    {
                        image.sprite = entity.Icon;
                    }
                }
                if (this._label)
                {
                    this._label.text = entity.Name;
                }
            }
        }

        public void                 PreSelected()
        {
            this._onPreSelected.Invoke(this._entity);
        }

        public void                 OnClicked()
        {
            this._onClicked.Invoke(this._entity);
        }
        #endregion
    }
}
