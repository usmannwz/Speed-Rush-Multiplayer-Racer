using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameUIController : MonoBehaviour
{
    private RaceMonitor rm;

    //public Text lapInfoText;
    Text pNameText;
    public Transform target;

    CanvasGroup canvasGroup;
    public Renderer carRend;

    CheckPointManager cpManager;

    [HideInInspector] public int carRego;

    bool regoSet = false;
    // Start is called before the first frame update
    void Start()
    {
        rm = FindObjectOfType<RaceMonitor>();
        this.transform.SetParent(GameObject.Find("Canvas_PlayerUI").GetComponent<Transform>(), false);
        pNameText = this.GetComponent<Text>();

        canvasGroup = GetComponent<CanvasGroup>();

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(!rm.raceStarted) { canvasGroup.alpha = 0; return; }

        if (carRend == null) return;

        if (!regoSet)
        {
            carRego = LeaderBoard.RegisterCar(pNameText.text);
            regoSet = true;
            return;
        }

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        bool carInView = GeometryUtility.TestPlanesAABB(planes, carRend.bounds);
        canvasGroup.alpha = carInView ? 1 : 0;

        this.transform.position = Camera.main.WorldToScreenPoint(target.position + Vector3.up * 2.5f);

        if (cpManager == null)
            cpManager = target.GetComponentInChildren<CheckPointManager>();

        LeaderBoard.SetPosition(carRego, cpManager.lap, cpManager.nextCP_m, cpManager.checkpoint_pos, cpManager.timeEntered);
        //string postition = LeaderBoard.GetPositionString(carRego);

        //lapInfoText.text = postition;
    }
}
