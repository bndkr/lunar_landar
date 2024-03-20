using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
public class Terrain
{
    public Terrain(int width, int height, int numberLandingZones, int landingZoneSize)
    {
        _width = width;
        _height = height;
        _numberLandingZones = numberLandingZones;
        _landingZoneSize = landingZoneSize;
        _terrain = new int[width];
        _random = new Random();
        GenerateTerrain();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        var vertices = new VertexPositionColor[_width * 2];

        for (var i = 0; i < _width; i++)
        {
            // The bottom vertex of the column
            vertices[i * 2] = new VertexPositionColor(new Vector3(i, _height, 0), Color.Green);

            // The top vertex of the column
            var height = _terrain[i];
            vertices[i * 2 + 1] = new VertexPositionColor(new Vector3(i, _height - height, 0), Color.Green);
        }
        spriteBatch.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices, 0, _width * 2 - 2);
    }

    private void Subdivide(Dictionary<int, int> terrainMap, int left, int right, int variance)
    {
        if (right - left < 2)
        {
            return;
        }

        var midpoint = (left + right) / 2;
        var height = (float)(terrainMap[left] + terrainMap[right]) / 2 + (_random.Next(variance * 2) - variance);

        bool landingZone = false;
        for (int i = 0; i < _numberLandingZones; i++)
        {
            if (midpoint > _landingSitesLeft[i] && midpoint < _landingSitesRight[i])
            {
                landingZone = true;
                for (int j = _landingSitesLeft[i]; j < _landingSitesRight[i]; j++)
                {
                    terrainMap[j] = terrainMap[left];
                }
            }
        }
        if (!landingZone)
        {
            terrainMap[midpoint] = (height <= 10) ? 10 : (int)height;
        }

        Subdivide(terrainMap, left, midpoint, variance / 2);
        Subdivide(terrainMap, midpoint, right, variance / 2);
    }

    private void GenerateTerrain()
    {
        var terrainMap = new Dictionary<int, int>();
        terrainMap.Add(0, _height / 3);
        terrainMap.Add(_width, _height / 3);

        _landingSitesLeft = new int[_numberLandingZones];
        _landingSitesRight = new int[_numberLandingZones];

        // generate landing zones that don't overlap
        for (var i = 0; i < _numberLandingZones; i++)
        {
            int landingZoneStart;
            int landingZoneEnd;
            bool overlap;
            do
            {
                landingZoneStart = (int)((float)_width / 4) + _random.Next(_width - _landingZoneSize - (int)((float)_width / 4));
                landingZoneEnd = landingZoneStart + _landingZoneSize;

                overlap = false;
                for (var j = 0; j < i; j++)
                {
                    if (landingZoneStart <= _landingSitesRight[j] && landingZoneEnd >= _landingSitesLeft[j])
                    {
                        overlap = true;
                        break;
                    }
                }
            } while (overlap);

            _landingSitesLeft[i] = landingZoneStart;
            _landingSitesRight[i] = landingZoneEnd;
        }

        Subdivide(terrainMap, 0, _width, _height / 2);

        for (var i = 0; i < _width; i++)
        {
            if (terrainMap.ContainsKey(i))
            {
                _terrain[i] = terrainMap[i];
            }
            else
            {
                _terrain[i] = _terrain[i - 1];
            }
        }
    }

    public bool CollidesWithRectangle(float centerX, float centerY, int width, int height)
    {
        for (var i = 0; i < _width; i++)
        {
            if (centerX + width / 2 > i && centerX - width / 2 < i)
            {
                if (centerY - height / 2 < _terrain[i])
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool IsXValueInLandingZone(float x)
    {
        for (var i = 0; i < _numberLandingZones; i++)
        {
            if (x > _landingSitesLeft[i] && x < _landingSitesRight[i])
            {
                return true;
            }
        }
        return false;
    }

    private int _width;
    private int _height;
    private int _numberLandingZones;
    private int _landingZoneSize;

    private int[] _landingSitesLeft;
    private int[] _landingSitesRight;
    private int[] _terrain;
    private Random _random;
}