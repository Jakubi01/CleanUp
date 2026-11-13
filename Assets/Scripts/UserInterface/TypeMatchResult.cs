using System.Collections;
using TMPro;
using UnityEngine;

namespace UserInterface
{
    public class TypeMatchResult : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TMP_Text typeMatchResultText;
        
        [Header("Message")]
        [SerializeField, Multiline] private string typeMatch = "Type Match!";
        [SerializeField, Multiline] private string typeMismatch = "Oops! Wrong type";
        
        private const float FadeDuration = 1f;
        private const float HoldDuration = 1f;
        
        private Coroutine _coroutine;

        private void Awake()
        {
            canvasGroup.alpha = 0f;
        }
        
        public void SetText(bool isMatch)
        {
            typeMatchResultText.color = isMatch ? Color.green : Color.red;
            typeMatchResultText.text = isMatch ? typeMatch : typeMismatch;
        }

        public void ShowText()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
            
            _coroutine = StartCoroutine(StartResultAnimation());
        }

        private IEnumerator StartResultAnimation()
        {
            yield return Fade(1f);
            yield return new WaitForSeconds(HoldDuration);
            yield return Fade(0f);
            _coroutine = null;
        }
        
        private IEnumerator Fade(float target)
        {
            while (!Mathf.Approximately(canvasGroup.alpha, target))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, Time.deltaTime / FadeDuration);
                yield return null;
            }
            canvasGroup.alpha = target;
        }
    }
}