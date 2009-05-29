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
    using System.Collections.Generic;

    /// <summary>
    /// Holds information about the ongoing resolve process (what types have been used etc)
    /// </summary>
    public class ResolverContext
    {
        private IList<IRegistration> usedRegistrations;
        public IEnumerable<IRegistration> UsedRegistrations
        {
            get
            {
                return usedRegistrations;
            }
        }

        public ResolverContext()
        {
            usedRegistrations = new List<IRegistration>();
        }

        public virtual void ConsumeRegistration(IRegistration registration)
        {
            usedRegistrations.Add(registration);
        }

        public static ResolverContext CreateContextFromContext(ResolverContext context)
        {
            var resolverContext = new ResolverContext();
            foreach(var usedRegistration in context.UsedRegistrations)
            {
                resolverContext.ConsumeRegistration(usedRegistration);
            }
            return resolverContext;
        }
    }
}