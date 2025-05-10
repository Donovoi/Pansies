using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Language;
using PoshCode.Pansies.ColorSpaces;
using PoshCode.Pansies.ColorSpaces.Comparisons;
using PoshCode.Pansies.Palettes;

namespace PoshCode.Pansies.Palettes
{
    public class NamedPalette<T> : Palette<T>, IReadOnlyDictionary<string, T>, IArgumentCompleter where T : IColorSpace, new()
    {
        private List<string> names = new List<string>();

        public IEnumerable<CompletionResult> CompleteArgument(string commandName, string parameterName, string wordToComplete, CommandAst commandAst, IDictionary fakeBoundParameters)
        {
            WildcardPattern wildcard = new WildcardPattern(wordToComplete + "*", WildcardOptions.IgnoreCase);

            foreach (var name in names.Where(wildcard.IsMatch))
            {
                yield return new CompletionResult(name.ToString(), nativeColors[names.IndexOf(name)].ToVTEscapeSequence(true) + " \e[0m " + name, CompletionResultType.ParameterValue, name);
            }
        }

        public NamedPalette()
        {
        }

        public NamedPalette(IColorSpaceComparison comparisonAlgorithm) : this()
        {
            ComparisonAlgorithm = comparisonAlgorithm;
        }

        public override IColorSpaceComparison ComparisonAlgorithm { get; set; } = new CieDe2000Comparison();

        public virtual void Add(string key, T value) {
            names.Add(key);
            Add(value);
        }

        public virtual bool ContainsKey(string key) => names.Contains(key, StringComparer.OrdinalIgnoreCase);


        // public virtual void Add(KeyValuePair<string, T> items) => dictionary.Add(items);

        // public virtual bool Contains(KeyValuePair<string, T> item) => dictionary.Contains(item);

        // public virtual void CopyTo(KeyValuePair<string, T>[] array, int arrayIndex) => dictionary.CopyTo(array, arrayIndex);

        // public virtual bool Remove(KeyValuePair<string, T> item) => dictionary.Remove(item);

        public virtual IEnumerable<string> Keys => names;
        public virtual IEnumerable<string> Names => names;

        public virtual IEnumerable<T> Values => nativeColors;

        IEnumerable<string> IReadOnlyDictionary<string, T>.Keys => throw new System.NotImplementedException();

        IEnumerable<T> IReadOnlyDictionary<string, T>.Values => throw new System.NotImplementedException();

        int IReadOnlyCollection<KeyValuePair<string, T>>.Count => throw new System.NotImplementedException();

        T IReadOnlyDictionary<string, T>.this[string key] => throw new System.NotImplementedException();

        IEnumerator<KeyValuePair<string, T>> IEnumerable<KeyValuePair<string, T>>.GetEnumerator() {
            for (int i = 0; i < names.Count; i++)
            {
                yield return new KeyValuePair<string, T>(names[i], nativeColors[i]);
            }
        }

        public int IndexOf(string key) => names.FindIndex(x => x.Equals(key, StringComparison.OrdinalIgnoreCase));
        public string FindClosestColorName(IColorSpace color)
        {
            return names[FindClosestColor<T>(color).Index];
        }

        public T GetValue(string key) {
            var index = IndexOf(key);
            if (index >= 0)
            {
                return nativeColors[index];
            }
            throw new KeyNotFoundException();
        }

        new public void Insert(int index, T item)
        {
            ((Palette<T>)this).Insert(index, item);
        }

        new public bool Remove(T item)
        {
            var index = nativeColors.IndexOf(item);
            if (index >= 0)
            {
                ((Palette<T>)this).RemoveAt(index);
                names.RemoveAt(index);
            }
            return index >= 0;
        }
        public virtual bool Remove(string key)
        {
            var index = IndexOf(key);
            if (index >= 0) {
                ((Palette<T>)this).RemoveAt(index);
                names.RemoveAt(index);
            }
            return index >= 0;
        }

        new public void RemoveAt(int index)
        {
            ((Palette<T>)this).RemoveAt(index);
            names.RemoveAt(index);
        }

        bool IReadOnlyDictionary<string, T>.ContainsKey(string key) => ContainsKey(key);

        bool IReadOnlyDictionary<string, T>.TryGetValue(string key, out T value) => TryGetValue(key, out value);

        public virtual bool TryGetValue(string key, out T value)
        {
            var index = IndexOf(key);
            if (index >= 0)
            {
                value = nativeColors[index];
                return true;
            }
            value = default;
            return false;
        }

        new public KeyValuePair<string,T> this[int index]
        {
            get => new KeyValuePair<string, T>(names[index], nativeColors[index]);
        }
    }
}
