using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace COTL_API.Utility;

public class CinematicCameraManager
{
    public static List<IEnumerator> ActiveFocusPoints = [];
    public static Quaternion defaultCameraRotation = Quaternion.Euler(315f, 0f, 0f);

    public static void Zoom(float targetZoom)
    {
        GameManager.GetInstance().CameraSetTargetZoom(targetZoom);
    }

    public static void ZoomReset()
    {
        GameManager.GetInstance().CameraResetTargetZoom();
    }

    public static GameObject CreateAndActivateFocusPoint(Vector3 position, Quaternion rotation)
    {
        var cam = CreateFocusPoint(position, rotation);
        SetFollowTarget(cam);
        DOTween.Kill("CinematicFocusPoint");
        //rotate via dotween
        CameraFollowTarget.Instance?.transform.DORotate(rotation.eulerAngles, 1f).SetId("CinematicFocusPoint");

        return cam;
    }

    public static void CreateAndPrepareTimedFocusPoint(Vector3 position, Quaternion rotation, float duration)
    {
        ActiveFocusPoints.Add(CreateTimedFocusPoint(position, rotation, duration));
    }

    private static IEnumerator CreateTimedFocusPoint(Vector3 position, Quaternion rotation, float duration,
        float zoom = 1f)
    {
        var cam = CreateFocusPoint(position, rotation);
        SetFollowTarget(cam);
        DOTween.Kill("CinematicFocusPoint");
        //rotate via dotween
        CameraFollowTarget.Instance?.transform.DORotate(rotation.eulerAngles, 1f).SetId("CinematicFocusPoint");
        Zoom(zoom);
        yield return new WaitForSeconds(duration);
    }

    private static GameObject CreateFocusPoint(Vector3 position, Quaternion rotation)
    {
        var cam = new GameObject("CinematicCameraFocusPoint");
        cam.transform.position = position;
        cam.transform.rotation = rotation;
        return cam;
    }

    public static IEnumerator ActivateAllCreatedFocusPoints()
    {
        foreach (var cam in ActiveFocusPoints) yield return cam;
        ResetAllFocusPoints();
    }

    public static void ResetAllFocusPoints(float speed = 1f)
    {
        GameManager.GetInstance().CamFollowTarget.ResetTargetCamera(speed);
        ActiveFocusPoints.Clear();
    }

    public static void SetFollowTarget(GameObject target)
    {
        //single target only
        ResetAllFocusPoints();
        ResetCameraTargets();
        CameraFollowTarget.Instance?.ClearAllTargets();
        CameraFollowTarget.Instance?.CleanTargets();
        CameraFollowTarget.Instance?.AddTarget(target, 1f);
    }

    public static void AddFollowTarget(GameObject target, float weight = 1f)
    {
        CameraFollowTarget.Instance?.AddTarget(target, weight);
    }

    public static void RemoveFollowTarget(GameObject target)
    {
        CameraFollowTarget.Instance?.RemoveTarget(target);
    }

    public static void ResetCameraTargets()
    {
        CameraFollowTarget.Instance?.ClearAllTargets(); //set rotation back to 315 0 0 
        CameraFollowTarget.Instance?.transform.DORotate(defaultCameraRotation.eulerAngles, 1f);
        GameManager.instance?.AddPlayersToCamera();
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
        {
            LetterBox.Hide(showHudAfterHide);
        }
    }

    public static void ShowHUD(bool show, int delay = 1)
    {
        if (show)
            HUD_Manager.Instance?.Show(delay);
        else
            HUD_Manager.Instance?.Hide(false);
    }
}