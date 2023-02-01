using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The expected base component class of the this project's in-house components
/// </summary>
public class MyComponent : MonoBehaviour
{
    // These variables describe the setting of the component
    #region Description
    [Tooltip("To decide when the component will set.")]
    [SerializeField]
    protected AutoSetOption autoSetOption = AutoSetOption.WhenAwake;
    protected enum AutoSetOption
    {
        WhenAwake, // You may consider this option if you just want the component to set itself
        ForceWhenAwake, // You may consider this option for a singleton object that will need to reset when scene changes
        WhenStart,
        ForceWhenStart,
        None // You may consider this option if you want other component to call Set() for you.
    }

    [Tooltip("To decide which components will set following the setting of this one")]
    [SerializeField]
    protected MyComponent[] settingChain;

    #endregion
    // These variables describe the current state of the component
    #region State
    bool _isSet = false;
    public bool IsSet
    {
        get
        {
            return _isSet;
        }
    }
    #endregion

    protected virtual void _set(Dictionary<string, object> args = null)
    {
        // implement component setting here 
        if (settingChain != null)
            Array.ForEach(settingChain, (s) => { if (s.enabled) s.Set(); });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="forceSet">Force the component to set even if it is already set</param>
    /// <param name="args">The customized arguments for setting the components</param>
    public void Set(bool forceSet = false, Dictionary<string, object> args = null)
    {
        // check if the script need to set. If already set, ignore requrest unless is a force set.
        if (!forceSet && _isSet)
        {
            return;
        }
        this._set(args);
        _isSet = true;
    }

    protected virtual void Awake()
    {
        if (autoSetOption == AutoSetOption.WhenAwake)
        {
            this.Set();
        }
        else if (autoSetOption == AutoSetOption.ForceWhenAwake)
        {
            this.Set(true);
        }
    }

    protected virtual void Start()
    {
        if (autoSetOption == AutoSetOption.WhenStart)
        {
            this.Set();
        }
        else if (autoSetOption == AutoSetOption.ForceWhenStart)
        {
            this.Set(true);
        }
    }
}
