﻿#pragma checksum "D:\dnt\3\Discuz.Silverlight\Discuz.WebCam\HaoRan.WebCam\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "B2ECE5A56B9C83F8859F2D0A4C16F8C3"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.21006.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using HaoRan.WebCam;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace HaoRan.WebCam {
    
    
    public partial class MainPage : System.Windows.Controls.Page {
        
        internal System.Windows.Controls.Canvas LayoutRoot;
        
        internal System.Windows.Controls.Image Viewport;
        
        internal HaoRan.WebCam.ImageButton selectImage;
        
        internal HaoRan.WebCam.ImageButton webCam;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/HaoRan.WebCam;component/MainPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Canvas)(this.FindName("LayoutRoot")));
            this.Viewport = ((System.Windows.Controls.Image)(this.FindName("Viewport")));
            this.selectImage = ((HaoRan.WebCam.ImageButton)(this.FindName("selectImage")));
            this.webCam = ((HaoRan.WebCam.ImageButton)(this.FindName("webCam")));
        }
    }
}
