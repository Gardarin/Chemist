using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.IO;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization.Formatters.Binary;

namespace Chemist.Models
{
    public class Medicament
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LatinName { get; set; }
        public string Code { get; set; }
        public bool ByPrescription { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }
        public byte[] Picture { get; set; }
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public string Image { get; set; }

        public void SetPicture(Bitmap bitmap)
        {
            //BinaryFormatter bf = new BinaryFormatter();
            MemoryStream bufer= new MemoryStream();
            //bf.Serialize(bufer, bitmap);
            //bufer.Read(Picture, 0, (int)bufer.Length);
            bitmap.Save(bufer, System.Drawing.Imaging.ImageFormat.Bmp);
            Picture = new byte[(int)bufer.Length];
            Picture = bufer.ToArray();
        }

        public byte[] GtePicture()
        {
            //BinaryFormatter bf = new BinaryFormatter();
            //MemoryStream bufer = new MemoryStream();
            //bufer.Write(Picture,0,Picture.Length);
            //Bitmap bitmap = (Bitmap)bf.Deserialize(bufer);

            return Picture;
        }
    }
}