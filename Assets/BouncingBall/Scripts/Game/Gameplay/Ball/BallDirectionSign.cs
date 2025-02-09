using BouncingBall.InputSystem.Controller;
using UniRx;
using UnityEngine;
using Zenject;

namespace BouncingBall.Game.Gameplay.BallObject
{
    public class BallDirectionSign : MonoBehaviour
    {
        private IPointingDirection _inputController;

        [Inject]
        public void Сonstructor(IPointingDirection inputController)
        {
            Debug.Log("Зарегестрирован указатель шара");
            _inputController = inputController;
            _inputController.PointerLocation.Skip(1).Subscribe(Tets);
            _inputController.IsDirectionSet.Skip(1).Subscribe(x => gameObject.SetActive(x));
            gameObject.SetActive(false);
        }

        private void Tets(Vector3 mousePosition)
        {
            Vector3 direction = mousePosition - transform.position;
            transform.rotation = Quaternion.LookRotation(direction); // Поворачиваем по направлению к мыши

            // Изменяем масштаб Pointer по оси Y в зависимости от расстояния
            float distanceToMouse = Vector3.Distance(transform.position, mousePosition);
            Vector3 newScale = transform.localScale;
            newScale.z = distanceToMouse; // Устанавливаем новый масштаб по оси Y
            transform.localScale = newScale;
        }

        //TODO - Придумать способ отписик 
    }
}
