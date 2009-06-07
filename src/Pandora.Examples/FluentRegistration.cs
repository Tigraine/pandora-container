namespace Pandora.Examples
{
    using Tests.Testclasses;

    public class FluentRegistration
    {
        public void ContainerSetup()
        {
            var store = new ComponentStore();
            //Add components to the store.
            store.Register(p => p.Service<IService>()
                .Implementor<ClassWithNoDependencies>());
            //Create the container
            var container = new PandoraContainer(store);
        }

        public void RegisterSimpleTypeFluently()
        {
            var store = new ComponentStore();

            /*
             * Register ClassWithNoDependencies implementing 
             * IService with default Lifestyle and no Parameters
             */
            store.Register(p => p.Service<IService>()
                                    .Implementor<ClassWithNoDependencies>());
        }

        public void RegisterNamedType()
        {
            var store = new ComponentStore();

            /*
             * Every registration can be assigned a unique name to 
             * be able to retrieve it in case there is more than one IService
             * See PandoraContainer.Resolve<T>(string)
             */
            store.Register(p => p.Service<IService>("componentName")
                                    .Implementor<ClassWithNoDependencies>());
        }

        public void RegisterMultipleServicesInOneClosure()
        {
            var store = new ComponentStore();
            store.Register(p =>
                               {
                                   p.Service<IService>()
                                       .Implementor<ClassWithDependencyOnItsOwnService>();
                                   p.Service<IService>()
                                       .Implementor<ClassWithNoDependencies>();
                               });
        }

        public void SpecifyLifestyle()
        {
            var store = new ComponentStore();

            /*
             * After Implementor was set Lifestyle can be specified through:
             * .Lifestyle.Singleton
             */
            store.Register(p => p.Service<IService>()
                                    .Implementor<ClassWithNoDependencies>()
                                    .Lifestyle.Singleton());

            //or:

            store.Register(p => p.Service<IService>()
                                    .Implementor<ClassWithNoDependencies>()
                                    .Lifestyle.Transient());
        }

        public void CustomLifestyle()
        {
            /*
             * Any instance implementing ILifestyle can be passed to the container.
             */
            var store = new ComponentStore();
            var myLifestyle = new CustomLifestyle();

            store.Register(p => p.Service<IService>()
                                    .Implementor<ClassWithNoDependencies>()
                                    .Lifestyle.Custom(myLifestyle));
        }

        public void ParameterSpecification()
        {
            var store = new ComponentStore();

            /*
             * Any number of parameters can be specified after .Implementor
             * Note: Parameter values have to be registered components in the container.
             */

            store.Register(p => 
                           p.Service<IService>()
                               .Implementor<ClassWithNoDependencies>()
                               .Parameters("param1").Set("componentName")
                               .Parameters("param2").Set("somethingElse"));
        }

        /*
         * This also means that if a component requires a string this String has to be specified explicitly
         */
        public void SpecifyStringParameter()
        {
            var store = new ComponentStore();
            var myString = "The string to be passed to ClassDependingOnAString";
            store.Register(p => 
                               {
                                   p.Service<ClassDependingOnAString>()
                                       .Implementor<ClassDependingOnAString>()
                                       .Parameters("dependency").Equals("stringComponentName");
                                   p.Service<string>("stringComponentName")
                                       .Instance(myString);
                               });
        }
    }
}