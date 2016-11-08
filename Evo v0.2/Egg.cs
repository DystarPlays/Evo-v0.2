using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evo_v0._2
{
    class Egg
    {
        private Texture2D pTexture;
        public Texture2D Texture
        {
            get { return pTexture; }
        }

        private int pSize = 3;
        public int Size
        {
            get { return pSize; }
        }

        private Vector2 pPosition;
        public Vector2 Position
        {
            get { return pPosition; }
        }

        private float pGestation;
        public float Gestation
        {
            get { return pGestation; }
        }

        private string[] pParentData;
        public string[] ParentData
        {
            get { return pParentData; }
        }

        public Egg(Vector2 Pos, int GestPeriod, string[] parData, GraphicsDevice graphicsDevice)
        {
            pParentData = parData;
            pPosition = Pos;
            pGestation = GestPeriod;

            Color[] colorData = new Color[pSize * pSize];

            for (int i = 0; i < pSize * pSize; i++)
            {
                colorData[i] = Color.Beige;
            }
            pTexture = new Texture2D(graphicsDevice, pSize, pSize);
            pTexture.SetData<Color>(colorData);
        }

        public void UpdateEgg(GameTime Time, GraphicsDevice graphicsDevice, Point gameSpaceLimit, List<Creature> Creatures,List<Egg>Eggs)
        {
            pGestation -= (float)Time.ElapsedGameTime.TotalSeconds;
            if (pGestation <= 0)
            {
                Creatures.Add(new Creature(pPosition, gameSpaceLimit,graphicsDevice, pParentData));
                Eggs.Remove(this);
            }
        }
    }
}
