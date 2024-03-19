using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
    public class Terrain
{
    public Terrain(int width, int height)
    {
        _width = width;
        _height = height;
        _terrain = new int[width];
        _random = new Random();
        GenerateTerrain();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        // Create an array of vertices for the triangle strip
        var vertices = new VertexPositionColor[_width * 2];

        for (var i = 0; i < _width; i++)
        {
            // The bottom vertex of the column
            vertices[i * 2] = new VertexPositionColor(new Vector3(i, _height, 0), Color.Green);

            // The top vertex of the column
            var height = _terrain[i];
            vertices[i * 2 + 1] = new VertexPositionColor(new Vector3(i, _height - height, 0), Color.Green);
        }

        // Draw the triangle strip
        spriteBatch.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices, 0, _width * 2 - 2);
    }

    private void GenerateTerrain()
    {
        for (var i = 0; i < _width; i++)
        {
            _terrain[i] = _random.Next(_height);
        }
    }

    private int _width;
    private int _height;
    private int[] _terrain;
    private Random _random;
}