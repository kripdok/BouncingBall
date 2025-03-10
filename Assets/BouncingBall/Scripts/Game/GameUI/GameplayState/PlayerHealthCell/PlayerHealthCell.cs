using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BouncingBall.Game.UI.GameplayState
{
    public class PlayerHealthCell : MonoBehaviour
    {
        [SerializeField] private Image _cell;
        [Inject] private Transform _parent;

        private void Awake()
        {
            transform.SetParent(_parent);
            EnableCell();
        }

        public void DisableCell()
        {
            _cell.gameObject.SetActive(false);
        }

        public void EnableCell()
        {
            _cell.gameObject.SetActive(true);
        }
    }
}
