using System;
using System.Collections.Generic;
#if !NET_DOTS
using System.Linq;
#endif
using Unity;
using Unity.Collections;
using Unity.Entities;

namespace Unity.Entities
{
    internal class ComponentSystemSorter<TBeforeAttribute, TAfterAttribute>
        where TBeforeAttribute : Attribute, ISystemOrderAttribute
        where TAfterAttribute : Attribute, ISystemOrderAttribute
    {
        public class CircularSystemDependencyException : Exception
        {
            public CircularSystemDependencyException(IEnumerable<Type> chain)
            {
                Chain = chain;
#if NET_DOTS
                var lines = new List<string>();
                UnityEngine.Debug.Log($"The following systems form a circular dependency cycle (check their [{typeof(TBeforeAttribute)}]/[{typeof(TAfterAttribute)}] attributes):");
                foreach (var s in Chain)
                {
                    var name = TypeManager.GetSystemName(s);
                    UnityEngine.Debug.Log(name);
                }
#endif
            }

            public IEnumerable<Type> Chain { get; }

#if !NET_DOTS
            public override string Message
            {
                get
                {
                    var lines = new List<string>
                    {
                        $"The following systems form a circular dependency cycle (check their [{typeof(TBeforeAttribute)}]/[{typeof(TAfterAttribute)}] attributes):"
                    };
                    foreach (var s in Chain)
                    {
                        lines.Add($"- {s.ToString()}");
                    }

                    return lines.Aggregate((str1, str2) => str1 + "\n" + str2);
                }
            }
#endif
        }

        private class Heap
        {
            private readonly TypeHeapElement[] _elements;
            private int _size;
            private readonly int _capacity;
            private static readonly int BaseIndex = 1;

            public Heap(int capacity)
            {
                _capacity = capacity;
                _size = 0;
                _elements = new TypeHeapElement[capacity + BaseIndex];
            }

            public bool Empty => _size <= 0;

            public void Insert(TypeHeapElement e)
            {
#if ENABLE_UNITY_COLLECTIONS_CHECKS || UNITY_DOTS_DEBUG
                if (_size >= _capacity)
                {
                    throw new InvalidOperationException($"Attempted to Insert() to a full heap.");
                }
#endif
                var i = BaseIndex + _size++;
                while (i > BaseIndex)
                {
                    var parent = i / 2;

                    if (e.CompareTo(_elements[parent]) > 0)
                    {
                        break;
                    }

                    _elements[i] = _elements[parent];
                    i = parent;
                }

                _elements[i] = e;
            }

            public TypeHeapElement Peek()
            {
#if ENABLE_UNITY_COLLECTIONS_CHECKS || UNITY_DOTS_DEBUG
                if (Empty)
                {
                    throw new InvalidOperationException($"Attempted to Peek() an empty heap.");
                }
#endif
                return _elements[BaseIndex];
            }

            public TypeHeapElement Extract()
            {
#if ENABLE_UNITY_COLLECTIONS_CHECKS || UNITY_DOTS_DEBUG
                if (Empty)
                {
                    throw new InvalidOperationException($"Attempted to Extract() from an empty heap.");
                }
#endif
                var top = _elements[BaseIndex];
                _elements[BaseIndex] = _elements[_size--];
                if (!Empty)
                {
                    Heapify(BaseIndex);
                }

                return top;
            }

            private void Heapify(int i)
            {
                // The index taken by this function is expected to be already biased by BaseIndex.
                // Thus, m_Heap[size] is a valid element (specifically, the final element in the heap)
                //Debug.Assert(i >= BaseIndex && i < (_size+BaseIndex), $"heap index {i} is out of range with size={_size}");
                var val = _elements[i];
                while (i <= _size / 2)
                {
                    var child = 2 * i;
                    if (child < _size && _elements[child + 1].CompareTo(_elements[child]) < 0)
                    {
                        child++;
                    }

                    if (val.CompareTo(_elements[child]) < 0)
                    {
                        break;
                    }

                    _elements[i] = _elements[child];
                    i = child;
                }

                _elements[i] = val;
            }
        }

        public struct TypeHeapElement : IComparable<TypeHeapElement>
        {
            private readonly NativeText.ReadOnly typeName;
            public int unsortedIndex;

            public TypeHeapElement(int index, Type t)
            {
                unsortedIndex = index;
                typeName = TypeManager.GetSystemName(t);
            }

            public int CompareTo(TypeHeapElement other)
            {
                // Workaround for missing string.CompareTo() in HPC#. This is not a fully compatible substitute,
                // but should be suitable for comparing system names.
                if (typeName.Length < other.typeName.Length)
                    return -1;
                if (typeName.Length > other.typeName.Length)
                    return 1;
                for (int i = 0; i < typeName.Length; ++i)
                {
                    if (typeName[i] < other.typeName[i])
                        return -1;
                    if (typeName[i] > other.typeName[i])
                        return 1;
                }
                return unsortedIndex.CompareTo(other.unsortedIndex);
            }
        }

        private static Dictionary<Type, int> lookupDictionary = null;
        private static int LookupSystemElement(Type t, SystemElement[] elements)
        {
            if (lookupDictionary == null)
            {
                lookupDictionary = new Dictionary<Type, int>();
                for (int i = 0; i < elements.Length; ++i)
                {
                    lookupDictionary[elements[i].Type] = i;
                }
            }

            if (lookupDictionary.ContainsKey(t))
                return lookupDictionary[t];
            return -1;
        }

        internal struct SystemElement
        {
            public Type Type;
            public UpdateIndex Index;
            public int OrderingBucket; // 0 = OrderFirst, 1 = none, 2 = OrderLast
            public List<Type> updateBefore;
            public int nAfter;
        }

        internal static void Sort(SystemElement[] elements)
        {
            lookupDictionary = null;
            var sortedElements = new SystemElement[elements.Length];
            int nextOutIndex = 0;

            var readySystems = new Heap(elements.Length);
            for (int i = 0; i < elements.Length; ++i)
            {
                if (elements[i].nAfter == 0)
                {
                    readySystems.Insert(new TypeHeapElement(i, elements[i].Type));
                }
            }

            while (!readySystems.Empty)
            {
                var sysIndex = readySystems.Extract().unsortedIndex;
                var elem = elements[sysIndex];

                sortedElements[nextOutIndex++] = new SystemElement
                {
                    Type = elem.Type,
                    Index = elem.Index,
                };
                foreach (var beforeType in elem.updateBefore)
                {
                    int beforeIndex = LookupSystemElement(beforeType, elements);
                    if (beforeIndex < 0) throw new Exception("Bug in SortSystemUpdateList(), beforeIndex < 0");
                    if (elements[beforeIndex].nAfter <= 0)
                        throw new Exception("Bug in SortSystemUpdateList(), nAfter <= 0");

                    elements[beforeIndex].nAfter--;
                    if (elements[beforeIndex].nAfter == 0)
                    {
                        readySystems.Insert(new TypeHeapElement(beforeIndex, elements[beforeIndex].Type));
                    }
                }
                elements[sysIndex].nAfter = -1; // "Remove()"
            }

#if ENABLE_UNITY_COLLECTIONS_CHECKS || UNITY_DOTS_DEBUG
            for (int i = 0; i < elements.Length; ++i)
            {
                if (elements[i].nAfter != -1)
                {
                    // Since no System in the circular dependency would have ever been added
                    // to the heap, we should have values for everything in sysAndDep. Check,
                    // just in case.
                    var visitedSystems = new List<Type>();
                    var startIndex = i;
                    var currentIndex = i;
                    while (true)
                    {
                        if (elements[currentIndex].nAfter != -1)
                            visitedSystems.Add(elements[currentIndex].Type);

                        currentIndex = LookupSystemElement(elements[currentIndex].updateBefore[0], elements);
                        if (currentIndex < 0 || currentIndex == startIndex || elements[currentIndex].nAfter == -1)
                        {
                            throw new CircularSystemDependencyException(visitedSystems);
                        }
                    }
                }
            }
#endif

            // Replace input array with sorted array
            for (int i = 0; i < elements.Length; ++i)
                elements[i] = sortedElements[i];
        }

        static string SysName(Type stype)
        {
            if (!TypeManager.IsSystemType(stype))
            {
#if !NET_DOTS
                // in the editor or with full .NET, return the full name to make it easy to find types
                return stype.FullName;
#else
                return "(unknown type / type not inheriting from SystemBase/ComponentSystem)";
#endif
            }

            return TypeManager.GetSystemName(stype).ToString();
        }

        internal static void FindConstraints(Type parentType, SystemElement[] sysElems)
        {
            lookupDictionary = null;

            for (int i = 0; i < sysElems.Length; ++i)
            {
                var systemType = sysElems[i].Type;

                var before = TypeManager.GetSystemAttributes(systemType, typeof(TBeforeAttribute));
                var after = TypeManager.GetSystemAttributes(systemType, typeof(TAfterAttribute));
                foreach (var attr in before)
                {
                    var dep = attr as TBeforeAttribute;

                    if (CheckBeforeConstraints(parentType, dep, systemType))
                        continue;

                    int depIndex = LookupSystemElement(dep.SystemType, sysElems);
                    if (depIndex < 0)
                    {
                        Debug.LogWarning(
                            $"Ignoring invalid [{typeof(TBeforeAttribute)}] attribute on {SysName(systemType)} targeting {SysName(dep.SystemType)}.\n"
                            + $"This attribute can only order systems that are members of the same {nameof(ComponentSystemGroup)} instance.\n"
                            + $"Make sure that both systems are in the same system group with [UpdateInGroup(typeof({SysName(parentType)}))],\n"
                            + $"or by manually adding both systems to the same group's update list.");
                        continue;
                    }

                    sysElems[i].updateBefore.Add(dep.SystemType);
                    sysElems[depIndex].nAfter++;
                }

                foreach (var attr in after)
                {
                    var dep = attr as TAfterAttribute;

                    if (CheckAfterConstraints(parentType, dep, systemType))
                        continue;

                    int depIndex = LookupSystemElement(dep.SystemType, sysElems);
                    if (depIndex < 0)
                    {
                        Debug.LogWarning(
                            $"Ignoring invalid [{typeof(TAfterAttribute)}] attribute on {SysName(systemType)} targeting {SysName(dep.SystemType)}.\n"
                            + $"This attribute can only order systems that are members of the same {nameof(ComponentSystemGroup)} instance.\n"
                            + $"Make sure that both systems are in the same system group with [UpdateInGroup(typeof({SysName(parentType)}))],\n"
                            + $"or by manually adding both systems to the same group's update list.");
                        continue;
                    }

                    sysElems[depIndex].updateBefore.Add(systemType);
                    sysElems[i].nAfter++;
                }
            }
        }

        private static bool CheckBeforeConstraints(Type parentType, TBeforeAttribute dep, Type systemType)
        {
            if (!typeof(ComponentSystemBase).IsAssignableFrom(dep.SystemType) && !typeof(ISystem).IsAssignableFrom(dep.SystemType))
            {
                Debug.LogWarning(
                    $"Ignoring invalid [{typeof(TBeforeAttribute)}] attribute on {SysName(systemType)} because {SysName(dep.SystemType)} is not a subclass of {nameof(ComponentSystemBase)} or {nameof(ISystem)}.\n"
                    + $"Set the target parameter of [{typeof(TBeforeAttribute)}] to a system class in the same {nameof(ComponentSystemGroup)} as {SysName(systemType)}.");
                return true;
            }

            if (dep.SystemType == systemType)
            {
                Debug.LogWarning(
                    $"Ignoring invalid [{typeof(TBeforeAttribute)}] attribute on {SysName(systemType)} because a system cannot be updated before itself.\n"
                    + $"Set the target parameter of [{typeof(TBeforeAttribute)}] to a different system class in the same {nameof(ComponentSystemGroup)} as {SysName(systemType)}.");
                return true;
            }

            int systemBucket = ComponentSystemGroup.ComputeSystemOrdering(systemType, parentType);
            int depBucket = ComponentSystemGroup.ComputeSystemOrdering(dep.SystemType, parentType);
            if (depBucket > systemBucket)
            {
                // This constraint is redundant, but harmless; it is accounted for by the bucketing order, and can be quietly ignored.
                return true;
            }
            if (depBucket < systemBucket)
            {
                Debug.LogWarning(
                    $"Ignoring invalid [{typeof(TBeforeAttribute)}({SysName(dep.SystemType)})] attribute on {SysName(systemType)} because OrderFirst/OrderLast has higher precedence.");
                return true;
            }

            return false;
        }

        private static bool CheckAfterConstraints(Type parentType, TAfterAttribute dep, Type systemType)
        {
            if (!typeof(ComponentSystemBase).IsAssignableFrom(dep.SystemType) && !typeof(ISystem).IsAssignableFrom(dep.SystemType))
            {
                Debug.LogWarning(
                    $"Ignoring invalid [{typeof(TAfterAttribute)}] attribute on {SysName(systemType)} because {SysName(dep.SystemType)} is not a subclass of {nameof(ComponentSystemBase)} or {nameof(ISystem)}.\n"
                    + $"Set the target parameter of [{typeof(TAfterAttribute)}] to a system class in the same {nameof(ComponentSystemGroup)} as {SysName(systemType)}.");
                return true;
            }

            if (dep.SystemType == systemType)
            {
                Debug.LogWarning(
                    $"Ignoring invalid [{typeof(TAfterAttribute)}] attribute on {SysName(systemType)} because a system cannot be updated after itself.\n"
                    + $"Set the target parameter of [{typeof(TAfterAttribute)}] to a different system class in the same {nameof(ComponentSystemGroup)} as {SysName(systemType)}.");
                return true;
            }

            int systemBucket = ComponentSystemGroup.ComputeSystemOrdering(systemType, parentType);
            int depBucket = ComponentSystemGroup.ComputeSystemOrdering(dep.SystemType, parentType);
            if (depBucket < systemBucket)
            {
                // This constraint is redundant, but harmless; it is accounted for by the bucketing order, and can be quietly ignored.
                return true;
            }
            if (depBucket > systemBucket)
            {
                Debug.LogWarning(
                    $"Ignoring invalid [{typeof(TAfterAttribute)}({SysName(dep.SystemType)})] attribute on {SysName(systemType)} because OrderFirst/OrderLast has higher precedence.");
                return true;
            }

            return false;
        }
    }
}
