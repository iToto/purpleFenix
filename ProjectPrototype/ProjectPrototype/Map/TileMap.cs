using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectPrototype
{
    class TileMap
    {
        public Texture2D tileSheet;
        int tileSize;

        public TileMap(ContentManager content, String tileSheetFilename, int tileSize)
        {
            this.tileSheet = content.Load<Texture2D>(tileSheetFilename);
            this.tileSize = tileSize;
        }

        public Rectangle GetTileRectangle(int tileNumber)
        {
            tileNumber -= 1;
            int numberOfTileColumns = tileSheet.Width / tileSize;
            int numberOfTileRows = tileSheet.Height / tileSize;

            Vector2 topLeft = new Vector2(tileNumber % numberOfTileColumns, 
                tileNumber / numberOfTileRows);

            Rectangle tileRectangle = new Rectangle((int)topLeft.X * tileSize, 
                (int)topLeft.Y * tileSize, tileSize, tileSize);

            return tileRectangle;
        }
    }
}
