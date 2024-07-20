using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class Dice : MonoBehaviour
{
    [SerializeField] 
    private DiceView _diceView;
    
    [SerializeField]
    private Rigidbody _rigidbody;

    [SerializeField] 
    private int _number;
    
    private Transform _cachedTransform;
    
    private void Awake()
    {
        _cachedTransform = transform;
    }

    public void ThrowWithRandomNumber(IAnimation<Transform, TransformAnimationKey> transformAnimation)
    {
        var number = Random.Range(1, 7);
        Throw(transformAnimation, number);
    }

    public void Throw(IAnimation<Transform, TransformAnimationKey> transformAnimation, int number)
    {
        _rigidbody.isKinematic = true;
        _number = number;

#if UNITY_EDITOR
        gameObject.name = _number.ToString();
#endif
        
        var endRotation = transformAnimation.LastKey.Rotation;
        _diceView.RotateWithNumberOnTop(number, endRotation);
        
        StartCoroutine(Animate(transformAnimation));
    }

    private IEnumerator Animate(IAnimation<Transform, TransformAnimationKey> transformAnimation)
    {
        var elapsedTime = Time.deltaTime;
        transformAnimation.ApplyFirstFrame(_cachedTransform);
        yield return null;
        
        while (transformAnimation.IsContinue(elapsedTime))
        {
            transformAnimation.ApplyTo(_cachedTransform, elapsedTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        transformAnimation.ApplyLastFrame(_cachedTransform);
    }
}