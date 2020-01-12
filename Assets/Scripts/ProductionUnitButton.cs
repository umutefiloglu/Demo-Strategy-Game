using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ProductionUnitButton : MonoBehaviour
{
    #region Variables
    [SerializeField]
    GameObject powerPlantButton;
    [SerializeField]
    GameObject barracksButton;
    [SerializeField]
    GameObject ChildPowerPlant;
    [SerializeField]
    GameObject ChildBarracks;
    [SerializeField]
    Sprite barracksButtonClickedSprite;
    [SerializeField]
    Sprite powerPlantButtonClickedSprite;

    [SerializeField]
    Sprite defaultbarracksButtonSprite;
    [SerializeField]
    Sprite defaultPowerPlantButtonSprite;
    #endregion

    private void Start()
    {
        ChildBarracks = transform.GetChild(0).gameObject;
        ChildPowerPlant = transform.GetChild(1).gameObject;

        defaultbarracksButtonSprite = ChildBarracks.GetComponent<Button>().image.sprite;
        defaultPowerPlantButtonSprite = ChildPowerPlant.GetComponent<Button>().image.sprite;
    }

    private void Update()
    {
        if (ChildPowerPlant.GetComponent<Button>().image.sprite == powerPlantButtonClickedSprite && powerPlantButton.GetComponent<PowerPlantButton>().CanBeDragged == false)
        {
            powerPlantButton.transform.position = ChildPowerPlant.transform.position;
        }
        else if (ChildBarracks.GetComponent<Button>().image.sprite == barracksButtonClickedSprite && barracksButton.GetComponent<BarrackButton>().CanBeDragged == false)
        {
            barracksButton.transform.position = ChildBarracks.transform.position;
        }
    }

    public void PowerPlantButtonClick()
    {
        if (AStarPathFinding2D.pathfindingStatus != 1)
        {
            Debug.Log("Power Plant button is clicked!");
            powerPlantButton.transform.position = ChildPowerPlant.transform.position;

            //change sprite
            ChildPowerPlant.GetComponent<Button>().image.sprite = powerPlantButtonClickedSprite;
            ChildBarracks.GetComponent<Button>().image.sprite = defaultbarracksButtonSprite;
            //Locate other button back
            barracksButton.transform.position = barracksButton.GetComponent<BarrackButton>().OldPosition;
        }
    }
    public void BarracksButtonClick()
    {
        if (AStarPathFinding2D.pathfindingStatus != 1)
        {
            Debug.Log("Barracks button is clicked!");
            barracksButton.transform.position = ChildBarracks.transform.position;

            //change sprite
            ChildBarracks.GetComponent<Button>().image.sprite = barracksButtonClickedSprite;
            ChildPowerPlant.GetComponent<Button>().image.sprite = defaultPowerPlantButtonSprite;
            //Locate other button back
            powerPlantButton.transform.position = powerPlantButton.GetComponent<PowerPlantButton>().OldPosition;
        }
    }

    public void ReverseButtons(Vector2 value)
    {
        //change sprites
        ChildPowerPlant.GetComponent<Button>().image.sprite = defaultPowerPlantButtonSprite;
        ChildBarracks.GetComponent<Button>().image.sprite = defaultbarracksButtonSprite;

        //locate production buttons back
        powerPlantButton.transform.position = powerPlantButton.GetComponent<PowerPlantButton>().OldPosition;
        barracksButton.transform.position = barracksButton.GetComponent<BarrackButton>().OldPosition;
    }
}
