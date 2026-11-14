using System;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UserInterface
{
    public class GameOver : MonoBehaviour
    {
        [Header("Setting")]
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TMP_Text resultText;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private Button reTryButton;
        [SerializeField] private Button quitButton;
        
        [Header("Message")]
        [SerializeField, Multiline] private string winMessage = "Thank you for your efforts.\n You collected all the trash.";
        [SerializeField, Multiline] private string loseMessage = "Study recycling again.";

        private void Awake()
        {
            canvasGroup.alpha = 0;
            
            reTryButton.onClick.AddListener(OnRetryButtonClicked);
            quitButton.onClick.AddListener(OnQuitButtonClicked);
        }

        private void OnDisable()
        {
            reTryButton.onClick.RemoveAllListeners();
            quitButton.onClick.RemoveAllListeners();
        }

        public void SetResultText(GameOverType gameOverType)
        {
            resultText.text = gameOverType switch
            {
                GameOverType.CollectAllGarbage => winMessage,
                GameOverType.LossAllScore => loseMessage,
                _ => "None"
            };
            
            resultText.color = gameOverType == GameOverType.CollectAllGarbage ? Color.green : Color.red;
        }

        public void SetScoreText(int score)
        {
            scoreText.text = "Score\t" + score;
        }

        public void Show()
        {
            if (canvasGroup is null) return;

            canvasGroup.alpha = 1f;
        }

        private void OnRetryButtonClicked()
        {
            GameManager.Instance.ClearFlags();
            SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        }

        private void OnQuitButtonClicked()
        {
#if  UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}