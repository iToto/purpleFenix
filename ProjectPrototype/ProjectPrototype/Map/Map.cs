﻿//LavaTileSet Reference: http://www.hbgames.org/forums/viewtopic.php?t=57100

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;

namespace ProjectPrototype
{
    class Map
    {
        int distanceTravelled;
        int scrollSpeed = 1;

        XmlTextReader xml;
        TileMap tileMap;

        Viewport viewport;

        int numLayers = 0;
        int mapWidth = 0;
        int mapHeight = 0;

        const int ROWS_PER_SCREEN = 25;

        public bool HasReachedEnd { private set; get; }

        public int upperBound;
        public int lowerBound;

        //XML variables
        private int tileSize;
       
        private int totalTerrains;
        private int[,] mapData;
        private int[,] enemyData;

        public Map(string xmlPath, ContentManager content, String tileSheetFilename, GraphicsDevice graphicsDevice)
        {
            xml = new XmlTextReader(xmlPath);
            
            Initialize();

            tileMap = new TileMap(content, tileSheetFilename, this.tileSize);
            viewport = graphicsDevice.Viewport;
            this.HasReachedEnd = false;
        }

        public void Initialize()
        {
            xml.ReadToFollowing("Tile_Size");
            xml.Read();
            tileSize = xml.ReadContentAsInt();

            xml.ReadToFollowing("Layers");
            xml.Read();
            numLayers = xml.ReadContentAsInt();

            xml.ReadToFollowing("Width");
            xml.Read();
            mapWidth = xml.ReadContentAsInt();

            xml.ReadToFollowing("Height");
            xml.Read();
            mapHeight = xml.ReadContentAsInt();

            //Load Map
            mapData = new int[mapHeight, mapWidth];
            
            enemyData = new int[mapHeight, mapWidth];
            xml.ReadToFollowing("Layer");

            for (int layer = 0; layer < numLayers; ++layer)
            {
                xml.ReadToFollowing("RowInfo");
                for (int row = 0; row < mapHeight; ++row)
                {
                    xml.Read();
                    string[] temp;
                    temp = xml.ReadContentAsString().Split(',');
                    for (int col = 0; col < mapWidth; ++col)
                    {
                        if (layer == 0)
                        {
                            mapData[row, col] = Convert.ToInt16(temp[col]);
                        }
                    }
                    xml.ReadToNextSibling("RowInfo");
                }
            }

            xml.ReadToFollowing("TerrainLayer");
            xml.ReadToFollowing("TerrainRowInfo");
            for (int row = 0; row < mapHeight; ++row)
            {
                xml.Read();
                string[] temp;
                temp = xml.ReadContentAsString().Split(',');
                for (int col = 0; col < mapWidth; ++col)
                {
                    enemyData[row, col] = Convert.ToInt16(temp[col]) - 1;
                }

                xml.ReadToNextSibling("TerrainRowInfo");
            }

            this.lowerBound = this.mapHeight;
            this.upperBound = this.lowerBound - ROWS_PER_SCREEN;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Start drawing at -1.
            int screenRow = -1;

            for (int row = this.upperBound; row < this.lowerBound; ++row)
            {
                for (int col = 0; col < mapWidth; ++col)
                {
                    if (!(mapData[row, col] == -1))
                    {
                        int index = mapData[row, col];

                        spriteBatch.Draw(tileMap.tileSheet,
                            new Vector2(col * tileSize,
                                screenRow * tileSize + distanceTravelled % tileSize),
                                tileMap.GetTileRectangle(index),
                                Color.White);
                    }
                }
                ++screenRow;
            }
        }

        public void Update(List<Enemy> enemies)
        {
            if (!HasReachedEnd)
            {
                distanceTravelled += this.scrollSpeed;
                if (distanceTravelled % tileSize == 0)
                {
                    --this.upperBound;
                    --this.lowerBound;
                    List<Enemy> newEnemies = LoadEnemies(this.upperBound);
                    enemies.AddRange(newEnemies);
                }

                if (this.upperBound <= 0)
                {
                    this.upperBound = 0;
                    this.lowerBound = ROWS_PER_SCREEN;
                    this.HasReachedEnd = true;
                }
            }
        }

        private List<Enemy> LoadEnemies(int row)
        {
            List<Enemy> newEnemies = new List<Enemy>();
            for (int col = 0; col < mapWidth; ++col)
            {
                if (enemyData[row, col] != -1)
                {
                    Enemy newEnemy = EnemyFactory.buildEnemy(enemyData[row, col]);
                    Vector2 position = new Vector2(col * tileSize, -1 * tileSize);
                    newEnemy.position = position;
                    newEnemies.Add(newEnemy);
                }
            }

            return newEnemies;
        }
    }
}
