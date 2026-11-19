using Input;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface
{
    public class GameHUD : MonoBehaviour
    {
        [SerializeField] private Image crossHair;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text interactDesc;
        [SerializeField] private TMP_Text dropDesc;
        [SerializeField] private GameObject gameRuleDesc;

        private void Awake()
        {
            SetGameHUDVisibility(false);
        }

        private void Start()
        {
            XRPlayerInput.CursorControl(CursorLockMode.Confined, true);
        }

        public void UpdateScoreText(int score)
        {
            scoreText.text = score.ToString();
        }

        public void SetGameHUDVisibility(bool visible)
        {            
            crossHair?.gameObject.SetActive(visible);
            scoreText?.gameObject.SetActive(visible);
            interactDesc?.gameObject.SetActive(visible);
            dropDesc?.gameObject.SetActive(visible);
        }

        public void OnStartButtonClicked()
        {
            gameRuleDesc?.SetActive(false);
            SetGameHUDVisibility(true);
            XRPlayerInput.CursorControl(CursorLockMode.Locked, false);
            GameManager.Instance.gameStart = true;
        }

        public void OnQuitButtonClicked()
        {
#if  UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}