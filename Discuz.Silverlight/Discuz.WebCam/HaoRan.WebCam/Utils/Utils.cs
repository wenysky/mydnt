﻿using System;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Windows.Browser;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.ComponentModel;

using HaoRan.WebCam.MixObjectsSoapClient;

namespace HaoRan.WebCam
{
    public class Utils
    {
        public static string GetUserId()
        {
            return GetCookie("userid");
            //return "1";
        }

        public static string GetCookie(String key)
        {
            if (string.IsNullOrEmpty(HtmlPage.Document.Cookies))
                return null;

            //找到想应的cookie键值
            string result = (from c in
                                 (from cookie in HtmlPage.Document.Cookies.Split(';')
                                  where cookie.Contains(key + "=")
                                  select cookie.Split('&')).FirstOrDefault()
                             where c.Contains(key + "=")
                             select c).FirstOrDefault().ToString();

            if(string.IsNullOrEmpty(result))
                return null;

            return result.Substring(result.IndexOf(key + "=") + key.Length + 1);
        }

        public static bool Exists(String key, String value)
        {
            return HtmlPage.Document.Cookies.Contains(String.Format("{0}={1}", key, value));
        }

        public static UploadAvatarSoapClient CreateServiceClient()
        {
            var endpointAddr = new EndpointAddress(new Uri(Application.Current.Host.Source, ServiceUrl));
            var binding = new BasicHttpBinding();
            var ctor = typeof(UploadAvatarSoapClient).GetConstructor(new Type[] { typeof(Binding), typeof(EndpointAddress) });
            return (UploadAvatarSoapClient)ctor.Invoke(new object[] { binding, endpointAddr });
        }

        /// <summary>
        /// 获得指定Url参数的int类型值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url参数的int类型值</returns>
        public static int StrToInt(string str, int defValue)
        {
            if (str == null)
                return defValue;

            if (str.Length > 0 && str.Length <= 11 && System.Text.RegularExpressions.Regex.IsMatch(str, @"^[-]?[0-9]*$"))
            {
                if ((str.Length < 10) || (str.Length == 10 && str[0] == '1') || (str.Length == 11 && str[0] == '-' && str[1] == '1'))
                    return Convert.ToInt32(str);
            }
            return defValue;
        }

   
        /// <summary>
        /// 获取认证信息
        /// </summary>
        /// <returns></returns>
        public static CredentialInfo GetCredentialInfo()
        {
                CredentialInfo _creinfo = new CredentialInfo();
                _creinfo.UserID = Utils.StrToInt(Utils.GetCookie("userid"), 0);
                _creinfo.Password = Utils.GetCookie("password");

                if (App.GetInitParmas.ContainsKey("authToken") && !string.IsNullOrEmpty(App.GetInitParmas["authToken"]))
                    _creinfo.AuthToken = App.GetInitParmas["authToken"];

                //_creinfo.UserID = 1;
                return _creinfo;
        }

        /// <summary>
        /// 从字符串的指定位置截取指定长度的子字符串
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="startIndex">子字符串的起始位置</param>
        /// <param name="length">子字符串的长度</param>
        /// <returns>子字符串</returns>
        public static string CutString(string str, int startIndex, int length)
        {
            if (startIndex >= 0)
            {
                if (length < 0)
                {
                    length = length * -1;
                    if (startIndex - length < 0)
                    {
                        length = startIndex;
                        startIndex = 0;
                    }
                    else
                        startIndex = startIndex - length;
                }

                if (startIndex > str.Length)
                    return "";
            }
            else
            {
                if (length < 0)
                    return "";
                else
                {
                    if (length + startIndex > 0)
                    {
                        length = length + startIndex;
                        startIndex = 0;
                    }
                    else
                        return "";
                }
            }

            if (str.Length - startIndex < length)
                length = str.Length - startIndex;

            return str.Substring(startIndex, length);
        }

        /// <summary>
        /// 从字符串的指定位置开始截取到字符串结尾的了符串
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="startIndex">子字符串的起始位置</param>
        /// <returns>子字符串</returns>
        public static string CutString(string str, int startIndex)
        {
            return CutString(str, startIndex, str.Length);
        }


        public static ObservableCollection<BitmapImage> GetPreviewImage(string url, string userid)
        {
            ObservableCollection<BitmapImage> bitmapImageList = new ObservableCollection<BitmapImage>();

            userid = FormatUid(userid);

            string avatarFileName = string.Format("/avatars/upload/{0}/{1}/{2}/{3}_avatar_",
                   userid.Substring(0, 3), userid.Substring(3, 2), userid.Substring(5, 2), userid.Substring(7, 2));

            //启动远程头像
            if (App.GetInitParmas.ContainsKey("ftpUrl") && !string.IsNullOrEmpty(App.GetInitParmas["ftpUrl"]))
                avatarFileName = App.GetInitParmas["ftpUrl"] + avatarFileName;
            else
                avatarFileName = url.Replace("/services/UploadAvatar.asmx", avatarFileName);

            //Utils.ShowMessageBox(avatarFileName + "large.jpg?random=" + new Random().Next(1, 10000));
            BitmapImage largeImage = new BitmapImage(new Uri(avatarFileName + "large.jpg?random=" + new Random().Next(1, 10000), UriKind.RelativeOrAbsolute));
            bitmapImageList.Add(largeImage);

            BitmapImage mediumImage = new BitmapImage(new Uri(avatarFileName + "medium.jpg?random=" + new Random().Next(1, 10000), UriKind.RelativeOrAbsolute));
            bitmapImageList.Add(mediumImage);

            BitmapImage smallImage = new BitmapImage(new Uri(avatarFileName + "small.jpg?random=" + new Random().Next(1, 10000), UriKind.RelativeOrAbsolute));
            bitmapImageList.Add(smallImage);

            return bitmapImageList;
        }

        /// <summary>
        /// 格式化Uid为9位标准格式
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static string FormatUid(string uid)
        {
            return uid.PadLeft(9, '0');
        }

        public static string ServiceUrl
        {
            get
            {
                string path = Application.Current.Host.Source.AbsoluteUri;
                path = path.Substring(0, path.IndexOf("/silverlight/Avatar"));
                return path + "/services/UploadAvatar.asmx";

                //发布到discuz!nt时注释这段代码，并使用上面的注释代码
                //return Application.Current.Host.Source.AbsoluteUri.Replace(Application.Current.Host.Source.AbsolutePath, "") + "/UploadAvatar.asmx";
            }
        }

        public static void ShowMessageBox(string context)
        {
            ShowMessageBox(null, context);
        }

        public static void ShowMessageBox(string title, string context)
        {
            CWMessageBox cwMessageBox = new CWMessageBox();
            cwMessageBox.Message.Text = context;

            if (!string.IsNullOrEmpty(title))
                cwMessageBox.Title = title;

            cwMessageBox.Show();
        }

        public static UserFile UploadUserFile(string fileName, FrameworkElement image, FrameworkElement focus, PropertyChangedEventHandler eventHandler)
        {
            FileCollection _files = new FileCollection("1", 1024000);
            WriteableBitmap bmp = new WriteableBitmap(image, null);
            System.IO.Stream dstStream = new System.IO.MemoryStream();
            JpegHelper.EncodeJpeg(bmp, dstStream);
            dstStream.Position = 0;//用于上传时从新读取

            MixObjectsSoapClient.Point point = new MixObjectsSoapClient.Point();
            point.X = (int) Double.Parse(focus.GetValue(Canvas.LeftProperty).ToString());
            point.Y = (int) Double.Parse(focus.GetValue(Canvas.TopProperty).ToString());

            MixObjectsSoapClient.Size size = new MixObjectsSoapClient.Size();
            size.Width = (int)Double.Parse(focus.Width.ToString());
            size.Height = (int)Double.Parse(focus.Height.ToString());

            UserFile imageFile = new UserFile() { FileName = fileName, FileStream = dstStream, GrabPoint = point, GrabSize = size };// ofd.File;           
            imageFile.PropertyChanged += eventHandler;
            _files.Add(imageFile);
            _files.UploadFiles();
            _files.RemoveAt(0);
            return imageFile;
        }
    }
}
