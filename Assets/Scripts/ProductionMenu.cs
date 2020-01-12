using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductionMenu : MonoBehaviour
{
    #region Variables
    private bool isCrossedBottom;
    private bool isCrossedTop;

    private List<GameObject> buttonCouples;

    private GameObject topmostButtonCouple;
    private GameObject bottommostButtonCouple;

    private Vector3 topmostButtonCreationPosition;
    private Vector3 bottommostButtonCreationPosition;

    [SerializeField]
    ScrollRect scrollingAction;

    Camera mainCamera;
    #endregion

    #region Unity Functions
    // Start is called before the first frame update
    void Start()
    {
        isCrossedBottom = false;
        isCrossedTop = false;

        //get button couples
        buttonCouples = new List<GameObject>();
        //initialize
        for (int i = 0; transform.childCount != i; i++)
        {
            buttonCouples.Add(transform.GetChild(i).gameObject);
            //Debug.Log("Child Added!" + i);

            //add scrolling listeners
            scrollingAction.onValueChanged.AddListener(transform.GetChild(i).GetComponent<ProductionUnitButton>().ReverseButtons);
        }

        topmostButtonCouple = buttonCouples[0];
        bottommostButtonCouple = buttonCouples[0];

        mainCamera = Camera.main;

        Debug.Log(buttonCouples);
        
        FindExtremeButtons();
    }

    // Update is called once per frame
    void Update()
    {
        CheckCrossings();
        if (isCrossedBottom == true)
        {
            CreateButtonBottom();
        }
        if (isCrossedTop == true)
        {
            CreateButtonTop();
        }
    }
    #endregion

    #region Custom Functions
    void CreateButtonTop()
    {
        FindExtremeButtons();
        //topmostButtonCouple.transform.position = new Vector3(topmostButtonCouple.transform.position.x, mainCamera.orthographicSize, topmostButtonCouple.transform.position.z);
        bottommostButtonCouple.transform.localPosition = topmostButtonCreationPosition;
        //bottommostButtonCouple.GetComponent<BarrackButton>().Start();

        FindExtremeButtons();
        isCrossedTop = false;
    }
    
    void CreateButtonBottom()
    {
        FindExtremeButtons();
        //bottommostButtonCouple.transform.position = new Vector3(bottommostButtonCouple.transform.position.x, -mainCamera.orthographicSize, bottommostButtonCouple.transform.position.z);
        topmostButtonCouple.transform.localPosition = bottommostButtonCreationPosition;
        //topmostBarrackButton.GetComponent<BarrackButton>().Start();

        FindExtremeButtons();
        isCrossedBottom = false;
    }

    //finds topmost and botommost objects
    void FindExtremeButtons()
    {
        //o(n)
        foreach (var buttonCouple in buttonCouples)
        {
            if (buttonCouple.transform.localPosition.y <= bottommostButtonCouple.transform.localPosition.y)
            {
                bottommostButtonCouple = buttonCouple;
                bottommostButtonCreationPosition = new Vector3(bottommostButtonCouple.transform.localPosition.x, bottommostButtonCouple.transform.localPosition.y - 45f, bottommostButtonCouple.transform.localPosition.z);
            }
            if (buttonCouple.transform.localPosition.y >= topmostButtonCouple.transform.localPosition.y)
            {
                topmostButtonCouple = buttonCouple;
                topmostButtonCreationPosition = new Vector3(topmostButtonCouple.transform.localPosition.x, topmostButtonCouple.transform.localPosition.y + 45f, topmostButtonCouple.transform.localPosition.z);
            }
            
        }
        //Debug.Log("Topmost creation position: " + topmostButtonCreationPosition + ", Bottommost creation position: " + bottommostButtonCreationPosition);
    }
    
    //checks if extreme object's position should be moved or not
    void CheckCrossings()
    {
        if (topmostButtonCouple.transform.position.y < mainCamera.orthographicSize)
        {
            isCrossedTop = true;
            //Debug.Log("Top Crossed!");
        }
        if (bottommostButtonCouple.transform.position.y > -mainCamera.orthographicSize)
        {
            isCrossedBottom = true;
            //Debug.Log("Bottom Crossed!");
        }
    }

    /*
    public void UpButtonFunction()
    {
        foreach (var barrackButton in barrackButtons)
        {
            barrackButton.transform.position = new Vector3(barrackButton.transform.position.x, barrackButton.transform.position.y + 0.1f, barrackButton.transform.position.z);

            barrackButton.GetComponent<BarrackButton>().Start();
        }
        foreach (var powerPlantButton in powerPlantButtons)
        {
            powerPlantButton.transform.position = new Vector3(powerPlantButton.transform.position.x, powerPlantButton.transform.position.y + 0.1f, powerPlantButton.transform.position.z);

            powerPlantButton.GetComponent<PowerPlantButton>().Start();
        }
    }
    
    public void DownButtonFunction()
    {
        foreach (var barrackButton in barrackButtons)
        {
            barrackButton.transform.position = new Vector3(barrackButton.transform.position.x, barrackButton.transform.position.y - 0.1f, barrackButton.transform.position.z);

            barrackButton.GetComponent<BarrackButton>().Start();
        }
        foreach (var powerPlantButton in powerPlantButtons)
        {
            powerPlantButton.transform.position = new Vector3(powerPlantButton.transform.position.x, powerPlantButton.transform.position.y - 0.1f, powerPlantButton.transform.position.z);

            powerPlantButton.GetComponent<PowerPlantButton>().Start();
        }
    }
    */
    #endregion
}
