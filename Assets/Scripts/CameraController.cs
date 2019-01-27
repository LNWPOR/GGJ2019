using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    //  TODO: Check on game state condition about enter modes and shit

    public Transform followTarget;

    public Transform rotationPoint;

    [Range(0.0f, 1.0f)]
    public float followDelay = 0.07f;

    public bool isEnableRotation;

    public Transform cameraObject;

    public float shakeAmount = 0.7f;

    public float shakeDecayRate = 3.0f;

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.J)) {
            StartCoroutine(this.Shake(shakeAmount, shakeDecayRate));
        }

        //  If has no follow target then simply do nothing
        if (followTarget == null)
            return;

        //
        //  Position
        //

        //  Linear interpolate position to mimick smooth following on X and Y axis, but remain camera depth
        this.transform.position = new Vector3(
                                        Mathf.Lerp(this.transform.position.x, followTarget.position.x, followDelay)
                                        , Mathf.Lerp(this.transform.position.y, followTarget.position.y, followDelay)
                                        , this.transform.position.z
                                    );

        //
        //  Rotation
        //

        if (isEnableRotation == true) {
            //  check if target rotation point exist or not
            if (rotationPoint != null) {
                //  Cal Vector2 vector
                Vector3 rotationPointToSelfDir = (followTarget.position - rotationPoint.position);
                rotationPointToSelfDir.z = 0.0f;
                rotationPointToSelfDir = rotationPointToSelfDir.normalized;

                //  Cal rotation angle
                float rotationAngle = Mathf.Atan2(rotationPointToSelfDir.x, rotationPointToSelfDir.y) * -Mathf.Rad2Deg;

                //  Transform into quarternion
                Quaternion quarternion = new Quaternion();
                quarternion.eulerAngles = new Vector3(0, 0, rotationAngle);

                this.transform.rotation = quarternion;
            }
        }
    }

    public IEnumerator Shake(float shakeAmount, float shakeDecayRate) {
        if (cameraObject == null)
            yield break;

        Vector3 originalPos = cameraObject.localPosition;

        while (shakeAmount > 0) {
            Vector2 shake = Random.insideUnitCircle * shakeAmount;

            cameraObject.localPosition = originalPos + new Vector3(shake.x, shake.y, 0);


            shakeAmount -= shakeDecayRate * Time.deltaTime;
            yield return null;
        }

        cameraObject.localPosition = originalPos;

    }
}
