using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] SceneLoader sceneLoader = null;
    
    [Header("Menu")]
    [SerializeField] bool isMenu;
    [SerializeField] string enterScene;
    [SerializeField] bool quitGame;

    [Header("Gameplay")]
    [SerializeField] bool isWinCondition;
    [SerializeField] float nextSceneDelay = 3f;
    [SerializeField] bool loadSpecificScene;
    [SerializeField] string specificSceneName;

    //TODO: Link two portals


    private void OnTriggerEnter(Collider other)
    {
        if (isMenu && other.gameObject.tag == "Player") //Wenn es ein MenüPortal ist dann soll es nur mit dem Spieler interagieren
        {
            sceneLoader.LoadScene(enterScene);
        }

        else if (isWinCondition && other.gameObject.GetComponent<MovableObject>().IsPlayerObject()) //Soll nur mit der Spielerkugel interagieren und die Winning condition auslösen
        {
            MovableObject playerBall = other.gameObject.GetComponent<MovableObject>();
            playerBall.SetVelocityX(0f);
            playerBall.SetVelocityZ(0f);

            //TODO: PartikelEffekt

            sceneLoader.LoadNextSceneAfterTime(nextSceneDelay);
            
        }
        else if (quitGame && other.gameObject.tag == "Player")
        {
            sceneLoader.QuitGame();
        }
        else if (other.gameObject.GetComponent<MovableObject>().IsPlayerObject() && loadSpecificScene) //Soll nur mit der Spielerkugel interagieren und die Winning condition auslösen
        {
            MovableObject playerBall = other.gameObject.GetComponent<MovableObject>();
            playerBall.SetVelocityX(0f);
            playerBall.SetVelocityZ(0f);

            //TODO: PartikelEffekt

            sceneLoader.LoadSpecificSceneAfterTime(nextSceneDelay, specificSceneName);

        }


    }
}
