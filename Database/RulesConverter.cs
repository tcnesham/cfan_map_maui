using System;
using System.Linq;
using CFAN.SchoolMap.Model;
using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Converters;

namespace CFAN.SchoolMap.Database
{
    public class RolesConverter : DocumentConverter<Role[]>
    {
        public RolesConverter(Type targetType, Role[] arg1) : base(targetType, arg1) {}

        public override bool ConvertFrom(DocumentObject value, out object? result)
        {
            if (value.Type == DocumentObjectType.List)
            {
                result = value.List.Select(v=>(Role)v.Int64).ToArray();
                return true;
            }
            result = new Role[0];
            return false;
        }

        public override bool ConvertTo(object? value, out object? result)
        {
            if (value is Role[] a)
            {
                result = "";//new DocumentObject();
                return true;
            }
            result = ""; //new DocumentObject()
            return false;
        }
    }
}

