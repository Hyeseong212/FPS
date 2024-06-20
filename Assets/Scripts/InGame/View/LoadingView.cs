using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingView : MonoBehaviour
{
    [SerializeField] Image loadingImg;
    private void Start()
    {
        StartLoading();
    }

    IEnumerator Loadingbar()
    {
        float duration = 2f; // 2 seconds
        float elapsedTime = 0f;
        float targetFill = 0.6f; // 60%

        // First phase: Fill to 60% over 2 seconds
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            loadingImg.fillAmount = Mathf.Clamp01((elapsedTime / duration) * targetFill);
            yield return null;
        }

        // Wait until isSyncOK is true
        while (!InGameSessionController.Instance.inGameSessionInfo.isSyncOK)
        {
            yield return null; // Wait for the next frame
        }

        // Fill from 60% to 90% over 1 second
        float syncDuration = 1f;
        float syncElapsedTime = 0f;
        float startFill = 0.6f;
        float endFill = 0.9f;

        while (syncElapsedTime < syncDuration)
        {
            syncElapsedTime += Time.deltaTime;
            loadingImg.fillAmount = Mathf.Lerp(startFill, endFill, syncElapsedTime / syncDuration);
            yield return null;
        }

        // Wait until isPlayerInfoOK is true
        while (!InGameSessionController.Instance.inGameSessionInfo.isPlayerInfoOK)
        {
            yield return null; // Wait for the next frame
        }

        // Instantly fill to 100%
        loadingImg.fillAmount = 1f;
        InGameSessionController.Instance.inGameSessionInfo.isLoadingOK = true;
        // Disable this GameObject
        gameObject.SetActive(false);
    }
    // Call this function to start the loading bar
    public void StartLoading()
    {
        StartCoroutine(Loadingbar());
    }
}
