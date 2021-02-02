using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
using Forms = System.Windows.Forms;

namespace PresenterWS.Extensions.Standard.Elements
{
    /// <summary>
    /// RichText_pdContent.xaml 的交互逻辑
    /// </summary>
    [Design.PropDesigner.PropertyDesignerInfo(typeof(RichText))]
    public partial class RichText_pdDocument : Design.PropDesigner.PropertyDesigner
    {
        public RichText_pdDocument()
        {
            InitializeComponent();
            System.Drawing.Text.InstalledFontCollection installedFontCollection = new System.Drawing.Text.InstalledFontCollection();
            foreach (System.Drawing.FontFamily fam in installedFontCollection.Families)
            {
                ComboBox_FontFamily.Items.Add(new FontFamily(fam.Name));
            }

        }
        #region RTF Writer
        #region Variablen

        //private bool dataChanged = false; // ungespeicherte Textänderungen     

        private string privateText = null; // Inhalt der RTFBox im txt-Format
        public string text
        {
            get
            {
                TextRange range = new TextRange(RichTextControl.Document.ContentStart, RichTextControl.Document.ContentEnd);
                return range.Text;
            }
            set
            {
                privateText = value;
            }
        }

        private string zeilenangabe; // aktuelle Zeile der Cursorposition
        private int privAktZeile = 1;
        public int aktZeile
        {
            get { return privAktZeile; }
            set
            {
                privAktZeile = value;
                zeilenangabe = "Ze: " + value;
                LabelZeileNr.Content = zeilenangabe;
            }
        }

        private string spaltenangabe; // aktuelle Spalte der Cursorposition
        private int privAktSpalte = 1;
        public int aktSpalte
        {
            get { return privAktSpalte; }
            set
            {
                privAktSpalte = value;
                spaltenangabe = "Sp: " + value;
                LabelSpalteNr.Content = spaltenangabe;
            }
        }

        #endregion Variablen     

        #region ControlHandler

        //
        // Nach dem Laden des Control
        //
        private void RTFEditor_Loaded(object sender, RoutedEventArgs e)
        {
            // Schrifttypen- und -größen-Initialisierung
            TextRange range = new TextRange(RichTextControl.Selection.Start, RichTextControl.Selection.End);
            ComboBox_FontFamily.SelectedValue = range.GetPropertyValue(FlowDocument.FontFamilyProperty).ToString();
            ComboBox_FontSize.Text = range.GetPropertyValue(FlowDocument.FontSizeProperty).ToString();

            // aktuelle Zeilen- und Spaltenpositionen angeben
            aktZeile = Zeilennummer();
            aktSpalte = Spaltennummer();

            RichTextControl.Focus();
        }


        #endregion ControlHandler

        #region private ToolBarHandler

        //
        // ToolStripButton Open wurde gedrückt
        //
        private void ToolStripButtonOpen_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                SetRTF("");


                // Configure open file dialog box
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.FileName = ""; // Default file name
                dlg.DefaultExt = ".rtf"; // Default file extension
                dlg.Filter = "RichText documents (.rtf)|*.rtf|Text documents (.txt)|*.txt"; // Filter files by extension

                // Show open file dialog box
                Nullable<bool> result = dlg.ShowDialog();

                // Process open file dialog box results
                if (result == true)
                {
                    string path = dlg.FileName;

                    // Open document
                    TextRange range;
                    FileStream fStream;

                    if (File.Exists(path))
                    {
                        range = new TextRange(RichTextControl.Document.ContentStart, RichTextControl.Document.ContentEnd);
                        fStream = new FileStream(path, FileMode.OpenOrCreate);
                        range.Load(fStream, DataFormats.Rtf);
                        fStream.Close();
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message + "\r\n\r\n" + ex.StackTrace, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        //
        // ToolStripButton Print wurde gedrückt
        //
        private void ToolStripButtonPrint_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                // Configure printer dialog box
                PrintDialog dlg = new PrintDialog();
                dlg.PageRangeSelection = PageRangeSelection.AllPages;
                dlg.UserPageRangeEnabled = true;

                // Show and process save file dialog box results
                if (dlg.ShowDialog() == true)
                {
                    //use either one of the below    
                    // dlg.PrintVisual(RichTextControl as Visual, "printing as visual");
                    dlg.PrintDocument((((IDocumentPaginatorSource)RichTextControl.Document).DocumentPaginator), "printing as paginator");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message + "\r\n\r\n" + ex.StackTrace, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error); }

        }

        //
        // ToolStripButton Strikeout wurde gedrückt
        // (läßt sich nicht durch Command in XAML abarbeiten)
        //
        private void ToolStripButtonStrikeout_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                // TODO: Ereignishandlerimplementierung hier einfügen.
                TextRange range = new TextRange(RichTextControl.Selection.Start, RichTextControl.Selection.End);

                TextDecorationCollection tdc = (TextDecorationCollection)RichTextControl.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
                if (tdc == null || !tdc.Equals(TextDecorations.Strikethrough))
                {
                    tdc = TextDecorations.Strikethrough;

                }
                else
                {
                    tdc = new TextDecorationCollection();
                }
                range.ApplyPropertyValue(Inline.TextDecorationsProperty, tdc);

            }
            catch (Exception ex) { MessageBox.Show(ex.Message + "\r\n\r\n" + ex.StackTrace, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error); }

        }

        //
        // ToolStripButton Textcolor wurde gedrückt
        //
        private void ToolStripButtonTextcolor_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Forms.ColorDialog colorDialog = new Forms.ColorDialog();
                //colorDialog.Owner = this;
                if (colorDialog.ShowDialog() == Forms.DialogResult.OK)
                {
                    TextRange range = new TextRange(RichTextControl.Selection.Start, RichTextControl.Selection.End);

                    range.ApplyPropertyValue(FlowDocument.ForegroundProperty, new SolidColorBrush(Color.FromArgb(colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B)));
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message + "\r\n\r\n" + ex.StackTrace, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error); }

        }

        //
        // ToolStripButton Backgroundcolor wurde gedrückt
        //
        private void ToolStripButtonBackcolor_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Forms.ColorDialog colorDialog = new Forms.ColorDialog();
                //colorDialog.Owner = this;
                if (colorDialog.ShowDialog() == Forms.DialogResult.OK)
                {
                    TextRange range = new TextRange(RichTextControl.Selection.Start, RichTextControl.Selection.End);

                    range.ApplyPropertyValue(FlowDocument.BackgroundProperty, new SolidColorBrush(Color.FromArgb(colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B)));
                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message + "\r\n\r\n" + ex.StackTrace, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error); }

        }

        //
        // ToolStripButton Subscript wurde gedrückt
        // (läßt sich auch durch Command in XAML machen. 
        // Damit dann aber ein wirkliches Subscript angezeigt wird, muß die benutzte Font OpenType sein:
        // http://msdn.microsoft.com/en-us/library/ms745109.aspx#variants
        // Um auch für alle anderen Schriftarten Subscript zu realisieren, läßt sich stattdessen die Baseline Eigenschaft verändern)
        //
        private void ToolStripButtonSubscript_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var currentAlignment = RichTextControl.Selection.GetPropertyValue(Inline.BaselineAlignmentProperty);

                BaselineAlignment newAlignment = ((BaselineAlignment)currentAlignment == BaselineAlignment.Subscript) ? BaselineAlignment.Baseline : BaselineAlignment.Subscript;
                RichTextControl.Selection.ApplyPropertyValue(Inline.BaselineAlignmentProperty, newAlignment);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message + "\r\n\r\n" + ex.StackTrace, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error); }

        }

        //
        // ToolStripButton Superscript wurde gedrückt
        // (läßt sich auch durch Command in XAML machen. 
        // Damit dann aber ein wirkliches Superscript angezeigt wird, muß die benutzte Font OpenType sein:
        // http://msdn.microsoft.com/en-us/library/ms745109.aspx#variants
        // Um auch für alle anderen Schriftarten Superscript zu realisieren, läßt sich stattdessen die Baseline Eigenschaft verändern)
        //
        private void ToolStripButtonSuperscript_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var currentAlignment = RichTextControl.Selection.GetPropertyValue(Inline.BaselineAlignmentProperty);

                BaselineAlignment newAlignment = ((BaselineAlignment)currentAlignment == BaselineAlignment.Superscript) ? BaselineAlignment.Baseline : BaselineAlignment.Superscript;
                RichTextControl.Selection.ApplyPropertyValue(Inline.BaselineAlignmentProperty, newAlignment);

            }
            catch (Exception ex) { MessageBox.Show(ex.Message + "\r\n\r\n" + ex.StackTrace, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error); }

        }

        //
        // Textgröße wurde vom Benutzer verändert
        //
        private void ComboBox_FontSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (ComboBox_FontSize.Text == "")
                    return;

                string fontHeight = (string)ComboBox_FontSize.Text;

                if (fontHeight != null)
                {
                    RichTextControl.Selection.ApplyPropertyValue(System.Windows.Controls.RichTextBox.FontSizeProperty, fontHeight);
                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message + "\r\n\r\n" + ex.StackTrace, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error); }

        }

        //
        // andere Font wurde vom Benutzer ausgewählt
        //
        private void ComboBox_FontFamily_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (ComboBox_FontFamily.Text == "")
                    return;


                string fontName = ComboBox_FontFamily.Text;

                if (fontName != null)
                {
                    RichTextControl.Selection.ApplyPropertyValue(System.Windows.Controls.RichTextBox.FontFamilyProperty, new FontFamily(fontName));
                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message + "\r\n\r\n" + ex.StackTrace, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        //
        // Alignmentstatus anpassen
        //
        private void ToolStripButtonAlignLeft_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ToolStripButtonAlignLeft.IsChecked == true)
                {
                    ToolStripButtonAlignCenter.IsChecked = false;
                    ToolStripButtonAlignRight.IsChecked = false;
                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message + "\r\n\r\n" + ex.StackTrace, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error); }

        }

        //
        // Alignmentstatus anpassen
        //
        private void ToolStripButtonAlignCenter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ToolStripButtonAlignCenter.IsChecked == true)
                {
                    ToolStripButtonAlignLeft.IsChecked = false;
                    ToolStripButtonAlignRight.IsChecked = false;
                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message + "\r\n\r\n" + ex.StackTrace, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error); }

        }

        //
        // Alignmentstatus anpassen
        //
        private void ToolStripButtonAlignRight_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ToolStripButtonAlignRight.IsChecked == true)
                {
                    ToolStripButtonAlignCenter.IsChecked = false;
                    ToolStripButtonAlignLeft.IsChecked = false;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message + "\r\n\r\n" + ex.StackTrace, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error); }

        }

        #endregion private ToolBarHandler

        #region private RichTextBoxHandler

        //
        // Formatierungen des markierten Textes anzeigen
        //
        private void RichTextControl_SelectionChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                // markierten Text holen
                TextRange selectionRange = new TextRange(RichTextControl.Selection.Start, RichTextControl.Selection.End);


                if (selectionRange.GetPropertyValue(FontWeightProperty).ToString() == "Bold")
                {
                    ToolStripButtonBold.IsChecked = true;
                }
                else
                {
                    ToolStripButtonBold.IsChecked = false;
                }

                if (selectionRange.GetPropertyValue(FontStyleProperty).ToString() == "Italic")
                {
                    ToolStripButtonItalic.IsChecked = true;
                }
                else
                {
                    ToolStripButtonItalic.IsChecked = false;
                }

                if (selectionRange.GetPropertyValue(Inline.TextDecorationsProperty) == TextDecorations.Underline)
                {
                    ToolStripButtonUnderline.IsChecked = true;
                }
                else
                {
                    ToolStripButtonUnderline.IsChecked = false;
                }

                if (selectionRange.GetPropertyValue(Inline.TextDecorationsProperty) == TextDecorations.Strikethrough)
                {
                    ToolStripButtonStrikeout.IsChecked = true;
                }
                else
                {
                    ToolStripButtonStrikeout.IsChecked = false;
                }

                if (selectionRange.GetPropertyValue(FlowDocument.TextAlignmentProperty).ToString() == "Left")
                {
                    ToolStripButtonAlignLeft.IsChecked = true;
                }

                if (selectionRange.GetPropertyValue(FlowDocument.TextAlignmentProperty).ToString() == "Center")
                {
                    ToolStripButtonAlignCenter.IsChecked = true;
                }

                if (selectionRange.GetPropertyValue(FlowDocument.TextAlignmentProperty).ToString() == "Right")
                {
                    ToolStripButtonAlignRight.IsChecked = true;
                }

                // Sub-, Superscript Buttons setzen
                try
                {
                    switch ((BaselineAlignment)selectionRange.GetPropertyValue(Inline.BaselineAlignmentProperty))
                    {
                        case BaselineAlignment.Subscript:
                            ToolStripButtonSubscript.IsChecked = true;
                            ToolStripButtonSuperscript.IsChecked = false;
                            break;

                        case BaselineAlignment.Superscript:
                            ToolStripButtonSubscript.IsChecked = false;
                            ToolStripButtonSuperscript.IsChecked = true;
                            break;

                        default:
                            ToolStripButtonSubscript.IsChecked = false;
                            ToolStripButtonSuperscript.IsChecked = false;
                            break;
                    }
                }
                catch (Exception)
                {
                    ToolStripButtonSubscript.IsChecked = false;
                    ToolStripButtonSuperscript.IsChecked = false;
                }

                // Get selected font and height and update selection in ComboBoxes
                ComboBox_FontFamily.Text = selectionRange.GetPropertyValue(FlowDocument.FontFamilyProperty) != DependencyProperty.UnsetValue ? selectionRange.GetPropertyValue(FlowDocument.FontFamilyProperty).ToString() : "";
                ComboBox_FontSize.Text = selectionRange.GetPropertyValue(FlowDocument.FontSizeProperty) != DependencyProperty.UnsetValue ? selectionRange.GetPropertyValue(FlowDocument.FontSizeProperty).ToString() : "";
                ComboBox_LineHeight.Text = selectionRange.GetPropertyValue(FlowDocument.LineHeightProperty) != DependencyProperty.UnsetValue ? selectionRange.GetPropertyValue(FlowDocument.LineHeightProperty).ToString() : "";

                // Ausgabe der Zeilennummer
                aktZeile = Zeilennummer();

                // Ausgabe der Spaltennummer
                aktSpalte = Spaltennummer();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message + "\r\n\r\n" + ex.StackTrace, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error); }

        }

        //
        // wurden Textänderungen gemacht
        //
        private void RichTextControl_TextChanged(object sender, TextChangedEventArgs e)
        {
            //dataChanged = true;
            if (!IsElementLoading)
            {
                foreach (Engine.PwElement ele in DesignerItems)
                {
                    if (ele is RichText)
                        ((RichText)ele).Document = System.Windows.Markup.XamlReader.Parse(System.Windows.Markup.XamlWriter.Save(RichTextControl.Document)) as FlowDocument;
                }
            }

        }

        //
        // Tastendruck erzeugt ein neues Zeichen in der gewählten Font
        //
        private void RichTextControl_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //dataChanged = true;

                string fontName = string.IsNullOrEmpty(ComboBox_FontFamily.Text) ? FontFamily.Source : ComboBox_FontFamily.Text;
                string fontHeight = ComboBox_FontSize.Text;
                TextRange range = new TextRange(RichTextControl.Selection.Start, RichTextControl.Selection.End);

                range.ApplyPropertyValue(TextElement.FontFamilyProperty, fontName);
                range.ApplyPropertyValue(TextElement.FontSizeProperty, fontHeight);

            }
            catch (Exception ex) { MessageBox.Show(ex.Message + "\r\n\r\n" + ex.StackTrace, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error); }
        }
        private void PropertyDesigner_Loaded(object sender, RoutedEventArgs e)
        {
            // aktuelle Zeilen- und Spaltenpositionen angeben
            aktZeile = Zeilennummer();
            aktSpalte = Spaltennummer();

        }

        private void Thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            UIElement thumb = e.Source as UIElement;

            //    防止Thumb控件被拖出容器。  
            //    if (nTop <= 0)
            //        nTop = 0;
            //    if (nTop >= (g.Height - myThumb.Height))
            //        nTop = g.Height - myThumb.Height;
            //    if (nLeft <= 0)
            //        nLeft = 0;
            //    if (nLeft >= (g.Width - myThumb.Width))
            //        nLeft = g.Width - myThumb.Width;
            //    Canvas.SetTop(myThumb, nTop);
            //    Canvas.SetLeft(myThumb, nLeft);
            //    tt.Text = "Top:" + nTop.ToString() + "\nLeft:" + nLeft.ToString();


            dplDocEditor.Height = Math.Max(dplDocEditor.ActualHeight + e.VerticalChange, 0);
        }
        private void ComboBox_FontFamily_DropDownClosed(object sender, EventArgs e)
        {
            RichTextControl.Focus();
        }

        private void ComboBox_FontSize_DropDownClosed(object sender, EventArgs e)
        {
            RichTextControl.Focus();
        }
        //
        // Tastenkombinationen auswerten
        //
        private void RichTextControl_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                // Ctrl + B
                if ((Keyboard.Modifiers == ModifierKeys.Control) && (e.Key == Key.B))
                {
                    if (ToolStripButtonBold.IsChecked == true)
                    {
                        ToolStripButtonBold.IsChecked = false;
                    }
                    else
                    {
                        ToolStripButtonBold.IsChecked = true;
                    }
                }

                // Ctrl + I
                if ((Keyboard.Modifiers == ModifierKeys.Control) && (e.Key == Key.I))
                {
                    if (ToolStripButtonItalic.IsChecked == true)
                    {
                        ToolStripButtonItalic.IsChecked = false;
                    }
                    else
                    {
                        ToolStripButtonItalic.IsChecked = true;
                    }
                }

                // Ctrl + U
                if ((Keyboard.Modifiers == ModifierKeys.Control) && (e.Key == Key.U))
                {
                    if (ToolStripButtonUnderline.IsChecked == true)
                    {
                        ToolStripButtonUnderline.IsChecked = false;
                    }
                    else
                    {
                        ToolStripButtonUnderline.IsChecked = true;
                    }
                }

                // Ctrl + O
                if ((Keyboard.Modifiers == ModifierKeys.Control) && (e.Key == Key.O))
                {
                    ToolStripButtonOpen_Click(sender, e);
                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message + "\r\n\r\n" + ex.StackTrace, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error); }

        }

        #endregion private RichTextBoxHandler

        #region private Funktionen

        //
        // Gibt die Zeilennummer der aktuellen Cursorposition zurück
        //
        private int Zeilennummer()
        {
            TextPointer caretLineStart = RichTextControl.CaretPosition.GetLineStartPosition(0);
            TextPointer p = RichTextControl.Document.ContentStart.GetLineStartPosition(0);
            int currentLineNumber = 1;

            // Vom Anfang des RTF-Box Inhaltes wird vorwärts solange weitergezählt, bis die aktuelle Cursorposition erreicht ist.
            while (true)
            {
                if (caretLineStart.CompareTo(p) < 0)
                {
                    break;
                }
                int result;
                p = p.GetLineStartPosition(1, out result);
                if (result == 0)
                {
                    break;
                }
                currentLineNumber++;
            }
            return currentLineNumber;
        }

        //
        // Gibt die Spaltennummer der aktuellen Cursorposition zurück
        private int Spaltennummer()
        {
            TextPointer caretPos = RichTextControl.CaretPosition;
            TextPointer p = RichTextControl.CaretPosition.GetLineStartPosition(0);
            int currentColumnNumber = Math.Max(p.GetOffsetToPosition(caretPos) - 1, 0);

            return currentColumnNumber;
        }

        #endregion private Funktionen

        #region öffentliche Funktionen

        //
        // Alle Daten löschen
        //
        public void Clear()
        {
            //dataChanged = false;
            RichTextControl.Document.Blocks.Clear();
        }

        //
        // Inhalt der RichTextBox als RTF setzen
        //
        public void SetRTF(string rtf)
        {
            TextRange range = new TextRange(RichTextControl.Document.ContentStart, RichTextControl.Document.ContentEnd);

            // Exception abfangen für StreamReader und MemoryStream, ArgumentException abfangen für range.Load bei rtf=null oder rtf=""          
            try
            {
                // um die Load Methode eines TextRange Objektes zu benutzen wird ein MemoryStream Objekt erzeugt
                using (MemoryStream rtfMemoryStream = new MemoryStream())
                {
                    using (StreamWriter rtfStreamWriter = new StreamWriter(rtfMemoryStream))
                    {
                        rtfStreamWriter.Write(rtf);
                        rtfStreamWriter.Flush();
                        rtfMemoryStream.Seek(0, SeekOrigin.Begin);

                        range.Load(rtfMemoryStream, DataFormats.Rtf);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //
        // RTF Inhalt der RichTextBox als RTF-String zurückgeben
        //
        public string GetRTF()
        {
            TextRange range = new TextRange(RichTextControl.Document.ContentStart, RichTextControl.Document.ContentEnd);

            // Exception abfangen für StreamReader und MemoryStream
            try
            {
                // um die Load Methode eines TextRange Objektes zu benutzen wird ein MemoryStream Objekt erzeugt
                using (MemoryStream rtfMemoryStream = new MemoryStream())
                {
                    using (StreamWriter rtfStreamWriter = new StreamWriter(rtfMemoryStream))
                    {
                        range.Save(rtfMemoryStream, DataFormats.Rtf);

                        rtfMemoryStream.Flush();
                        rtfMemoryStream.Position = 0;
                        StreamReader sr = new StreamReader(rtfMemoryStream);
                        return sr.ReadToEnd();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion öffentliche Funktionen

        #region TODO

        //private float zoomFaktor = 1.0F; // Zoom Faktor der RTFBox

        // für das Scrollen bei Drag&Drop Operationen
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        //private const long WM_USER = &H400;
        private const long WM_USER = 1024;

        private void SliderZoom_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //SendMessage(hwnd, EM_SETZOOM, 10, 10);

            //int result = SendMessage(new System.Windows.Interop.WindowInteropHelper(Application.Current.MainWindow).Handle, (int)WM_USER + 255, (int)e.NewValue * 10, 10);

        }


        // In der StatusBar Ausgabe von Grossschreibweise, Num Lock, Zoom

        #endregion TODO

        #endregion

        private void ComboBox_LineHeight_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (ComboBox_LineHeight.Text == "")
                    return;


                string lineHeight = ComboBox_LineHeight.Text;

                if (lineHeight != null)
                {
                    RichTextControl.Selection.ApplyPropertyValue(FlowDocument.LineHeightProperty, lineHeight);
                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message + "\r\n\r\n" + ex.StackTrace, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error); }

        }
        bool IsElementLoading = false;
        private void PropertyDesigner_DesignerItemsChanged(object sender, EventArgs e)
        {
            LoadElements();
        }
        public void LoadElements()
        {
            IsElementLoading = true;

            foreach (Engine.PwElement ele in DesignerItems)
            {
                if (!(ele is RichText))
                    break;

                if (ele == DesignerItems[0])
                {
                    RichText i = (RichText)ele;
                    RichTextControl.Document = System.Windows.Markup.XamlReader.Parse(System.Windows.Markup.XamlWriter.Save(i.Document)) as FlowDocument;

                }
                else
                {
                    RichText i = (RichText)ele;
                    if (System.Windows.Markup.XamlWriter.Save(i.Document) != System.Windows.Markup.XamlWriter.Save(RichTextControl.Document))
                        RichTextControl.Document = null;

                }
            }

            IsElementLoading = false;
        }

    }
}
