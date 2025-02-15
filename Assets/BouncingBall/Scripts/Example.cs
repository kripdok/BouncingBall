
using UnityEngine;

public class Example : MonoBehaviour
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

        float moveHorizontal = Input.GetAxis("Horizontal"); // A/D ��� �������
        float moveVertical = Input.GetAxis("Vertical"); // W/S ��� �������

        // ������������ ����������� ��������
        Vector3 direction = new Vector3(moveHorizontal, 0, moveVertical).normalized * Time.fixedDeltaTime * 10;

        _rigidbody.AddForce(direction);
    }


}

//public float moveSpeed = 5f; // �������� �����������
//public float rotationSpeed = 100f; // �������� ��������

//private void Update()
//{
//    Move();
//    Rotate();
//}

//private void Move()
//{
//    // �������� ���� � ����������
//    float moveHorizontal = Input.GetAxis("Horizontal"); // A/D ��� �������
//    float moveVertical = Input.GetAxis("Vertical"); // W/S ��� �������

//    // ������������ ����������� ��������
//    Vector3 direction = new Vector3(moveHorizontal, 0, moveVertical).normalized;

//    // ���� ���� ��������, ���������� ���
//    if (direction.magnitude > 0)
//    {
//        transform.position += direction * moveSpeed * Time.deltaTime;
//    }
//}

//private void Rotate()
//{
//    // �������� ���� � ����������
//    float moveHorizontal = Input.GetAxis("Horizontal") *-1; // A/D ��� �������
//    float moveVertical = Input.GetAxis("Vertical"); // W/S ��� �������

//    // ������������ ���� �������� �� ������ �����
//    float yaw = moveHorizontal * rotationSpeed * Time.deltaTime; // �������� ������ ���������� ��� Z
//    float pitch = moveVertical * rotationSpeed * Time.deltaTime; // �������� ������ ���������� ��� X

//    // ������� ����������� ��� ����������� ��������
//    Quaternion rotationZ = Quaternion.Euler(0, 0, yaw); // �������� ������ ��� Z
//    Quaternion rotationX = Quaternion.Euler(pitch, 0, 0); // �������� ������ ��� X

//    // ��������� ���������� ��������
//    transform.rotation = rotationZ * rotationX * Quaternion.Euler(transform.rotation.eulerAngles);
//}