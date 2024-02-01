using CookingPrototype.Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CookingPrototype.UI
{
    public sealed class MenuWindow : MonoBehaviour
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TMP_Text _foodCountTMP;

        private bool _startGame;
        public void Start()
        {
            var gc = GameplayController.Instance;
            _playButton.onClick.AddListener(gc.StartGame);
            _playButton.onClick.AddListener(HideMenu);
        }

        public void SetFoodCount(int orderTarget)
        {
            _foodCountTMP.text = orderTarget.ToString();
        }

        private void Update()
        {
            if (_startGame == false) return;
            
            _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, 0, Time.deltaTime);
            if (_canvasGroup.alpha == 0)
            {
                gameObject.SetActive(false);
            }
        }

        private void HideMenu()
        {
            _startGame = true;
        }
    }
}