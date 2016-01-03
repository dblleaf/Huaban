using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace Huaban.UWP.Controls
{
	public class HBGridPanel : Panel
	{
		public HBGridPanel()
		{
			this.HorizontalAlignment = HorizontalAlignment.Center;
		}
		public static readonly DependencyProperty ColumnWidthProperty = DependencyProperty.Register("ColumnWidth", typeof(double), typeof(HBGridPanel), new PropertyMetadata(0.0));
		public static readonly DependencyProperty MinColumnsProperty = DependencyProperty.Register("MinColumnCount", typeof(int), typeof(HBGridPanel), new PropertyMetadata(2));
		public static readonly DependencyProperty HeightWidthDiffProperty = DependencyProperty.Register("HeightWidthDiff", typeof(double), typeof(HBGridPanel), new PropertyMetadata(0.0));
		public double ColumnWidth
		{
			get
			{
				return (double)this.GetValue(ColumnWidthProperty);
			}
			set
			{
				this.SetValue(ColumnWidthProperty, value);
			}
		}
		public double HeightWidthDiff
		{
			get
			{
				return (double)this.GetValue(HeightWidthDiffProperty);
			}
			set
			{
				this.SetValue(HeightWidthDiffProperty, value);
			}
		}

		public int MinColumnCount
		{
			get
			{
				return (int)this.GetValue(MinColumnsProperty);
			}
			set
			{
				this.SetValue(MinColumnsProperty, value);
			}
		}
		//计算实际列数
		//当设置列宽的时候，取按 照宽度计算出的列数(width/ColumnWidth) 和 最小列数(MinColumnCount)  的较大者
		private int GetColumnCount(Size size)
		{
			int cCount = Math.Max(MinColumnCount, 1);//防止小于1
			double width = size.Width;
			if (ColumnWidth == 0)
				return cCount;
			else
				return Convert.ToInt32(Math.Max(cCount, Math.Floor(width / ColumnWidth)));
		}
		//计算实际列宽
		//最小列数(MinColumnCount) 和 列宽(ColumnWidth) 的积 大于最大宽度的时候，取最大宽度/列数
		private double GetColumnWidth(Size size)
		{
			int cCount = Math.Max(MinColumnCount, 1);
			double width = size.Width;
			if (cCount * ColumnWidth > width)
				return width / cCount;
			return ColumnWidth;
		}
		protected override Size MeasureOverride(Size availableSize)
		{
			double flowWidth = GetColumnWidth(availableSize);//实际列宽
			int columnCount = GetColumnCount(availableSize);//实际列数
			double[] ys = new double[columnCount];//存储每行每个元素的高度
			int i = 0;//计数
			double top = 0;//距离顶端的距离
			Size elemMeasureSize = new Size(flowWidth, double.PositiveInfinity);
			foreach (UIElement elem in Children)
			{
				elem.Measure(elemMeasureSize);

				int col = i % columnCount;
				if (col == 0)//每行开始的时候
				{
					top += ys.Max();//计算已存储行元素最高值，然后累计
					ys = new double[columnCount];//初始化存储每行元素高度的数组
				}
				if (HeightWidthDiff != 0)
					ys[col] = flowWidth + HeightWidthDiff;
				else
					ys[col] = elem.DesiredSize.Height;//存储高度

				i++;
			}
			top = top + ys.Max();//计算出总高度
			return new Size(availableSize.Width, top);
		}
		protected override Size ArrangeOverride(Size finalSize)
		{
			double flowWidth = GetColumnWidth(finalSize);
			int columnCount = GetColumnCount(finalSize);

			double[] xs = new double[columnCount];
			double[] ys = new double[columnCount];
			foreach (int idx in Enumerable.Range(0, columnCount))
			{
				xs[idx] = idx * flowWidth;
			}
			int i = 0;
			double top = 0;
			double height = 0;
			foreach (UIElement elem in Children)
			{
				int col = i % columnCount;
				if (col == 0)
					top = top + ys.Max();

				if (HeightWidthDiff != 0)
					height = flowWidth + HeightWidthDiff;
				else
					height = elem.DesiredSize.Height;

				ys[col] = height;

				Point pt = new Point(xs[col], top);

				elem.Arrange(new Rect(pt, new Size(flowWidth, height)));

				i++;
			}
			return base.ArrangeOverride(finalSize);
		}
	}
}
