using Xunit;
using System;
using System.Linq;
using Task05;

namespace Task05Tests
{
    public class TestClass
    {
        public int PublicField;
        private string _privateField = string.Empty;
        public int Property { get; set; } = 0;

        public void Method(int input) { }
    }

    [Serializable]
    public class AttributedClass { }

    public class ClassAnalyzerTests
    {
        [Fact]
        public void GetPublicMethods_ReturnsCorrectMethods()
        {
            var analyzer = new ClassAnalyzer(typeof(TestClass));
            var methods = analyzer.GetPublicMethods();

            Assert.Contains("Method", methods);
        }

        [Fact]
        public void GetAllFields_IncludesPrivateFields()
        {
            var analyzer = new ClassAnalyzer(typeof(TestClass));
            var fields = analyzer.GetAllFields();

            Assert.Contains("_privateField", fields);
            Assert.Contains("PublicField", fields);
        }

        [Fact]
        public void GetProperties_ReturnsCorrectProperties()
        {
            var analyzer = new ClassAnalyzer(typeof(TestClass));
            var properties = analyzer.GetProperties();

            Assert.Contains("Property", properties);
        }

        [Fact]
        public void GetMethodParams_ReturnsCorrectSignature()
        {
            var analyzer = new ClassAnalyzer(typeof(TestClass));
            var result = analyzer.GetMethodParams("Method").FirstOrDefault();

            Assert.NotNull(result);
            Assert.Contains("input: Int32", result);
            Assert.Contains("Returns: Void", result);
        }

        [Fact]
        public void HasAttribute_ReturnsTrueWhenAttributeExists()
        {
            var analyzer = new ClassAnalyzer(typeof(AttributedClass));
            Assert.True(analyzer.HasAttribute<SerializableAttribute>());
        }

        [Fact]
        public void HasAttribute_ReturnsFalseWhenAttributeMissing()
        {
            var analyzer = new ClassAnalyzer(typeof(TestClass));
            Assert.False(analyzer.HasAttribute<SerializableAttribute>());
        }
    }
}