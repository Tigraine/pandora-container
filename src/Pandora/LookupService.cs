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
    using System;
    using System.Collections.Generic;

    public class LookupService : IComponentLookup
    {
        private readonly IComponentStore componentStore;

        public LookupService(IComponentStore componentStore)
        {
            this.componentStore = componentStore;
        }

        private IRegistration SkipParents(IEnumerable<IRegistration> candidates, ICollection<IRegistration> parents)
        {
            foreach (var candidate in candidates)
            {
                if (!parents.Contains(candidate)) return candidate;
            }
            throw new KeyNotFoundException();
        }

        public virtual IRegistration LookupType(Type targetType, ResolverContext context)
        {
            IList<IRegistration> localParents = new List<IRegistration>(context.UsedRegistrations);
            try
            {
                var registrations = componentStore.GetRegistrationsForService(targetType);
                var registration = SkipParents(registrations, localParents);
                context.ConsumeRegistration(registration);
                return registration;
            }
            catch (KeyNotFoundException)
            {
                throw new ServiceNotFoundException(targetType.FullName);
            }
        }
    }
}