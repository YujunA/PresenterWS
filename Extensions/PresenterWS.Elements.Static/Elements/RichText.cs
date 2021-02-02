using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Xml.Linq;

namespace PresenterWS.Extensions.Standard.Elements
{
    [Engine.PwElementInfo("富文本", "表示对 FlowDocument 对象进行操作的超文本编辑控件。", "TB_RichTextBox", "基本")]
    [TemplatePart(Name = "PART_RichTextBox", Type = typeof(System.Windows.Controls.RichTextBox))]
    public class RichText : Engine.PwElement
    {
        public System.Windows.Controls.RichTextBox RichTextBox { get; private set; }
        private FlowDocument _cacheDoc = null;

        static RichText()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(RichText), new FrameworkPropertyMetadata(typeof(RichText)));
        }
        public new System.Drawing.Image ToolboxBitmap { get { return Properties.Resources.TB_RichTextBox; } }

        #region Properties
        public FlowDocument Document
        {
            get { return RichTextBox != null ? RichTextBox.Document : null; }
            set
            {
                if (RichTextBox != null)
                    RichTextBox.Document = value;
                else
                    _cacheDoc = value;
            }
        }
        public static readonly DependencyProperty DocumentProperty = DependencyProperty.Register("Document", typeof(FlowDocument), typeof(RichText));
        #endregion
        public override XElement Serialize()
        {
            XElement ele = base.Serialize();
            ele.Add
            (
                new XElement("FlowDocument", System.Windows.Markup.XamlWriter.Save(this.Document))
            );

            return ele;
        }
        public override void Deserialize(XElement itemXML, double OffsetX, double OffsetY)
        {
            base.Deserialize(itemXML, OffsetX, OffsetY);

            try
            {
                this.Document = System.Windows.Markup.XamlReader.Parse(itemXML.Element("FlowDocument").Value) as FlowDocument;
            }
            catch { }
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            try
            {
                RichTextBox = (System.Windows.Controls.RichTextBox)this.Template.FindName("PART_RichTextBox", this);

                if (_cacheDoc != null)
                    RichTextBox.Document = _cacheDoc;
                _cacheDoc = null;
            }
            catch { throw; }

        }

    }
}
