using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RobotArmController : MonoBehaviour
{
    [SerializeField]
    private SimpleIKController ikController;

    [SerializeField]
    private string url = "http://192.168.0.182/sliders";

    [SerializeField]
    private bool sendAtStart = false;

    [SerializeField]
    private bool update = false;
    [SerializeField]
    private bool smooth = true;



    // Start is called before the first frame update
    void Start()
    {
        if (sendAtStart) SendAngles();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetExtremityPitch(float value)
    {
        ikController.extremityRotation = new Vector2(value, 0);
    }

    public void SetIp(string value)
    {
        url = value;
    }

    public void SetUpdate(bool value)
    {
        update = value;
    }

    public void SetSmooth(bool value)
    {
        smooth = value;
    }

    [ContextMenu("StartSendingAngles")]
    public void SendAngles()
    {
        StartCoroutine(Upload());
    }

    IEnumerator Upload()
    {
        WWWForm form = new WWWForm();
        form.AddField("BaseYaw", ikController.angles[0].ToString());
        form.AddField("BasePitch", ikController.angles[1].ToString());
        form.AddField("ElbowPitch", ikController.angles[2].ToString());
        form.AddField("WristPitch", ikController.angles[3].ToString());
        form.AddField("WristRoll", ikController.angles[4].ToString());
        form.AddField("smooth", smooth.ToString());

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
                if(update) StartCoroutine(Upload());
            }
        }
    }
}
