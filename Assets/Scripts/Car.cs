using Unity.VisualScripting;
using UnityEngine;
using Zenject;
public class Car : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _rigidbody;

    [Inject]
    private PlayerData _playerData;

    public void FixedUpdate()
    {
        Vector3 finalPosition = _rigidbody.position + transform.forward * (_playerData.CarSpeed * Time.fixedDeltaTime);
        _rigidbody.MovePosition(finalPosition);
    }

    private void OnValidate()
    {
        _rigidbody = transform.GetOrAddComponent<Rigidbody>();
        _rigidbody.isKinematic = true;
        _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
    }
}