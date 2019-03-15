using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace iHuaban.Core.Controls
{
    public class ExtendedGridView : GridView
    {
        public ExtendedGridView()
        {
            this.SizeChanged += ExtendedGridView_SizeChanged;
        }

        private void ExtendedGridView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                List<double> scaleSize = null;

                if (!string.IsNullOrEmpty(this.ScaleSize))
                {
                    scaleSize = this.ScaleSize.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries).Select(o => double.Parse(o)).ToList();
                }

                double width = e.NewSize.Width - this.Padding.Left - this.Padding.Right;

                double col = Math.Floor(width / (this.CellMinWidth));

                //if (col <= 1)
                //    col = 2;

                //double w = 13;
                //if (ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
                //    w = 6.1;

                CellWidth = (double)Math.Floor((width) / col) - this.Padding.Left - this.Padding.Right;
                if (scaleSize?.Count == 2)
                {
                    CellHeight = CellWidth * scaleSize[1] / scaleSize[0];
                }
            }
            catch (Exception ex)
            {
                string a = ex.Message;
            }
        }
        public string ScaleSize
        {
            get
            {
                return (string)this.GetValue(ScaleSizeProperty);
            }
            set
            {
                this.SetValue(ScaleSizeProperty, value);
            }
        }

        public static readonly DependencyProperty ScaleSizeProperty
            = DependencyProperty.Register(
                "ScaleSize",
                typeof(string),
                typeof(ExtendedGridView),
                new PropertyMetadata(string.Empty));

        public double CellMinWidth
        {
            get
            {
                return (double)this.GetValue(CellMinWidthProperty);
            }
            set
            {
                this.SetValue(CellMinWidthProperty, value);
            }
        }

        public static readonly DependencyProperty CellMinWidthProperty
            = DependencyProperty.Register(
                "CellMinWidth",
                typeof(double),
                typeof(ExtendedGridView),
                new PropertyMetadata(240));

        public double CellWidth
        {
            get
            {
                return (double)this.GetValue(PinWidthProperty);
            }
            set
            {
                this.SetValue(PinWidthProperty, value);
            }
        }

        public static readonly DependencyProperty PinWidthProperty
            = DependencyProperty.Register(
                "CellWidth",
                typeof(double),
                typeof(ExtendedGridView),
                new PropertyMetadata(235));

        public double CellHeight
        {
            get
            {
                return (double)this.GetValue(PinHeightProperty);
            }
            set
            {
                this.SetValue(PinHeightProperty, value);
            }
        }

        public static readonly DependencyProperty PinHeightProperty
            = DependencyProperty.Register(
                "CellHeight",
                typeof(double),
                typeof(ExtendedGridView),
                new PropertyMetadata(double.NaN));

    }

    public class ScaleSize
    {
        public ScaleSize() { }
        public ScaleSize(double width, double height)
        {
            ScaleWidth = width;
            ScaleHeight = height;
        }
        public double ScaleWidth { set; get; }
        public double ScaleHeight { set; get; }
    }
}
