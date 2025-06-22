using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Grayscale {
    public class Effect {
        private static readonly bool CopySupported = (SystemInfo.copyTextureSupport & CopyTextureSupport.RTToTexture) != 0;

        private readonly Material _material;
        private readonly EffectParameter[] _effectParameters;

        private readonly CacheStorage<Texture2D> _textureCache = new CacheStorage<Texture2D>();
        private readonly CacheStorage<Sprite> _spriteCache = new CacheStorage<Sprite>();

        public Effect(string shaderResourcePath, params EffectParameter[] effectParameters) : this(Resources.Load<Shader>(shaderResourcePath),
            effectParameters) {
        }

        public Effect(Shader shader, params EffectParameter[] effectParameters) {
            _material = new Material(shader);
            _effectParameters = effectParameters;
        }

        public Cached<Texture2D> ApplyCached(Texture2D input, params object[] parameters) {
            parameters = Helper.GetDefaultParameters(_effectParameters, parameters, out Hash128 hash);
            hash.Append(input.GetHashCode());

            return _textureCache.TryGet(hash, out Cached<Texture2D> result) ? result : _textureCache.Set(hash, Calculate(input, parameters));

        }

        public Texture2D Apply(Texture2D input, params object[] parameters) {
            parameters = Helper.GetDefaultParameters(_effectParameters, parameters, out Hash128 hash);
            hash.Append(input.GetHashCode());

            return _textureCache.TryGet(hash, out Cached<Texture2D> result) ? result : Calculate(input, parameters);
        }

        public Cached<Sprite> ApplyCached(Sprite input, params object[] parameters) {
            parameters = Helper.GetDefaultParameters(_effectParameters, parameters, out Hash128 hash);
            hash.Append(input.GetHashCode());

            return _spriteCache.TryGet(hash, out Cached<Sprite> result) ? result : _spriteCache.Set(hash, Calculate(input, parameters));

        }

        public Sprite Apply(Sprite input, params object[] parameters) {
            parameters = Helper.GetDefaultParameters(_effectParameters, parameters, out Hash128 hash);
            hash.Append(input.GetHashCode());

            return _spriteCache.TryGet(hash, out Cached<Sprite> result) ? result : Calculate(input, parameters);
        }

        private Sprite Calculate(Sprite input, object[] values) =>
            Sprite.Create(Calculate(input.texture, values), input.rect, input.pivot, input.pixelsPerUnit, 0, SpriteMeshType.FullRect);

        private Texture2D Calculate(Texture2D input, object[] values) {
            int width = input.width;
            int height = input.height;

            for (int i = 0; i < values.Length; i++) {
                EffectParameter parameter = _effectParameters[i];
                object value = values[i];

                switch (parameter.ParameterType) {
                    case ParameterType.Int:
                        _material.SetInt(parameter.Id, Convert.ToInt32(value));
                        break;
                    case ParameterType.Float:
                        Debug.Log(value);
                        _material.SetFloat(parameter.Id, Convert.ToSingle(value));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            RenderTexture rt = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.ARGB32);
            Graphics.Blit(input, rt, _material);

            // copy from rt to result
            Texture2D result = new Texture2D(width, height, TextureFormat.ARGB32, false);

            if (CopySupported) {
                Graphics.CopyTexture(rt, result);
            }
            else {
                RenderTexture.active = rt;
                result.ReadPixels(new Rect(0, 0, width, height), 0, 0);
                result.Apply();
                RenderTexture.active = null;
            }

            RenderTexture.ReleaseTemporary(rt);

            return result;
        }
    }
}
