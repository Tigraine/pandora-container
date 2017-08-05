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

    public class FluentParameterOptions<T>
    {
        private readonly NormalRegistrationCommand command;
        private readonly FluentImplementorOptions<T> parentOptions;
        private readonly string parameterName;

        public FluentParameterOptions(NormalRegistrationCommand command, FluentImplementorOptions<T> parentOptions, string parameterName)
        {
            this.command = command;
            this.parentOptions = parentOptions;
            this.parameterName = parameterName;
        }

        public FluentImplementorOptions<T> Set(string value)
        {
            if (!command.Parameters.ContainsKey(parameterName))
                command.Parameters.Add(parameterName, value);
            else
                throw new ArgumentException(String.Format("Parametername {0} was already registered", parameterName));
            return parentOptions;
        }
    }
}