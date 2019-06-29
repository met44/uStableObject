using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace                                   uStableObject.Data
{
    [CreateAssetMenu(menuName = "uStableObject/Var/AudioClip list", order = 1)]
    public class                            AudioClipList : ScriptableObject
    {
        [SerializeField] AudioClip[]        _clips;

        public AudioClip[]                  Clips {  get { return this._clips; } }
        public AudioClip                    RandClip {  get { return this._clips[Random.Range(0, this._clips.Length)]; } }
    }
}
