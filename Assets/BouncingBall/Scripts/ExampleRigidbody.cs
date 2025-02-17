using UnityEngine;

public class ExampleRigidbody : MonoBehaviour
{
    [SerializeField] private Vector3 _direction;


    private Rigidbody _rigidbody;


    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Update()
    {

        float moveHorizontal = Input.GetAxis("Horizontal"); // A/D ��� �������
        float moveVertical = Input.GetAxis("Vertical"); // W/S ��� �������

        // ������������ ����������� ��������
        Vector3 direction = new Vector3(moveHorizontal, 0, moveVertical).normalized * Time.fixedDeltaTime * 10;

        _rigidbody.AddForce(direction);
    }
}
