using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface
{
    public class GameHUD : MonoBehaviour
    {
        [SerializeField] private Image crossHair;
        [SerializeField] private TMP_Text scoreText;

        public void UpdateScoreText(int score)
        {
            scoreText.text = score.ToString();
        }

        public void Hide()
        {
            if (crossHair is null || scoreText is null) return;
            
            crossHair.gameObject.SetActive(false);
            scoreText.gameObject.SetActive(false);
        }
    }
}