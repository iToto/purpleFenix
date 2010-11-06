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
        int width = 0;
        int height = 0;


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
            xml.ReadToFollowing("Tile_Size");
            xml.Read();
            tileSize = xml.ReadContentAsInt();

            xml.ReadToFollowing("Layers");
            xml.Read();
            numLayers = xml.ReadContentAsInt();

            xml.ReadToFollowing("Width");
            xml.Read();
            width = xml.ReadContentAsInt();

            xml.ReadToFollowing("Height");
            xml.Read();
            height = xml.ReadContentAsInt();

            //Load Map
            mapData = new int[numLayers, width, height];
            xml.ReadToFollowing("Layer");

            for (int layer = 0; layer < numLayers; ++layer)
            {
                xml.ReadToDescendant("RowInfo");
                for (int row = 0; row < height; ++row)
                {
                    xml.Read();
                    string[] temp;
                    temp = xml.ReadContentAsString().Split(',');
                    for (int col = 0; col < width; ++col)
                    {
                        foreach (string value in temp)
                        {
                            mapData[layer, row, col] = Convert.ToInt16(value);
                        }
                    }
                    xml.ReadToNextSibling("RowInfo");
                }
                xml.ReadToNextSibling("Layer");
            }
        }

        public void draw(ScreenManager screen)
        {
            for (int layer = 0; layer < numLayers; layer++)
            {
                for (int row = 0; row < height; row++)
                {
                    for (int col = 0; col < width; col++)
                    {
                        screen.SpriteBatch.Draw(mapTileSet, Vector2.Zero, new Rectangle(col * tileSize, row * tileSize, tileSize, tileSize), Color.White);
                    }
                    
                }
            }
        }
    
    }
}
