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
    using System.Collections.Generic;

    public class FluentRegistration
    {
        internal readonly ComponentStore store;
        internal IRegistration componentRegistration = new Registration();

        private readonly IList<ICommand> commands = new List<ICommand>();

        internal RegistrationWriter Writer
        {
            get { return new RegistrationWriter(componentRegistration); }
        }

        public FluentRegistration(ComponentStore store)
        {
            this.store = store;
        }

        public FluentServiceOptions<T> Service<T>()
        {
            return Service<T>(null);
        }

        public FluentServiceOptions<T> Service<T>(string name)
        {
            componentRegistration = new Registration {Name = name, Service = typeof (T)};
            store.AddRegistration(componentRegistration);
            return new FluentServiceOptions<T>(this);
        }

        public AutoConfiguration AutoConfigure
        {
            get
            {
                throw new NotImplementedException();
/*                return new AutoConfiguration(this);*/
            }
        }

        public GenericServiceOptions Generic(Type generic, string name)
        {
            var command = new GenericServiceCommand
                              {
                                  Service = generic, 
                                  Name = name
                              };
            commands.Add(command);
            return new GenericServiceOptions(command);
        }

        public GenericServiceOptions Generic(Type generic)
        {
            return Generic(generic, null);
        }

        internal void Commit()
        {
            foreach (var list in commands)
            {
                list.Execute(store);
            }
        }
    }

    internal interface ICommand
    {
        void Execute(ComponentStore store);
    }

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

        public void Execute(ComponentStore store)
        {
            foreach (var type in ForTypes)
            {
                var reg = new Registration()
                              {
                                  Name = Name,
                                  Service = Service.MakeGenericType(new[] {type}),
                                  Implementor = Implementor.MakeGenericType(new[] {type})
                              };
                store.AddRegistration(reg);
            }
        }
    }

    public class GenericServiceOptions
    {
        private readonly GenericServiceCommand command;

        public GenericServiceOptions(GenericServiceCommand command)
        {
            this.command = command;
        }

        public GenericImplementorOptions Implementor(Type implementor)
        {
            command.Implementor = implementor;
            return new GenericImplementorOptions(command);
        }
    }

    public class GenericImplementorOptions
    {
        private readonly GenericServiceCommand command;

        public GenericImplementorOptions(GenericServiceCommand command)
        {
            this.command = command;
        }

        public GenericImplementorOptions ForTypes(params Type[] types)
        {
            command.ForTypes = types;
            return this;
        }
    }
}