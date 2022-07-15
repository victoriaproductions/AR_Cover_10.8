using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using Vuforia;

public class InitializeManager : MonoBehaviour
{
    public List<ParticleTypes> pars;

    public GameObject cautionCell;
    private string dataset;
    string dataSetPath = "Vuforia/ar_cover.xml";
    private void Start()
    {
        cautionCell.SetActive(false);
        Initialize();
    }

    public void ToggleCheck(bool isOn)
    {
        PlayerPrefs.SetInt("Check", isOn ? 1 : 0);
    }

    private void Initialize()
    {
        VuforiaApplication.Instance.OnVuforiaInitialized += OnVuforiaInitialized;
        string curVer = Application.version;
        if (PlayerPrefs.GetString("Ver") != curVer)
        {
            PlayerPrefs.SetInt("Check", 0);
        }
        PlayerPrefs.SetString("Ver", curVer);

        VuforiaBehaviour.Instance.enabled = true;

        dataset = "Toto_Cover_Test";
        Handheld.StopActivityIndicator();

    }
    void OnVuforiaInitialized(VuforiaInitError error)
    {
        if (error == VuforiaInitError.NONE)
            OnVuforiaStarted();
    }
    void OnVuforiaStarted()
    {
        // Create an Image Target from the database.
        for (int i = 1; i < 41; i++)
        {

            print($"TT{i.ToString("D2")}_00");
            var mImageTarget = VuforiaBehaviour.Instance.ObserverFactory.CreateImageTarget(dataSetPath,$"TT{i.ToString("D2")}_00");
            mImageTarget.OnTargetStatusChanged += OnTargetStatusChanged;

            if (mImageTarget!=null)
            {
                LoadingController cont = FindObjectOfType<LoadingController>();
                mImageTarget.gameObject.transform.parent = transform;
                mImageTarget.gameObject.transform.localScale = Vector3.one* 1024;
                mImageTarget.gameObject.AddComponent<TrackableHandler>();
                mImageTarget.gameObject.GetComponent<TrackableHandler>().manager = this;
                mImageTarget.gameObject.GetComponent<TrackableHandler>().controller = cont;
                mImageTarget.gameObject.GetComponent<TrackableHandler>().cautionCell = cautionCell;
                mImageTarget.gameObject.GetComponent<TrackableHandler>().src = GetComponent<AudioSource>();
                if (PlayerPrefs.GetInt("Check") == 0)
                {
                    cautionCell.SetActive(true);
                }
            }
            else
            {
                Debug.LogError("<color=yellow>Failed to load dataset: '" + (dataset) + "'</color>");
            }
        }
    }

    void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        Debug.Log($"target status: {status.Status}");
    }

//    private void LoadDataset()
//    {
//        ObjectTracker objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
//        DataSet dataSet = objectTracker.CreateDataSet();

//        if (dataSet.Load(dataset))
//        {
//            if (!objectTracker.ActivateDataSet(dataSet))
//            {
//                // Note:
//                Tracker cannot have more than 1000 total targets activated
//                Debug.Log("<color=yellow>Failed to Activate DataSet: " + (dataset) + "</color>");
//            }

//            if (!objectTracker.Start())
//            {
//                Debug.Log("<color=yellow>Tracker Failed to Start.</color>");
//            }

//            IEnumerable<TrackableBehaviour> tbs = TrackerManager.Instance.GetStateManager().GetTrackableBehaviours();
//            LoadingController cont = FindObjectOfType<LoadingController>();

//            foreach (TrackableBehaviour tb in tbs)
//            {
//                if (tb.name.Equals("New Game Object"))
//                {
//                    tb.gameObject.name = tb.TrackableName;
//                    tb.gameObject.transform.parent = transform;
//                    tb.gameObject.AddComponent<TrackableHandler>();
//                    tb.gameObject.GetComponent<TrackableHandler>().manager = this;
//                    tb.gameObject.GetComponent<TrackableHandler>().controller = cont;
//                    tb.gameObject.GetComponent<TrackableHandler>().cautionCell = cautionCell;
//                    tb.gameObject.GetComponent<TrackableHandler>().src = GetComponent<AudioSource>();
//                }
//            }

//            if (PlayerPrefs.GetInt("Check") == 0)
//            {
//                cautionCell.SetActive(true);
//            }
//        }
//        else
//        {
//            Debug.LogError("<color=yellow>Failed to load dataset: '" + (dataset) + "'</color>");
//        }
//    }
}

[System.Serializable]
public class ParticleTypes
{
    public string name;
    public string[] objectName;
}