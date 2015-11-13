//ONDARZA ORTEGA JOAQUIN
//DISEÑO ASISTIDO POR COMPUTADORA
//SEMESTRE 2016-1
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//usings de AutoCAD
//using Autodesk.AutoCAD.ApplicationServices;
//using Autodesk.AutoCAD.EditorInput;
//using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Colors;

namespace AutoCADAPI.Lab3
{
    class Imagen
    {
        public Point2d rect;
        public Point3d centerImage;
        public List<Color> colors;
        public List<Pixel> pixels;
        public List<Entity> ents;
        double sizePixels;

        public Imagen(Point2d rect, Point3d centerImage, List<Color> colors, double sizePixels)
        {
            this.rect = rect;
            this.centerImage = centerImage;
            this.colors = colors;
            this.sizePixels = sizePixels;
            this.pixels = new List<Pixel>();
            this.ents = new List<Entity>();
            PixelsConfig();
        }
        private void PixelsConfig()
        {
            int count = 0;
            double auxX  = ((-this.rect.X / 2) * this.sizePixels) + this.sizePixels / 2;
            double auxY0 = (( this.rect.Y / 2) * this.sizePixels) - this.sizePixels / 2;
            double auxY1 = 0;
            for (int i = 0; i < this.rect.X; i++)
            {
                auxY1 = auxY0;
                for (int j = 0; j < this.rect.Y; j++)
                {
                    Pixel aux = new Pixel(this.colors[count], new Point3d(this.centerImage.X + auxX, this.centerImage.Y + auxY1, centerImage.Z), this.sizePixels);
                    this.pixels.Add(aux);
                    this.ents.Add(aux.face);
                    auxY1 -= this.sizePixels;
                    count++;
                }
                auxX += this.sizePixels;
            }
         }
    }
}
