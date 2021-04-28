using System.Collections;
using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    Coroutine deathCoroutine;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Ground")
        {
            deathCoroutine = StartCoroutine(DeathCoroutine());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(deathCoroutine != null)
        {
            StopCoroutine(deathCoroutine);
        }
    }

    private IEnumerator DeathCoroutine()
    {
        yield return new WaitForSeconds(3f);
        Debug.Log("Dead");
        GameManager.Instance.ResetStage();
    }
}
