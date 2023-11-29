/*
 * 
    Author's Name:          Audrey Bernier Larose
    Last Modified By:       Audrey Bernier Larose
    Last Date Modified:     November 28, 2023
    Program Description:    Static utility class containing reusable methods.
    Revision History:       October 28, 2023: Initial script and documentation.
                            November 10, 2023: Adjusted the y-position of the obj in Movement()
                            November 21, 2023: Add an if statement in the Movement function
                            November 28, 2023: Using pos in LookAt function in Movement() instead of other.transform.position
 */
using UnityEngine;

public static class Utils
{
    /// <summary>
    /// Determines if an object is close enough to this object based on a
    /// predetermined distance.
    /// </summary>
    /// <param name="distance"></param>
    /// <param name="obj"></param>
    /// <param name="other"></param>
    /// <returns>
    ///     True if the other object is within the specified distance.
    /// </returns>
    public static bool OtherCloseEnough(float distance, GameObject obj, GameObject other) {        
        return Vector3.Distance(obj.transform.position, other.transform.position) <= distance;
    }

    /// <summary>
    /// Checks if the other object is in front of this object.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="other"></param>
    /// <param name="cosOtherFOVOver2InRad"></param>
    /// <returns>
    ///     True if the other object is in front of this object.
    /// </returns>
    public static bool OtherInFront(GameObject obj, GameObject other, float cosOtherFOVOver2InRad) {
        Vector3 otherHeading = (other.transform.position - obj.transform.position).normalized;
        float cosAngle = Vector3.Dot(otherHeading, obj.transform.forward);

        return cosAngle > cosOtherFOVOver2InRad;
    }

    /// <summary>
    /// Check if this object senses the other object based on a fields of view and a distance.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="other"></param>
    /// <param name="cosOtherFOVOver2InRad"></param>
    /// <param name="distance"></param>
    /// <returns>
    ///     True if the other object is within the specified field of view and distance.
    /// </returns>
    public static bool SenseOther(GameObject obj, GameObject other, float cosOtherFOVOver2InRad, float distance) {
        return OtherInFront(obj, other, cosOtherFOVOver2InRad)
            && OtherCloseEnough(distance, obj, other);
    }

    /// <summary>
    /// Checks if the current value is below a threshold.
    /// </summary>
    /// <param name="threshold"></param>
    /// <param name="current"></param>
    /// <returns>
    ///     True if the current value is below the threshold.
    /// </returns>
    public static bool IsBelowThreshold(float threshold, float current) {
        return threshold >= current;
    }

    /// <summary>
    /// Sets the movement of an object based on the other object's position.
    /// </summary>
    /// <param name="isFollowing"></param>
    /// <param name="obj"></param>
    /// <param name="other"></param>
    /// <param name="pos"></param>
    /// <param name="speed"></param>
    public static void Movement(bool isFollowing, GameObject obj, GameObject other, out Vector3 pos, float speed) {
        Vector3 objHeading = (other.transform.position - obj.transform.position);
        float objDistance = objHeading.magnitude;
        objHeading.Normalize();

        Vector3 movement = speed * Time.deltaTime * objHeading;
        Vector3.ClampMagnitude(movement, objDistance);

        Vector3 newPos = new Vector3(movement.x, 0, movement.z);
        pos = isFollowing ? obj.transform.position + newPos : obj.transform.position - newPos;

        obj.transform.LookAt(pos);        
    }
}