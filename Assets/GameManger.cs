using GLTFast;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManger : MonoBehaviour
{
    public Material MAT;
    public static GameManger instance;
    #region "fps"

    public Text FrameRate;
 
    int frameCount = 0;
    float dt = 0.0F;
    float fps = 0.0F;
    public float updateRateSeconds = 4.0F;
    #endregion
    #region "UIItem"
    public GameObject Loading;
    public GameObject RestButton;
    public GameObject StartButton;
    #endregion
    public GameObject Model=>this.gameObject;
    public bool success;

  
    public GtflLOAD gt => this.GetComponent<GtflLOAD>();
   private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
  
        RestButton.SetActive(false);
        Loading.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        CountFrameRate();

        if (success)
        {
            SetMaterial();
            success = false;
            RestButton.SetActive(true);
            Loading.SetActive(false);
        }
       
    }
    public void SetMaterial()
    {

        var allChildren = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer child in allChildren)
        {
            child.gameObject.GetComponent<MeshRenderer>().material = MAT;
      
        }

    }
    public void StartLoad()
    {
        gt.GetComponent<GtflLOAD>().enabled = true;
        Loading.SetActive(true);
        StartButton.SetActive(false);
    }
    public void ResetModel()
    {
        this.transform.rotation=Quaternion.identity;
        Camera.main.fieldOfView = 60;
    }
    public void CountFrameRate()
    {
        frameCount++;
        dt += Time.unscaledDeltaTime;
        if (dt > 1.0 / updateRateSeconds)
        {
            fps = frameCount / dt;
            frameCount = 0;
            dt -= 1.0F / updateRateSeconds;
        }
        FrameRate.text =fps.ToString("0.0")+"FPS";
    }
}
