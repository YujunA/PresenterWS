using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml.Linq;

namespace PresenterWS.Extensions.Standard.Elements
{
    [Engine.PwElementInfo("标签","提供一个轻型控件，用于显示少量流内容。","TB_Label","基本")]
    public class Label : Engine.PwElement
    {

        static Label()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Label), new FrameworkPropertyMetadata(typeof(Label)));
        }
        public new System.Drawing.Image ToolboxBitmap { get { return Properties.Resources.TB_Label; } }

        #region Properties
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(Label),new PropertyMetadata(""));
        public TextDecoration TextDecoration
        {
            get { return (TextDecoration)GetValue(TextDecorationProperty); }
            set { SetValue(TextDecorationProperty, value); }
        }
        public static readonly DependencyProperty TextDecorationProperty = DependencyProperty.Register("TextDecoration", typeof(TextDecoration), typeof(Label), new PropertyMetadata(null));
        public double LineHeight
        {
            get { return (double)GetValue(LineHeightProperty); }
            set { SetValue(LineHeightProperty, value); }
        }
        public static readonly DependencyProperty LineHeightProperty = DependencyProperty.Register("LineHeight", typeof(double), typeof(Label), new PropertyMetadata(double.NaN));
        public TextAlignment TextAlignment
        {
            get { return (TextAlignment)GetValue(TextAlignmentProperty); }
            set { SetValue(TextAlignmentProperty, value); }
        }
        public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.Register("TextAlignment", typeof(TextAlignment), typeof(Label), new PropertyMetadata(TextAlignment.Left));
        public TextWrapping TextWrapping
        {
            get { return (TextWrapping)GetValue(TextWrappingProperty); }
            set { SetValue(TextWrappingProperty, value); }
        }
        public static readonly DependencyProperty TextWrappingProperty = DependencyProperty.Register("TextWrapping", typeof(TextWrapping), typeof(Label), new PropertyMetadata(TextWrapping.Wrap));
        public TextTrimming TextTrimming
        {
            get { return (TextTrimming)GetValue(TextTrimmingProperty); }
            set { SetValue(TextTrimmingProperty, value); }
        }
        public static readonly DependencyProperty TextTrimmingProperty = DependencyProperty.Register("TextTrimming", typeof(TextTrimming), typeof(Label), new PropertyMetadata(TextTrimming.None));


        #endregion

        public override XElement Serialize()
        {
            XElement ele = base.Serialize();
            ele.Add
            (
                new XAttribute("Text", this.Text != null ? this.Text : ""),
                new XAttribute("LineHeight", this.LineHeight),
                new XAttribute("TextAlignment", this.TextAlignment),
                new XAttribute("TextWrapping", this.TextWrapping),
                new XAttribute("TextTrimming", this.TextTrimming),
                new XAttribute("Foreground", this.Foreground != null ? XamlWriter.Save(this.Foreground) : ""),
                new XAttribute("FontFamily", this.FontFamily),
                new XAttribute("FontSize", this.FontSize),
                new XAttribute("FontWeight", this.FontWeight),
                new XAttribute("HorizontalContentAlignment", this.HorizontalContentAlignment),
                new XAttribute("VerticalContentAlignment", this.VerticalContentAlignment)
            );

            return ele;
        }
        public override void Deserialize(XElement itemXML, double OffsetX, double OffsetY)
        {
            base.Deserialize(itemXML, OffsetX, OffsetY);

            try
            {
                this.Text = itemXML.Attribute("Text").Value as string;
            }
            catch { }
            try
            {
                this.LineHeight = Double.Parse(itemXML.Attribute("LineHeight").Value, CultureInfo.InvariantCulture);
                this.FontFamily = new FontFamily(itemXML.Attribute("FontFamily").Value);
                this.FontSize = Double.Parse(itemXML.Attribute("FontSize").Value, CultureInfo.InvariantCulture);
            }
            catch { }
            try
            {
                this.HorizontalContentAlignment = (HorizontalAlignment)(Enum.Parse(typeof(HorizontalAlignment), itemXML.Attribute("HorizontalContentAlignment").Value, true));
                this.VerticalContentAlignment = (VerticalAlignment)(Enum.Parse(typeof(VerticalAlignment), itemXML.Attribute("VerticalContentAlignment").Value, true));
                this.TextAlignment = (TextAlignment)(Enum.Parse(typeof(TextAlignment), itemXML.Attribute("TextAlignment").Value, true));
                this.TextWrapping = (TextWrapping)(Enum.Parse(typeof(TextWrapping), itemXML.Attribute("TextWrapping").Value, true));
                this.TextTrimming = (TextTrimming)(Enum.Parse(typeof(TextTrimming), itemXML.Attribute("TextTrimming").Value, true));
            }
            catch { }
            try
            {
                this.Foreground = itemXML.Attribute("Foreground").Value != "" ? XamlReader.Parse(itemXML.Attribute("Foreground").Value) as Brush : null;
                this.FontWeight = (FontWeight)(new FontWeightConverter().ConvertFromString(itemXML.Attribute("FontWeight").Value));
                this.FontStyle = (FontStyle)(new FontStyleConverter().ConvertFromString(itemXML.Attribute("FontStyle").Value));
            }
            catch { }
        }

    }
}
