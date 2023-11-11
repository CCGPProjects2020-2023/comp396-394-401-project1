/*
 * 
    Author's Name:          Audrey Bernier Larose
    Last Modified By:       Audrey Bernier Larose
    Last Date Modified:     November 11, 2023
    Program Description:    Abstract class used as a base to other ammunition types. 
    Revision History:       October 28, 2023: Initial script and documentation.
                            October 29, 2023: Changed the visibility of the start method from private to protected internal.
                            November 8, 2023: Added the OnTriggerEnter() method here, so that all derived classes can access it.
                            November 11, 2023: Added a scoreManager field to update the player score.
 */

using UnityEngine;

public class Ammunition : MonoBehaviour
{
    ScoreManager scoreManager;

    [Header("Ammunition Properties")]
    public float speed = 10f;

    [Header("Directional Properties")]
    protected internal GameObject other;
    protected internal Vector3 movement;

    /// <summary>
    /// Start method invoked by unity once the object is instantiated. 
    /// This will be called by the subclasses of this class.
    /// </summary>
    protected internal void Start()
    {
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        other = GameObject.FindGameObjectWithTag("Player");

        Vector3 objHeading = other.transform.position - transform.position;
        objHeading.Normalize();

        movement = speed * Time.deltaTime * objHeading;
    }

    /// <summary>
    /// Trigger function called by unity when this object's collider
    /// enter's another object.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))        
            scoreManager.Remove(5);
        
    }
}
