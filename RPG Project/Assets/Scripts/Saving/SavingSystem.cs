using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Saving{
    public class SavingSystem : MonoBehaviour{

        
        public void Save(string saveFile){
            Dictionary<string,object> state = LoadFile(saveFile);
            CaptureState(state);
            SaveFile(saveFile, state);
        }

        private void SaveFile(string saveFile, object state)
        {
            string path = GetPathFromSaveFile(saveFile);
            using(FileStream stream = File.Open(path, FileMode.Create)){
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, state);
            } 
        }

        public void Load(string saveFile){
           
            RestoreState(LoadFile(saveFile));
        }

        public IEnumerator LoadLastScene(string saveFile){
            Dictionary<string, object> state = LoadFile(saveFile);
            if(state.ContainsKey("LastSceneBuildIndex")) {
                int buildIndex = (int) state["LastSceneBuildIndex"];
                if(SceneManager.GetActiveScene().buildIndex != buildIndex){
                    yield return SceneManager.LoadSceneAsync(buildIndex);
                }
            }
            RestoreState(state);
        }

        private Dictionary<string,object> LoadFile(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            if(File.Exists(path)){
                using(FileStream stream = File.Open(path, FileMode.Open)){
                    BinaryFormatter formatter = new BinaryFormatter();
                    return (Dictionary<string,object>) formatter.Deserialize(stream);
                }
            }
            return new Dictionary<string, object>();
        }

        private string GetPathFromSaveFile(string saveFile){
            return Path.Combine(Application.persistentDataPath, saveFile+".sav");
        }

        private void RestoreState(Dictionary<string,object> state){
            foreach(var saveableEntities in FindObjectsOfType<SaveableEntity>()){
                if(state.ContainsKey(saveableEntities.GetUniqueIdentifier())){
                    saveableEntities.RestoreState(state[saveableEntities.GetUniqueIdentifier()]);
                }
            }
        }
        private void CaptureState(Dictionary<string,object> state){
            
            foreach(var saveableEntities in FindObjectsOfType<SaveableEntity>()){
                state[saveableEntities.GetUniqueIdentifier()] = saveableEntities.CaptureState();
            }
            state["LastSceneBuildIndex"] = SceneManager.GetActiveScene().buildIndex;
            
        }

     


                // private byte[] SerializeVector(Vector3 vector){
        //     byte[] vectorBytes = new byte[3 * 4];
        //     BitConverter.GetBytes(vector.x).CopyTo(vectorBytes, 0);
        //     BitConverter.GetBytes(vector.y).CopyTo(vectorBytes, 4);
        //     BitConverter.GetBytes(vector.z).CopyTo(vectorBytes, 8);
        //     return vectorBytes;
        // }

        // private Vector3 DeserializeVector(byte[] buffer){
        //     Vector3 result = new Vector3();
        //     result.x = BitConverter.ToSingle(buffer, 0);
        //     result.y = BitConverter.ToSingle(buffer, 4);
        //     result.z = BitConverter.ToSingle(buffer, 8);
        //     return result;
        // }
    }
}