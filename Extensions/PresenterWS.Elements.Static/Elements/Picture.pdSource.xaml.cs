using PresenterWS.Design.PropDesigner;
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

namespace PresenterWS.Extensions.Standard.Elements
{
    /// <summary>
    /// Picture_pdSource.xaml 的交互逻辑
    /// </summary>
   [PropertyDesignerInfo(typeof(Picture))]
    public partial class Picture_pdSource : PropertyDesigner
    {
        bool IsElementLoading = false;

        public Picture_pdSource()
        {
            InitializeComponent();
        }

        private void ResourceSelector_ResourceLinkChanged(object sender, EventArgs e)
        {
            if (IsElementLoading)
                return;
            foreach (Engine.PwElement ele in DesignerItems)
            {
                if (!(ele is Picture))
                    break;

                Picture i = (Picture)ele;
                i.Resource = resourceSelector.ResourceLink;
            }
        }
        private void PropertyDesigner_DesignerItemsChanged(object sender, EventArgs e)
        {
            LoadElements();
        }
        public void LoadElements()
        {
            IsElementLoading = true;

            foreach (Engine.PwElement ele in DesignerItems)
            {
                if (!(ele is Picture))
                    break;

                if (ele == DesignerItems[0])
                {
                    Picture i = (Picture)ele;
                    resourceSelector.ResourceLink = i.Resource;
                    cmbStretch.SelectedIndex = (int)i.Stretch;
                    cmbStretchDirection.SelectedIndex = (int)i.StretchDirection;
                }
                else
                {
                    Picture i = (Picture)ele;
                    if(!resourceSelector.ResourceLink.Equals(i.Resource))
                        resourceSelector.ResourceLink = new Engine.PwResourceLink("", Engine.ResourceKind.Resource);
                    if (cmbStretch.SelectedIndex != (int)i.Stretch)
                        cmbStretch.SelectedIndex = -1;
                    if (cmbStretchDirection.SelectedIndex != (int)i.StretchDirection)
                        cmbStretchDirection.SelectedIndex = -1;

                }
            }

            IsElementLoading = false;
        }

        private void cmdStretch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsElementLoading)
                return;
            foreach (Engine.PwElement ele in DesignerItems)
            {
                if (!(ele is Picture))
                    break;

                Picture i = (Picture)ele;
                i.Stretch = (Stretch)cmbStretch.SelectedIndex;
            }

        }

        private void cmbStretchDirection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsElementLoading)
                return;
            foreach (Engine.PwElement ele in DesignerItems)
            {
                if (!(ele is Picture))
                    break;

                Picture i = (Picture)ele;
                i.StretchDirection = (StretchDirection)cmbStretchDirection.SelectedIndex;
            }

        }
    }
}
