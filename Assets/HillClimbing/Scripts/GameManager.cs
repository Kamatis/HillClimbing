using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TMP_Text distanceText;
    public TMP_Text wheelieText;
    public Transform car;

    private Coroutine wheelieShowCoroutine;
    private float maxDistanceTravelled = 0f;

    private void Awake()
    {
        Instance = this;
    }

    public void ResetStage()
    {
        SceneManager.LoadScene("Game");
    }

    public void Wheelie()
    {
        if(wheelieShowCoroutine != null)
        {
            StopCoroutine(wheelieShowCoroutine);
        }

        wheelieShowCoroutine = StartCoroutine(ShowWheelie());
    }

    private IEnumerator ShowWheelie()
    {
        wheelieText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        wheelieText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(maxDistanceTravelled < car.position.x)
        {
            maxDistanceTravelled = Mathf.Floor(car.position.x);
            distanceText.text = $"{maxDistanceTravelled} m";
        }
    }

    public static GameManager Instance;
}
