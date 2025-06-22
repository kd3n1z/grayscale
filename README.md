# grayscale

Grayscale is a lightweight Unity package for applying customizable image effects with flexible parameters and easy
integration.

## Installation

-   Open Package Manager
-   Add package from git URL:
    `https://github.com/kd3n1z/grayscale.git`

## Built-in Effects

### Grayscale

Converts the image to grayscale.

#### Parameters

1. **\_RedWeight**: Type: `float`, Default: `0.299`
2. **\_GreenWeight**: Type: `float`, Default: `0.587`
3. **\_BlueWeight**: Type: `float`, Default: `0.114`

#### Example Usage

```csharp
BuiltinEffects.Grayscale.Apply(sprite);
BuiltinEffects.Grayscale.Apply(sprite, 1, 0, 0); // using only the red channel
```

### Negative

Inverts the colors of the image.

#### Example Usage

```csharp
BuiltinEffects.Negative.Apply(sprite);
```

### BoxBlur

Applies box blur to the image.

#### Parameters

1. **\_BlurRadius**: Type: `int`, Default: `10`

#### Example Usage

```csharp
BuiltinEffects.Blur.Apply(sprite);
BuiltinEffects.Blur.Apply(sprite, 20); // with a radius of 20
```

### Pixelate

Applies pixelation to the image.

#### Parameters

1. **\_PixelSize**: Type: `int`, Default: `10`

#### Example Usage

```csharp
BuiltinEffects.Pixelate.Apply(sprite);
BuiltinEffects.Pixelate.Apply(sprite, 20); // with a pixel size of 20
```

## License

This project is licensed under the MIT license. See the [LICENSE.md](LICENSE.md) file for details.
