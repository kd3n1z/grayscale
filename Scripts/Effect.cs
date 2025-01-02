using System;
using System.Collections.Generic;
using UnityEngine;

namespace Grayscale {
    public class Effect {
        private static readonly int InputTextureShaderProperty = Shader.PropertyToID("_InputTexture");
        private static readonly int OutputTextureShaderProperty = Shader.PropertyToID("_OutputTexture");
        private static readonly int TextureSizeShaderProperty = Shader.PropertyToID("_TextureSize");

        private readonly ComputeShader _shader;
        private readonly Dictionary<Hash128, Texture2D> _cache = new Dictionary<Hash128, Texture2D>();

        public Sprite Apply(Sprite sprite, params object[] parameters) {
            Texture2D result = Apply(sprite.texture, parameters);

            return Sprite.Create(result, new Rect(0, 0, result.width, result.height), sprite.pivot);
        }

        public Texture2D Apply(Texture2D input, params object[] parameters) {
            object[] parameterValues = new object[_effectParameters.Length];
            int[] parameterHashCodes = new int[_effectParameters.Length];

            for (int i = 0; i < parameters.Length; i++) {
                object value = parameters[i];

                parameterValues[i] = value;
                parameterHashCodes[i] = value.GetHashCode();
            }

            for (int i = parameters.Length; i < _effectParameters.Length; i++) {
                object value = _effectParameters[i].DefaultValue;

                parameterValues[i] = value;
                parameterHashCodes[i] = value.GetHashCode();
            }

            Hash128 hash = input.imageContentsHash;
            hash.Append(parameterHashCodes);

            if (_cache.TryGetValue(hash, out Texture2D result)) {
                return result;
            }

            Texture2D resultTexture = Calculate(input, parameterValues);

            _cache.Add(hash, resultTexture);

            return resultTexture;
        }

        private Texture2D Calculate(Texture2D input, object[] values) {
            int width = input.width;
            int height = input.height;

            RenderTexture renderTexture = new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32) {
                enableRandomWrite = true
            };

            renderTexture.Create();

            for (int i = 0; i < values.Length; i++) {
                EffectParameter parameter = _effectParameters[i];
                object value = values[i];

                switch (parameter.ParameterType) {
                    case ParameterType.Int:
                        _shader.SetInt(parameter.Id, Convert.ToInt32(value));
                        break;
                    case ParameterType.Float:
                        _shader.SetFloat(parameter.Id, Convert.ToSingle(value));
                        break;
                    case ParameterType.Bool:
                        _shader.SetBool(parameter.Id, Convert.ToBoolean(value));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            _shader.SetInts(TextureSizeShaderProperty, width, height);
            _shader.SetTexture(0, InputTextureShaderProperty, input);
            _shader.SetTexture(0, OutputTextureShaderProperty, renderTexture);

            _shader.Dispatch(0, Mathf.CeilToInt(width / 8.0f), Mathf.CeilToInt(height / 8.0f), 1);

            RenderTexture.active = renderTexture;
            Texture2D resultTexture = new Texture2D(width, height, TextureFormat.ARGB32, false);
            resultTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            resultTexture.Apply();

            RenderTexture.active = null;
            renderTexture.Release();

            return resultTexture;
        }

        private readonly EffectParameter[] _effectParameters;

        public Effect(string shaderResourcePath, params EffectParameter[] effectParameters) : this(Resources.Load<ComputeShader>(shaderResourcePath), effectParameters) { }

        public Effect(ComputeShader shader, params EffectParameter[] effectParameters) {
            _shader = shader;
            _effectParameters = effectParameters;
        }
    }
}