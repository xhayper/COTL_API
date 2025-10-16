using System.Collections;
using UnityEngine;

namespace COTL_API.Utility;

public class CinematicCameraManager
{

    public static List<IEnumerator> ActiveFocusPoints = [];
    public static void Zoom(float targetZoom)
    {
        GameManager.GetInstance().CameraSetTargetZoom(targetZoom);
    }

    public static void ZoomReset()
    {
        GameManager.GetInstance().CameraResetTargetZoom();
    }

    public static SimpleSetCamera CreateAndActivateFocusPoint(Vector3 position)
    {
        var cam = CreateFocusPoint(position);
        cam.Play();
        return cam;
    }

    public static void CreateAndPrepareTimedFocusPoint(Vector3 position, float duration)
    {
        ActiveFocusPoints.Add(CreateTimedFocusPoint(position, duration));
    }

    private static IEnumerator CreateTimedFocusPoint(Vector3 position, float duration)
    {
        var cam = CreateFocusPoint(position);
        cam.Play();
        yield return new WaitForSeconds(duration);
    }

    private static SimpleSetCamera CreateFocusPoint(Vector3 position)
    {
        var cam = new GameObject("CinematicCameraFocusPoint");
        cam.transform.position = position;
        var ssc = cam.AddComponent<SimpleSetCamera>();
        ssc.AutomaticallyActivate = false;
        return ssc;
    }

    public static IEnumerator ActivateAllCreatedFocusPoints()
    {
        foreach (var cam in ActiveFocusPoints)
        {
            yield return cam;
        }
    }

    public static void ResetAllFocusPoints(float speed = 1f)
    {
        GameManager.GetInstance().CamFollowTarget.ResetTargetCamera(speed);
        ActiveFocusPoints.Clear();
    }

    public static void AddFollowTarget(GameObject target, float weight = 1f)
    {
        CameraFollowTarget.Instance?.AddTarget(target, weight);
    }

    public static void RemoveFollowTarget(GameObject target)
    {
        CameraFollowTarget.Instance?.RemoveTarget(target);
    }

    public static void SetCameraLimits(bool enabled, Bounds limits)
    {
        if (!enabled)
            CameraFollowTarget.Instance?.DisableCameraLimits();
        else
            CameraFollowTarget.Instance?.SetCameraLimits(limits);
    }

    public static void ShowLetterbox(bool show, bool showHudAfterHide = true, string subtitle = "")
    {
        if (show)
        {
            LetterBox.Show(false);
            if (subtitle != "")
                LetterBox.Instance?.ShowSubtitle(subtitle);
        }

        else
            LetterBox.Hide(showHudAfterHide);
    }

    public static void ShowHUD(bool show, int delay = 1)
    {
        if (show)
            HUD_Manager.Instance?.Show(delay);
        else
            HUD_Manager.Instance?.Hide(false);
    }
}
