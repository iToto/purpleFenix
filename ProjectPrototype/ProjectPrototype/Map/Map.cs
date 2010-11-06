﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using System.Xml;

namespace ProjectPrototype
{
    class Map
    {
        public Texture2D sprite;
        public int distanceTravelled;
        private XmlTextReader xml;
        private Texture2D mapTileSet;
        int numLayers = 0;
        int mapWidth = 0;
        int mapHeight = 0;
        int imgWidth = 0;
        int imgHeight = 0;


        //XML variables
        private int tileSize;
       
        private int totalTerrains;
        private int[,,] mapData;

        public Map(string xmlPath, Texture2D map)
        {
            xml = new XmlTextReader(xmlPath);
            mapTileSet = map;
        }

        public void load ()
        {
            xml.ReadToFollowing("Width");
            xml.Read();
            imgWidth = xml.ReadContentAsInt();

            xml.ReadToFollowing("Height");
            xml.Read();
            imgHeight = xml.ReadContentAsInt();


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
            mapData = new int[numLayers, mapWidth, mapHeight];
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
        }

        public void draw(ScreenManager screen)
        {
            for (int layer = 0; layer < numLayers; ++layer)
            {
                for (int row = 0; row < mapHeight; ++row)
                {
                    for (int col = 0; col < mapWidth; ++col)
                    {
                        //screen.SpriteBatch.Draw(mapTileSet, new Vector2(col*tileSize,row*tileSize), new Rectangle(col * tileSize, row * tileSize, tileSize, tileSize), Color.White);
                        if (!(mapData[layer, row, col] == -1))
                            screen.SpriteBatch.Draw(mapTileSet, new Vector2(col * tileSize, row * tileSize), new Rectangle((mapData[layer, row, col] % (imgWidth / tileSize)) * tileSize, ((row + 1) * tileSize), tileSize, tileSize), Color.White);
                    }

                }
            }
        }
    }
}
