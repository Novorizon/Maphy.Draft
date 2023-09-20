using Unity.Entities.SourceGen.SystemGenerator;
using Xunit;

public class EntityQueryErrorTests
{
    [Fact]
    public void DC0062_EntityQueryInvalidMethod()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        using Unity.Entities;

        public partial class MySystem : SystemBase
        {
            struct Foo : IComponentData {}

            void MyTest()
            {
                Entities.WithAll<Foo>().WithName(""Bar"").DestroyEntity();
            }

            protected override void OnUpdate() {}
        }", "DC0062");
    }
}
