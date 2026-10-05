using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace RPG.SceneManagement
{
    public class Fade : MonoBehaviour
    {
        private CanvasGroup canvasGroup;
        private void Awake()
        {
        canvasGroup = GetComponent<CanvasGroup>();
        }
        private void Start()
        {
          //StartCoroutine(FadeOut(3f));
        }

        public IEnumerator FadeIn(float time){
            while(canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += Time.deltaTime / time;
                yield return null;
            }
        }

        public IEnumerator FadeOut(float time){
            while(canvasGroup.alpha>0)
            {
                canvasGroup.alpha -= Time.deltaTime / time;
                yield return null;
            }
        }
    }
}
