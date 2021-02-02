using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// TextPropDesigner.xaml 的交互逻辑
    /// </summary>
    [Design.PropDesigner.PropertyDesignerInfo(typeof(Label))]
    public partial class Label_PropDesigner : Design.PropDesigner.PropertyDesigner
    {
        bool IsElementLoading = false;
        public Label_PropDesigner()
        {
            InitializeComponent();

            System.Drawing.Text.InstalledFontCollection installedFontCollection = new System.Drawing.Text.InstalledFontCollection();
            foreach (System.Drawing.FontFamily fam in installedFontCollection.Families)
            {
                ComboBox_Font.Items.Add(new FontFamily(fam.Name));
            }

        }

        public void LoadElements()
        {
            IsElementLoading = true;

            foreach(Engine.PwElement ele in DesignerItems)
            {
                if (!(ele is Label))
                    break;

                if(ele==DesignerItems[0])
                {
                    Label i = (Label)ele;
                    TextBox_Content.Text = i.Text;
                    ComboBox_Font.Text = i.FontFamily.Source;
                    ComboBox_Font.SelectedItem = i.FontFamily;
                    TextBox_FontSize.Text = i.FontSize.ToString();
                    BrushDropDown_Foreground.Brush = i.Foreground;
                    ToggleButton_IsBold.IsChecked = (i.FontWeight == FontWeights.Bold);
                    ToggleButton_IsItalic.IsChecked = (i.FontStyle == FontStyles.Italic);

                    TextBox_LineHeight.Text = i.LineHeight.ToString();
                    ComboBox_TextWrapping.SelectedIndex = (int)i.TextWrapping;
                }
                else
                {
                    Label i = (Label)ele;
                    if (i.Text != TextBox_Content.Text)
                        TextBox_Content.Text = "";
                    if (i.FontFamily != ComboBox_Font.SelectedItem)
                        ComboBox_Font.SelectedItem = null;
                    if (i.FontSize.ToString() != TextBox_FontSize.Text)
                        TextBox_FontSize.Text = "";
                    if (i.Foreground != BrushDropDown_Foreground.Brush)
                        BrushDropDown_Foreground.Brush = null;
                    if ((i.FontWeight==FontWeights.Bold) != ToggleButton_IsBold.IsChecked)
                        ToggleButton_IsBold.IsChecked = null;
                    if ((i.FontStyle == FontStyles.Italic) != ToggleButton_IsItalic.IsChecked)
                        ToggleButton_IsItalic.IsChecked = null;
                    if (i.LineHeight.ToString() != TextBox_LineHeight.Text)
                        TextBox_LineHeight.Text = "";
                    if ((int)i.TextWrapping != ComboBox_TextWrapping.SelectedIndex)
                        ComboBox_TextWrapping.SelectedIndex = -1;

                }
            }

            IsElementLoading = false;
        }

        private void PropertyDesigner_DesignerItemsChanged(object sender, EventArgs e)
        {
            LoadElements();
        }

        private void ComboBox_Font_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsElementLoading)
                return;

            foreach (Engine.PwElement ele in DesignerItems)
            {
                if (ele is Label)
                    ((Label)ele).FontFamily = ComboBox_Font.SelectedItem as FontFamily;
            }

        }

        private void ComboBox_Font_TextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void TextBox_FontSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsElementLoading)
                return;

            try
            {
                foreach (Engine.PwElement ele in DesignerItems)
                {
                    if (ele is Label)
                        ((Label)ele).FontSize = Convert.ToDouble(TextBox_FontSize.Text);
                }

            }
            catch
            {
                e.Handled = true;
            }
        }

        private void ComboBox_Foreground_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ToggleButton_IsBold_Click(object sender, RoutedEventArgs e)
        {
            if (IsElementLoading)
                return;

            foreach (Engine.PwElement ele in DesignerItems)
            {
                if (ele is Label)
                    ((Label)ele).FontWeight = ToggleButton_IsBold.IsChecked.GetValueOrDefault(false) ? FontWeights.Bold : FontWeights.Normal;
            }

        }

        private void ToggleButton_IsItalic_Click(object sender, RoutedEventArgs e)
        {
            if (IsElementLoading)
                return;

            foreach (Engine.PwElement ele in DesignerItems)
            {
                if (ele is Label)
                    ((Label)ele).FontStyle = ToggleButton_IsItalic.IsChecked.GetValueOrDefault(false) ? FontStyles.Italic : FontStyles.Normal;
            }

        }

        private void ToggleButton_HasOverline_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ToggleButton_HasStrickThrough_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ToggleButton_HasBaseline_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ToggleButton_HasUnderline_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_LineHeight_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsElementLoading)
                return;

            try
            {
                foreach (Engine.PwElement ele in DesignerItems)
                {
                    if (ele is Label)
                        ((Label)ele).LineHeight = Convert.ToDouble(TextBox_LineHeight.Text);
                }
            }
            catch
            {
                e.Handled = true;
            }
        }

        private void ToggleButton_AlignLeft_Click(object sender, RoutedEventArgs e)
        {
            if (IsElementLoading)
                return;

            ToggleButton_AlignLeft.IsChecked = true;
            ToggleButton_AlignHCenter.IsChecked = false;
            ToggleButton_AlignRight.IsChecked = false;
            ToggleButton_AlignJustify.IsChecked = false;

            try
            {
                foreach (Engine.PwElement ele in DesignerItems)
                {
                    if (ele is Label)
                    {
                        ((Label)ele).HorizontalContentAlignment = HorizontalAlignment.Left;
                        ((Label)ele).TextAlignment = TextAlignment.Left;
                    }
                }
            }
            catch
            {
                e.Handled = true;
            }
        }

        private void ToggleButton_AlignHCenter_Click(object sender, RoutedEventArgs e)
        {
            if (IsElementLoading)
                return;

            ToggleButton_AlignLeft.IsChecked = false;
            ToggleButton_AlignHCenter.IsChecked = true;
            ToggleButton_AlignRight.IsChecked = false;
            ToggleButton_AlignJustify.IsChecked = false;

            try
            {
                foreach (Engine.PwElement ele in DesignerItems)
                {
                    if (ele is Label)
                    {
                        ((Label)ele).HorizontalContentAlignment = HorizontalAlignment.Center;
                        ((Label)ele).TextAlignment = TextAlignment.Center;
                    }
                }
            }
            catch
            {
                e.Handled = true;
            }

        }

        private void ToggleButton_AlignRight_Click(object sender, RoutedEventArgs e)
        {
            if (IsElementLoading)
                return;

            ToggleButton_AlignLeft.IsChecked = false;
            ToggleButton_AlignHCenter.IsChecked = false;
            ToggleButton_AlignRight.IsChecked = true;
            ToggleButton_AlignJustify.IsChecked = false;

            try
            {
                foreach (Engine.PwElement ele in DesignerItems)
                {
                    if (ele is Label)
                    {
                        ((Label)ele).HorizontalContentAlignment = HorizontalAlignment.Right;
                        ((Label)ele).TextAlignment = TextAlignment.Right;
                    }
                }
            }
            catch
            {
                e.Handled = true;
            }

        }

        private void ToggleButton_AlignTop_Click(object sender, RoutedEventArgs e)
        {
            if (IsElementLoading)
                return;

            ToggleButton_AlignTop.IsChecked = true;
            ToggleButton_AlignVCenter.IsChecked = false;
            ToggleButton_AlignBottom.IsChecked = false;

            try
            {
                foreach (Engine.PwElement ele in DesignerItems)
                {
                    if (ele is Label)
                    {
                        ((Label)ele).VerticalContentAlignment = VerticalAlignment.Top;
                    }
                }
            }
            catch
            {
                e.Handled = true;
            }

        }

        private void ToggleButton_AlignVCenter_Click(object sender, RoutedEventArgs e)
        {
            if (IsElementLoading)
                return;

            ToggleButton_AlignTop.IsChecked = false;
            ToggleButton_AlignVCenter.IsChecked = true;
            ToggleButton_AlignBottom.IsChecked = false;

            try
            {
                foreach (Engine.PwElement ele in DesignerItems)
                {
                    if (ele is Label)
                    {
                        ((Label)ele).VerticalContentAlignment = VerticalAlignment.Center;
                    }
                }
            }
            catch
            {
                e.Handled = true;
            }

        }

        private void ToggleButton_AlignBottom_Click(object sender, RoutedEventArgs e)
        {
            if (IsElementLoading)
                return;

            ToggleButton_AlignTop.IsChecked = false;
            ToggleButton_AlignVCenter.IsChecked = false;
            ToggleButton_AlignBottom.IsChecked = true;

            try
            {
                foreach (Engine.PwElement ele in DesignerItems)
                {
                    if (ele is Label)
                    {
                        ((Label)ele).VerticalContentAlignment = VerticalAlignment.Bottom;
                    }
                }
            }
            catch
            {
                e.Handled = true;
            }

        }

        private void ComboBox_TextWrapping_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsElementLoading)
                return;

            foreach (Engine.PwElement ele in DesignerItems)
            {
                if (ele is Label)
                    ((Label)ele).TextWrapping = (TextWrapping)ComboBox_TextWrapping.SelectedIndex;
            }

        }

        private void ComboBox_TextTrimming_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsElementLoading)
                return;

            foreach (Engine.PwElement ele in DesignerItems)
            {
                if (ele is Label)
                    ((Label)ele).TextTrimming = (TextTrimming)ComboBox_TextTrimming.SelectedIndex;
            }

        }

        private void TextBox_Content_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsElementLoading)
                return;

            foreach(Engine.PwElement ele in DesignerItems)
            {
                if (ele is Label)
                    ((Label)ele).Text = TextBox_Content.Text;
            }
        }

        private void BrushDropDown_Foreground_BrushChanged(object sender, EventArgs e)
        {
            if (IsElementLoading)
                return;

            foreach (Engine.PwElement ele in DesignerItems)
            {
                if (ele is Label)
                    ((Label)ele).Foreground = BrushDropDown_Foreground.Brush;
            }

        }

        private void ToggleButton_AlignJustify_Click(object sender, RoutedEventArgs e)
        {
            if (IsElementLoading)
                return;

            ToggleButton_AlignLeft.IsChecked = false;
            ToggleButton_AlignHCenter.IsChecked = false;
            ToggleButton_AlignRight.IsChecked = false;
            ToggleButton_AlignJustify.IsChecked = true;

            try
            {
                foreach (Engine.PwElement ele in DesignerItems)
                {
                    if (ele is Label)
                    {
                        ((Label)ele).HorizontalContentAlignment = HorizontalAlignment.Stretch;
                        ((Label)ele).TextAlignment = TextAlignment.Justify;
                    }
                }
            }
            catch
            {
                e.Handled = true;
            }

        }
    }
}
