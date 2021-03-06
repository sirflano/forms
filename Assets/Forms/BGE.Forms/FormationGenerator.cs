﻿using UnityEngine;
using System.Collections.Generic;

namespace BGE.Forms
{
    public class FormationGenerator : School {
        public int sideWidth = 10;
        public float gap = 50;

        [Range(0.0f, 1.0f)]
        public float variance = 0.1f;

        public GameObject leaderPrefab;
        public GameObject followerPrefab;

        private List<Vector3> positions = new List<Vector3>();


        void GenerateCreaturePosition(Vector3 pos, Vector3 startPos, int current, int depth)
        {
            positions.Add(pos);
            pos.z = startPos.z;
            if (current < depth)
            {
                if (pos.x <= transform.position.x)
                {
                    Vector3 left = new Vector3(-1, 0, -1) * gap;
                    left.x *= Random.Range(1.0f, 1.0f - variance);
                    left.y += gap * Random.Range(-variance, variance);
                    left.z *= - Random.Range(1.0f - variance, 1.0f + variance);
                    GenerateCreaturePosition(pos + left, startPos, current + 1, depth);
                
                }
                if (pos.x >= transform.position.x)
                {
                    Vector3 right = new Vector3(1, 0, -1) * gap;
                    right.x *= Random.Range(1.0f, 1.0f - variance);
                    right.y += gap * Random.Range(-variance, variance);
                    right.z *= - Random.Range(1.0f - variance, 1.0f + variance);
                    GenerateCreaturePosition(pos + right, startPos, current + 1, depth);
                }
            }
        }

        void GeneratePositions()
        {
            positions.Clear();
            GenerateCreaturePosition(transform.position, transform.position, 0, sideWidth);
        }

        void OnDrawGizmos()
        {
            if (!Application.isPlaying)
            {
                GeneratePositions();
                if (isActiveAndEnabled)
                {
                    foreach (Vector3 pos in positions)
                    {
                        Gizmos.color = Color.magenta;
                        Gizmos.DrawCube(pos, Vector3.one * gap * 0.5f);
                    }
                }
            }
        }

        GameObject ClosestChild(GameObject go)
        {
            float closest = float.MaxValue;
            int closestIndex = -1;
            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject child = transform.GetChild(i).gameObject;
                if (child != go)
                {
                    float distance = Vector3.Distance(go.transform.position, child.transform.position);
                    if (distance < closest)
                    {
                        closestIndex = i;
                        closest = distance;
                    }
                }
            }
            return transform.GetChild(closestIndex).gameObject;
        }


        // Use this for initialization
        void Start () {
            GeneratePositions();
            GameObject leader = null;

            for (int i = 0; i < positions.Count; i++)
            {
                if (i == 0)
                {
                    leader = GameObject.Instantiate<GameObject>(leaderPrefab);
                    leader.transform.position = positions[i];
                    leader.transform.parent = transform;
                    leader.SetActive(true);
                }
                else
                {
                    GameObject go = GameObject.Instantiate<GameObject>(followerPrefab);
                    go.transform.position = positions[i];
                    go.transform.parent = transform;
                    go.SetActive(true);
                    Formation formation = go.GetComponentInChildren<Formation>();
                    if (formation == null)
                    {
                        formation = go.GetComponentInChildren<Boid>().gameObject.AddComponent<Formation>();
                        formation.weight = 100.0f;
                    }
                    GameObject myLeader = leader; // ClosestChild(go);
                    Boid boid = myLeader.GetComponentInChildren<Boid>();
                    formation.leader = myLeader.gameObject;
                    go.GetComponentInChildren<Boid>().school = this;
                }
            }

            Utilities.SetLayerRecursively(this.gameObject, this.gameObject.layer);
        }
	
        // Update is called once per frame
        void Update () {
	
        }
    }
}