using UnityEngine;

public static class Vector3Extensions
{
    public static Vector3 Randomize(this Vector3 vector, float min, float max)
    {
        vector.x = Random.Range(min, max);
        vector.y = Random.Range(min, max);
        vector.z = Random.Range(min, max);
        return vector;
    }
}