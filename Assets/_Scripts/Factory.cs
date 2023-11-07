/*
 * 
    Author's Name:          Audrey Bernier Larose based on this tutorial: https://www.patrykgalach.com/2019/03/28/implementing-factory-design-pattern-in-unity/
    Last Modified By:       Audrey Bernier Larose
    Last Date Modified:     October 28, 2023
    Program Description:    Generic factory class used as a base class for other factories.
    Revision History:       October 28, 2023: Initial script and documentation.
 */

using UnityEngine;

public class Factory<T> : MonoBehaviour where T : MonoBehaviour {
    [SerializeField]
    private T prefab;

    /// <summary>
    /// Instantiates and returns an instance of an object.
    /// </summary>
    /// <param name="pos">
    ///     Position where the object is to be instantiated.
    /// </param>
    /// <returns></returns>
    public T GetNewInstance(Vector3 pos) {
        return Instantiate(prefab, pos, Quaternion.identity);
    }

    /// <summary>
    /// Instantiate and returns an instance of an object.
    /// </summary>
    /// <returns></returns>
    public T GetNewInstance() {
        return Instantiate(prefab);
    }
}
