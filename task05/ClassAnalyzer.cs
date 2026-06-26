using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace Task05
{
    public class ClassAnalyzer
    {
        private readonly Type _type;

        public ClassAnalyzer(Type type)
        {
            _type = type;
        }

        public IEnumerable<string> GetPublicMethods()
        {
            return _type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                        .Select(m => m.Name);
        }

        public IEnumerable<string> GetMethodParams(string methodName)
        {
            var method = _type.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance);
            
            if (method == null) 
                return Enumerable.Empty<string>();

            string parametersInfo = string.Join(", ", method.GetParameters()
                .Select(p => $"{p.Name}: {p.ParameterType.Name}"));

            return new[] { $"{parametersInfo} -> Returns: {method.ReturnType.Name}" };
        }

        public IEnumerable<string> GetAllFields()
        {
            return _type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                        .Select(f => f.Name);
        }

        public IEnumerable<string> GetProperties()
        {
            return _type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Select(p => p.Name);
        }

        public bool HasAttribute<T>() where T : Attribute
        {
            return _type.GetCustomAttribute<T>() != null;
        }
    }
}