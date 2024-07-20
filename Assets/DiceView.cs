using System;
using UnityEngine;

public class DiceView : MonoBehaviour
{
    [SerializeField]
    private Vector3[] _sideDirections = 
    { 
        Vector3.up,
        Vector3.forward,
        Vector3.left,
        Vector3.right,
        Vector3.back, 
        Vector3.down
    };

    private Transform _cachedTransform;

    private void Awake()
    {
        _cachedTransform = transform;
    }

    public void RotateWithValueOnTop(int value, Quaternion endRotation)
    {
        var endValue = GetValueFromRotation(endRotation);
        var endDirection = _sideDirections[endValue];
        var targetDirection = _sideDirections[value - 1];
        _cachedTransform.rotation *= Quaternion.FromToRotation(targetDirection, endDirection);
    }

    private int GetValueFromRotation(Quaternion endRotation)
    {
        var temp = _cachedTransform.rotation;
        _cachedTransform.rotation = endRotation;

        var directions = new Vector3[_sideDirections.Length];

        for (int i = 0; i < _sideDirections.Length; i++)
            directions[i] = _cachedTransform.localToWorldMatrix * _sideDirections[i];
        
        var maxDot = -1f;
        var maxIndex = -1;

        for (var i = 0; i < directions.Length; i++)
        {
            var dot = Vector3.Dot(directions[i], Vector3.up);
            if (dot <= maxDot)
                continue;
            
            maxDot = dot;
            maxIndex = i;
        }

        _cachedTransform.rotation = temp;
        return maxIndex;
    }
}