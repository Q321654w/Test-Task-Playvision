using UnityEngine;

public class TransformAnimation : IAnimation<Transform, TransformAnimationKey>
{
    private readonly float _animationStep;
    private readonly TransformAnimationKey[] _animationKeys;
    
    public int Length => _animationKeys.Length;
    public TransformAnimationKey FirstKey => _animationKeys[0];
    public TransformAnimationKey LastKey => _animationKeys[Length - 1];

    public TransformAnimation(float animationStep, TransformAnimationKey[] animationKeys)
    {
        _animationStep = animationStep;
        _animationKeys = animationKeys;
    }
    
    public TransformAnimationKey this[int index] => _animationKeys[index];
    
    public bool IsContinue(float elapsedTime)
    {
        return (int)(elapsedTime / _animationStep) < _animationKeys.Length - 1;
    }

    public void ApplyTo(Transform transform, float elapsedTime)
    {
        if (IsContinue(elapsedTime))
            ApplyFrameAtTime(transform, elapsedTime);
        else
            ApplyLastFrame(transform);
    }

    private void ApplyFrameAtTime(Transform transform, float elapsedTime)
    {
        var previousIndex = (int)(elapsedTime / _animationStep);
        var nextIndex = previousIndex + 1;
        
        var previousKey = _animationKeys[previousIndex];
        var nextKey = _animationKeys[nextIndex];

        var elapsedSinceLastKey = elapsedTime - previousIndex * _animationStep;
        var t = elapsedSinceLastKey / _animationStep;

        transform.position = Vector3.Lerp(previousKey.Position, nextKey.Position, t);
        transform.rotation = Quaternion.Lerp(previousKey.Rotation, nextKey.Rotation, t);
    }

    public void ApplyLastFrame(Transform transform)
    {
        var key = LastKey;
        transform.position = key.Position;
        transform.rotation = key.Rotation;
    }
    
    public void ApplyFirstFrame(Transform transform)
    {
        var key = FirstKey;
        transform.position = key.Position;
        transform.rotation = key.Rotation;
    }
}