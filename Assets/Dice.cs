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
    
    private Transform _cachedTransform;
    
    private void Awake()
    {
        _cachedTransform = transform;
    }

    public void Throw(TransformAnimation transformAnimation)
    {
        var value = Random.Range(1, 7);
        Throw(transformAnimation, value);
    }

    public void Throw(TransformAnimation transformAnimation, int value)
    {
        _rigidbody.isKinematic = true;
        
        var endRotation = transformAnimation.LastKey.Rotation;
        _diceView.RotateWithValueOnTop(value, endRotation);
        
        StartCoroutine(Animate(transformAnimation));
    }

    private IEnumerator Animate(TransformAnimation transformAnimation)
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