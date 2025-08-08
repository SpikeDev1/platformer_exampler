using System;
using UnityEngine;

namespace Gamemaker.Utility.Unity3D
{
    static public class ArtHelper
    {
        /// <summary>
        /// Creates a quad mesh
        /// </summary>
        /// <param name="vertecs">Bottom right corner (other vertices go clockwise), total of 4 vertices</param>
        static public Mesh CreateQuad(Vector3[] vertecs)
        {
            if (vertecs.Length < 4) return null;
            Mesh m = new Mesh();

            Vector3[] v = new Vector3[4];
            v[0] = vertecs[0];
            v[1] = vertecs[1];
            v[2] = vertecs[2];
            v[3] = vertecs[3];

            Color[] c = new Color[4];
            for (int i = 0; i < 4; i++)
                c[i] = new Color(1, 1, 1, 1);

            Vector2[] uv = new Vector2[4];
            uv[2] = new Vector2(1f, 1f);
            uv[1] = new Vector2(1f, 0f);
            uv[0] = new Vector2(0f, 0f);
            uv[3] = new Vector2(0f, 1f);

            m.vertices = v;
            m.colors = c;
            m.uv = uv;
            m.triangles = new int[6] { 2, 1, 0, 0, 3, 2 };
            m.name = "mQuad";
            m.RecalculateNormals();
            m.RecalculateBounds();

            return m;
        }

        /// <summary>
        /// Creates a quad mesh
        /// </summary>
        /// <param name="vertecs">Bottom right corner (other vertices go clockwise), total of 4 vertices</param>
        /// <param name="uv">UV coordinates for the mesh</param>
        static public Mesh CreateQuad(Vector3[] vertecs, Vector2[] uv)
        {
            if (vertecs.Length < 4) return null;
            Mesh m = new Mesh();

            Vector3[] v = new Vector3[4];
            v[0] = vertecs[0];
            v[1] = vertecs[1];
            v[2] = vertecs[2];
            v[3] = vertecs[3];

            Color[] c = new Color[4];
            for (int i = 0; i < 4; i++)
                c[i] = new Color(1, 1, 1, 1);

            if (uv.Length == 0)
            {
                uv = new Vector2[4];
                uv[2] = new Vector2(1f, 1f);
                uv[1] = new Vector2(1f, 0f);
                uv[0] = new Vector2(0f, 0f);
                uv[3] = new Vector2(0f, 1f);
            }

            m.vertices = v;
            m.colors = c;
            m.uv = uv;
            m.triangles = new int[6] { 2, 1, 0, 0, 3, 2 };
            m.name = "mQuad";
            m.RecalculateNormals();
            m.RecalculateBounds();

            return m;
        }

        /// <summary>
        /// Creates a quad mesh (pivot in the center)
        /// </summary>
        /// <param name="width">Width of the quad along the X axis (in Unity units)</param>
        /// <param name="height">Height of the quad along the Z axis (in Unity units)</param>
        /// <param name="pivot01">Pivot point normalized [0,1] for X (0 = left, 1 = right) and Z (0 = bottom, 1 = top)</param>
        /// <returns></returns>
        static public Mesh CreateQuad(float width, float height, Vector2 pivot01)
        {
            Vector3[] v = new Vector3[4];
            v[0] = new Vector3(-width * pivot01.x, 0f, -height * pivot01.y);
            v[1] = new Vector3(width * (1f - pivot01.x), 0f, -height * pivot01.y);
            v[2] = new Vector3(width * (1f - pivot01.x), 0f, height * (1f - pivot01.y));
            v[3] = new Vector3(-width * pivot01.x, 0f, height * (1f - pivot01.y));

            return CreateQuad(v);
        }
    }

}


