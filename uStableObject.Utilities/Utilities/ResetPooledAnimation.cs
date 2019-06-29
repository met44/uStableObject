using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace                               uStableObject
{
    public class                        ResetPooledAnimation : MonoBehaviour
    {
        #region Input Data
        [SerializeField] Renderer[]     _renderers;
        #endregion

        #region Members
        bool                            _shouldEnableRenderersNextFrame;
        bool                            _shouldEnableRenderers;
        #endregion

        #region Unity
        void                            Awake()
        {
            this.OnDespawned();
        }

        void                            Update()
        {
            if (this._shouldEnableRenderersNextFrame)
            {
                this._shouldEnableRenderersNextFrame = false;
                this._shouldEnableRenderers = true;
            }
            else if (this._shouldEnableRenderers)
            {
                this._shouldEnableRenderers = false;
                foreach (var renderer in this._renderers)
                {
                    renderer.enabled = true;
                }
                this.enabled = false;
            }
        }
        #endregion

        #region Triggers
        void                            OnSpawned()
        {
            this._shouldEnableRenderersNextFrame = true;
            this.enabled = true;
        }

        void                            OnDespawned()
        {
            foreach (var renderer in this._renderers)
            {
                renderer.enabled = false;
            }
        }
        #endregion
    }
}
