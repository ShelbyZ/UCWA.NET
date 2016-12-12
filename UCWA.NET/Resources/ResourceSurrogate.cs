using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace UCWA.NET.Resources
{
    public class ResourceSurrogate : IDataContractSurrogate
    {
        public object GetCustomDataToExport(Type clrType, Type dataContractType)
        {
            throw new NotImplementedException();
        }

        public object GetCustomDataToExport(MemberInfo memberInfo, Type dataContractType)
        {
            throw new NotImplementedException();
        }

        public Type GetDataContractType(Type type)
        {
            if (type == typeof(Communication))
            {
                return typeof(Dictionary<string, object>);
            }

            return type;
        }

        public object GetDeserializedObject(object obj, Type targetType)
        {
            if (obj.GetType() == typeof(Dictionary<string, object>) && targetType == typeof(Communication))
            {
                var dictionary = obj as Dictionary<string, object>;
                var targetObject = Activator.CreateInstance(targetType);
                foreach (var item in GetProperties(targetType))
                {
                    var attr = item.GetCustomAttribute<DataMemberAttribute>();
                    object value;
                    if (dictionary.TryGetValue(attr.Name, out value))
                    {
                        if (item.PropertyType == typeof(string[]))
                        {
                            item.SetValue(targetObject, Array.ConvertAll(value as object[], s => (string)s));
                        }
                        else
                        {
                            item.SetValue(targetObject, value);
                        }
                        dictionary.Remove(attr.Name);
                    }
                }

                var resource = targetObject as Communication;
                foreach (var item in dictionary)
                {
                    //resource.SetExtraData(item);
                }

                return targetObject;
            }

            return obj;
        }

        public void GetKnownCustomDataTypes(Collection<Type> customDataTypes)
        {
        }

        public object GetObjectToSerialize(object obj, Type targetType)
        {
            return obj;
        }

        public Type GetReferencedTypeOnImport(string typeName, string typeNamespace, object customData)
        {
            throw new NotImplementedException();
        }

        public CodeTypeDeclaration ProcessImportedType(CodeTypeDeclaration typeDeclaration, CodeCompileUnit compileUnit)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<PropertyInfo> GetProperties(Type type)
        {
            return from p in type.GetProperties()
                   where p.CanRead && p.CanWrite && p.GetCustomAttributes<DataMemberAttribute>() != null
                   select p;
        }
    }
}
