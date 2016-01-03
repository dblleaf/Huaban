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
	public class HBWaterFullPanel : Panel
	{
		public static double[] ColumnHeight;
		public HBWaterFullPanel()
		{
			//根据列数，实例化用来存放每列高度的数组  
			ColumnHeight = new double[ColumnCount];
		}
		public int ColumnCount
		{
			get
			{
				return (int)this.GetValue(ColumnCountProperty);
			}
			set
			{
				this.SetValue(ColumnCountProperty, value);
			}
		}

		public static readonly DependencyProperty ColumnCountProperty = DependencyProperty.Register("ColumnCount", typeof(int), typeof(HBWaterFullPanel), new PropertyMetadata(2));

		/// <summary>  
		/// Properties the changed.  
		/// </summary>  
		/// <param name="sender">The sender.</param>  
		/// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>  
		public static void PropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			ColumnHeight = new double[(int)e.NewValue];
			if (sender == null || e.NewValue == e.OldValue)
				return;
			sender.SetValue(ColumnCountProperty, e.NewValue);

		}

		/// <summary>  
		/// 当在派生类中重写时，请测量子元素在布局中所需的大小，然后确定 <see cref="T:System.Windows.FrameworkElement" /> 派生类的大小。  
		/// 更新当前元素与其子元素的布局，以下处理都属于 测量 处理，并非实际布局  
		/// </summary>  
		/// <param name="availableSize">此元素可以赋给子元素的可用大小。可以指定无穷大值，这表示元素的大小将调整为内容的可用大小。</param>  
		/// <returns>此元素在布局过程中所需的大小，这是由此元素根据对其子元素大小的计算而确定的。</returns>  
		protected override Size MeasureOverride(Size availableSize)
		{

			KeyValuePair<double, int>[] flowLens = new KeyValuePair<double, int>[ColumnCount];
			foreach (int idx in Enumerable.Range(0, ColumnCount))
			{
				flowLens[idx] = new KeyValuePair<double, int>(0.0, idx);
			}

			// 我们就用2个纵向流来演示，获取每个流的宽度。
			double flowWidth = availableSize.Width / ColumnCount;

			// 为子控件提供沿着流方向上，无限大的空间
			Size elemMeasureSize = new Size(flowWidth, double.PositiveInfinity);

			foreach (UIElement elem in Children)
			{
				// 让子控件计算它的大小。
				elem.Measure(elemMeasureSize);
				Size elemSize = elem.DesiredSize;

				double elemLen = elemSize.Height;
				var pair = flowLens[0];

				// 子控件添加到最短的流上，并重新计算最短流。
				// 因为我们为了求得流的长度，必须在计算大小这一步时就应用一次布局。但实际的布局还是会在Arrange步骤中完成。
				flowLens[0] = new KeyValuePair<double, int>(pair.Key + elemLen, pair.Value);
				flowLens = flowLens.OrderBy(p => p.Key).ToArray();
			}

			return new Size(availableSize.Width, flowLens.Last().Key);
		}



		/// <summary>  
		/// 在派生类中重写时，请为 <see cref="T:System.Windows.FrameworkElement" /> 派生类定位子元素并确定大小。  
		/// 更新当前元素与其子元素的布局，以下处理都属于 实际 处理，元素布局都将基于此  
		/// </summary>  
		/// <param name="finalSize">父级中此元素应用来排列自身及其子元素的最终区域。</param>  
		/// <returns>所用的实际大小。</returns>  
		protected override Size ArrangeOverride(Size finalSize)
		{
			//清空所有列的高度  
			KeyValuePair<double, int>[] flowLens = new KeyValuePair<double, int>[ColumnCount];

			double flowWidth = finalSize.Width / ColumnCount;

			// 要用到流的横坐标了，我们用一个数组来记录（其实最初是想多加些花样，用数组来方便索引横向偏移。不过本例中就只进行简单的乘法了）
			double[] xs = new double[ColumnCount];

			foreach (int idx in Enumerable.Range(0, ColumnCount))
			{
				flowLens[idx] = new KeyValuePair<double, int>(0.0, idx);
				xs[idx] = idx * flowWidth;
			}

			foreach (UIElement elem in Children)
			{
				// 直接获取子控件大小。
				Size elemSize = elem.DesiredSize;
				double elemLen = elemSize.Height;

				var pair = flowLens[0];
				double chosenFlowLen = pair.Key;
				int chosenFlowIdx = pair.Value;

				// 此时，我们需要设定新添加的空间的位置了，其实比measure就多了一个Point信息。接在流中上一个元素的后面。
				Point pt = new Point(xs[chosenFlowIdx], chosenFlowLen);

				// 调用Arrange进行子控件布局。并让子控件利用上整个流的宽度。
				elem.Arrange(new Rect(pt, new Size(flowWidth, elemSize.Height)));

				// 重新计算最短流。
				flowLens[0] = new KeyValuePair<double, int>(chosenFlowLen + elemLen, chosenFlowIdx);
				flowLens = flowLens.OrderBy(p => p.Key).ToArray();
			}

			// 直接返回该方法的参数。
			return finalSize;
		}


	}
}
