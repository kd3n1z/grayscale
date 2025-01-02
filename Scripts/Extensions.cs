using UnityEngine;

namespace Grayscale {
    public static class Extensions {
        public static Texture2D ApplyEffect(this Texture2D texture, Effect effect, params object[] parameters) => effect.Apply(texture, parameters);
        public static Sprite ApplyEffect(this Sprite sprite, Effect effect, params object[] parameters) => effect.Apply(sprite, parameters);
    }
}