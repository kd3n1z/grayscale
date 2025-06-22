using System;
using System.Collections.Generic;
using UnityEngine;

namespace Grayscale {
    internal static class Helper {
        internal static object[] GetDefaultParameters(IReadOnlyList<EffectParameter> parameters, IReadOnlyList<object> values, out Hash128 hash) {
            object[] result = new object[parameters.Count];
            
            int minCount = Math.Min(parameters.Count, values.Count);

            hash = new Hash128();

            for (int i = 0; i < minCount; i++) {
                object value = values[i];

                result[i] = value;
                hash.Append(value.GetHashCode());
            }

            for (int i = minCount; i < parameters.Count; i++) {
                object value = parameters[i].DefaultValue;

                result[i] = value;
                hash.Append(value.GetHashCode());
            }

            return result;
        }
    }
}
