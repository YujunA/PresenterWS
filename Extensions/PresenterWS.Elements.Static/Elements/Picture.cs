using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace PresenterWS.Extensions.Standard.Elements
{
    [Engine.PwElementInfo("图片", "表示用于显示图像的控件。", "TB_Image", "基本")]
    public class Picture : Engine.PwElement
    {
        public new System.Drawing.Image ToolboxBitmap { get { return Properties.Resources.TB_Image; } }
        static Picture()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Picture), new FrameworkPropertyMetadata(typeof(Picture)));
        }

        #region Properties

        
        public Engine.PwResourceLink Resource
        {
            get { return (Engine.PwResourceLink)GetValue(ResourceProperty); }
            set { SetValue(ResourceProperty, value); this.Source = (value.Resource as ResManager.ImageResouce)?.Source;  }
        }
        public static readonly DependencyProperty ResourceProperty = DependencyProperty.Register("Resource", typeof(Engine.PwResourceLink), typeof(Picture),new PropertyMetadata(Resource_Changed));

        private static void Resource_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is Picture)
            {
                ((Picture)d).Source = (((Engine.PwResourceLink)e.NewValue).Resource as ResManager.ImageResouce)?.Source;
            }
        }

        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            private set { SetValue(SourcePropertyKey, value); }
        }
        private static readonly DependencyPropertyKey SourcePropertyKey = DependencyProperty.RegisterReadOnly("Source", typeof(ImageSource), typeof(Picture),new PropertyMetadata());
        public static readonly DependencyProperty SourceProperty = SourcePropertyKey.DependencyProperty;

        public Stretch Stretch
        {
            get { return (Stretch)GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }
        public static readonly DependencyProperty StretchProperty = DependencyProperty.Register("Stretch", typeof(Stretch), typeof(Picture),new PropertyMetadata(Stretch.Uniform));

        public StretchDirection StretchDirection
        {
            get { return (StretchDirection)GetValue(StretchDirectionProperty); }
            set { SetValue(StretchDirectionProperty, value); }
        }
        public static readonly DependencyProperty StretchDirectionProperty = DependencyProperty.Register("StretchDirection", typeof(StretchDirection), typeof(Picture), new PropertyMetadata(StretchDirection.Both));

        #endregion

        public override XElement Serialize()
        {
            XElement ele = base.Serialize();
            ele.Add
            (
                new XAttribute("Resource", this.Resource),
                new XAttribute("Stretch", this.Stretch),
                new XAttribute("StretchDirection", this.StretchDirection)
            );

            return ele;
        }
        public override void Deserialize(XElement itemXML, double OffsetX, double OffsetY)
        {
            base.Deserialize(itemXML, OffsetX, OffsetY);

            try
            {
                this.Resource = new Engine.PwResourceLink(itemXML.Attribute("Resource").Value);
            }
            catch { }
            try
            {
                this.Stretch = (Stretch)(Enum.Parse(typeof(Stretch), itemXML.Attribute("Stretch").Value, true));
                this.StretchDirection = (StretchDirection)(Enum.Parse(typeof(StretchDirection), itemXML.Attribute("StretchDirection").Value, true));
            }
            catch { }

        }
    }
}
