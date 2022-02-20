using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;
using System.Drawing;
using System.IO;
using System.Net;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
//using System.Web.UI.WebControls;



    namespace LajtIt.Bll
{


    public static class Helper
    {
        private static LajtIt.Bll.Helper.Cache<string, string> cacheString = new LajtIt.Bll.Helper.Cache<string, string>();

        private static LajtIt.Bll.Helper.Cache<string, Dal.Settings> cache = new LajtIt.Bll.Helper.Cache<string, Dal.Settings>();
        public static string GetCachedValue(string fieldName, string userName)
        {
            string key = String.Format("{0}{1}", fieldName, userName);

            string value = cacheString.Get(key);
            if (value == null)
                return null;
            else
                return value;

        }
        public static void SetCachedValue(string fieldName, string userName, string value)
        {
            string key = String.Format("{0}{1}", fieldName, userName);

            cacheString.Set(key, value);

        }
        public class WebClientDownload : WebClient
        {
            /// <summary>
            /// Time in milliseconds
            /// </summary>
            public int Timeout { get; set; }

            public WebClientDownload() : this(60000) { }

            public WebClientDownload(int timeout)
            {
                this.Timeout = timeout;
            }

            protected override WebRequest GetWebRequest(Uri address)
            {
                var request = base.GetWebRequest(address);
                if (request != null)
                {
                    request.Timeout = 3600000;// this.Timeout;
                }
                return request;
            }
        }

        public static void PerformImageResizeAndPutOnCanvas(string pFilePath, string pFileName, int pWidth, int pHeight, string pOutputFileName)
        {

            System.Drawing.Image imgBef;
            imgBef = System.Drawing.Image.FromFile(pFilePath + pFileName);


            System.Drawing.Image _imgR;
            _imgR = Imager.Resize(imgBef, pWidth, pHeight, true);


            System.Drawing.Image _img2;
            _img2 = Imager.PutOnCanvas(_imgR, pWidth, pHeight, System.Drawing.Color.White);

            //Save JPEG  
            Imager.SaveJpeg(pFilePath + pOutputFileName, _img2);

        }
    public static byte[] ResizeImage(string pFilePath, string pFileName, int pWidth, int pHeight)
        {

            System.Drawing.Image imgBef;
            imgBef = System.Drawing.Image.FromFile(pFilePath + pFileName);


            System.Drawing.Image _imgR;
            _imgR = Imager.Resize(imgBef, pWidth, pHeight, true);


            System.Drawing.Image _img2;
            _img2 = Imager.PutOnCanvas(_imgR, pWidth, pHeight, System.Drawing.Color.White);

            return CopyImageToByteArray(_img2);
        }

        internal static int GetRandomNumber(int min, int max)
        {
            Random random = new Random(); return random.Next(min, max);
        }

        /// <summary>
        /// Method to "convert" an Image object into a byte array, formatted in PNG file format, which 
        /// provides lossless compression. This can be used together with the GetImageFromByteArray() 
        /// method to provide a kind of serialization / deserialization. 
        /// </summary>
        /// <param name="theImage">Image object, must be convertable to PNG format</param>
        /// <returns>byte array image of a PNG file containing the image</returns>
        public static byte[] CopyImageToByteArray(Image theImage)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                theImage.Save(memoryStream, ImageFormat.Png);
                return memoryStream.ToArray();
            }
        }
        public static byte[] StreamToByteArray(Stream stream)
        {
            if (stream is MemoryStream)
            {
                return ((MemoryStream)stream).ToArray();
            }
            else
            {
                // Jon Skeet's accepted answer 
                return ReadFully(stream);
            }
        }
        public static byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }


        public static bool BatchProcessingIsLock { get
            {

                Dal.SettingsHelper sh = new Dal.SettingsHelper();
                Dal.Settings s = sh.GetSetting("BATCHLOCK");

                if (s == null || s.IntValue.Value == 0)
                    return true;

                return false;

            }
        }

        public static Dal.Settings GetSetting(string code)
        {
            Dal.Settings value = cache.Get(code);
            if (value == null)
            {
                Dal.SettingsHelper sh = new Dal.SettingsHelper();
                Dal.Settings setting = sh.GetSetting(code);
                cache.Set(code, setting);
                return setting;

            }
            else
                return value as Dal.Settings;
        }


        public class Cache<TKey, TValue>
        {
            private readonly Dictionary<TKey, CacheItem> _cache = new Dictionary<TKey, CacheItem>();
            private TimeSpan _maxCachingTime;

            /// <summary>
            /// Creates a cache which holds the cached values for an infinite time.
            /// </summary>
            public Cache()
                : this(TimeSpan.MaxValue)
            {
            }

            /// <summary>
            /// Creates a cache which holds the cached values for a limited time only.
            /// </summary>
            /// <param name="maxCachingTime">Maximum time for which the a value is to be hold in the cache.</param>
            public Cache(TimeSpan maxCachingTime)
            {
                _maxCachingTime = maxCachingTime;
            }

            /// <summary>
            /// Tries to get a value from the cache. If the cache contains the value and the maximum caching time is
            /// not exceeded (if any is defined), then the cached value is returned, else a new value is created.
            /// </summary>
            /// <param name="key">Key of the value to get.</param>
            /// <param name="createValue">Fuction creating a new value.</param>
            /// <returns>A cached or a new value.</returns>
            public TValue Get(TKey key)
            {
                CacheItem cacheItem;
                if (_cache.TryGetValue(key, out cacheItem) && (DateTime.Now - cacheItem.CacheTime) <= _maxCachingTime)
                {
                    return cacheItem.Item;
                }
                return default(TValue);
            }
            public void Set(TKey key, TValue value)
            {
                //CacheItem cacheItem;
                //if (_cache.TryGetValue(key, out cacheItem) && (DateTime.Now - cacheItem.CacheTime) <= _maxCachingTime)
                //{
                //    return cacheItem.Item;
                //}
                _cache[key] = new CacheItem(value);
                // return value;
            }
            private struct CacheItem
            {
                public CacheItem(TValue item)
                    : this()
                {
                    Item = item;
                    CacheTime = DateTime.Now;
                }

                public TValue Item { get; private set; }
                public DateTime CacheTime { get; private set; }
            }

        }

        internal static string ReplaceInvalidAllegroCharactersFromDescription(string content)
        {
            if (String.IsNullOrEmpty(content))
                return content;

            content= content.Replace("&", ""); 

            return content;
        }
        public static string ReplacePolishCharacters(string text)
        {
            string output = text.Trim();
            string[,] znakiSpecjalne = {
    { "Ą", "A" }, { "Ć", "C" }, { "Ę", "E" }, { "Ł", "L" }, { "Ń", "N" }, { "Ó", "O" }, { "Ś", "S" }, { "Ź", "Z" }, { "Ż", "Z" },
    { "ą", "a" }, { "ć", "c" }, { "ę", "e" }, { "ł", "l" }, { "ń", "n" }, { "ó", "o" }, { "ś", "s" }, { "ź", "z" }, { "ż", "z" }
    };
            for (int i = 0; i < znakiSpecjalne.GetLength(0); i++)
            {
                output = output.Replace(znakiSpecjalne[i, 0], znakiSpecjalne[i, 1]);
            }

            return output;
        }

        internal static string ReplaceInvalidAllegroCharacters(string allegroName)
        {
            if (String.IsNullOrEmpty(allegroName))
                return allegroName;


            Dictionary<string, string> characters = new Dictionary<string, string>();
            characters.Add("Ö", "O");
            characters.Add("Ö".ToLower(), "o");
            characters.Add("Ä", "A");
            characters.Add("Ä".ToLower(), "a");
            characters.Add("Å", "A");
            characters.Add("Å".ToLower(), "a");
            characters.Add("É", "E"); 
            characters.Add("É".ToLower(), "e");
             
            characters.Add("º", "");
            characters.Add("І", "I");
            characters.Add("і".ToLower(), "i");
            characters.Add("&", "");

            foreach (var r in characters)
                allegroName = allegroName.Replace(r.Key, r.Value);

            return allegroName;
        }

        public class ProductCategoryMessage
        {
            public bool IsError { get; set; }
            public int Id { get; set; }
            public string ErrorMessage { get; set; }
            public decimal Cost { get; set; }
        }
        public static DateTime GetDateFromUnix(long unixTime)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(unixTime).ToLocalTime();

        }

        public static void CopyProperties(this object source, object destination)
        {
            Dal.Helper.CopyProperties(source, destination);
        }


        public static bool HasAllegroOption(int? optionValue, Enums.AllegroOptions allegroOptions)
        {
            if (!optionValue.HasValue)
                return false;

            return (optionValue.Value & (int)allegroOptions) != 0;


            //int originalInt = optionValue.Value;
            //byte[] bytes = BitConverter.GetBytes(originalInt);
            //BitArray bits = new BitArray(bytes);
            //int ndx = 19; //or whatever ndx you actually care about

            //string s = String.Join("", bits.Cast<bool>().Select(x => x?"1":"0").ToArray());

            //if (bits[ndx] == true)
            //{
            //    //Console.WriteLine("Bit at index {0} is on!", ndx);
            //    return true;
            //}
            //return false;
        }

        public static bool IsSet<T>(T flags, T flag) where T : struct
        {
            int flagsValue = (int)(object)flags;
            int flagValue = (int)(object)flag;

            return (flagsValue & flagValue) != 0;
        }

        public static void Set<T>(ref T flags, T flag) where T : struct
        {
            int flagsValue = (int)(object)flags;
            int flagValue = (int)(object)flag;

            flags = (T)(object)(flagsValue | flagValue);
        }

        public static void Unset<T>(ref T flags, T flag) where T : struct
        {
            int flagsValue = (int)(object)flags;
            int flagValue = (int)(object)flag;

            flags = (T)(object)(flagsValue & (~flagValue));
        }


        public static void UpdateProductCatalogImages(string location)
        {
            Dal.ProductCatalogHelper h = new Dal.ProductCatalogHelper();
            List<Dal.ProductCatalogImage> images = h.GetProductCatalogImages();

            foreach (Dal.ProductCatalogImage image in images)
            {
                string fileName = String.Format(location, image.FileName);

                if (!File.Exists(fileName))
                    continue;

                Bitmap bmp = new Bitmap(fileName);
                int height = bmp.Height;
                int width = bmp.Width;
                FileInfo fi = new FileInfo(fileName);
                int size = (int)fi.Length;
                Dal.ProductCatalogImage i = new Dal.ProductCatalogImage()
                {
                    Height = height,
                    Size = size,
                    Width = width,
                    ImageId = image.ImageId
                };
                h.UpdateProductCatalogImage(i);
            }
        }
        public static void ProductCatalogThumbsImage(string location)
        {
            Dal.ProductCatalogHelper h = new Dal.ProductCatalogHelper();
            List<Dal.ProductCatalogImage> images = h.GetProductCatalogImages();

         

            foreach (Dal.ProductCatalogImage image in images)
            {
                CreateThumbImage(location, image.FileName);

                 
            }
        }

        public static void CreateThumbImage(string location, string fn)
        {

            if (!File.Exists(String.Format(location, fn)))
                return;

            string thumb = String.Format(@"{0}_m{1}", System.IO.Path.GetFileNameWithoutExtension(fn), System.IO.Path.GetExtension(fn));
            if (File.Exists(String.Format(location, thumb)))
                return;

            PerformImageResizeAndPutOnCanvas(String.Format(location, ""), fn, 150, 150, thumb);


        }

        public static void ToJPG(string folder)
        {
            if (!Directory.Exists(String.Format(@"{0}\png", folder)))
                Directory.CreateDirectory(String.Format(@"{0}\png", folder));

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            string name = null;
            try
            {

                string[] files = System.IO.Directory.GetFiles(folder);

                foreach (string file in files)
                {
                    string extension = System.IO.Path.GetExtension(file).ToLower();
                    if (extension == ".png")
                    {
                        //System.Drawing.Imaging.EncoderParameters par = new System.Drawing.Imaging.EncoderParameters();
                        //System.Drawing.Imaging.EncoderParameter p = new System.Drawing.Imaging.EncoderParameter()
                        //par.Param

                          name = System.IO.Path.GetFileNameWithoutExtension(file);



                        List<Dal.ProductCatalogImage> images = pch.GetProductCatalogImage(System.IO.Path.GetFileName(file));

                        if (images == null || images.Count == 0)
                            continue ;


                        string path = System.IO.Path.GetDirectoryName(file);
                        Image png = Image.FromFile(file);
                        string newFileName = null;
                        using (var b = new Bitmap(png.Width, png.Height))
                        {
                            b.SetResolution(png.HorizontalResolution, png.VerticalResolution);

                            using (var g = Graphics.FromImage(b))
                            {
                                g.Clear(Color.White);
                                g.DrawImageUnscaled(png, 0, 0);
                            }
                            newFileName = name + ".jpg";
                            string newPath = path + @"/" + newFileName;

                            b.Save(newPath, System.Drawing.Imaging.ImageFormat.Jpeg);

                            FileInfo fi = new FileInfo(newPath);
                            int size = (int)fi.Length;
                            List<Dal.ProductCatalogImage> imagesToAdd = new List<Dal.ProductCatalogImage>();

                            foreach (Dal.ProductCatalogImage image in images)
                            {
                                Dal.ProductCatalogImage img = new Dal.ProductCatalogImage()
                                {
                                    Description = image.Description,
                                    FileExists = true,
                                    FileName = newFileName,
                                    FriendlyFileName = null,
                                    Height = png.Height,
                                    ImageTypeId = image.ImageTypeId,
                                    InsertDate = image.InsertDate,
                                    IsActive = image.IsActive,
                                    IsThumbnail = image.IsThumbnail,
                                    LinkUrl = image.LinkUrl,
                                    OriginalFileName = image.OriginalFileName,
                                    Priority = image.Priority,
                                    ProductCatalogId = image.ProductCatalogId,
                                    //ShopImageId = null,
                                    Width = png.Width,
                                    UploadedToServer = false,
                                    Size = size,
                                    Title = image.Title

                                };
                                imagesToAdd.Add(img);
                            }
                            png.Dispose();
                            pch.SetProductCatalogImagesConvert(imagesToAdd, images.Select(x => x.ImageId).ToArray());
                        }


                        File.Move(file, file.Replace(name, String.Format(@"png\{0}", name)));

                    }
                }
            }
            catch (Exception ex)
            {
                Bll.ErrorHandler.SendError(ex, String.Format("Nazwa plik {0}", name));

            }
        }

    }
    public static class Imager
    {
        /// <summary>  
        /// Save image as jpeg  
        /// </summary>  
        /// <param name="path">path where to save</param>  
        /// <param name="img">image to save</param>  
        public static void SaveJpeg(string path, Image img)
        {
            var qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);
            var jpegCodec = GetEncoderInfo("image/jpeg");

            var encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;
            img.Save(path, jpegCodec, encoderParams);
        }

        /// <summary>  
        /// Save image  
        /// </summary>  
        /// <param name="path">path where to save</param>  
        /// <param name="img">image to save</param>  
        /// <param name="imageCodecInfo">codec info</param>  
        public static void Save(string path, Image img, ImageCodecInfo imageCodecInfo)
        {
            var qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);

            var encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;
            img.Save(path, imageCodecInfo, encoderParams);
        }

        /// <summary>  
        /// get codec info by mime type  
        /// </summary>  
        /// <param name="mimeType"></param>  
        /// <returns></returns>  
        public static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            return ImageCodecInfo.GetImageEncoders().FirstOrDefault(t => t.MimeType == mimeType);
        }

        /// <summary>  
        /// the image remains the same size, and it is placed in the middle of the new canvas  
        /// </summary>  
        /// <param name="image">image to put on canvas</param>  
        /// <param name="width">canvas width</param>  
        /// <param name="height">canvas height</param>  
        /// <param name="canvasColor">canvas color</param>  
        /// <returns></returns>  
        public static Image PutOnCanvas(Image image, int width, int height, Color canvasColor)
        {
            var res = new Bitmap(width, height);
            using (var g = Graphics.FromImage(res))
            {
                g.Clear(canvasColor);
                var x = (width - image.Width) / 2;
                var y = (height - image.Height) / 2;
                g.DrawImageUnscaled(image, x, y, image.Width, image.Height);
            }

            return res;
        }

        /// <summary>  
        /// the image remains the same size, and it is placed in the middle of the new canvas  
        /// </summary>  
        /// <param name="image">image to put on canvas</param>  
        /// <param name="width">canvas width</param>  
        /// <param name="height">canvas height</param>  
        /// <returns></returns>  
        public static Image PutOnWhiteCanvas(Image image, int width, int height)
        {
            return PutOnCanvas(image, width, height, Color.White);
        }

        /// <summary>  
        /// resize an image and maintain aspect ratio  
        /// </summary>  
        /// <param name="image">image to resize</param>  
        /// <param name="newWidth">desired width</param>  
        /// <param name="maxHeight">max height</param>  
        /// <param name="onlyResizeIfWider">if image width is smaller than newWidth use image width</param>  
        /// <returns>resized image</returns>  
        public static Image Resize(Image image, int newWidth, int maxHeight, bool onlyResizeIfWider)
        {
            if (onlyResizeIfWider && image.Width <= newWidth) newWidth = image.Width;

            var newHeight = image.Height * newWidth / image.Width;
            if (newHeight > maxHeight)
            {
                // Resize with height instead  
                newWidth = image.Width * maxHeight / image.Height;
                newHeight = maxHeight;
            }

            var res = new Bitmap(newWidth, newHeight);

            using (var graphic = Graphics.FromImage(res))
            {
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphic.SmoothingMode = SmoothingMode.HighQuality;
                graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphic.CompositingQuality = CompositingQuality.HighQuality;
                graphic.DrawImage(image, 0, 0, newWidth, newHeight);
            }

            return res;
        }

        /// <summary>  
        /// Crop an image   
        /// </summary>  
        /// <param name="img">image to crop</param>  
        /// <param name="cropArea">rectangle to crop</param>  
        /// <returns>resulting image</returns>  
        public static Image Crop(Image img, Rectangle cropArea)
        {
            var bmpImage = new Bitmap(img);
            var bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
            return bmpCrop;
        }

        public static byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }

        public static Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

        //The actual converting function  
        public static string GetImage(object img)
        {
            return "data:image/jpg;base64," + Convert.ToBase64String((byte[])img);
        }


        public static void PerformImageResizeAndPutOnCanvas(string pFilePath, string pFileName, int pWidth, int pHeight, string pOutputFileName)
        {

            System.Drawing.Image imgBef;
            imgBef = System.Drawing.Image.FromFile(pFilePath + pFileName);


            System.Drawing.Image _imgR;
            _imgR = Imager.Resize(imgBef, pWidth, pHeight, true);


            System.Drawing.Image _img2;
            _img2 = Imager.PutOnCanvas(_imgR, pWidth, pHeight, System.Drawing.Color.White);

            //Save JPEG  
            Imager.SaveJpeg(pFilePath + pOutputFileName, _img2);

        }
    }
}
