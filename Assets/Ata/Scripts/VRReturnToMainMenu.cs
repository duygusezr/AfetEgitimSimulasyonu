using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable))]
public class VRReturnToMainMenu : MonoBehaviour
{
    [Header("Main Menu Scene")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    [Header("Hold Settings")]
    [SerializeField] private float holdDuration = 1f;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private Coroutine holdCoroutine;

    private void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
    }

    private void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    private void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        holdCoroutine = StartCoroutine(HoldToReturn());
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        if (holdCoroutine != null)
        {
            StopCoroutine(holdCoroutine);
            holdCoroutine = null;
        }
    }

    private IEnumerator HoldToReturn()
    {
        float timer = 0f;

        while (timer < holdDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene(mainMenuSceneName);
    }
}
