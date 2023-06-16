using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraMovement : MonoBehaviour
{
    public float followingSpeed = 2f;
    public bool lookingDown;
    public Transform target;

    private Transform cameraTransform;

    public float shakeDuration;

    public float shakeAmount;

    public float decreaseFactor;

    Vector3 originalPosition;
    Vector3 newPosition;
    private Dictionary<int, float> minCamera;

    void Awake() {
        if(cameraTransform == null) {
            cameraTransform = GetComponent<Transform>();
        }
        shakeDuration = 0f;
        shakeAmount = 0.1f;
        decreaseFactor = 1.0f;
        lookingDown = false;
        minCamera = new Dictionary<int, float>();
        minCamera.Add(1, 1.36f);
        minCamera.Add(2, 1.45f);
        minCamera.Add(3, 1.32f);
    }

    void OnEnable() {
        originalPosition = cameraTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) target = GameObject.FindWithTag("Player").transform;

        if(lookingDown) newPosition = new Vector3(Math.Max(target.position.x, minCamera[SceneManager.GetActiveScene().buildIndex]), target.position.y - 0.75f, target.position.z);
        else newPosition = new Vector3(Math.Max(target.position.x, minCamera[SceneManager.GetActiveScene().buildIndex]), target.position.y + 0.5f, target.position.z);
        newPosition.z = -10;
        transform.position = Vector3.Slerp(transform.position, newPosition, followingSpeed * Time.deltaTime);

        if (shakeDuration > 0) {
            cameraTransform.localPosition = originalPosition + UnityEngine.Random.insideUnitSphere * shakeAmount * 0.5f;

            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
    }

    public void ShakeCamera() {
        originalPosition = cameraTransform.localPosition;
        shakeDuration = 0.15f;
    }
}
