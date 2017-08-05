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
    using System.Linq;
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
            return null;
        }

        public virtual IRegistration LookupType(Query query, ResolverContext context)
        {
            IList<IRegistration> localParents = new List<IRegistration>(context.UsedRegistrations);
            var registrations = componentStore.GetRegistrationsForService(query.ServiceType);
            if (query.Name != null)
            {
                var @default = registrations.SingleOrDefault(p => p.Name == query.Name);
                if (@default == null)
                    throw new ServiceNotFoundException(query.ServiceType, query.Name);
                return @default;
            }
            var registration = SkipParents(registrations, localParents);
            if (registration == null)
                return null;
            context.ConsumeRegistration(registration);
            return registration;
        }
    }
}