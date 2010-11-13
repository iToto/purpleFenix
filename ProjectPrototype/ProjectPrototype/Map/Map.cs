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
        
        private int numberOfRows;
        private int numberOfCols;

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

            this.numberOfRows = imgHeight / tileSize;
            this.numberOfCols = imgWidth / tileSize;
        }

        public void draw(ScreenManager screen)
        {
            for (int layer = 0; layer < numLayers; ++layer)
            {
                for (int row = 0; row < mapHeight; ++row)
                {
                    for (int col = 0; col < mapWidth; ++col)
                    {
                        if (!(mapData[layer, row, col] == -1))
                        {
                            int index = mapData[layer, row, col];
                            int sourceColumn = ((index % numberOfCols) * tileSize) - tileSize;
                            int sourceRow = ((index / numberOfRows) * tileSize);

                            screen.SpriteBatch.Draw(mapTileSet, 
                                new Vector2(col * tileSize, row * tileSize), 
                                new Rectangle(sourceColumn, sourceRow, 
                                    tileSize, tileSize), 
                                    Color.White);
                        }
                    }

                }
            }
        }
    }
}
