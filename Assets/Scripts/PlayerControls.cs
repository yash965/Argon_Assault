using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] GameObject[] lasers;

    [Header("Translation Setup")]
    [Tooltip("speed factor for all directions")] [SerializeField] float controlSpeed = 10f;
    [Tooltip("Range for moving left and right")] [SerializeField] float xRange = 5f;
    [Tooltip("Range for moving up and down")] [SerializeField] float yRange = 2f;

    [Header("Rotational Setup")]
    [SerializeField] float positionPitchFactor = -2f;
    [SerializeField] float controlPitchFactor = -10f;
    [SerializeField] float positionYawFactor = 5f;
    [SerializeField] float controlRollFactor = -15f;

    float xThrow, yThrow;


    void Update()
    {
        ProcessTranslation();
        ProcessRotation();
        ProcessFiring();
    }

    void ProcessRotation()
    {
        float pitchDueToPosition = transform.localPosition.y * positionPitchFactor;
        float pitchDueToControl = yThrow * controlPitchFactor;

        float pitch =  pitchDueToPosition + pitchDueToControl;          // x-Rotation
        float yaw = transform.localPosition.x * positionYawFactor;        // y-Rotation
        float roll = xThrow * controlRollFactor;                        // z-Rotation

        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);

        // *NOTE* 

        // FOR PITCH(x-axis)
        // Here for pitch(x-axis) we are changing position as well as controls(we are using controls because as soon as we lift the
        // finger from the respective key it will go to zero(it toggles between -1 and 1),hence there will be no impact but we want
        // some impact on position due to rotation(which is done by Quaternion.Euler) due to x-axis, but to stay even after lifting the key for controls thats why we have chosen
        // position change we use transform.LocalPosition.

        // FOR YAW(y-axis)
        // Here we are only changing position thats why we have used only transform.LocalPosition.
        // we dont want position to toggle(rotate back and forth with its respective axis(y-axix)) thats why we did'nt use controls.
        
        // FOR ROLL(z-axis)
        // Here we have only used controls only bcause we want the plane to go toggle at that axis.
        // we dont want position to change here.
    }

    void ProcessTranslation()
    {
        xThrow = Input.GetAxis("Horizontal");
        yThrow = Input.GetAxis("Vertical");

        // For x-axis
        float xOffset = xThrow * Time.deltaTime * controlSpeed;
        float xPos = xOffset + transform.localPosition.x;
        float xClampedPos = Mathf.Clamp(xPos, -xRange, xRange);

        // For y-axis
        float yOffset = yThrow * Time.deltaTime * controlSpeed;
        float yPos = yOffset + transform.localPosition.y;
        float yClampedPos = Mathf.Clamp(yPos, -yRange, yRange);

        transform.localPosition = new Vector3(xClampedPos, yClampedPos, transform.localPosition.z);
    }

    void ProcessFiring()
    {
        if(Input.GetButton("Fire1"))
        {
            SetLaserActive(true);
        }
        else
        {
            SetLaserActive(false);
        }
    }

    void SetLaserActive(bool isActive)
    {
        foreach(GameObject shoot in lasers)
        {
            var emissionModule =  shoot.GetComponent<ParticleSystem>().emission;
            emissionModule.enabled = isActive;
        }
    }
}
