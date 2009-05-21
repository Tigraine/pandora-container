using System;
using System.Collections.Generic;
using System.Reflection;

namespace Pandora.Tests.Mocks
{
    public class ComponentStoreStub : IComponentStore
    {
        IDictionary<string, int> callsToMethod = new Dictionary<string, int>();

        public int GetCallCount(string methodName)
        {
            if (callsToMethod.ContainsKey(methodName))
                return callsToMethod[methodName];
            return 0;
        }

        private IList<Type> getResult = new List<Type>();

        public void AddResultForGet(Type result)
        {
            getResult.Add(result);
        }

        public void Add<T, TType>() where T : class where TType : T
        {
            var methodName = MethodBase.GetCurrentMethod().Name;
            CountMethodCall(methodName);
        }

        private void CountMethodCall(string methodName)
        {
            if (!callsToMethod.ContainsKey(methodName))
                callsToMethod.Add(methodName, 0);
            callsToMethod[methodName]++;
        }

        public Type Get<T>() where T : class
        {
            var methodName = MethodBase.GetCurrentMethod().Name;
            CountMethodCall(methodName);

            var result = getResult[0];
            getResult.RemoveAt(0);

            return result;
        }
    }
}