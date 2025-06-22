using System.Collections.Generic;
using UnityEngine;

namespace Grayscale {
    internal class CacheStorage<T> {
        private readonly Dictionary<Hash128, T> _storage = new Dictionary<Hash128, T>();

        public Cached<T> Set(Hash128 hash, T value) {
            _storage[hash] = value;
            return new Cached<T>(this, hash, value);
        }

        public bool TryGet(Hash128 hash, out Cached<T> result) {
            result = null;

            if (!_storage.TryGetValue(hash, out T value)) {
                return false;
            }

            result = new Cached<T>(this, hash, value);
            return true;
        }

        public void Remove(Hash128 hash) {
            _storage.Remove(hash);
        }
    }
}
