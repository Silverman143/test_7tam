using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.Test_7tam
{
    public static class MathMethods 
    {
        public static Vector2[] GetRandom2DPointsOnGroundSprite(Transform groundTransform, int amount, float offset)
        {
            Vector2[] points = new Vector2[amount];

            // Get the bounds of the sprite in world space
            SpriteRenderer spriteRenderer = groundTransform.GetComponent<SpriteRenderer>();
            Bounds bounds = spriteRenderer.bounds;

            // Compute the range of x and y coordinates based on the sprite size and position
            float halfWidth = bounds.extents.x-offset;
            float halfHeight = bounds.extents.y-offset;
            Vector3 pos = groundTransform.position;
            Vector2 xRange = new Vector2(pos.x - halfWidth, pos.x + halfWidth);
            Vector2 yRange = new Vector2(pos.y - halfHeight, pos.y + halfHeight);

            // Generate random points within the range
            for (int i = 0; i < amount; i++)
            {
                float x = Random.Range(xRange.x, xRange.y);
                float y = Random.Range(yRange.x, yRange.y);
                points[i] = new Vector2(x, y);
            }

            return points;
        }
    }
}
