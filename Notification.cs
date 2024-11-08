using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StorySystem.Editor
{
    internal class Notification : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _text;
        [SerializeField]
        private Image _background;

        private readonly WaitForSeconds _animationStartDelay;
        private readonly WaitForSeconds _alphaChangeDelay;

        private static Notification _instance;

        public Notification()
        {
            _animationStartDelay    = new WaitForSeconds(5f);
            _alphaChangeDelay       = new WaitForSeconds(0.005f);

            _instance = this;
        }

        public static void Send(string message)
        {
            _instance.gameObject.SetActive(true);

            _instance._text.text = message;

            _instance.StartCoroutine(_instance.PlayAnimation());
        }

        private IEnumerator PlayAnimation()
        {
            yield return _animationStartDelay;

            Color backgroundColor = _background.color,  startBackgroundColor  = _background.color;
            Color textColor       = _text.color,        startTextColor        = _text.color;

            for (float alpha = 1f; alpha > 0; alpha -= 0.01f)
            {
                backgroundColor.a   = alpha;
                textColor.a         = alpha;

                _background.color   = backgroundColor;
                _text.color         = textColor;

                yield return _alphaChangeDelay;
            }

            gameObject.SetActive(false);

            _background.color   = startBackgroundColor;
            _text.color         = startTextColor;
        }
    }
}