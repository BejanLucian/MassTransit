namespace MassTransit.LegacySupport.SerializationCustomization
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.Serialization;

    public class LegacyBinder :
        SerializationBinder
    {
        Dictionary<string, Type> _map = new Dictionary<string, Type>();

        public void AddMap(string oldTypeName, Type newType)
        {
            _map.Add(oldTypeName, newType);
        }

        public override Type BindToType(string assemblyName, string typeName)
        {
            if(_map.ContainsKey(typeName))
                return _map[typeName];

            try
            {
                Assembly ass = Assembly.Load(assemblyName);
                return ass.GetType(typeName);
            }
            catch (Exception ex)
            {   
                throw new LegacySerializationException("Failed serializing {0}, {1}".FormatWith(assemblyName, typeName), ex);
            }
            
        }
    }

    [Serializable]
    public class LegacySerializationException :
        Exception
    {
        public LegacySerializationException()
        {
        }

        public LegacySerializationException(string message) : base(message)
        {
        }

        public LegacySerializationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LegacySerializationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}