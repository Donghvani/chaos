using System;
using System.IO;
using SkiaSharp;

namespace PlayGround
{
    public class Chaos
    {
        Random Rand { get; }
        public int Width { get; }
        public int Height { get; }
        public int NumberOfPoints { get; }
        public int Step { get; }
        public SKColor BgColor { get; }
        public string BaseDir { get; }

        public Chaos(
            int width, int height,
            int numberOfPoints, int step, SKColor bgColor,
            string baseDir)
        {
            Width = width;
            Height = height;
            NumberOfPoints = numberOfPoints;
            Step = step;
            BgColor = bgColor;
            BaseDir = baseDir;
            Rand = new Random();
        }

        public SKColor GetRandomColor(bool useAplha = false)
        {
            byte red = (byte)Rand.Next(0, 256);
            byte green = (byte)Rand.Next(0, 256);
            byte blue = (byte)Rand.Next(0, 256);
            byte alpha = (byte)Rand.Next(0, 256);

            if (useAplha)
            {
                return new SKColor(red, green, blue, alpha);
            }
            return new SKColor(red, green, blue);
        }

        public float GetRandomRadius(int min, int max)
        {
            var radius0 = Rand.Next(min, max);
            var radius1 = Rand.NextDouble();
            double radius = radius0 + radius1;
            return (float)radius;
        }

        public SKPaint GetRandomSKPaint()
        {
            //return new SKPaint { Color = SKColors.Red };
            return new SKPaint { Color = GetRandomColor() };
        }

        public SKPoint Shift(SKPoint point, float multiplyBy)
        {
            return new SKPoint(point.X * multiplyBy, point.Y * multiplyBy);
        }

        public SKPoint GetNextPoint(SKPoint? prevPoint, SKPoint lastPoint)
        {
            var xChange = Rand.Next(0 - Step, 1 + Step);
            var yChange = Rand.Next(0 - Step, 1 + Step);

            var newPointX = lastPoint.X + xChange;
            var newPointY = lastPoint.Y + yChange;

            if (newPointX < 0 || newPointY < 0 || newPointX > Width || newPointY > Height)                
            {
                return GetNextPoint(prevPoint, lastPoint);
            }

            var newPoint = new SKPoint(newPointX, newPointY);

            if (newPoint.Equals(prevPoint) || newPoint.Equals(lastPoint))
            {
                return GetNextPoint(prevPoint, lastPoint);
            }
            return newPoint;
        }

        public SKPoint[] GetSKPoints(SKPoint initialPoint, int count)
        {
            var sKPoints = new SKPoint[count];
            var lastPoint = initialPoint;

            for (var index = 0; index < count; index++)
            {
                SKPoint? prevPoint = initialPoint;
                if (index > 1)
                {
                    prevPoint = sKPoints[index - 2];
                }
                var point = GetNextPoint(prevPoint, lastPoint);
                sKPoints[index] = point;

                lastPoint = point;
            }

            return sKPoints;
        }

        public void Draw(string fileName)
        {
            using (var bitmap = new SKBitmap(Width, Height))
            {
                using (var canvas = new SKCanvas(bitmap))
                {                    
                    canvas.Clear(BgColor);
                    var initialPoint = new SKPoint(Width / 2, Height / 2);
                    var points = GetSKPoints(initialPoint, NumberOfPoints);
                    SKPoint? lastPoint = null;

                    foreach (var point in points)
                    {
                        //canvas.DrawPoint(point, GetRandomColor());
                        //canvas.DrawPoint(point, SKColors.Black);
                        if (lastPoint.HasValue)
                        {
                            var paint = GetRandomSKPaint();
                            paint.StrokeWidth = 1;
                            paint.IsAntialias = true;
                            //paint.Color = SKColors.Black;
                            canvas.DrawLine(lastPoint.Value, point, paint);
                            canvas.DrawCircle(point, GetRandomRadius(1,3), paint);                  
                        }
                        lastPoint = point;                        
                    }

                    using (var image = SKImage.FromBitmap(bitmap))
                    using (var data = image.Encode(SKEncodedImageFormat.Png, 80))
                    {
                        // save the data to a stream
                        using (var stream = File.OpenWrite(Path.Combine(BaseDir, $"{fileName}.png")))
                        {
                            data.SaveTo(stream);
                        }
                    }
                }
            }
        }
    }
}
