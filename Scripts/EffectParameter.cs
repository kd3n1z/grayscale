using UnityEngine;

namespace Grayscale {
    public struct EffectParameter {
        public readonly ParameterType ParameterType;
        public readonly int Id;
        public readonly object DefaultValue;

        public EffectParameter(ParameterType type, string name, object defaultValue) {
            ParameterType = type;
            Id = Shader.PropertyToID(name);
            DefaultValue = defaultValue;
        }
    }

    public enum ParameterType {
        Int,
        Float
    }
}
