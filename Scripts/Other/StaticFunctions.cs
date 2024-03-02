using System.Collections.Generic;
using UnityEngine;

public static class StaticFunctions 
{
    public static Vector3 Vector2ToVector3XZ(this Vector2 inputVector)
    {
        return new Vector3(inputVector.x, 0f, inputVector.y);
    }
    
    public static Vector3 Vector2ToVector3XZInversed(this Vector2 inputVector)
    {
        return new Vector3(inputVector.y, 0f, inputVector.x);
    }
    
    public static Vector2 Vector3XZToVector2(this Vector3 inputVector)
    {
        return new Vector2(inputVector.x, inputVector.z);
    }
    
    public static Vector3 Vector3ToFlat(this Vector3 inputVector)
    {
        return new Vector3(inputVector.x, 0f, inputVector.z);
    }

    public static float GetCircleLength(float radius)
    {
        return 2 * Mathf.PI * radius;
    }

    public static T GetRandom<T>(this List<T> targetList)
    {
        return targetList[Random.Range(0, targetList.Count)];
    }
}
