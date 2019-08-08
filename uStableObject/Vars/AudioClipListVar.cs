using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace uStableObject.Data
{
    [CreateAssetMenu(menuName = "uStableObject/Var/AudioClip List Var", order = 2)]
    public class                            AudioClipListVar : GameEventData
    {
        [SerializeField] AudioClipList      _list;

        public AudioClipList                AudioClipList
        {
            get
            {
                return (this._list);
            }
            set
            {
                if (this._list != value)
                {
                    this._list = value;
                    this.Changed.Invoke();
                }
            }
        }

        protected override void             RaiseIsNotNull()
        {
            this.ForceRaise(this._list != null);
        }

        public override void                Clear()
        {
            this.AudioClipList = null;
        }
    }
}
