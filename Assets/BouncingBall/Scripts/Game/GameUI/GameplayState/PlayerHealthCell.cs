using UnityEngine;
using UnityEngine.UI;

namespace BouncingBall.Game.UI.GameplayState
{
    public class PlayerHealthCell : MonoBehaviour
    {
        [SerializeField] private Image _cell;

        private void Awake()
        {
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
