using BouncingBall.Game.Data;
using UniRx;
using UnityEngine;

public class MouseInputDevice : IInputDevice
{
    public ReactiveProperty<bool> IsDirectionSet { get; private set; }
    public ReactiveProperty<Vector3> Direction { get; private set; }
    public ReactiveProperty<float> ZScale { get; private set; }
    public ReactiveProperty<float> Angle { get; private set; }

    private Plane _plane;
    private Vector3 _ballPosition;
    private GameDataManager _gameDataManager;

    private bool _isTest;

    public MouseInputDevice(GameDataManager gameDataManager)
    {
        IsDirectionSet = new();
        Direction = new();
        ZScale = new();
        Angle = new();

        _plane = new(Vector3.up, Vector3.zero);
        _gameDataManager = gameDataManager;
        _gameDataManager.GameData.BallModel.ReadPosition.Subscribe(SetBallPositionAndPlanePoint);
        _isTest = false;
    }

    public void SetTest()
    {
        _isTest = true;
    }

    public void SetRotationAndScale()
    {
        if (_isTest)
        {
            IsDirectionSet.Value = Input.GetMouseButton(0);
            _isTest = IsDirectionSet.Value;
        }
    }

    public void TryDisableIsDirectionSet()
    {
        if (_isTest)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (_plane.Raycast(ray, out float distance))
            {
                var position = ray.GetPoint(distance);
                CalculationScaleZ(position);
                СalculatуDirection(position);
            }
        }
    }

    private void СalculatуDirection(Vector3 position)
    {
        Vector3 direction = position - _ballPosition;

        if (direction != Vector3.zero)
        {
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            Direction.Value = new Vector3(0, angle, 0);
            Direction.Value = new Vector3(direction.x, 0, direction.z);
            Angle.Value = angle;
        }
    }

    private void CalculationScaleZ(Vector3 position)
    {
        ZScale.Value = Vector3.Distance(_ballPosition, position);
    }

    private void SetBallPositionAndPlanePoint(Vector3 position)
    {
        _ballPosition = position;
        _plane.SetNormalAndPosition(Vector3.up, position);
    }
}

