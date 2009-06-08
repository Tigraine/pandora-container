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
            var command = new NormalRegistrationCommand();
            command.Name = name;
            command.Service = typeof (T);
            commands.Add(command);
            return new FluentServiceOptions<T>(command);
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
}