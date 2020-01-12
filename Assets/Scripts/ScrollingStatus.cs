using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingStatus : MonoBehaviour
{
    #region Variables
    [SerializeField]
    GameObject powerPlantButton;
    [SerializeField]
    GameObject barracksButton;
    #endregion

    #region Unity Functions
    // Update is called once per frame
    void Update()
    {
        if (barracksButton.GetComponent<BarrackButton>().CanBeDragged == true || powerPlantButton.GetComponent<PowerPlantButton>().CanBeDragged == true || AStarPathFinding2D.pathfindingStatus == 1)
        {
            GetComponent<ScrollRect>().enabled = false;
        }
        else
        {
            GetComponent<ScrollRect>().enabled = true;
        }
    }


    #endregion
}
