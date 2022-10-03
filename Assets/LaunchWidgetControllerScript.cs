using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaunchWidgetControllerScript : MonoBehaviour
{

    [SerializeField] private GameObject launchWidgetCursor;
    [SerializeField] private GameObject LaunchWidgetObject;

    public float launchCursorSpeed = 1;
    public float weakMargin = 300;
    public float mediumMargin = 150;
    private float strongMargin = 40;
    private bool isFrozen = false;

    // Start is called before the first frame update
    void Start()
    {
        launchWidgetCursor.transform.localPosition = new Vector3(0, -167, 0);
        LaunchWidgetObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (SceneDirector21SEP2022.isCrouched && !isFrozen)
        {
            LaunchWidgetObject.SetActive(true);
            Vector3 movePosition =
                new Vector3(launchWidgetCursor.transform.localPosition.x + launchCursorSpeed,
                    launchWidgetCursor.transform.localPosition.y, launchWidgetCursor.transform.localPosition.z);
            launchWidgetCursor.transform.localPosition = movePosition;
            if (Mathf.Abs(movePosition.x) > weakMargin)
            {
                launchCursorSpeed *= -1;
            }

            if (movePosition.x <= 40 && movePosition.x >-40)
            {
                SceneDirector21SEP2022.jumpMultiplier = 1.5f;
            }
            
            if ((movePosition.x <= 150 && movePosition.x >40) || (movePosition.x >= -150 && movePosition.x <-40))
            {
                SceneDirector21SEP2022.jumpMultiplier = 1.0f;
            }
            
            if ((movePosition.x <= 300 && movePosition.x >150) || (movePosition.x >= -300 && movePosition.x <-150))
            {
                SceneDirector21SEP2022.jumpMultiplier = 0.75f;
            }
        }
        else
        {
            LaunchWidgetObject.SetActive(false);
        }
    }

}
