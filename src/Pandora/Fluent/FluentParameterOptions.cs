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
    public class FluentParameterOptions<T>
    {
        private readonly FluentRegistration registration;
        private readonly FluentServiceOptions<T> parentOptions;
        private readonly string parameterName;

        public FluentParameterOptions(FluentRegistration registration, FluentServiceOptions<T> parentOptions, string parameterName)
        {
            this.registration = registration;
            this.parentOptions = parentOptions;
            this.parameterName = parameterName;
        }

        public FluentServiceOptions<T> Set(string value)
        {
            registration.componentRegistration.Parameters[parameterName] = value;
            return parentOptions;
        }
    }
}