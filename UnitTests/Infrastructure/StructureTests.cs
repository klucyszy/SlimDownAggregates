using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.Fluent;
using Assembly = System.Reflection.Assembly;

//add a using directive to ArchUnitNET.Fluent.ArchRuleDefinition to easily define ArchRules
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace UnitTests.Infrastructure;

// TO FIX: THIS IS NOT WORKING
public class StructureTests
{
    private static readonly Architecture Architecture = new ArchLoader()
        .LoadAssemblies(
            Assembly.Load("LibraryMembership"))
        .Build();
    
    private readonly IObjectProvider<IType> DomainTypes =
        Types().That().ResideInNamespace("LibraryMembership.Slimmed.Domain").As("Domain types");
    
    private readonly IObjectProvider<IType> ApplicationTypes =
        Types().That().ResideInNamespace("LibraryMembership.Slimmed.Application").As("Application types");
    
    private readonly IObjectProvider<IType> InfrastructureTypes =
        Types().That().ResideInNamespace("LibraryMembership.Slimmed.Infrastructure").As("Infrastructure types");
    
    private readonly IObjectProvider<IType> PresentationTypes =
        Types().That().ResideInNamespace("LibraryMembership.Slimmed.Presentation").As("Presentation types");
    
    [Fact]
    public void DomainShouldNotHaveDependencyOnOtherLayers()
    {
 
        IArchRule domainTypesShouldNotDependOnApplicationLayer = Types().That().Are(DomainTypes)
            .Should().NotDependOnAny(ApplicationTypes)
            .As("Domain should not have dependency on Application layer");
        
        IArchRule domainTypesShouldNotDependOnInfrastructureLayer = Types().That().Are(DomainTypes)
            .Should().NotDependOnAny(InfrastructureTypes)
            .As("Domain should not have dependency on Infrastructure layer");
        
        IArchRule domainTypesShouldNotDependOnPresentationLayer = Types().That().Are(DomainTypes)
            .Should().NotDependOnAny(PresentationTypes)
            .As("Domain should not have dependency on Presentation layer");

        bool result = domainTypesShouldNotDependOnApplicationLayer
            .And(domainTypesShouldNotDependOnInfrastructureLayer)
            .And(domainTypesShouldNotDependOnPresentationLayer)
            .HasNoViolations(Architecture);

        Assert.True(result, "Domain should not have dependency on other layers");
    }
    
    [Fact]
    public void DomainShouldNotDependOnOtherNamespaces()
    {
        IArchRule domainTypesShouldOnlyDependOnDomainNamespace = Types().That().Are(DomainTypes)
            .Should().NotDependOnAny(Types().That().ResideInNamespace("LibraryMembership.Slimmed.Application"))
            .AndShould().NotDependOnAny(Types().That().ResideInNamespace("LibraryMembership.Slimmed.Infrastructure"))
            .AndShould().NotDependOnAny(Types().That().ResideInNamespace("LibraryMembership.Slimmed.Presentation"))
            .As("Domain should only have dependencies within its own namespace");

        bool result = domainTypesShouldOnlyDependOnDomainNamespace.HasNoViolations(Architecture);

        Assert.True(result, "Domain should not have dependencies on other namespaces");
    }
    
    [Fact]
    public void DomainShouldNotDependOnOtherNamespaces2()
    {
        IArchRule domainTypesShouldOnlyDependOnDomainNamespace = Types().That().Are(DomainTypes)
            .Should().NotDependOnAny(Types().That().ResideInNamespace("LibraryMembership.Slimmed.Application"))
            .AndShould().NotDependOnAny(Types().That().ResideInNamespace("LibraryMembership.Slimmed.Infrastructure"))
            .AndShould().NotDependOnAny(Types().That().ResideInNamespace("LibraryMembership.Slimmed.Presentation"))
            .AndShould().NotDependOnAny(Types().That().ResideInNamespace("LibraryMembership.Slimmed.Infrastructure.Persistence"))
            .As("Domain should only have dependencies within its own namespace");

        bool result = domainTypesShouldOnlyDependOnDomainNamespace.HasNoViolations(Architecture);

        Assert.True(result, "Domain should not have dependencies on other namespaces");
    }
}