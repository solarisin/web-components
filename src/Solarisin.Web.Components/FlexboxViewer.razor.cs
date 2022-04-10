using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.Utilities;
using System.Drawing;
using Microsoft.AspNetCore.Components.Web;

namespace Solarisin.Web.Components
{
    public partial class FlexboxViewer
	{
		[Parameter]
		public string Class { get; set; } = "";

		[CascadingParameter]
		public MudTheme? Theme { get; set; }
		
        public enum StructureMethod
		{
			Html,
			MudBlazor
		}

		private StructureMethod _structureMethod = StructureMethod.MudBlazor;
		private string[] _validColors =
		{
		    Colors.Amber.Default,
		    Colors.Blue.Default,
		    Colors.BlueGrey.Default,
		    Colors.Brown.Default,
		    Colors.Cyan.Default,
		    Colors.DeepOrange.Default,
		    Colors.DeepPurple.Default,
		    Colors.Green.Default,
		    Colors.Grey.Default,
		    Colors.Indigo.Default,
		    Colors.LightBlue.Default,
		    Colors.LightGreen.Default,
		    Colors.Lime.Default,
		    Colors.Orange.Default,
		    Colors.Pink.Default,
		    Colors.Purple.Default,
		    Colors.Red.Default,
		    Colors.Teal.Default,
		    Colors.Yellow.Default
	    };
        private string[] _styles = Array.Empty<string>();
        private int _nextStyleIndex;

		protected string Classname =>
		    new CssBuilder("pa-1")
				.AddClass(Class, when: !string.IsNullOrEmpty(Class))
				.Build();

		protected string RandomColor => _validColors[new Random().Next(_validColors.Length)];
		
        protected override void OnInitialized()
        {
            // only generate the styles once
            GenerateStyles(12);
        }

        protected override void OnParametersSet()
        {
            _nextStyleIndex = 0;
        }

        protected string ContrastColor(string bgColor)
		{
			// get color from hex
			System.Drawing.Color color = (System.Drawing.Color)(new ColorConverter().ConvertFromString(bgColor) ?? System.Drawing.Color.Black);

			// Counting the perceptive luminance - human eye favors green color...      
			double luminance = (0.299 * color.R + 0.587 * color.G + 0.114 * color.B) / 255;

			string darkContrast = "#FFFFFF";
			string brightContrast = "#000000";
			if (Theme != null)
			{
				// bright color - dark contrast
				darkContrast = Theme.Palette.TextPrimary.ToString(MudColorOutputFormats.Hex);

				// dark color - bright contrast
				brightContrast = Theme.PaletteDark.TextPrimary.ToString(MudColorOutputFormats.Hex);
			}

			return luminance > 0.5 ? darkContrast : brightContrast;
		}

		protected void GenerateStyles(int count)
		{
			_styles = new string[count];
			for (int i = 0; i < count; i++)
			{
				_styles[i] = RandomColor;
			}
		}

		protected string ColorStyle(string style = "", int styleIndex = -1)
        {
            var nextStyleIndex = _nextStyleIndex++;
            var modStyleIndex = nextStyleIndex % _styles.Length;

			if (styleIndex == -1)
				styleIndex = modStyleIndex;
			var color = _styles[styleIndex];
			var textColor = ContrastColor(color);
			return $"color: {textColor};background-color: {color};{style}";
		}
	}
}
