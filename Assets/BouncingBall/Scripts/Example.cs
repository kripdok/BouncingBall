
using BouncingBall.CustomPhysics;
using UnityEngine;
using UnityEngine.EventSystems;

public class Example : MonoBehaviour , IPointerClickHandler
{
    [SerializeField] private Vector3 _direction;


    private CustomRigidbody _rigidbody;


    private void Start()
    {
        _rigidbody = GetComponent<CustomRigidbody>();
    }

    public void SetForce()
    {
        _rigidbody.AddForce(_direction);
    }

    public void Update()
    {

        float moveHorizontal = Input.GetAxis("Horizontal"); // A/D или стрелки
        float moveVertical = Input.GetAxis("Vertical"); // W/S или стрелки

        // Рассчитываем направление движения
        Vector3 direction = new Vector3(moveHorizontal, 0, moveVertical).normalized * Time.fixedDeltaTime * 50;

        _rigidbody.AddForce(direction);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(1);
    }
}