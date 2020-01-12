using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenAspectRatioScaler : MonoBehaviour
{
    //IMPORTANT NOTE: Before building for mobile, default orientation should be set to landscape mode while building.
    //Ratio scaler works fine on desktop but when tried on my device, it is jittering and some visual bugs appear.
    [SerializeField]
    CanvasScaler canvasScaler;

    [SerializeField]
    Camera mainCamera;

    [SerializeField]
    //4:3 = 4/3
    float defaultAspectRatio;

    float currentAspectRatio;
    float defaultCanvasWidth;
    float defaultCanvasHeight;

    Vector2 expectedCanvasResolution;

    float expectedCameraWidth;
    float currentCameraWidth;

    private void Awake()
    {
        defaultCanvasHeight = canvasScaler.referenceResolution.y;
        defaultCanvasWidth = canvasScaler.referenceResolution.x;
        Debug.Log("Canvas Reference Resolution: " + defaultCanvasWidth + " x " + defaultCanvasHeight);

        currentAspectRatio = mainCamera.aspect;
        Debug.Log("Current aspect Ratio: " + currentAspectRatio);

        currentCameraWidth = mainCamera.rect.width;

        AspectRatioScaler();
    }

    void AspectRatioScaler()
    {
        if (currentAspectRatio == defaultAspectRatio)
        {
            Debug.Log("Current aspect ratio: " + currentAspectRatio + " equals default aspect ratio: " + defaultAspectRatio);
            Debug.Log("No need to change aspect ratio.");
        }
        else
        {
            //scale canvas first. Scale only x width.
            //defaultAspectRatio ---- defaultReferenceResolution
            //currentAspectRatio ---- ??(expectedCanvasResolution)
            expectedCanvasResolution = new Vector2(currentAspectRatio * defaultCanvasWidth / defaultAspectRatio, defaultCanvasHeight);

            //then scale camera's width to crop unwanted areas
            //defaultAspectRatio ---- currentCameraWidth
            //currentAspectRatio ---- ??(expectedCameraWidth)
            expectedCameraWidth = defaultAspectRatio * currentCameraWidth / currentAspectRatio;

            //assign new values to canvas and camera
            canvasScaler.referenceResolution = expectedCanvasResolution;
            mainCamera.rect = new Rect(mainCamera.transform.position.x + (1 - expectedCameraWidth)/2 , mainCamera.transform.position.y, expectedCameraWidth, mainCamera.rect.height);
        }
    }

}
