using TMPro;
using UnityEngine;

namespace UserInterface
{
    public class GameHUD : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;

        public void UpdateScoreText(int score)
        {
            scoreText.text = score.ToString();
        }
    }
}