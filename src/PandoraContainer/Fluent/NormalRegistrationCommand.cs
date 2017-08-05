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

namespace PandoraContainer.Fluent
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Lifestyles;

    public class NormalRegistrationCommand : ICommand
    {
        public string Name { get; set; }
        public Type Service { get; set; }
        public Type Implementor { get; set; }
        public ILifestyle Lifestyle { get; set; }
        public IDictionary<string, string> Parameters = new Dictionary<string, string>();

        public void Execute(IComponentStore store)
        {
            var reg = new Registration
                          {
                              Name = Name,
                              Implementor = Implementor,
                              Service = Service,
                              Parameters = Parameters,
                          };
            if (Lifestyle != null)
                reg.Lifestyle = Lifestyle;
            store.AddRegistration(reg);
        }

        public void SetInstance(object instance)
        {
            Lifestyle = new InjectedInstanceLifestyle(instance);
        }
    }
}