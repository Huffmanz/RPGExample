using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        private CanvasGroup canvasGroup;

        private void Start(){
            canvasGroup = GetComponent<CanvasGroup>();
            StartCoroutine(FadeOutIn());
        }

        IEnumerator FadeOutIn(){
            yield return FadeOut(3f);
            yield return FadeIn(2f);
        }
        public IEnumerator FadeOut(float time){
            while(canvasGroup.alpha  < 1f){
                yield return canvasGroup.alpha += Time.deltaTime / time;
            }
        }

        public IEnumerator FadeIn(float time){
            while(canvasGroup.alpha  > 0f){
                yield return canvasGroup.alpha -= Time.deltaTime / time;
            }
        }
    }
}
