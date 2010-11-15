﻿using System;
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

        public int upperBound;
        public int lowerBound;

        //XML variables
        private int tileSize;
       
        private int totalTerrains;
        private int[,,] mapData;

        public Map(string xmlPath, ContentManager content, String tileSheetFilename, GraphicsDevice graphicsDevice)
        {
            xml = new XmlTextReader(xmlPath);
            
            Initialize();

            tileMap = new TileMap(content, tileSheetFilename, this.tileSize);
            viewport = graphicsDevice.Viewport;
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
            mapData = new int[numLayers, mapHeight, mapWidth];
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
                        mapData[layer, row, col] = Convert.ToInt16(temp[col]);
                    }
                    xml.ReadToNextSibling("RowInfo");
                }
                xml.ReadToNextSibling("Layer");
            }

            this.lowerBound = this.mapHeight;
            this.upperBound = this.lowerBound - 20;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Start drawing at -1.
            int screenRow = -1;

            for (int layer = 0; layer < numLayers; ++layer)
            {
                for (int row = this.upperBound; row < this.lowerBound; ++row)
                {
                    for (int col = 0; col < mapWidth; ++col)
                    {
                        if (!(mapData[layer, row, col] == -1))
                        {
                            int index = mapData[layer, row, col];

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
        }

        public void Update()
        {
            distanceTravelled += this.scrollSpeed;
            if (distanceTravelled % tileSize == 0)
            {
                --this.upperBound;
                --this.lowerBound;
            }
        }
    }
}
