using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace                                   uStableObject.Data
{
    [CreateAssetMenu(menuName = "uStableObject/Var/Bool Group", order = 2)]
    public class                            BoolVarGroup : BoolVar, IGameEventListener<bool>
    {
        [SerializeField] List<BoolVar>      _members;
        [SerializeField] BoolVar            _fallbackMember;

        BoolVar                             _toggledOnMember;
        BoolVar                             _prevToggledOnMember;
        bool                                _togglingGroup;

        public override bool                Value
        {
            get
            {
                return (base.Value);
            }
            set
            {
                if (!value)
                {
                    if (this._toggledOnMember != null)
                    {
                        this._toggledOnMember.Value = false;
                    }
                }
            }
        }

        void                                OnEnable()
        {
            this._toggledOnMember = null;
            foreach (var member in this._members)
            {
                if (member.Value)
                {
                    if (this._toggledOnMember == null)
                    {
                        this._toggledOnMember = member;
                    }
                    else
                    {
                        member.Value = false;
                    }
                }
                member.Register(this);
            }
            if (this._toggledOnMember == null && this._fallbackMember != null)
            {
                this._fallbackMember.Value = true;
                this._toggledOnMember = this._fallbackMember;
            }
        }

        public void                         AddMember(BoolVar member)
        {
            if (!this._members.Contains(member))
            {
                this._members.Add(member);
                member.Register(this);
            }
        }

        public void                         RemoveMember(BoolVar member)
        {
            this._members.Remove(member);
            member.Unregister(this);
        }

        public void                         OnEventRaised(bool param)
        {
            if (!this._togglingGroup)
            {
                this._togglingGroup = true;
                try
                {
                    this._prevToggledOnMember = this._toggledOnMember;

                    //switch off previous
                    if (this._toggledOnMember != null)
                    {
                        if (this._toggledOnMember != this._fallbackMember || param)
                        {
                            this._toggledOnMember.Value = false;
                            this._toggledOnMember = null;
                        }
                    }
                    if (param)
                    {
                        foreach (var member in this._members)
                        {
                            if (member.Value)
                            {
                                this._toggledOnMember = member;
                                break;
                            }
                        }
                    }
                    if (this._toggledOnMember == null && this._fallbackMember != null)
                    {
                        this._toggledOnMember = this._fallbackMember;
                        this._toggledOnMember.Value = true;
                    }
                    base.Value = this._toggledOnMember != null;
                }
                catch (System.Exception ex)
                {
                    Debug.LogException(ex);
                }
                this._togglingGroup = false;
            }
        }

        public void                         RollBackToPrevious()
        {
            //event chain will set things up properly
            if (this._prevToggledOnMember != null)
            {
                this._prevToggledOnMember.Value = true; 
            }
            else if (this._toggledOnMember != null)
            {
                this._toggledOnMember.Value = false;
            }
        }
    }
}
