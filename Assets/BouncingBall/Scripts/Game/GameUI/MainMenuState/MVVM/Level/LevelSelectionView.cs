using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BouncingBall.Game.UI.MainMenuState.MVVM
{
    public class LevelSelectionView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _text;

        [Inject]
        private void Constructor(LevelSelectionViewModel viewModel, Transform transform)
        {
            gameObject.transform.SetParent(transform);
            _text.text = viewModel.LevelName;
            _button.onClick.AsObservable().Subscribe(_ => viewModel.Clicked.Execute()).AddTo(this);
        }
    }
}
