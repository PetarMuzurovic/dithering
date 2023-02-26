using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dithering
{
    internal class Program
    {
        public Bitmap Grayscale(Bitmap slika)
        {
            for (int y = 0; y < slika.Height; y++)
            {
                for (int x = 0; x < slika.Width; x++)
                {
                    Color pixelColor = slika.GetPixel(x, y);
                    int gray = (int)((pixelColor.R * 0.3) + (pixelColor.G * 0.59) + (pixelColor.B * 0.11));
                    Color newColor = Color.FromArgb(gray, gray, gray);

                    slika.SetPixel(x, y, newColor);
                }
            }

            return slika;
        }

        public Bitmap Dither(Bitmap slika, int gornjaVrednost)
        {
            for (int y = 0; y < slika.Height; y++)
            {
                for (int x = 0; x < slika.Width; x++)
                {
                    Color pixelColor = slika.GetPixel(x, y);

                    decimal crnoBelo = (decimal)(((decimal)(pixelColor.R) / gornjaVrednost));
                    int newColor = (int)Math.Round(crnoBelo);
                    if (newColor < 0) newColor = 0;
                    if (newColor > 1) newColor = 1;

                    Color Boja = Color.FromArgb(newColor * 255, newColor * 255, newColor * 255);
                    slika.SetPixel(x, y, Boja);
                }
            }
            return slika;
        }

        public Bitmap FloydSteinberg(Bitmap slika, int gornjaVrednost, int Odbacivanje)
        {

            for (int y = 0; y < slika.Height; y++)
            {
                for (int x = 0; x < slika.Width; x++)
                {
                    Color pixelColor = slika.GetPixel(x, y);

                    decimal crnoBelo = (decimal)(((decimal)(pixelColor.R) / gornjaVrednost));
                    int newColor = (int)Math.Round(crnoBelo);
                    if (newColor < 0) newColor = 0;
                    if (newColor > 1) newColor = 1;

                    int razlikaPixela = (int)((float)(pixelColor.R) - (float)(newColor * 255));

                    Color Boja;
                    Color sledeci;

                    if (Math.Abs(razlikaPixela) >= Odbacivanje)
                    {
                        if (x + 1 < slika.Width)
                        {
                            sledeci = slika.GetPixel(x + 1, y);
                            int res = (int)(sledeci.R + razlikaPixela * 0.4375);
                            if (res < 0) res = 0;
                            if (res > 255) res = 255;
                            Boja = Color.FromArgb(res, res, res);
                            slika.SetPixel(x + 1, y, Boja);
                        }
                        if (x - 1 > 0 && y + 1 < slika.Height)
                        {
                            sledeci = slika.GetPixel(x - 1, y + 1);
                            int res = (int)(sledeci.R + razlikaPixela * 0.1875);
                            if (res < 0) res = 0;
                            if (res > 255) res = 255;
                            Boja = Color.FromArgb(res, res, res);
                            slika.SetPixel(x - 1, y + 1, Boja);
                        }
                        if (y + 1 < slika.Height)
                        {
                            sledeci = slika.GetPixel(x, y + 1);
                            int res = (int)(sledeci.R + razlikaPixela * 0.3125);
                            if (res < 0) res = 0;
                            if (res > 255) res = 255;
                            Boja = Color.FromArgb(res, res, res);
                            slika.SetPixel(x, y + 1, Boja);
                        }
                        if (x + 1 < slika.Width && y + 1 < slika.Height)
                        {
                            sledeci = slika.GetPixel(x + 1, y + 1);
                            int res = (int)(sledeci.R + (razlikaPixela * 0.0625));
                            if (res < 0) res = 0;
                            if (res > 255) res = 255;
                            Boja = Color.FromArgb(res, res, res);
                            slika.SetPixel(x + 1, y + 1, Boja);
                        }
                    }

                    Boja = Color.FromArgb(newColor * 255, newColor * 255, newColor * 255);
                    slika.SetPixel(x, y, Boja);
                    
                }
            }

            return slika;
        }

        static void Main(string[] args)
        {
            Console.Write("Lokacija: ");
            string lokacija = Console.ReadLine();

            Bitmap slika = new Bitmap(lokacija);
            Program pr = new Program();

            Console.Write("Maksimalan broj (def 255): ");
            int gornjaVrednost = Convert.ToInt32(Console.ReadLine());

            Console.Write("Tolerancija ostatka (def 0): ");
            int Odbacivanje = Convert.ToInt32(Console.ReadLine());




            slika.Save("normalna.jpg");
            
            Bitmap gray = pr.Grayscale(slika);
            gray.Save("siva.jpg");

            Console.Write("Obicno (1(da) ili 0(ne)): ");
            int flag = Convert.ToInt32(Console.ReadLine());
            if (flag == 1)
            {
                Bitmap obicno = pr.Dither(gray, gornjaVrednost);
                obicno.Save("obicno.jpg");
            }

            Bitmap final = pr.FloydSteinberg(gray, gornjaVrednost, Odbacivanje);
            final.Save("final.jpg");

        }
    }
}
