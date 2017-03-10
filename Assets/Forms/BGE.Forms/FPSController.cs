﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace BGE.Forms
{
    public class FPSController : MonoBehaviour
    {
        public GameObject mainCamera;
        public float speed = 50.0f;

        public bool vrMode = true;

        // Use this for initialization
        void Start()
        {
        }

        void Yaw(float angle)
        {
            Quaternion rot = Quaternion.AngleAxis(angle, Vector3.up);
            transform.rotation = rot * transform.rotation;
        }

        void Roll(float angle)
        {
            Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = rot * transform.rotation;
        }

        void Pitch(float angle)
        {
            float invcosTheta1 = Vector3.Dot(transform.forward, Vector3.up);
            float threshold = 0.95f;
            if ((angle > 0 && invcosTheta1 < (-threshold)) || (angle < 0 && invcosTheta1 > (threshold)))
            {
                return;
            }

            // A pitch is a rotation around the right vector
            Quaternion rot = Quaternion.AngleAxis(angle, transform.right);

            transform.rotation = rot * transform.rotation;
        }

        void Walk(float units)
        {
            transform.position += mainCamera.transform.forward * units;
        }

        void Fly(float units)
        {
            transform.position += Vector3.up * units;
        }

        void Strafe(float units)
        {
            transform.position += mainCamera.transform.right * units;
            
        }

        // Update is called once per frame
        void Update()
        {
            float mouseX, mouseY;
            float speed = this.speed;

            float runAxis = Input.GetAxis("Fire1");

            if (Input.GetKey(KeyCode.LeftShift) || runAxis != 0)
            {
                speed *= 2f;
            }

            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }

            if (Input.GetKey(KeyCode.LeftShift) || runAxis != 0)
            {
                speed *= 5.0f;
            }
            
            if (Input.GetKey(KeyCode.E))
            {
                Fly(Time.deltaTime * speed);
            }
            if (Input.GetKey(KeyCode.F))
            {
                Fly(-Time.deltaTime * speed);
            }



            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");

            Yaw(mouseX * speed * Time.deltaTime);
            Pitch(mouseY * speed * Time.deltaTime);


            float joyX = Input.GetAxis("Joy X");
            float joyY = Input.GetAxis("Joy Y");

            Yaw(joyX * speed * Time.deltaTime);
            Fly(-joyY * speed * Time.deltaTime);

            float contWalk = Input.GetAxis("Vertical");
            float contStrafe = Input.GetAxis("Horizontal");
            Walk(contWalk * speed * Time.deltaTime);
            Strafe(contStrafe * speed * Time.deltaTime);
        }
    }
}