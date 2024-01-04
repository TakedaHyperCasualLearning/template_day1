using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Donuts
{
    public class FadeLayer : MonoBehaviour
    {
        [SerializeField] protected CanvasGroup canvasGroup;
        

        public void ForceOverlay()
        {
            gameObject.SetActive(true);

            canvasGroup.alpha = 1;
        }

        public IEnumerator FadeOut(float period)
        {
            gameObject.SetActive(true);
            float t = 0;
            float startAlpha = canvasGroup.alpha;
            while (t <= period)
            {
                t += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(startAlpha, 1, t / period);
                yield return null;
            }
            canvasGroup.alpha = 1;
        }


        public IEnumerator FadeIn(float period)
        {
            gameObject.SetActive(true);
            float t = 0;
            float startAlpha = canvasGroup.alpha;
            while (t <= period)
            {
                t += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(startAlpha, 0, t / period);
                yield return null;
            }
            canvasGroup.alpha = 0;
            gameObject.SetActive(false);
        }
    }
}