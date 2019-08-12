using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using uStableObject.Data;

namespace                                   uStableObject.Utilities
{
    [RequireComponent(typeof(AudioSource))]
    public class                            AudioClipListPlayer : MonoBehaviour
    {
        #region Input Data
        [HideInInspector]
        [SerializeField] AudioSource        _audioSource;
        [SerializeField] AudioClipListVar   _targetAudioClipList;
        [SerializeField] bool               _fireOnClipEndsEvent;
        [SerializeField] UnityEvent         _onClipBeganEvent;
        [SerializeField] UnityEvent         _onClipEndsEvent;
        #endregion

        void                                Reset()
        {
            this._audioSource = this.GetComponent<AudioSource>();
        }

        #region Triggers
        public void                         PlayRandom()
        {
            if (this.gameObject.activeInHierarchy)
            {
                var randClip = this._targetAudioClipList.AudioClipList.RandClip;
                this._audioSource.clip = randClip;
                this._audioSource.Play();
                this._onClipBeganEvent.Invoke();
                if (this._fireOnClipEndsEvent)
                {
                    CancelInvoke("FireClipEndsEvent");
                    Invoke("FireClipEndsEvent", randClip.length);
                }
            }
        }

        public void                         PlayRandomIfNotPlaying()
        {
            if (this.gameObject.activeInHierarchy && !this._audioSource.isPlaying)
            {
                this.PlayRandom();
            }
        }

        public void                         CancelPlay()
        {
            this._audioSource.Stop();
            CancelInvoke("FireClipEndsEvent");
        }

        void                                FireClipEndsEvent()
        {
            this._onClipEndsEvent.Invoke();
        }
        #endregion
    }
}
