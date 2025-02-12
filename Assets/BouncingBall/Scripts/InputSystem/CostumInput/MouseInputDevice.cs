using UniRx;
using UnityEngine;

public class MouseInputDevice : IInputDevice
{
    public ReactiveProperty<bool> IsDirectionSet { get; private set; }
    public ReactiveProperty<Vector3> RotationAmount { get; private set; }
    public ReactiveProperty<float> ZScale { get; private set; }

    private Plane _plane;
    private bool _isCooldown;

    public MouseInputDevice()
    {
        _isCooldown = false;
        IsDirectionSet = new();
        RotationAmount = new();
        ZScale = new();

        _plane = new(Vector3.up, Vector3.zero);
    }

    public void SetRotationAndScale()
    {
        IsDirectionSet.Value = Input.GetMouseButton(0);
    }

    public void TryDisableIsDirectionSet()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (_plane.Raycast(ray, out float distance))
        {
            var position = ray.GetPoint(distance);
            float distanceFromCenter = Vector3.Distance(new Vector3(0, 0, 0), position);

            // Устанавливаем ZScale как расстояние от центра экрана до мыши, ограниченное 0-3
            ZScale.Value = Mathf.Clamp(distanceFromCenter, 0, 3f);

            Vector3 direction = position - Vector3.zero; // Позиция объекта (например, 0,0,0)
            direction.y = 0; // Убираем вертикальную составляющую

            // Вычисляем угол поворота на основе направления
            if (direction != Vector3.zero)
            {
                // Находим угол в радианах
                float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg; // Используем x и z для расчета угла
                RotationAmount.Value = new Vector3(0, angle, 0); // Устанавливаем угол поворота только по оси Y
            }
        }
    }
}

