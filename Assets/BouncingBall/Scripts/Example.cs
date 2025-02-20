
using BouncingBall.CustomPhysics;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class Example : MonoBehaviour 
{
    [SerializeField] private Vector3 _direction;
    [SerializeField] private Transform _transfo;

    private CustomRigidbody _rigidbody;
   

    private void Start()
    {
        _rigidbody = GetComponent<CustomRigidbody>();
    }

    public void SetForce()
    {
        _rigidbody.AddForce(_direction);
    }

    private async void OnCollisionEnter(Collision collision)
    {
        var velocity = _rigidbody.TestVelocity;
        var directionOfCompression = Vector3.one;

        Vector3 normal = collision.GetContact(0).normal;
        Debug.Log($"1/   локальный поворот{_transfo.rotation}   обычный{_transfo.localRotation}\n ____________");
        var sumRotation = _transfo.rotation;
        transform.rotation = Quaternion.Euler(collision.transform.eulerAngles);
        Debug.Log($"2/  локальный поворот{_transfo.rotation}   обычный{_transfo.localRotation}");
        _transfo.rotation = sumRotation;
        if (normal.y == 1)
            return;

        directionOfCompression.z = 0.5f;

        float elapsedTime = 0f;
        var _compressionDuration = 0.3f;

        while (elapsedTime < _compressionDuration)
        {
            transform.localScale = Vector3.Lerp(Vector3.one, directionOfCompression, elapsedTime / _compressionDuration);
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }

        elapsedTime = 0;

        while (elapsedTime < _compressionDuration)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, elapsedTime / _compressionDuration);
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }

        transform.localScale = Vector3.one;
        _rigidbody.AddForce(Vector3.Reflect(velocity * 50, normal));
    }

    public void Update()
    {

        float moveHorizontal = Input.GetAxis("Horizontal"); // A/D или стрелки
        float moveVertical = Input.GetAxis("Vertical"); // W/S или стрелки

        if (moveVertical == 0 && moveHorizontal == 0)
            return;

        // Рассчитываем направление движения
        Vector3 direction = new Vector3(moveHorizontal, 0, moveVertical).normalized * Time.fixedDeltaTime * 200;

        _rigidbody.AddForce(direction);
      
    }
}