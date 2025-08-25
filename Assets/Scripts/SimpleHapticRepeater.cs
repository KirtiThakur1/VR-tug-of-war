using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class SimpleHapticRepeater : MonoBehaviour
{
    [Range(0f, 1f)]
    public float amplitude = 0.5f;
    public float duration = 0.2f;
    public float interval = 1f;

    private Coroutine hapticCoroutine;

    void Start()
    {
        hapticCoroutine = StartCoroutine(HapticLoop());
    }

    private IEnumerator HapticLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);

            // Get all right-hand devices
            var devices = new List<InputDevice>();
            InputDevices.GetDevicesAtXRNode(XRNode.RightHand, devices);

            foreach (var device in devices)
            {
                if (device.isValid &&
                    device.TryGetHapticCapabilities(out var capabilities) &&
                    capabilities.supportsImpulse)
                {
                    device.SendHapticImpulse(0, amplitude, duration);
                    Debug.Log($"[HAPTIC] Vibrated device: {device.name}");
                }
            }
        }
    }

    private void OnDisable()
    {
        if (hapticCoroutine != null)
            StopCoroutine(hapticCoroutine);
    }
}