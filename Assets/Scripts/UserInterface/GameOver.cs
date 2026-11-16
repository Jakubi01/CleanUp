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
        
        [Header("Score-based Messages (Game Clear)")]
        [SerializeField, Multiline] private string highTierMessage = "Excellent!\nnYou're a true recycling master!";
        [SerializeField, Multiline] private string midTierMessage = "Good job!\nYou've helped make the environment cleaner.";
        [SerializeField, Multiline] private string lowTierMessage = "All trash collected, but...\nTry aiming for a higher score next time!";
        
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

        public void SetResultText(int finalScore, int totalElements)
        {
            var sectionSize  = totalElements / 3;
            
            if (finalScore >= sectionSize * 2)
            {
                resultText.text = highTierMessage;
                resultText.color = Color.green;
            }
            else if (finalScore >= sectionSize)
            {
                resultText.text = midTierMessage;
                resultText.color = Color.darkGoldenRod;
            }
            else
            {
                resultText.text = lowTierMessage;
                resultText.color = Color.softRed;
            }
            
            scoreText.text = "Score\t" + finalScore;
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