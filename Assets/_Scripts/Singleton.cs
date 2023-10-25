/*  Author's Name:          Marcus Ngooi
 *  Last Modified By:       Marcus Ngooi
 *  Date Last Modified:     October 24, 2023
 *  Program Description:    Parent class implementing Singleton pattern. 
 *                          If a class needs to be a Singleton, just inherit this class.
 *  Revision History:       October 23, 2023: Initial Singleton script.
 *                          October 24, 2023: Added documentation.
 */

using UnityEngine;

/// <summary>
/// A generic class to provide a singleton implementation for inheriting classes.
/// </summary>
/// <typeparam name="T">The user-defined class</typeparam>
public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));
                if(instance == null)
                {
                    SetupInstance();
                }
            }
            return instance;
        }
    }
    public virtual void Awake()
    {
        RemoveDuplicates();
    }
    /// <summary>
    /// Create an instance when there isn't already one in the scene.
    /// </summary>
    private static void SetupInstance()
    {
        instance = (T)FindObjectOfType(typeof(T));
        if (instance == null)
        {
            GameObject gameObj = new()
            {
                name = typeof(T).Name
            };
            instance = gameObj.AddComponent<T>();
            DontDestroyOnLoad(gameObj);
        }
    }
    /// <summary>
    /// Remove duplicate instances.
    /// </summary>
    private void RemoveDuplicates()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
