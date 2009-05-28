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
using System;

namespace Pandora
{
    public class Registration : IRegistration
    {
        public Guid Guid {get;private set;}
        public Type Service { get; set; }
        public Type Implementor { get; set; }
        public string Name { get; set; }

        public Registration()
        {
            Guid = Guid.NewGuid();
        }

        private bool Equals(Registration other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Guid.Equals(Guid);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Registration)) return false;
            return Equals((Registration) obj);
        }

        public override int GetHashCode()
        {
            return Guid.GetHashCode();
        }
    }
}