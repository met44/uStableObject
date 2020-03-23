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
                this.PlayRandom(this._targetAudioClipList);
            }
        }

        public void                         PlayRandom(AudioClipListVar audioClipListVar)
        {
            if (this.gameObject.activeInHierarchy)
            {
                var randClip = audioClipListVar.AudioClipList.RandClip;
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

        public bool                         CanPlay()
        {
            return (this.gameObject.activeInHierarchy && !this._audioSource.isPlaying);
        }

        public void                         PlayRandomIfNotPlaying()
        {
            if (this.CanPlay())
            {
                this.PlayRandom();
            }
        }

        public void                         PlayRandomIfNotPlaying(AudioClipListVar audioClipListVar)
        {
            if (this.CanPlay())
            {
                this.PlayRandom(audioClipListVar);
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
