using UnityEngine;
using UnityEngine.UI;

namespace BouncingBall.Game.UI.GameplayState
{
    public class PlayerHealthCell : MonoBehaviour
    {
        [SerializeField] private Image _cell;

        public void Awake()
        {
            _cell.gameObject.SetActive(true);
        }

        public void DisableCell()
        {
            _cell.gameObject.SetActive(false);
        }

    }
}
