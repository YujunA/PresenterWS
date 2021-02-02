using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace PresenterWS.Extensions.Standard.ResManager
{
    [Engine.PwResourceInfo(Self = typeof(ImageResouce), DisplayName ="图片", OpenFileFilter ="图片文件 (...)|*.png;*.jpg;*.jpeg;*.bmp;*.gif;*.tif;*.tiff|所有文件 (*.*)|*.*")]
    public class ImageResouce : Engine.PwResource
    {
        //internal static class ImageSourceConvertor
        //{
        //    [DllImport("gdi32.dll", SetLastError = true)]
        //    private static extern bool DeleteObject(IntPtr hObject);

        //    public static BitmapSource ToBitmapSource(System.Drawing.Bitmap bmp)
        //    {
        //        try
        //        {
        //            IntPtr ptr = bmp.GetHbitmap();
        //            BitmapSource source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
        //                ptr, IntPtr.Zero, System.Windows.Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
                    
        //            DeleteObject(ptr);
        //            return source;
        //        }
        //        catch
        //        {
        //            return null;
        //        }
        //    }
        //    public static BitmapSource StreamToBitmapSource(Stream ms, bool canCloseMs=true)
        //    {
        //        System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(ms);
        //        BitmapSource source = ToBitmapSource(bmp);
                
        //        if(canCloseMs)
        //        {
        //            ms.Close();
        //            ms.Dispose();
        //        }

        //        bmp.Dispose();
        //        return source;
        //    }
        //}

        public BitmapImage Source { get; set; }
        public override System.Drawing.Image LargeIcon { get { return Properties.Resources.IL_ResImageLarge; } }
        public override System.Drawing.Image SmallIcon { get { return Properties.Resources.IL_ResImageSmall; } }
        public override long Length { get { return Source.StreamSource.Length; } }
        public BitmapEncodingMode EncodingMode { get; set; } = BitmapEncodingMode.Jpeg;

        public override void Read(Stream stream)
        {
            Source = new BitmapImage();
            stream.Seek(0, SeekOrigin.Begin);

            Source.BeginInit();

            Source.CacheOption = BitmapCacheOption.OnLoad;
            Source.StreamSource = stream is FileStream ? Engine.PwResource.FileStreamToMemoryStream(stream) : stream;

            Source.EndInit();
            Source.Freeze();

            //Source = BitmapFrame.Create(stream is FileStream ? Engine.PwResource.FileStreamToMemoryStream(stream) : stream);
            //stream.Seek(0, SeekOrigin.Begin);
            //Source = ImageSourceConvertor.StreamToBitmapSource(stream, true);
        }

        public override void Write(Stream stream)
        {
            BitmapEncoder encoder = CreateBitmapEncoder(this.EncodingMode);
            encoder.Frames.Add(BitmapFrame.Create(Source));
            encoder.Save(stream);
        }
        public override XElement Serialize()
        {
            XElement e = base.Serialize();
            e.Add(new XAttribute("EncodingMode", EncodingMode));

            return e;
        }
        public override void Read(Stream stream, XElement e)
        {
            Read(stream);
        }

        public static BitmapEncoder CreateBitmapEncoder(BitmapEncodingMode mode)
        {
            BitmapEncoder e = null;
            switch (mode)
            {
                case BitmapEncodingMode.Bmp:
                    e = new BmpBitmapEncoder();
                    break;
                case BitmapEncodingMode.Gif:
                    e = new GifBitmapEncoder();
                    break;
                case BitmapEncodingMode.Jpeg:
                    e = new JpegBitmapEncoder();
                    break;
                case BitmapEncodingMode.Png:
                    e = new PngBitmapEncoder();
                    break;
                case BitmapEncodingMode.Tiff:
                    e = new TiffBitmapEncoder();
                    break;
                case BitmapEncodingMode.Wmp:
                    e = new WmpBitmapEncoder();
                    break;
            }

            return e;
        }
        public static BitmapDecoder CreateBitmapDecoder(BitmapEncodingMode mode, Stream fs, BitmapCreateOptions createOpt,BitmapCacheOption cacheOpt)
        {
            BitmapDecoder e = null;

            switch (mode)
            {
                case BitmapEncodingMode.Bmp:
                    e = new BmpBitmapDecoder(fs, createOpt, cacheOpt);
                    break;
                case BitmapEncodingMode.Gif:
                    e = new GifBitmapDecoder(fs, createOpt, cacheOpt);
                    break;
                case BitmapEncodingMode.Jpeg:
                    e = new JpegBitmapDecoder(fs, BitmapCreateOptions.None, BitmapCacheOption.Default);
                    break;
                case BitmapEncodingMode.Png:
                    e = new PngBitmapDecoder(fs, createOpt, cacheOpt);
                    break;
                case BitmapEncodingMode.Tiff:
                    e = new TiffBitmapDecoder(fs, createOpt, cacheOpt);
                    break;
                case BitmapEncodingMode.Wmp:
                    e = new WmpBitmapDecoder(fs, createOpt, cacheOpt);
                    break;
            }
            return e;
        }

        public override byte[] Write()
        {
            //int nBytes = (int)Source.StreamSource.Length;//计算流的长度
            //byte[] byteArray = new byte[nBytes];//初始化用于MemoryStream的Buffer
            //int nBytesRead = Source.StreamSource.Read(byteArray, 0, nBytes);//将File里的内容一次性的全部读到byteArray中去
            //return byteArray;
            return null;
        }

        public override void Dispose()
        {
            Source.StreamSource.Close();
            Source.StreamSource.Dispose();

        }
    }
    [ValueConversion(typeof(Engine.PwResourceLink), typeof(BitmapImage))]
    public class ImageResourcesConverter : IValueConverter
    {
        //源属性传给目标属性时，调用此方法ConvertBack
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
                Console.WriteLine(((Engine.PwResourceLink)value).Path);
            return (((Engine.PwResourceLink)value).Resource as ImageResouce)?.Source;
        }

        //目标属性传给源属性时，调用此方法ConvertBack
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
    public enum BitmapEncodingMode
    {
        Bmp,
        Gif,
        Jpeg,
        Png,
        Tiff,
        Wmp
    }
}
