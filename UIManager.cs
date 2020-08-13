using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] SceneLoader sceneLoader = null;
    [SerializeField] GameObject escMenu = null;

    [SerializeField] Text velocityXText = null;
    [SerializeField] Text velocityYText = null;
    [SerializeField] Text accelerationXText = null;
    [SerializeField] Text accelerationYText = null;
    [SerializeField] Text accelerationTimer = null;
    [SerializeField] Text timerText = null;

    [SerializeField] Button timeFreezeButton = null;
    [SerializeField] Timerboard timerboard = null;

    public float addToTimerOnFreeze = 1f;
    public float addToTimerOnChange = 0.5f;

    GameObject selectedObject = null;
    MovableObject[] movableObjects;
    Text buttonText = null;

    private float[] objectParameters;
    private bool uiIsActive = false;
    private bool escMenuisActive = false;

    private void Start()
    {
        movableObjects = FindObjectsOfType<MovableObject>();
        Text buttonText = timeFreezeButton.GetComponentInChildren<Text>();
        escMenu.SetActive(false);
        //timerboard = timerboard.GetComponent<Timerboard>();
    }

    private void Update()
    {
        if (selectedObject)
        {
            GetObjectParameters();
        }
        
        UpdateTimerUI();
        ToggleEscapeMenu();
    }

    private void UpdateTimerUI()
    {
        float timer = timerboard.GetTimer();
        timerText.text = timer.ToString("F2") + " s"; 
    }
    private void GetObjectParameters()
    {
        objectParameters = selectedObject.GetComponent<MovableObject>().GetObjectParameters(); //Get the Array with the object Parameters

        for (int i = 0; i < objectParameters.Length; i++) //Refresh the parameters
        {
            RefreshUI(objectParameters[i], i);
        }
    }

    public void ToggleUIElements(bool _isActive)
    {
        uiIsActive = _isActive;



        foreach (Transform child in transform) //enable/disable GUI
        {
            if (child.name == "ObjectInfo")
            {
                child.gameObject.SetActive(uiIsActive);
            }
        }
    }
    private void ToggleEscapeMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            if (escMenu != null && escMenuisActive) //Resume the game and disable Pause Menu
            {
                escMenu.SetActive(false);
                escMenuisActive = false;
                Time.timeScale = 1f;
                FindObjectOfType<PlayerController>().LockMouse();
            }
            else if (escMenu != null && !escMenuisActive) //Pause the game and enable pause Menu
            {
                escMenu.SetActive(true);
                escMenuisActive = true;
                Time.timeScale = 0f;
                FindObjectOfType<PlayerController>().UnlockMouse();
            }
        }
    }
    

    private void RefreshUI(float _input, int _index)
    {
        switch (_index)
        {
            case 0:
                velocityXText.text = _input.ToString("F2") + " m/s";
                break;
            case 1:
                velocityYText.text = _input.ToString("F2") + " m/s";
                break;
            case 2:
                accelerationXText.text = _input.ToString("F2") + " m/s";
                break;
            case 3:
                accelerationYText.text = _input.ToString("F2") + " m/s";
                break;
            case 4:
                accelerationTimer.text = _input.ToString("F2") + " s";
                break;
            default:
                Debug.Log("Es scheint mehr Objektparameter zu geben als gedacht");
                break;

        }
    }

    public void SetSelection(GameObject _object)
    {
        selectedObject = _object;
    }

    public void IncreaseAccelerationX() //Called by Button
    {
        selectedObject.GetComponent<MovableObject>().SetAccelerationParameters(0f, 1f, 0f);
        timerboard.AddToTimer(addToTimerOnChange);
    }

    public void DecreaseAccelerationX() //Called by Button
    {
        selectedObject.GetComponent<MovableObject>().SetAccelerationParameters(0f, -1f, 0f);
        timerboard.AddToTimer(addToTimerOnChange);
    }

    public void IncreaseAccelerationY() //Called by Button
    {
        selectedObject.GetComponent<MovableObject>().SetAccelerationParameters(0f, 0f, 1f);
        timerboard.AddToTimer(addToTimerOnChange);
    }

    public void DecreaseAccelerationY() //Called by Button
    {
        selectedObject.GetComponent<MovableObject>().SetAccelerationParameters(0f, 0f, -1f);
        timerboard.AddToTimer(addToTimerOnChange);
    }

    public void IncreaseAccelerationTimer() //Called by Button
    {
        selectedObject.GetComponent<MovableObject>().SetAccelerationParameters(1f, 0f, 0f);
        timerboard.AddToTimer(addToTimerOnChange);
    }

    public void DecreaseAccelerationTimer() //Called by Button
    {
        selectedObject.GetComponent<MovableObject>().SetAccelerationParameters(-1f, 0f, 0f);
    }
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        sceneLoader.LoadScene("MainMenu");
    }
    public void QuitGame()
    {
        sceneLoader.QuitGame();
    }
    public void StartGame()
    {
        sceneLoader.LoadScene("Level1");
    }
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        sceneLoader.RestartScene();
    }


    public void SetTimeFreezeAll() //wird von Button gecalled
    {
        foreach (MovableObject movable in movableObjects)
        {
            movable.SetTimeFreeze(); 
        }

        timerboard.SetTimeFreeze();

        if (timeFreezeButton.GetComponentInChildren<Text>().text == "Freeze")
        {
            timeFreezeButton.GetComponentInChildren<Text>().text = "Unfreeze";
            timerboard.AddToTimer(addToTimerOnFreeze); //Each time you freeze the time, this gets added to the Timer
        }
        else
        {
            timeFreezeButton.GetComponentInChildren<Text>().text = "Freeze";
        }
    }
}
