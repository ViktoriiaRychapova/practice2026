using System;

namespace task07 
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property)]
    public class DisplayNameAttribute : Attribute
    {
        public string DisplayName { get; }

        public DisplayNameAttribute(string displayName)
        {
            DisplayName = displayName;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class VersionAttribute : Attribute
    {
        public int Major { get; }
        public int Minor { get; }

        public VersionAttribute(int major, int minor)
        {
            Major = major;
            Minor = minor;
        }
    }
}