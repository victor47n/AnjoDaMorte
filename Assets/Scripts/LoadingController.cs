using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingController : MonoBehaviour
{
    public static int scene;
    public GameObject loader;
    public static Dictionary<string, object> infos = new Dictionary<string, object>();

    public static void SetParams(string key, object value)
    {
        Debug.Log(key + " " + value);
        infos.Add(key, value);
    }

    public static void CallLoading(int scene)
    {
        LoadingController.scene = scene;
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);

    }

    void Awake()
    {
        Object.DontDestroyOnLoad(loader);
    }
    // Updates once per frame
    void Start()
    {
        // Inicia Coroutine para loading
        StartCoroutine(LoadNewScene());

    }

    IEnumerator LoadNewScene()
    {

        //Minimo de 3 segundos de carregamento
        yield return new WaitForSeconds(3);

        //Tarefa assincrona de carregamento de Cena
        AsyncOperation async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scene);

        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone)
        {
            yield return null;
        }
    }

}
