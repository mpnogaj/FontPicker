using Avalonia.Media;
using System;
using FontPicker.Properties;

namespace FontPicker;

internal static class Extensions
{
	public static string GetLocalized(this FontStyle fontStyle)
		=> fontStyle switch
		{
			FontStyle.Normal => Resources.NormalStyle,
			FontStyle.Italic => Resources.ItalicStyle,
			FontStyle.Oblique => Resources.ObliqueStyle,
			_ => throw new ArgumentOutOfRangeException(nameof(fontStyle), fontStyle, null)
		};

	public static string GetLocalized(this FontWeight fontWeight)
		=> fontWeight switch
		{
			FontWeight.Thin => Resources.ThinWeight,
			FontWeight.ExtraLight => Resources.ExtraThinWeight,
			FontWeight.Light => Resources.LightWeight,
			FontWeight.SemiLight => Resources.SemiLightWeight,
			FontWeight.Normal => Resources.NormalWeight,
			FontWeight.Medium => Resources.MediumWeight,
			FontWeight.DemiBold => Resources.DemiBoldWeight,
			FontWeight.Bold => Resources.BoldWeight,
			FontWeight.ExtraBold => Resources.ExtraBoldWeight,
			FontWeight.Black => Resources.BlackWeight,
			FontWeight.ExtraBlack => Resources.ExtraBlackWeight,
			_ => throw new ArgumentOutOfRangeException(nameof(fontWeight), fontWeight, null)
		};
}