using UnityEngine;

public readonly struct TransformAnimationKey
{
    private readonly Vector3 _position;
    private readonly Quaternion _rotation;

    public Vector3 Position => _position;
    public Quaternion Rotation => _rotation;

    public TransformAnimationKey(Vector3 position, Quaternion rotation)
    {
        _position = position;
        _rotation = rotation;
    }
}