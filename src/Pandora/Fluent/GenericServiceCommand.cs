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
    using System;
    using System.Linq;

    public class GenericServiceCommand : ICommand
    {
        public string Name { get; set;}
        private Type service;
        public Type Service
        {
            get { return service; }
            set
            {
                if (!value.IsGenericType)
                    throw new ArgumentException(String.Format("Type {0} is not generic and thus cannot be used as a generic registry", value.FullName));
                service = value;
            }
        }

        private Type implementor;
        public Type Implementor
        {
            get { return implementor; }
            set
            {
                if (!value.IsGenericType)
                    throw new ArgumentException(String.Format("Type {0} is not generic and thus cannot be used as a generic registry", value.FullName));
                implementor = value;
            }
        }

        public Type[] ForTypes { get; set; }

        public void Execute(IComponentStore store)
        {
            foreach (var type in ForTypes)
            {
                var reg = new Registration
                              {
                                  Name = Name,
                                  Service = Service.MakeGenericType(new[] {type}),
                                  Implementor = Implementor.MakeGenericType(new[] {type})
                              };
                store.AddRegistration(reg);
            }
        }
    }
}