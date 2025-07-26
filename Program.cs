using System;
using System.IO;

class Program
{
    struct RGB {
        public byte R, G, B;
        public RGB(byte r, byte g, byte b) {
            R = r; G = g; B = b;
        }
    }

    static readonly RGB[] VGA_COLORS = new RGB[]
    {
        new RGB(0, 0, 0),         // 0 - Preto
        new RGB(0, 0, 128),       // 1 - Azul escuro
        new RGB(0, 128, 0),       // 2 - Verde escuro
        new RGB(0, 128, 128),     // 3 - Ciano escuro
        new RGB(128, 0, 0),       // 4 - Vermelho escuro
        new RGB(128, 0, 128),     // 5 - Magenta escuro
        new RGB(128, 128, 0),     // 6 - Castanho
        new RGB(192, 192, 192),   // 7 - Cinza claro
        new RGB(128, 128, 128),   // 8 - Cinza escuro
        new RGB(0, 0, 255),       // 9 - Azul
        new RGB(0, 255, 0),       // 10 - Verde
        new RGB(0, 255, 255),     // 11 - Ciano
        new RGB(255, 0, 0),       // 12 - Vermelho
        new RGB(255, 0, 255),     // 13 - Magenta
        new RGB(255, 255, 0),     // 14 - Amarelo
        new RGB(255, 255, 255),   // 15 - Branco
    };

    static void Main()
    {
        Console.ForegroundColor = ConsoleColor.Black;
        Console.BackgroundColor = ConsoleColor.DarkYellow;
        Console.Clear();
        Console.Write("Largura: ");
        int width = int.Parse(Console.ReadLine());

        Console.Write("Altura: ");
        int height = int.Parse(Console.ReadLine());

        Console.Write("Cor VGA (0-15): ");
        int cor = int.Parse(Console.ReadLine());

        if (cor < 0 || cor > 15)
        {
            Console.WriteLine("Cor inválida.");
            return;
        }

        RGB color = VGA_COLORS[cor];

        int rowSize = width * 4; // 32 bits por pixel
        int imageSize = rowSize * height;
        int fileSize = 14 + 40 + imageSize;

        using (FileStream fs = new FileStream("new.bmp", FileMode.Create, FileAccess.Write))
        using (BinaryWriter bw = new BinaryWriter(fs))
        {
            // BITMAPFILEHEADER
            bw.Write((byte)'B');
            bw.Write((byte)'M');
            bw.Write(fileSize);
            bw.Write((short)0);
            bw.Write((short)0);
            bw.Write(14 + 40); // Offset dos dados de imagem

            // BITMAPINFOHEADER
            bw.Write(40);         // biSize
            bw.Write(width);
            bw.Write(height);
            bw.Write((short)1);   // biPlanes
            bw.Write((short)32);  // biBitCount
            bw.Write(0);          // biCompression (BI_RGB)
            bw.Write(imageSize);  // biSizeImage
            bw.Write(0);          // biXPelsPerMeter
            bw.Write(0);          // biYPelsPerMeter
            bw.Write(0);          // biClrUsed
            bw.Write(0);          // biClrImportant

            // Gerar dados da imagem
            byte[] row = new byte[rowSize];
            for (int x = 0; x < width; x++)
            {
                row[x * 4 + 0] = color.B;  // Blue
                row[x * 4 + 1] = color.G;  // Green
                row[x * 4 + 2] = color.R;  // Red
                row[x * 4 + 3] = 255;      // Alpha
            }

            // BMP guarda as linhas de baixo para cima
            for (int y = 0; y < height; y++)
            {
                bw.Write(row);
            }
        }

        Console.WriteLine("Ficheiro new.bmp criado com sucesso!");
    }
}
