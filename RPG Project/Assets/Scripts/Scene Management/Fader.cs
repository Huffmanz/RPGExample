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

        private Coroutine currentlyActiveFade;

        private void Awake(){
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutImmediately(){
            canvasGroup.alpha = 1;
        }
        IEnumerator FadeOutIn(){
            yield return FadeOut(3f);
            yield return FadeIn(2f);
        }
        public IEnumerator FadeOut(float time){
            return Fade(1, time);
        }

        public IEnumerator FadeIn(float time) { 
        
            return Fade(0, time);
        }

        public IEnumerator Fade(float target, float time)
        {
            if (currentlyActiveFade != null) StopCoroutine(currentlyActiveFade);
            currentlyActiveFade = StartCoroutine(FadeRoutine(target, time));
            yield return currentlyActiveFade;
        }

        private IEnumerator FadeRoutine(float target, float time)
        {
            while (!Mathf.Approximately(canvasGroup.alpha, target))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, Time.deltaTime / time);
                yield return null;
            }
        }



    }
}
