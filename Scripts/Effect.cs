using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Grayscale {
    public class Effect {
        private static readonly int InputTextureShaderProperty = Shader.PropertyToID("_InputTexture");
        private static readonly int OutputTextureShaderProperty = Shader.PropertyToID("_OutputTexture");
        private static readonly int TextureSizeShaderProperty = Shader.PropertyToID("_TextureSize");

        private readonly ComputeShader _shader;
        private readonly Dictionary<KeyValuePair<Texture2D, Hash128>, Texture2D> _cache = new Dictionary<KeyValuePair<Texture2D, Hash128>, Texture2D>();
        private readonly Dictionary<KeyValuePair<Sprite, Hash128>, Sprite> _spritesCache = new Dictionary<KeyValuePair<Sprite, Hash128>, Sprite>();

        public Sprite Apply(Sprite sprite, params object[] parameters) {
            Texture2D texture = sprite.texture;

            Hash128 hash = GetHashAndDefaultParameters(parameters, out object[] parameterValues);

            KeyValuePair<Sprite, Hash128> kvp = new KeyValuePair<Sprite, Hash128>(sprite, hash);

            if (_spritesCache.TryGetValue(kvp, out Sprite cachedResult)) {
                return cachedResult;
            }

            Sprite resultSprite = Sprite.Create(GetCachedOrCalculate(hash, texture, parameterValues), sprite.rect, sprite.pivot, sprite.pixelsPerUnit, 0, SpriteMeshType.FullRect);

            _spritesCache.Add(kvp, resultSprite);

            return resultSprite;
        }

        public Texture2D Apply(Texture2D texture, params object[] parameters) =>
            GetCachedOrCalculate(GetHashAndDefaultParameters(parameters, out object[] parameterValues), texture, parameterValues);

        public void Precache(Sprite sprite, params object[] parameters) => Apply(sprite, parameters);

        public void Precache(Texture2D texture, params object[] parameters) => Apply(texture, parameters);

        // ARGB32: 4 bytes per pixel
        public long GetCacheSize() => _cache.Values.Sum(e => e.width * e.height) * 4;

        public void ClearCache() {
            _cache.Clear();
            _spritesCache.Clear();
        }

        private Texture2D GetCachedOrCalculate(Hash128 hash, Texture2D texture, object[] parameterValues) {
            KeyValuePair<Texture2D, Hash128> kvp = new KeyValuePair<Texture2D, Hash128>(texture, hash);

            if (_cache.TryGetValue(kvp, out Texture2D result)) {
                return result;
            }

            Texture2D resultTexture = Calculate(texture, parameterValues);

            _cache.Add(kvp, resultTexture);

            return resultTexture;
        }

        private Hash128 GetHashAndDefaultParameters(IReadOnlyList<object> parameters, out object[] parameterValues) {
            parameterValues = GetDefaultParameters(parameters, out int[] parametersHashCodes);

            Hash128 hash = Hash128.Compute(parametersHashCodes);

            return hash;
        }

        private object[] GetDefaultParameters(IReadOnlyList<object> parameters, out int[] parameterHashCodes) {
            object[] parameterValues = new object[_effectParameters.Length];
            parameterHashCodes = new int[_effectParameters.Length];

            for (int i = 0; i < parameters.Count; i++) {
                object value = parameters[i];

                parameterValues[i] = value;
                parameterHashCodes[i] = value.GetHashCode();
            }

            for (int i = parameters.Count; i < _effectParameters.Length; i++) {
                object value = _effectParameters[i].DefaultValue;

                parameterValues[i] = value;
                parameterHashCodes[i] = value.GetHashCode();
            }

            return parameterValues;
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

            Texture2D resultTexture = new Texture2D(width, height, TextureFormat.ARGB32, false);
            Graphics.CopyTexture(renderTexture, resultTexture);

            return resultTexture;
        }

        private readonly EffectParameter[] _effectParameters;

        public Effect(string shaderResourcePath, params EffectParameter[] effectParameters) : this(Resources.Load<ComputeShader>(shaderResourcePath), effectParameters) { }

        public Effect(ComputeShader shader, params EffectParameter[] effectParameters) {
            _shader = shader;
            _effectParameters = effectParameters;

            if (shader == null || !shader.IsSupported(0)) {
                Debug.LogError("Shader is null or is not supported");
            }
        }
    }
}