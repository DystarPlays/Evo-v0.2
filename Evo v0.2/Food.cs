using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Evo_v0._2
{
    class Food
    {
        private Vector2 pPosition;
        public Vector2 Position
        {
            get { return pPosition; }
        }

        private int pSize = 3;
        public int Size
        {
            get { return pSize; }
        }

        private Texture2D pTexture;
        public Texture2D Texture
        {
            get { return pTexture; }
        }

        public Food(Point gameSpaceLimit, GraphicsDevice graphicsDevice)
        {
            pPosition = new Vector2(Worker.StaticRandom.Instance.Next(pSize, gameSpaceLimit.X - pSize), Worker.StaticRandom.Instance.Next(pSize, gameSpaceLimit.Y - pSize));
            Color[] colorData = new Color[pSize * pSize];

            for(int i = 0;i<pSize*pSize;i++)
            {
                colorData[i] = Color.LawnGreen;
            }
            pTexture = new Texture2D(graphicsDevice, pSize, pSize);
            pTexture.SetData<Color>(colorData);
        }

    }
}
