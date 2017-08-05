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

namespace PandoraContainer.Lifestyles
{
    using System;


    public class SingletonLifestyle : ILifestyle
    {
        private object singleton;
        private readonly object lockObject = new object();
        public object Execute(Func<object> action)
        {
            if (singleton == null)
            {
                lock (lockObject)
                {
                    if (singleton == null)
                    {
                        singleton = action();
                    }
                }
            }
            return singleton;
        }

        protected void SetSingleton(object instance)
        {
            lock (lockObject)
            {
                singleton = instance;
            }
        }

    }
}