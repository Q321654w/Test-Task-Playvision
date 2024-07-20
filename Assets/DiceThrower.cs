using UnityEngine;

public class DiceThrower : MonoBehaviour
{
    [SerializeField]
    private Dice _dicePrefab;
    
    [Space]
    
    [Header("Dice throw params")]
    [SerializeField]
    private float _throwForce;

    [SerializeField] 
    private float _torqueForce;

    [SerializeField] 
    private int _diceCount;
    
    public void ThrowDices()
    {
        var dices = CreateDices();

        var animations = TransformAnimationRecorder.CreateForObjects(dices);
        
        for (int i = 0; i < _diceCount; i++)
            dices[i].Throw(animations[i]);
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
        var offset = transform.right * -_diceCount / 2;
        
        foreach (var dice in dices)
        {
            dice.transform.position = transform.position + offset;
            offset += transform.right;
            
            
            var diceRigidbody = dice.GetComponent<Rigidbody>();

            var force = transform.forward * _throwForce;
            diceRigidbody.AddForce(force, ForceMode.Impulse);
            
            var torque = new Vector3().Randomize(0, _torqueForce);
            diceRigidbody.AddTorque(torque, ForceMode.Impulse);
        }
    }
}