﻿using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Fluid.Values;

namespace Fluid
{
    /// <summary>
    /// Represents the list of arguments of a function.
    /// </summary>
    public class FunctionArguments
    {
        public static readonly FunctionArguments Empty = new FunctionArguments();

        private List<FluidValue> _positional;
        private Dictionary<string, FluidValue> _named;

        public int Count => _positional?.Count ?? 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FluidValue At(int index)
        {
            if (_positional == null || index >= _positional.Count)
            {
                return NilValue.Instance;
            }

            return _positional[index];
        }

        public bool HasNamed(string name)
        {
            return _named != null && _named.ContainsKey(name);
        }

        public FluidValue this[string name]
        {
            get
            {
                if (_named != null && _named.TryGetValue(name, out var value))
                {
                    return value;
                }

                return NilValue.Instance;
            }
        }

        public FunctionArguments()
        {
        }

        public FunctionArguments(params FluidValue[] values)
        {
            _positional = new List<FluidValue>(values);
        }

        public FunctionArguments Add(FluidValue value)
        {
            return Add(null, value);
        }

        public FunctionArguments Add(string name, FluidValue value)
        {
            if (name != null)
            {
                _named ??= new Dictionary<string, FluidValue>();

                _named.Add(name, value);
            }

            _positional ??= new List<FluidValue>();

            _positional.Add(value);

            return this;
        }

        public IEnumerable<string> Names => _named?.Keys ?? System.Linq.Enumerable.Empty<string>();

        public IEnumerable<FluidValue> Values => _positional;

        internal object[] ValuesToObjectArray()
        {
            var array = new object[_positional.Count];
            for (var i = 0; i < array.Length; ++i)
            {
                array[i] = _positional[i].ToObjectValue();
            }

            return array;
        }
    }
}
