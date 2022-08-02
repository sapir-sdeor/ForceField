using System.Collections.Generic;
using Avrahamy.Math;
using Avrahamy.Meshes;
using UnityEngine;

namespace SpaceshipGenerator
{
    [CreateAssetMenu(menuName = "Spaceship Generator/BarNi", fileName = "BarNiSpaceshipGenerator")]
    public class BarNiSpaceshipGenerator : SpaceshipGenerator
    {
        public override void Generate(EditableMesh mesh)
        {
            // Get list of 23 points from func
            var singleFacePoints = FacePoints();

            var singleFaceTriangles = GetSingleFaceTriangles();

            var points = new List<Vector3>();
            var triangles = new int[singleFaceTriangles.Length * 4];

            CreateSingleFace(points, singleFacePoints, singleFaceTriangles, triangles);

            // Normalize points to max radius and reorient ship
            for (var index = 0; index < points.Count; index++)
            {
                points[index] /= 5;
                points[index] *= MAX_RADIUS;
                points[index] = points[index].RotateInDegreesAroundX(90);
            }

            ClampToRadius(points, MAX_RADIUS);

            var allPoints = CreateThreeFaces(points, singleFaceTriangles, triangles);

            mesh.SetPoints(allPoints.ToArray(), triangles);

            var uvs = FaceUvs();
            var allUvs = new List<Vector2>();
            allUvs.AddRange(uvs);
            allUvs.AddRange(uvs);
            allUvs.AddRange(uvs);
            allUvs.AddRange(uvs);
            mesh.Mesh.uv = allUvs.ToArray();
        }

        #region Getters For Mesh Variables

        private static List<Vector3> FacePoints()
        {
            var pointsRandMiddle = 8;
            var pointsRandEdges = 3;
            var randPoints = new List<float>();

            for (var index = 0; index < pointsRandMiddle; index++) randPoints.Add(Random.Range(0, 1f));

            for (var index = pointsRandMiddle; index < pointsRandEdges + pointsRandMiddle; index++)
                randPoints.Add(Random.Range(2f, 4f));
            randPoints.Add(Random.Range(4f, 6f));

            return new List<Vector3>
            {
                //the top of the pyramid-0
                new Vector3(0, randPoints[11], 0),
                //the left down front of the leg-1
                new Vector3(-randPoints[0], -4, -3.5f),
                //the right down front of the leg-2
                new Vector3(randPoints[0], -4, -3.5f),
                //the right down back of the leg-3
                new Vector3(randPoints[1], -4, -2.5f),
                //the left down back of the leg-4
                new Vector3(-randPoints[1], -4, -2.5f),
                //the left middle front of the leg-5
                new Vector3(-randPoints[2], -2, -3.5f),
                //the right middle front of the leg-6
                new Vector3(randPoints[2], -2, -3.5f),
                //the right middle back of the leg-7
                new Vector3(randPoints[3], -3, -2.5f),
                //the left middle back of the leg-8
                new Vector3(-randPoints[3], -3, -2.5f),
                //the left top back of the leg-9
                new Vector3(-randPoints[4], -1, -2.5f),
                //the left top back of the leg-10
                new Vector3(randPoints[4], -1, -2.5f),
                //the left top back of the leg-11
                new Vector3(-randPoints[5], 0, -1.5f),
                //the right top back of the leg-12
                new Vector3(randPoints[5], 0, -1.5f),
                //the middle top back of the leg-13
                new Vector3(-randPoints[6], -2, -1.5f),
                //the middle top back of the leg-14
                new Vector3(randPoints[6], -2, -1.5f),
                //the middle top back of the leg-15
                new Vector3(1.5f, -2, -1.5f),
                //the middle top back of the leg-16
                new Vector3(1.5f, 0, -1.5f),
                //the middle top back of the leg-17
                new Vector3(1.5f, randPoints[9] - 1, -1.5f),
                //the middle top back of the leg-18
                new Vector3(randPoints[7], randPoints[9] - 1, -1.5f),
                //the middle top back of the leg-19
                new Vector3(-randPoints[7], randPoints[9] - 1, -1.5f),
                //the middle top back of the leg-20
                new Vector3(-1.5f, randPoints[9] - 1, -1.5f),
                //the middle top back of the leg-21
                new Vector3(-1.5f, 0, -1.5f),
                //the middle top back of the leg-22
                new Vector3(-1.5f, -2, -1.5f),
                //the middle of the box basis - 23
                new Vector3(0, -randPoints[8], 0)
            };
        }

        private static List<Vector2> FaceUvs()
        {
            return new List<Vector2>
            {
                //face g - 4,3,8,,7,13,14
                new Vector2(0.8f, 0.15f),
                new Vector2(0.7f, 0.15f),
                new Vector2(0.8f, 0.25f),
                new Vector2(0.7f, 0.25f),
                new Vector2(0.8f, 0.35f),
                new Vector2(0.7f, 0.35f),

                //face f -  1,4,8,5,9,13,11,
                new Vector2(0.1f, 0),
                new Vector2(0.1f, 0),
                new Vector2(0.2f, 0.1f),
                new Vector2(0.1f, 0.1f),
                new Vector2(0.1f, 0.2f),
                new Vector2(0, 0.1f),
                new Vector2(0, 0.2f),

                //face e - 1,2,4,3
                new Vector2(0.25f, 0),
                new Vector2(0.35f, 0),
                new Vector2(0.25f, 0.1f),
                new Vector2(0.35f, 0.1f),

                //face d - 2,7,3,6,10,12,14,
                new Vector2(0.4f, 0.15f),
                new Vector2(0.5f, 0.15f),
                new Vector2(0.5f, 0.25f),
                new Vector2(0.4f, 0.25f),
                new Vector2(0.5f, 0.35f),
                new Vector2(0.6f, 0.35f),
                new Vector2(0.6f, 0.25f),

                //face c - 1,6,2,5,10,9,12,11
                new Vector2(0.25f, 0.15f),
                new Vector2(0.35f, 0.25f),
                new Vector2(0.35f, 0.15f),
                new Vector2(0.25f, 0.25f),
                new Vector2(0.35f, 0.35f),
                new Vector2(0.25f, 0.35f),
                new Vector2(0.35f, 0.45f),
                new Vector2(0.25f, 0.45f),

                //face b - 14,16,15,12,17,18,11,19,21,20,22,13,
                new Vector2(0.3f, 0.5f),
                new Vector2(0.4f, 0.6f),
                new Vector2(0.4f, 0.5f),
                new Vector2(0.3f, 0.6f),
                new Vector2(0.4f, 0.7f),
                new Vector2(0.3f, 0.7f),
                new Vector2(0.2f, 0.6f),
                new Vector2(0.2f, 0.7f),
                new Vector2(0.1f, 0.6f),
                new Vector2(0.1f, 0.7f),
                new Vector2(0.1f, 0.5f),
                new Vector2(0.2f, 0.5f),

                //face a -20, 0,19,18,17
                new Vector2(0.15f, 0.85f),
                new Vector2(0.3f, 0.95f),
                new Vector2(0.25f, 0.85f),
                new Vector2(0.35f, 0.85f),
                new Vector2(0.45f, 0.85f),

                //face h -14,15, 23,13,22
                new Vector2(0.75f, 0.7f),
                new Vector2(0.85f, 0.7f),
                new Vector2(0.65f, 0.85f),
                new Vector2(0.65f, 0.7f),
                new Vector2(0.55f, 0.7f)
            };
        }

        private static int[] GetSingleFaceTriangles()
        {
            return new[]
            {
                // G - Leg back - [0,12)
                4, 3, 8,
                8, 3, 7,
                8, 7, 13,
                13, 7, 14,

                // F - Leg left - [12, 27)
                1, 4, 8,
                8, 5, 1,
                5, 8, 9,
                8, 13, 11,
                11, 9, 8,

                // E - Leg bottom - [27, 33)
                1, 2, 4,
                2, 3, 4,

                // D - Leg right - [33, 48)
                2, 7, 3,
                7, 2, 6,
                6, 10, 7,
                7, 12, 14,
                12, 7, 10,

                // C - Leg front - [48, 66)
                1, 6, 2,
                6, 1, 5,
                5, 10, 6,
                10, 5, 9,
                9, 12, 10,
                12, 9, 11,

                // B - Body Face - [66, 96)
                14, 16, 15,
                16, 14, 12,
                12, 17, 16,
                17, 12, 18,
                11, 18, 12,
                18, 11, 19,
                21, 19, 11,
                19, 21, 20,
                22, 11, 13,
                11, 22, 21,

                // A - Pyramid - [96, 105)
                20, 0, 19,
                19, 0, 18,
                18, 0, 17,

                // H - Floor - [105, 114)
                14, 15, 23,
                13, 14, 23,
                22, 13, 23
            };
        }

        #endregion

        #region Helper Duplication Methods

        private static List<Vector3> CreateThreeFaces(List<Vector3> points, int[] singleFaceTriangles, int[] triangles)
        {
            var allPoints = new List<Vector3>(points.Count * 4);
            allPoints.AddRange(points);
            foreach (var point in points) allPoints.Add(point.RotateInDegreesAroundZ(90));

            foreach (var point in points) allPoints.Add(point.RotateInDegreesAroundZ(180));

            foreach (var point in points) allPoints.Add(point.RotateInDegreesAroundZ(270));


            for (var i = 0; i < singleFaceTriangles.Length; i++)
            {
                triangles[i + singleFaceTriangles.Length] = triangles[i] + points.Count;
                triangles[i + 2 * singleFaceTriangles.Length] = triangles[i] + 2 * points.Count;
                triangles[i + 3 * singleFaceTriangles.Length] = triangles[i] + 3 * points.Count;
            }

            return allPoints;
        }

        private static void CreateSingleFace(List<Vector3> points, List<Vector3> singleFacePoints,
            int[] singleFaceTriangles, int[] triangles)
        {
            void AddTriangles(int low, int high)
            {
                var d = DuplicatePointsFromTriangles(points, singleFacePoints, singleFaceTriangles,
                    low, high);
                for (var i = low; i < high; i++) triangles[i] = d[singleFaceTriangles[i]];
            }

            // Front face:
            // G
            AddTriangles(0, 12);
            //F
            AddTriangles(12, 27);
            //E
            AddTriangles(27, 33);
            //D
            AddTriangles(33, 48);
            //C
            AddTriangles(48, 66);
            //B 
            AddTriangles(66, 96);
            //A
            AddTriangles(96, 105);
            //  H:
            AddTriangles(105, singleFaceTriangles.Length);
        }

        private static Dictionary<int, int> DuplicatePointsFromTriangles(
            List<Vector3> toAdd, List<Vector3> points, int[] triangles, int low, int high)
        {
            var uniqueDict = new Dictionary<int, int>();
            for (var i = low; i < high; i++)
            {
                if (uniqueDict.ContainsKey(triangles[i])) continue;

                uniqueDict.Add(triangles[i], toAdd.Count);
                toAdd.Add(points[triangles[i]]);
            }

            return uniqueDict;
        }

        #endregion
    }
}