
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

        float moveHorizontal = Input.GetAxis("Horizontal"); // A/D или стрелки
        float moveVertical = Input.GetAxis("Vertical"); // W/S или стрелки

        // Рассчитываем направление движения
        Vector3 direction = new Vector3(moveHorizontal, 0, moveVertical).normalized * Time.fixedDeltaTime * 10;

        _rigidbody.AddForce(direction);
    }


}

//public float moveSpeed = 5f; // Скорость перемещения
//public float rotationSpeed = 100f; // Скорость вращения

//private void Update()
//{
//    Move();
//    Rotate();
//}

//private void Move()
//{
//    // Получаем ввод с клавиатуры
//    float moveHorizontal = Input.GetAxis("Horizontal"); // A/D или стрелки
//    float moveVertical = Input.GetAxis("Vertical"); // W/S или стрелки

//    // Рассчитываем направление движения
//    Vector3 direction = new Vector3(moveHorizontal, 0, moveVertical).normalized;

//    // Если есть движение, перемещаем шар
//    if (direction.magnitude > 0)
//    {
//        transform.position += direction * moveSpeed * Time.deltaTime;
//    }
//}

//private void Rotate()
//{
//    // Получаем ввод с клавиатуры
//    float moveHorizontal = Input.GetAxis("Horizontal") *-1; // A/D или стрелки
//    float moveVertical = Input.GetAxis("Vertical"); // W/S или стрелки

//    // Рассчитываем угол вращения на основе ввода
//    float yaw = moveHorizontal * rotationSpeed * Time.deltaTime; // Вращение вокруг глобальной оси Z
//    float pitch = moveVertical * rotationSpeed * Time.deltaTime; // Вращение вокруг глобальной оси X

//    // Создаем кватернионы для глобального вращения
//    Quaternion rotationZ = Quaternion.Euler(0, 0, yaw); // Вращение вокруг оси Z
//    Quaternion rotationX = Quaternion.Euler(pitch, 0, 0); // Вращение вокруг оси X

//    // Применяем глобальное вращение
//    transform.rotation = rotationZ * rotationX * Quaternion.Euler(transform.rotation.eulerAngles);
//}