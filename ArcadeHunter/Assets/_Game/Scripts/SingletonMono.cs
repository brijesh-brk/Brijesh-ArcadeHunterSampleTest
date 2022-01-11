using System;
using System.Diagnostics;
using UnityEngine;

public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance = null;
    public static T Instance
    {
        get
        {
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(instance);
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }
    public string Classname
    {
        get
        {
            return GetType().Name;
        }
    }

    public string CurrentMethodName
    {
        get
        {
            return new StackTrace().GetFrame(1).GetMethod().Name;
        }
    }
    public string ClassAndMethodName
    {
        get
        {
            return GetType().Name +" "+(new StackTrace().GetFrame(1).GetMethod().Name)+" ";
        }
    }
    public string ClassMethodName
    {
        get
        {
            return GetType().Name + " " + (new StackTrace().GetFrame(1).GetMethod().Name) + " ";
        }
    }
}

 
/// <summary>
/// A base class for the singleton design pattern.
/// </summary>
/// <typeparam name="T">Class type of the singleton</typeparam>
public abstract class Singleton<T> where T : class
{
    #region Members

    /// <summary>
    /// Static instance. Needs to use lambda expression
    /// to construct an instance (since constructor is private).
    /// </summary>
    private static readonly Lazy<T> sInstance = new Lazy<T>(() => CreateInstanceOfT());

    #endregion

    #region Properties

    /// <summary>
    /// Gets the instance of this singleton.
    /// </summary>
    public static T Instance { get { return sInstance.Value; } }

    #endregion

    #region Methods

    /// <summary>
    /// Creates an instance of T via reflection since T's constructor is expected to be private.
    /// </summary>
    /// <returns></returns>
    private static T CreateInstanceOfT()
    {
        return Activator.CreateInstance(typeof(T), true) as T;
    }

    #endregion
}