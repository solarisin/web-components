using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.Utilities;
using System.Drawing;
using Solarisin.Core.Extensions;

namespace Solarisin.Web.Components;
using SystemColor = System.Drawing.Color;

/// <summary>
/// The possible display structures of the flexbox viewer.
/// </summary>
public enum StructureMethod
{
	Html,		// utilizes only html tags
	MudBlazor	// utilizes MudBlazor components
}

/// <summary>
/// The flexbox viewer component demonstrates using css flexbox to achieve a standard layout.
/// </summary>
public partial class FlexboxViewer
{
	#region Parameters

	/// <summary>
	/// Specify any custom css classes to be applied to the flexbox viewer.
	/// </summary>
	[Parameter]
	public string Class { get; set; } = "";
	
	/// <summary>
	///  Specify the display structure of the flexbox viewer.
	/// </summary>
	[Parameter]
	public StructureMethod Structure { get; set; } = StructureMethod.MudBlazor;

	/// <summary>
	/// The site-wide MudTheme to be utilized.
	/// </summary>
	[CascadingParameter]
	public MudTheme? Theme { get; set; }

	#endregion

	#region Blazor Life Cycle Methods

	/// <summary>
	/// Called on initialization of the component to randomly generate styles once.
	/// </summary>
	protected override void OnInitialized()
	{
		// only generate the styles once
		GenerateStyles(12);
	}

	/// <summary>
	/// Called upon changes to the parameters to reset the next style index.
	/// </summary>
	protected override void OnParametersSet()
	{
		_nextStyleIndex = 0;
	}

	#endregion

	/// <summary>
	/// Constructs all css classes to be applied to the flexbox viewer, both intrinsic and user-defined.
	/// </summary>
	private string Classname =>
		new CssBuilder("pa-1")
			.AddClass(Class, when: !string.IsNullOrEmpty(Class))
			.Build();
	
	/// <summary>
	/// Determine a contrasting color to utilize for the text color.
	/// </summary>
	/// <param name="bgColor">The background color</param>
	/// <returns>A contrasting text color based on the bgColor.</returns>
    private string ContrastColor(string bgColor)
	{
		// Determine colors to use for dark and light backgrounds
		var darkContrast = Theme?.Palette.TextPrimary.ToString(MudColorOutputFormats.Hex) ?? "#FFFFFF";
		var brightContrast = Theme?.PaletteDark.TextPrimary.ToString(MudColorOutputFormats.Hex) ?? "#000000";

		// System.Drawing.Color from hex
		var color = (SystemColor)(new ColorConverter().ConvertFromString(bgColor) ?? SystemColor.Black);
			
		// Counting the perceptive luminance - human eye favors green color...
		double luminance = (0.299 * color.R + 0.587 * color.G + 0.114 * color.B) / 255;

		// Return the color based on the luminance
		return luminance > 0.5 ? darkContrast : brightContrast;
	}
    
	/// <summary>
	/// Stores the possible colors to be randomized for flexbox item styles.
	/// </summary>
	private readonly string[] _validColors =
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
	
	/// <summary>
	/// The random number generator to be used to generate random styles.
	/// </summary>
	private static readonly Random Random = new();
	
	/// <summary>
	/// Stores the list of randomly generated hex color codes to be applied to the flexbox items.
	/// </summary>
    private string[] _hexColors = Array.Empty<string>();
    
	/// <summary>
	/// Randomly generate the styles to be applied to the flexbox items.
	/// </summary>
	/// <param name="count">The number of styles to generate.</param>
    private void GenerateStyles(int count)
	{
		Random.Shuffle(_validColors);
		_hexColors = new string[count];
		for (int i = 0; i < count; i++)
		{
			_hexColors[i] = _validColors[i % _validColors.Length];
		}
	}

	/// <summary>
	/// Stores the next style index to be used for the flexbox item.
	/// </summary>
	private int _nextStyleIndex;
	
	/// <summary>
	/// Append to the given style to add a randomized background color and contrasting text color.
	/// </summary>
	/// <param name="style">Any custom style to append the generated color styles to.</param>
	/// <param name="styleIndex">Specify the index of the randomized style to use, -1 to use the next available style.</param>
	/// <returns>The css style to be applied to the flexbox item.</returns>
	private string ColorStyle(string style = "", int styleIndex = -1)
    {
        var nextStyleIndex = _nextStyleIndex++;
        var modStyleIndex = nextStyleIndex % _hexColors.Length;

		if (styleIndex == -1)
			styleIndex = modStyleIndex;
		var color = _hexColors[styleIndex];
		var textColor = ContrastColor(color);
		return $"color: {textColor};background-color: {color};{style}";
	}
}
