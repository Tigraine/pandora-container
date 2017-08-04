/*
 * Copyright 2009 Daniel Hölbling - http://www.tigraine.at
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *  http://www.apache.org/licenses/LICENSE-2.0
 *  
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

namespace Pandora.Fluent
{
    using System.Reflection;

    public class AutoConfiguration
    {
        private readonly FluentRegistration registration;
        private readonly AutoConfigCommand command = new AutoConfigCommand();

        public AutoConfiguration(FluentRegistration registration)
        {
            this.registration = registration;
        }

        public AssemblyAutoConfigOptions FromAssembly(Assembly assembly)
        {
            var options = new AssemblyAutoConfigOptions(command, assembly);
            command.Execute(registration);
            return options;
        }
    }

    public class AutoConfigCommand
    {
        public Assembly FromAssembly { get; set; }
        public void Execute(FluentRegistration registration)
        {
            var types = FromAssembly.GetTypes();
            foreach (var type in types)
            {
#if NET35
                if (type.IsInterface) continue;
                if (type.IsAbstract) continue;
#else
                if (type.GetTypeInfo().IsInterface) continue;
                if (type.GetTypeInfo().IsAbstract) continue;
#endif
                var typeRegistration = new Registration
                                           {
                                               Service = type,
                                               Implementor = type,
                                           };
                registration.store.AddRegistration(typeRegistration);
            }
        }
    }
    public class AssemblyAutoConfigOptions
    {
        public AssemblyAutoConfigOptions(AutoConfigCommand command, Assembly assembly)
        {
            command.FromAssembly = assembly;
        }
    }
}