using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace RPG.SceneManagement
{
    public class Fade : MonoBehaviour
    {
        private CanvasGroup canvasGroup;
        private Coroutine currentActiveCoroutine;
        private void Awake()
        {
        canvasGroup = GetComponent<CanvasGroup>();
        }
        private void Start()
        {
          //StartCoroutine(FadeOut(3f));
        }

        public IEnumerator FadeIn(float time){
           return Fades(1, time);
        }

        public IEnumerator FadeOut(float time)
        {

          return Fades(0, time);
           
        }
        public IEnumerator Fades(float target, float time)
        {
             if (currentActiveCoroutine != null){
                
                StopCoroutine(currentActiveCoroutine);
            }
             currentActiveCoroutine =  StartCoroutine(FadeCoroutine(target, time));
            yield return currentActiveCoroutine;
        }

        private IEnumerator FadeCoroutine(float targetAlpha, float time)
        {
             while(!Mathf.Approximately(canvasGroup.alpha, targetAlpha))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, targetAlpha, Time.deltaTime / time);
                yield return null;
            }
        }
       
    }
}
