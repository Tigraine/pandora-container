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
    using Lifestyles;

    public class FluentLifestyleOptions<T>
    {
        private readonly NormalRegistrationCommand command;
        private readonly FluentImplementorOptions<T> parentOptions;

        public FluentLifestyleOptions(NormalRegistrationCommand command, FluentImplementorOptions<T> parentOptions)
        {
            this.command = command;
            this.parentOptions = parentOptions;
        }

        public FluentImplementorOptions<T> Singleton()
        {
            command.Lifestyle = new SingletonLifestyle();
            return parentOptions;
        }
        public FluentImplementorOptions<T> Transient()
        {
            command.Lifestyle = new TransientLifestyle();
            return parentOptions;
        }

        public FluentImplementorOptions<T> Custom(ILifestyle lifestyle)
        {
            command.Lifestyle = lifestyle;
            return parentOptions;
        }
    }
}