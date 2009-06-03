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

namespace Pandora
{
    public class RegistrationParameter : IRegistrationParameter
    {
        private readonly IRegistration parentRegistry;

        private readonly string parameterName;
        private string parameterValue;

        public RegistrationParameter(IRegistration parentRegistry, string parameterName)
        {
            this.parentRegistry = parentRegistry;
            this.parameterName = parameterName;
        }

        public string ParameterName
        {
            get { return parameterName; }
        }

        public string ParameterValue
        {
            get { return parameterValue; }
        }

        public IRegistration Set(string value)
        {
            parameterValue = value;
            return parentRegistry;
        }
    }
}