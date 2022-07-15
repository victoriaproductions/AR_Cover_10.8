using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Android;

public class LoadingController : MonoBehaviour
{
    public AssetBundle bundle;
    private string bundleLoc;
    private string bundleURL = "https://arcover.oss-cn-beijing.aliyuncs.com/arcover";

    private string[] loadString = new string[4]
    {
        "카메라 권한이 설정될 동안 대기하는 중입니다.",
        "필요한 파일을 다운로드하는 중입니다.",
        "파일을 불러오는 중입니다.",
        "잠시만 기다려주세요. 곧 스캔이 가능합니다."
    };

    public Slider sld;
    public Text txt;

    private IEnumerator Start()
    {
        sld.gameObject.SetActive(false);
        txt.text = loadString[0];

        while (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
            yield return null;
        }

        sld.gameObject.SetActive(true);
        sld.value = 0;

        bundleLoc = string.Format("{0}/arcover", Application.persistentDataPath);

        if (!File.Exists(bundleLoc))
        {
            txt.text = loadString[1];

            UnityWebRequest www = new UnityWebRequest(bundleURL);
            www.downloadHandler = new DownloadHandlerFile(bundleLoc);
            www.SendWebRequest();
            while (!www.isDone)
            {
                sld.value = www.downloadProgress;
                yield return null;
            }

            sld.value = 0;
        }

        txt.text = loadString[2];

        AssetBundleCreateRequest req = AssetBundle.LoadFromFileAsync(bundleLoc);
        while (!req.isDone)
        {
            sld.value = req.progress;
            yield return null;
        }

        txt.text = loadString[3];
        sld.gameObject.SetActive(false);
        yield return null;
        bundle = req.assetBundle;

        DontDestroyOnLoad(gameObject);

        SceneManager.LoadScene("Scene");
    }
}
