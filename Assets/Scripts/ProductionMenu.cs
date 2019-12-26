using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionMenu : MonoBehaviour
{
    #region Variables
    private bool isCrossedBottom;
    private bool isCrossedTop;

    private GameObject[] barrackButtons;
    private GameObject[] powerPlantButtons;

    private GameObject topmostBarrackButton;
    private GameObject bottommostBarrackButton;

    private GameObject topmostPowerPlantButton;
    private GameObject bottommostPowerPlantButton;

    Camera mainCamera;
    #endregion

    #region Unity Functions
    // Start is called before the first frame update
    void Start()
    {
        isCrossedBottom = false;
        isCrossedTop = false;
        barrackButtons = GameObject.FindGameObjectsWithTag("BarrackButton");
        powerPlantButtons = GameObject.FindGameObjectsWithTag("PowerPlantButton");

        topmostBarrackButton = barrackButtons[0];
        bottommostBarrackButton = barrackButtons[0];

        topmostPowerPlantButton = powerPlantButtons[0];
        bottommostPowerPlantButton = powerPlantButtons[0];

        mainCamera = Camera.main;

        Debug.Log(barrackButtons);
        Debug.Log(powerPlantButtons);

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
        topmostBarrackButton.transform.position = new Vector3(topmostBarrackButton.transform.position.x, mainCamera.orthographicSize, topmostBarrackButton.transform.position.z);
        bottommostBarrackButton.transform.position = new Vector3(bottommostBarrackButton.transform.position.x, topmostBarrackButton.transform.position.y + 1f, bottommostBarrackButton.transform.position.z);
        bottommostBarrackButton.GetComponent<BarrackButton>().Start();

        topmostPowerPlantButton.transform.position = new Vector3(topmostPowerPlantButton.transform.position.x, mainCamera.orthographicSize, topmostPowerPlantButton.transform.position.z);
        bottommostPowerPlantButton.transform.position = new Vector3(bottommostPowerPlantButton.transform.position.x, topmostPowerPlantButton.transform.position.y + 1f, bottommostPowerPlantButton.transform.position.z);
        bottommostPowerPlantButton.GetComponent<PowerPlantButton>().Start();

        FindExtremeButtons();
        isCrossedTop = false;
    }
    
    void CreateButtonBottom()
    {
        bottommostBarrackButton.transform.position = new Vector3(bottommostBarrackButton.transform.position.x, -mainCamera.orthographicSize, bottommostBarrackButton.transform.position.z);
        topmostBarrackButton.transform.position = new Vector3(topmostBarrackButton.transform.position.x, bottommostBarrackButton.transform.position.y - 1f, topmostBarrackButton.transform.position.z);
        topmostBarrackButton.GetComponent<BarrackButton>().Start();

        bottommostPowerPlantButton.transform.position = new Vector3(bottommostPowerPlantButton.transform.position.x, -mainCamera.orthographicSize, bottommostPowerPlantButton.transform.position.z);
        topmostPowerPlantButton.transform.position = new Vector3(topmostPowerPlantButton.transform.position.x, bottommostPowerPlantButton.transform.position.y - 1f, topmostPowerPlantButton.transform.position.z);
        topmostPowerPlantButton.GetComponent<PowerPlantButton>().Start();

        FindExtremeButtons();
        isCrossedBottom = false;
    }

    //finds topmost and botommost objects
    void FindExtremeButtons()
    {
        //o(n)
        foreach (var barrackButton in barrackButtons)
        {
            if (barrackButton.transform.position.y < bottommostBarrackButton.transform.position.y)
            {
                bottommostBarrackButton = barrackButton;
            }
            if (barrackButton.transform.position.y > topmostBarrackButton.transform.position.y)
            {
                topmostBarrackButton = barrackButton;
            }
        }
        //o(n)
        foreach (var powerPlantButton in powerPlantButtons)
        {
            if (powerPlantButton.transform.position.y < bottommostPowerPlantButton.transform.position.y)
            {
                bottommostPowerPlantButton = powerPlantButton;
            }
            if (powerPlantButton.transform.position.y > topmostPowerPlantButton.transform.position.y)
            {
                topmostPowerPlantButton = powerPlantButton;
            }
        }
    }
    
    //checks if extreme object's position should be moved or not
    void CheckCrossings()
    {
        if (topmostBarrackButton.transform.position.y < mainCamera.orthographicSize)
        {
            isCrossedTop = true;
        }
        if (bottommostBarrackButton.transform.position.y > -mainCamera.orthographicSize)
        {
            isCrossedBottom = true;
        }
    }

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
    #endregion
}
