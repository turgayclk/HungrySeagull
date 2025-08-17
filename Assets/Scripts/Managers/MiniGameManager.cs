using System.Collections;
using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    [SerializeField] private Camera cinematicCamera;
    [SerializeField] private Transform[] cinematicPositions;
    [SerializeField] private CatchMiniGame catchMiniGame;

    private bool isMiniGameActive = false;
    public bool IsMiniGameActive => isMiniGameActive;

    private Coroutine timeoutCoroutine;

    public void StartCatchSequence(Transform fishTransform, Animator seagullAnimator, System.Action onSuccess, System.Action onFail)
    {
        if (cinematicPositions.Length > 0)
        {
            int randomIndex = Random.Range(0, cinematicPositions.Length);
            cinematicCamera.transform.position = cinematicPositions[randomIndex].position;
            cinematicCamera.transform.rotation = cinematicPositions[randomIndex].rotation;
        }

        cinematicCamera.gameObject.SetActive(true);
        isMiniGameActive = true;

        // MiniGame Baþlatýrken
        timeoutCoroutine = StartCoroutine(MiniGameTimeout(MiniGameUI.Instance.startTime, fishTransform, onFail));

        MiniGameUI.Instance.StartTimer();

        Debug.Log("MiniGameManager StartCatchSequence");

        // MiniGame de Zaman Durdurma
        Time.timeScale = 0.002f;

        catchMiniGame.StartMiniGame((success) =>
        {
            Debug.Log("MiniGameManager Mini Game Baþladý!");

            if (success)
            {
                // Baþarýlý veya baþarýsýz olunca durdururken
                if (timeoutCoroutine != null)
                {
                    StopCoroutine(timeoutCoroutine);
                    timeoutCoroutine = null;
                }

                StartCoroutine(CatchSequence(fishTransform, seagullAnimator, onSuccess));
            }
            else
            {
                // Baþarýlý veya baþarýsýz olunca durdururken
                if (timeoutCoroutine != null)
                {
                    StopCoroutine(timeoutCoroutine);
                    timeoutCoroutine = null;
                }

                EndMiniGameCleanup(fishTransform.gameObject);
                onFail?.Invoke();
            }
        });
    }

    private IEnumerator MiniGameTimeout(float seconds, Transform fish, System.Action onFail)
    {
        yield return new WaitForSecondsRealtime(seconds);

        if (isMiniGameActive)
        {
            Debug.Log("MiniGame Süresi Bitti!");

            MiniGameUI.Instance.StopTimer();
            catchMiniGame.gameObject.SetActive(false);
            EndMiniGameCleanup(fish.gameObject);

            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
        }

        timeoutCoroutine = null;
    }

    private IEnumerator CatchSequence(Transform fish, Animator seagullAnimator, System.Action onSuccess)
    {
        Time.timeScale = 0.15f;

        Vector3 startPos = seagullAnimator.transform.position;
        Vector3 dir = (fish.position - seagullAnimator.transform.position).normalized;
        Vector3 forwardOffset = dir * 0.4f;
        Vector3 downOffset = Vector3.down * 0.2f;
        Vector3 targetPos = fish.position - forwardOffset + downOffset;

        seagullAnimator.SetTrigger("OpenMouth");

        float journey = 0f;
        float duration = 1.5f;
        while (journey < duration)
        {
            journey += Time.unscaledDeltaTime;
            float t = Mathf.SmoothStep(0, 1, journey / duration);
            seagullAnimator.transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        fish.SetParent(seagullAnimator.transform);
        fish.localPosition = new Vector3(0.319000006f, 0.39199999f, 0.00100000005f);
        fish.localRotation = Quaternion.Euler(0.331f, 0.364f, 64.44f);

        yield return new WaitForSecondsRealtime(1f);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        EndMiniGameCleanup(fish.gameObject);

        yield return new WaitForSecondsRealtime(2f);

        fish.gameObject.SetActive(false);

        onSuccess?.Invoke();

    }

    private void EndMiniGameCleanup(GameObject fish)
    {
        cinematicCamera.gameObject.SetActive(false);
        isMiniGameActive = false;
        catchMiniGame.gameObject.SetActive(false);

        VisibleFish(fish);
    }

    private void VisibleFish(GameObject fish)
    {
        Transform fishesObject = GameObject.Find("Fishes")?.transform;
        if (fishesObject != null)
            fish.transform.SetParent(fishesObject);

        fish.SetActive(true);
        fish.transform.rotation = Quaternion.identity;

        FishJump fishJump = fish.GetComponent<FishJump>();
        fishJump.enabled = true;
        fishJump.StartRoutineFish();
    }
}
