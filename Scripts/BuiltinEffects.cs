namespace Grayscale {
    public static class BuiltinEffects {
        /// <summary>
        /// Converts the image to grayscale.
        /// Parameters:
        /// <list type="number">
        ///   <item>
        ///     <term><c>_RedWeight</c></term>
        ///     <description> Type: <see cref="float">float</see>, Default: <c>0.299</c></description>
        ///   </item>
        ///   <item>
        ///     <term><c>_GreenWeight</c></term>
        ///     <description> Type: <see cref="float">float</see>, Default: <c>0.587</c></description>
        ///   </item>
        ///   <item>
        ///     <term><c>_BlueWeight</c></term>
        ///     <description> Type: <see cref="float">float</see>, Default: <c>0.114</c></description>
        ///   </item>
        /// </list>
        /// </summary>
        /// <example>
        /// Apply grayscale:
        /// <code>
        /// BuiltinEffects.Grayscale.Apply(sprite);
        /// BuiltinEffects.Grayscale.Apply(sprite, 1f, 0f, 0f); // using only the red channel
        /// </code>
        /// </example>
        public static readonly Effect Grayscale = new Effect(
            "com.kd3n1z.grayscale/Grayscale",
            new EffectParameter(ParameterType.Float, "_RedWeight", 0.299f),
            new EffectParameter(ParameterType.Float, "_GreenWeight", 0.587f),
            new EffectParameter(ParameterType.Float, "_BlueWeight", 0.114f)
        );

        /// <summary>
        /// Inverts the colors of the image.
        /// </summary>
        /// <example>
        /// Apply negative:
        /// <code>
        /// BuiltinEffects.Negative.Apply(sprite);
        /// </code>
        /// </example>
        public static readonly Effect Negative = new Effect("com.kd3n1z.grayscale/Negative");

        /// <summary>
        /// Applies blur to the image.
        /// Parameters:
        /// <list type="number">
        ///   <item>
        ///     <term><c>_BlurRadius</c></term>
        ///     <description> Type: <see cref="int">int</see>, Default: <c>10</c></description>
        ///   </item>
        /// </list>
        /// </summary>
        /// <example>
        /// Apply blur:
        /// <code>
        /// BuiltinEffects.Blur.Apply(sprite);
        /// BuiltinEffects.Blur.Apply(sprite, 20); // with a radius of 20
        /// </code>
        /// </example>
        public static readonly Effect Blur = new Effect("com.kd3n1z.grayscale/Blur", new EffectParameter(ParameterType.Int, "_BlurRadius", 10));

        /// <summary>
        /// Applies pixelation to the image.
        /// Parameters:
        /// <list type="number">
        ///   <item>
        ///     <term><c>_PixelSize</c></term>
        ///     <description> Type: <see cref="int">int</see>, Default: <c>10</c></description>
        ///   </item>
        /// </list>
        /// </summary>
        /// <example>
        /// Apply pixelation:
        /// <code>
        /// BuiltinEffects.Pixelate.Apply(sprite);
        /// BuiltinEffects.Pixelate.Apply(sprite, 20); // with a pixel size of 20
        /// </code>
        /// </example>
        public static readonly Effect Pixelate = new Effect("com.kd3n1z.grayscale/Pixelate", new EffectParameter(ParameterType.Int, "_PixelSize", 10));
    }
}