using System;
using System.Collections.Generic;

namespace mg3.ioc
{
    public class TestContainer
    {
        public delegate object Creator(TestContainer container);

        private readonly Dictionary<string, object> configuration = new Dictionary<string, object>();
        private readonly Dictionary<Type, Creator> typeToCreator = new Dictionary<Type, Creator>();

        public Dictionary<string, object> Configuration
        {
            get { return configuration; }
        }

        public void Register<T>(Creator creator)
        {
            typeToCreator.Add(typeof(T), creator);
        }

        public T Create<T>()
        {
            return (T)typeToCreator[typeof(T)](this);
        }

        public T GetConfiguration<T>(string name)
        {
            return (T)configuration[name];
        }
    }
}
