using System;

namespace IniSerializer2
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class IniSectionAttribute: Attribute
    {
         public string ElementName { get; set; }
    }


    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class IniKeyAttribute : Attribute
    {
        public string ElementName { get; set; }
    }
}