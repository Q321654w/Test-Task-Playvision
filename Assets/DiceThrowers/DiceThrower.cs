using System;
using UnityEngine;

public class DiceThrower : MonoBehaviour
{
    [SerializeField] private Dice _dicePrefab;

    [Space] 
    
    [Header("Dice throw params")] 
    [SerializeField]
    private float _throwForce;

    [SerializeField] 
    private float _torqueForce;

    [SerializeField] 
    private int _diceCount;
    
    [SerializeField] 
    private float _spaceBetweenDices;

    [HideInInspector]
    public bool UseGivenNumbers;
    
    [HideInInspector]
    public int[] GivenNumbers;
    
    private Dice[] _dices;

    public void ThrowDices()
    {
        _dices = CreateDices();
        var animations = PhysicalTransformAnimationRecorder.CreateForObjects(_dices);

        if (UseGivenNumbers)
            ThrowWithGivenNumbers(animations);
        else
            ThrowWithRandom(animations);
    }

    private void ThrowWithRandom(TransformAnimation[] animations)
    {
        for (int i = 0; i < _diceCount; i++)
            _dices[i].ThrowWithRandomNumber(animations[i]);
    }

    private void ThrowWithGivenNumbers(TransformAnimation[] animations)
    {
        for (int i = 0; i < _diceCount; i++)
            _dices[i].Throw(animations[i], GivenNumbers[i]);
    }

    public void ClearPreviousDices()
    {
        for (int i = 0; i < _dices.Length; i++)
        {
            if (_dices[i] != null)
                Destroy(_dices[i].gameObject);
        }
    }

    public void ClearAllDices()
    {
        var dices = FindObjectsOfType<Dice>();
        for (int i = 0; i < dices.Length; i++)
            Destroy(dices[i].gameObject);
    }

    private Dice[] CreateDices()
    {
        var dices = new Dice[_diceCount];
        for (int i = 0; i < _diceCount; i++)
            dices[i] = Instantiate(_dicePrefab);

        InitializeDices(dices);

        return dices;
    }

    private void InitializeDices(Dice[] dices)
    {
        var cachedTransform = transform;
        var spawnPosition = cachedTransform.position + _diceCount / 2 * _spaceBetweenDices * cachedTransform.right;

        foreach (var dice in dices)
        {
            dice.transform.position = spawnPosition;
            spawnPosition -= cachedTransform.right * _spaceBetweenDices;

            var diceRigidbody = dice.GetComponent<Rigidbody>();

            var force = transform.forward * _throwForce;
            diceRigidbody.AddForce(force, ForceMode.Impulse);

            var torque = new Vector3().Randomize(0, _torqueForce);
            diceRigidbody.AddTorque(torque, ForceMode.Impulse);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        var position = transform.position + _diceCount / 2 * _spaceBetweenDices * transform.right;

        for (int i = 0; i < _diceCount; i++)
        {
            Gizmos.DrawWireSphere(position, 1f);
            position -= transform.right * _spaceBetweenDices;
        }
    }

    private void OnValidate()
    {
        for (int i = 0; i < GivenNumbers.Length; i++)
            if (GivenNumbers[i] < 1 || GivenNumbers[i] > 6)
                GivenNumbers[i] = Mathf.Clamp(GivenNumbers[i], 1,6);
        
        
        if(_diceCount == GivenNumbers.Length)
            return;
        
        
        Array.Resize(ref GivenNumbers, _diceCount);
        
        for (int i = 0; i < GivenNumbers.Length; i++)
            if (GivenNumbers[i] == 0)
                GivenNumbers[i] = 1;
    }
}