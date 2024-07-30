using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityFlip : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform Environment;
    [SerializeField] private Transform Player;
    [SerializeField] private Transform PlayerHead;
    [SerializeField] private GameObject HolographicModel;

    [SerializeField] private float rotationDuration = 0.5f;
    [SerializeField] private float slowMotionFactor = 0.1f;

    private bool isFlipping = false;
    private bool isPriviewing = false;

    private Quaternion targetRotation;
    private CapsuleCollider playerCollider;

    [SerializeField] private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        if (!isFlipping)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                ShowHolographicFlip(Vector3.right);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                ShowHolographicFlip(Vector3.left);
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                ShowHolographicFlip(Vector3.forward);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                ShowHolographicFlip(Vector3.back);
            }

            if(isPriviewing && Input.GetKeyDown(KeyCode.Return))
            {
                StartGravityFlip();
            }
        }
        
    }

    private void ShowHolographicFlip(Vector3 axis)
    {
        isPriviewing = true;
        
        targetRotation = Quaternion.AngleAxis(90, axis) * Environment.rotation;
        HolographicModel.SetActive(true);
        
        HolographicModel.transform.position = PlayerHead.position;
        HolographicModel.transform.rotation = targetRotation;
    }

    private void StartGravityFlip()
    {
        isFlipping = true;
        isPriviewing = false;
        HolographicModel.SetActive(false);
        
        playerCollider.enabled = false;

        StartCoroutine(RotateEnvironment(targetRotation));
    }

    IEnumerator RotateEnvironment(Quaternion endRotation)
    {
        Quaternion startRotation = Environment.rotation;
        float elapsedTime = 0;

        Time.timeScale = slowMotionFactor;

        rb.useGravity = false;

        while (elapsedTime < rotationDuration)
        {
            elapsedTime += Time.deltaTime;
            Environment.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / rotationDuration);
            yield return null;
        }

        Environment.rotation = endRotation;

        AdjustPlayerPosition();

        playerCollider.enabled = true;

        Time.timeScale = 1.0f;

        rb.useGravity = true;

        isFlipping = false;
    }

    private void AdjustPlayerPosition()
    {
        Player.position += Player.transform.up * 0.1f;
    }
}
