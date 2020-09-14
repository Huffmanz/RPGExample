using System.Collections;
using RPG.Saving;
using UnityEngine;

namespace RPG.SceneManagement{
    public class SavingWrapper : MonoBehaviour{
        const string defaultSaveFile = "save";
        [SerializeField] float fadeInTime;

        IEnumerator Start(){
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediately();
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
            yield return fader.FadeIn(fadeInTime);
        }
        private void Update(){
            if(Input.GetKeyDown(KeyCode.S)){
                Save();
            }
            if(Input.GetKeyDown(KeyCode.L)){
                Load();
            }
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                Delete();
            }
        }

        public void Save(){
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }

        public void Load(){
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }

        public void Delete()
        {
            GetComponent<SavingSystem>().Delete(defaultSaveFile);
        }
    }
}