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

namespace PandoraContainer
{
    using System;
    using System.Collections.Generic;
    using Fluent;

    public interface IComponentStore
    {
        IRegistration Add<T, TType>()
            where TType : T;

        IRegistration Add<T, TType>(string name)
            where TType : T;

        IList<IRegistration> GetRegistrationsForService<T>();

        IList<IRegistration> GetRegistrationsForService(Type type);
        IRegistration AddInstance<T>(string name, T instance);
        IRegistration AddInstance<T>(T instance);
        void AddRegistration(IRegistration registration);
        void Register(Action<FluentRegistration> registrationClosure);
    }
}