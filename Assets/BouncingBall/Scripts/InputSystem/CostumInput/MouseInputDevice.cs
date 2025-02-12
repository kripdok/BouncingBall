using BouncingBall.Game.Data;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

public class MouseInputDevice : IInputDevice
{
    public ReactiveProperty<bool> IsDirectionSet { get; private set; }
    public ReactiveProperty<Vector3> Direction { get; private set; }
    public ReactiveProperty<float> ZScale { get; private set; }

    private Plane _plane;
    private bool _isCooldown;

    private Vector3 _ballPosition;
    GameDataManager _gameDataManager;

    public MouseInputDevice(GameDataManager gameDataManager)
    {
        _isCooldown = false;
        IsDirectionSet = new();
        Direction = new();
        ZScale = new();

        _plane = new(Vector3.up, new Vector3(0, 0.5f, 0));
        _gameDataManager = gameDataManager;
    }

    public void SetRotationAndScale()
    {
        IsDirectionSet.Value = Input.GetMouseButton(0);
    }

    public void TryDisableIsDirectionSet()
    {
        _ballPosition = _gameDataManager.GameData.BallModel.ReadPosition.Value;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (_plane.Raycast(ray, out float distance))
        {
            var position = ray.GetPoint(distance);
            CalculationScaleZ(position);
            СalculatуDirection(position);
        }
    }

    private void СalculatуDirection(Vector3 position)
    {
        Vector3 direction = position - _ballPosition;

        if (direction != Vector3.zero)
        {
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            Direction.Value = new Vector3(0, angle, 0);
        }
    }

    private void CalculationScaleZ(Vector3 position)
    {
        float distanceFromCenter = Vector3.Distance(_ballPosition, position);
        ZScale.Value = Mathf.Clamp(distanceFromCenter, 0, 3f);
    }
}

