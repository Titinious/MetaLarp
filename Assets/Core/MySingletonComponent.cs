using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MySingletonComponent<T> : MyComponent
{
    // These variables link with components
    #region Link
    protected static T instance;
    public static T Instance
    {
        get
        {
            return instance;
        }
    }

    #endregion

    /// <summary>
    /// Set the singleton's instance.
    /// </summary>
    /// <param name="_singleton">Singleton.</param>
    protected void setSingleton(T _singleton)
    {
        instance = _singleton;
    }

    protected override void _set(Dictionary<string, object> args = null)
    {
        base._set();
        this.setSingleton(this.GetComponent<T>());
    }

    public static void InstantiateSingleton()
    {
        GameObject singletonGObj = new GameObject();
        singletonGObj.name = typeof(T).Name;
        MyComponent myComponent = (MyComponent) singletonGObj.AddComponent(typeof(T));
        myComponent.Set(true);

        Debug.Log(singletonGObj.name + " instantiated");
    }

    protected override void Awake()
    {
        if (autoSetOption == AutoSetOption.WhenAwake)
        {
            if(MySingletonComponent<T>.Instance == null)
                this.Set();
        }
        else if (autoSetOption == AutoSetOption.ForceWhenAwake)
        {
            this.Set(true);
        }
    }

    protected override void Start()
    {
        if (autoSetOption == AutoSetOption.WhenStart)
        {
            if (MySingletonComponent<T>.Instance == null)
                this.Set();
        }
        else if (autoSetOption == AutoSetOption.ForceWhenStart)
        {
            this.Set(true);
        }
    }
}
