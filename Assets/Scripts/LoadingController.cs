using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingController : MonoBehaviour {

    public class StoryData {
        public List<object> data;
    }

    public static int Scene;
    public GameObject Loader;
    public Text StoryText;
    public static Dictionary<string, object> Infos = new Dictionary<string, object> ();
    public StoryData Storys;
    public static void SetParams (string key, object value) {
        Debug.Log (key + " " + value);
        Infos.Add (key, value);
    }

    public static void CallLoading (int scene) {
        Scene = scene;
        SceneManager.LoadScene (1);
    }

    void Awake () {
        Loader = this.gameObject;
        //    Object.DontDestroyOnLoad (Loader);

    }
    // Updates once per frame
    void Start () {
        // Inicia Coroutine para loading

        var sr = new StreamReader (Application.dataPath + "/Texts/Story.json");
        var fileContents = sr.ReadToEnd ();
        sr.Close ();
        Storys = JsonConvert.DeserializeObject<StoryData> (fileContents);
        StartCoroutine (LoadNewScene ());

    }

    IEnumerator LoadNewScene () {

        //Exibe historia
        foreach (string v in Storys.data) {
            StoryText.text = v;
            Debug.Log (v.ToString ());
            yield return new WaitForSeconds (1);
        }

        //Tarefa assincrona de carregamento de Cena
        AsyncOperation async = SceneManager.LoadSceneAsync (Scene);

        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone) {
            yield return null;
        }
    }

}