using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PresenterWS.Extensions.Standard.ResManager
{
    /// <summary>
    /// ImageResource.xaml 的交互逻辑
    /// </summary>
    public partial class ImageResource_PreviewHandler : Engine.ResPreviewHandle
    {
        public override Type TargetType { get { return typeof(ImageResouce); } }
        public ImageResource_PreviewHandler()
        {
            InitializeComponent();
            foreach(BitmapEncodingMode s in Enum.GetValues(typeof(BitmapEncodingMode)))
            {
                cmbEncoding.Items.Add(s);
            }
        }

        private void ResPreviewHandler_PreviewResourceChanged(object sender, EventArgs e)
        {
            if(preview.PreviewResource is ImageResouce)
            {
                imagePreview.Source = ((ImageResouce)preview.PreviewResource).Source;
                txbSize.ToolTip = txbSize.Text = imagePreview.Source.Width.ToString() + " * " + imagePreview.Source.Height.ToString();
                txbLength.Text = imagePreview.Source is BitmapImage ? Engine.PwResource.ShowFileSize(((BitmapImage)imagePreview.Source).StreamSource.Length,false) : "";
                txbLength.ToolTip = imagePreview.Source is BitmapImage ? Engine.PwResource.ShowFileSize(((BitmapImage)imagePreview.Source).StreamSource.Length, true) : "";
                cmbEncoding.SelectedItem = ((ImageResouce)preview.PreviewResource).EncodingMode;
            }
        }

        private void cmbEncoding_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ImageResouce)preview.PreviewResource).EncodingMode != ((BitmapEncodingMode)cmbEncoding.SelectedItem))
                ((ImageResouce)preview.PreviewResource).EncodingMode = ((BitmapEncodingMode)cmbEncoding.SelectedItem);
        }
    }
}
